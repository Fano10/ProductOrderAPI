using System.Text;
using System.Text.Json;
using WebApiMagazin.Model;

namespace WebApiMagazin.function
{
    public class UseAPI
    {
        private readonly HttpClient _httpClient;

        public UseAPI()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> Paiment(CreditCard creditCard, string apiURl)
        {
            //Serialiset l'objet creditCard en JSON
            var cardJson = JsonSerializer.Serialize(creditCard);
            var content = new StringContent(cardJson , Encoding.UTF8, "application/json");
            //Envoyer une requete POST asynchrone
            HttpResponseMessage response = await _httpClient.PostAsync(apiURl,content);

            return response;
        }
    }
}
