using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApiMagazin.Data;
using WebApiMagazin.ExtensionMethods;
using WebApiMagazin.function;
using WebApiMagazin.Model;

namespace WebApiMagazin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //Pour ajouter un policy a un controlleur. On peut meme etre plus specifique et l'ajouter a une action methode
    //[EnableCors(SecurityMethods.DEFAULT_POLICY)]
    public class OrderController : Controller
    {
        private ContextDb _context;
        public OrderController(ContextDb context) {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder()
        {
            StreamReader reader = null!;
            JsonDocument doc = null!;
            IActionResult result = null;
            try
            {
                reader = new StreamReader(Request.Body, Encoding.UTF8);
                var body = await reader.ReadToEndAsync();

                doc = JsonDocument.Parse(body);
                JsonElement root = doc.RootElement;

                var detailsOrder = root.GetProperty("product");

                int id = detailsOrder.GetProperty("id").GetInt32();
                int quantity = detailsOrder.GetProperty("quantity").GetInt32();

                try
                {
                    var product = _context.Product.Where(product => product.id == id && product.In_stock == true).First();
                    Order order = new Order
                    {
                        id_product = id,
                        quantity = quantity,
                        total_price = (int)(quantity * product.Price),
                        paid = false
                    };
                    var entry = _context.Order.Add(order);
                    _context.SaveChanges();
                    result = RedirectToAction("GetOrder", "Order", new { id = entry.Entity.id });

                }
                catch (Exception ex) {

                    ErrorRequest errorRequest = new ErrorRequest(this, "out-of inventory", "Le produit demande n'est pas en inventaire");
                    result = errorRequest.Result();
                }
            }
            catch (Exception ex)
            {
                ErrorRequest errorRequest = new ErrorRequest(this, "missing-fields", "La creation d'une commande necessite un produit");
                result = errorRequest.Result();
            }
            finally
            {
                reader.Dispose();
                doc.Dispose();
            }
            return result;
        }
        [HttpGet("{id}")]
        //Permet de ne pas appliquer la politique de CORS
        //[DisableCors]
        public IActionResult GetOrder(int id)
        {
            IActionResult result = null;
            try
            {
                var order = _context.Order.Where(order => order.id == id).First();
                var product = _context.Product.Where(p => p.id == order.id_product).First();
                Client? client = null;
                CreditCard? card = null;
                Transaction? transaction = null;
                if (order.id_client != null)
                {
                    client = _context.Client.Where(c => c.id == order.id_client).First();
                }
                if (order.id_credit_card != null)
                {
                    card = _context.CreditCard.Where(c => c.id == order.id_credit_card).First();
                }
                if (order.id_transaction != null)
                {
                    transaction = _context.Transaction.Where(t => t.id == order.id_transaction).First();
                }
                SuccessRequest successRequest = new SuccessRequest(this, product, order, client, card, transaction);
                result = successRequest.Result(this);
            }
            catch
            {
                ErrorRequest errorRequest = new ErrorRequest(this, "out-of-order", "La commande n'existe pas");
                result = errorRequest.Result();
            }

            return result;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id)
        {
            StreamReader reader = null!;
            JsonDocument doc = null!;
            IActionResult result = null;
            JsonElement obj;
            try
            {
                reader = new StreamReader(Request.Body, Encoding.UTF8);
                var body = await reader.ReadToEndAsync();
                doc = JsonDocument.Parse(body);
                JsonElement root = doc.RootElement;
                obj = root.GetProperty("order");
                try
                {
                    string? email = obj.GetProperty("email").GetString();
                    string? country = obj.GetProperty("shipping_information").GetProperty("country").GetString();
                    string? address = obj.GetProperty("shipping_information").GetProperty("address").GetString();
                    string? postal_code = obj.GetProperty("shipping_information").GetProperty("postal_code").GetString();
                    string? city = obj.GetProperty("shipping_information").GetProperty("city").GetString();
                    string? province = obj.GetProperty("shipping_information").GetProperty("province").GetString();

                    Client client = new Client
                    {
                        email = email,
                        country = country,
                        address = address,
                        postal_code = postal_code,
                        city = city,
                        province = province
                    };

                    var entry = _context.Client.Add(client);
                    _context.SaveChanges();
                    Order order = _context.Order.Where(o => o.id == id).First();
                    order.id_client = entry.Entity.id;
                    _context.SaveChanges();
                    result = RedirectToAction("GetOrder", "Order", new { id = id });
                }
                catch
                {
                    ErrorRequest errorRequest = new ErrorRequest(this, "missing-fields", "Il manque un ou plusieurs champs qui sont obligatoires");
                    result = errorRequest.Result();
                }
            }
            catch
            {
                JsonElement root = doc.RootElement;
                try
                {
                    obj = root.GetProperty("credit_card");
                    Order order = _context.Order.Where(o => o.id == id).First();
                    if (order.paid == true)
                    {
                        result = new ErrorRequest(this, "already-paid", "La commande a deja ete payer").Result();
                    }
                    else
                    {
                        CreditCard card = new CreditCard
                        {
                            name = obj.GetProperty("name").GetString(),
                            number = obj.GetProperty("number").GetString(),
                            expiration_year = obj.GetProperty("expiration_year").GetInt32(),
                            expiration_month = obj.GetProperty("expiration_month").GetInt32(),
                            cvv = obj.GetProperty("cvv").GetString(),
                        };
                        string url = "http://localhost:5235/Payer";
                        UseAPI useAPI = new UseAPI();
                        HttpResponseMessage response = await useAPI.Paiment(card, url);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            Transaction transaction = JsonSerializer.Deserialize<Transaction>(responseContent, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            })!;

                            var entryCredit = _context.CreditCard.Add(card);
                            var entryTransaction = _context.Transaction.Add(transaction);
                            _context.SaveChanges();
                            order.id_credit_card = entryCredit.Entity.id;
                            order.id_transaction = entryTransaction.Entity.id;
                            order.paid = true;
                            _context.SaveChanges();
                            result = RedirectToAction("GetOrder", "Order", new { id = id });
                        }
                        else
                        {
                            result = this.StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                catch
                {
                    ErrorRequest errorRequest = new ErrorRequest(this, "fatal-error", "Operations non valides");
                    result = errorRequest.Result();
                }

            }
            finally
            {
                reader.Dispose();
                doc.Dispose();
            }
            return result;
        }
    }
}
