using Microsoft.AspNetCore.Mvc;
using WebApiMagazin.Controllers;
using WebApiMagazin.Model;

namespace WebApiMagazin.function
{
    public class SuccessRequest
    {
        private OrderController _orderController;
        private Product? _product;
        private Order? _order;
        private Client? _client;
        private CreditCard? _creditCard;
        private Transaction? _transaction;
        public SuccessRequest(OrderController orderController ,Product product, Order order) { 
            _orderController = orderController;
            this._product = product;
            this._order = order;
        }
        public SuccessRequest(OrderController orderController, Product product, Order order, Client client)
        {
            _orderController = orderController;
            this._product = product;
            this._order = order;
            this._client = client;
        }
        public SuccessRequest(OrderController orderController, Product product, Order order, Client client, CreditCard creditCard)
        {
            _orderController = orderController;
            this._product = product;
            this._order = order;
            this._client = client;
            this._creditCard = creditCard;
        }
        public SuccessRequest(OrderController orderController, Product product, Order order, Client client, CreditCard creditCard,Transaction transaction)
        {
            _orderController = orderController;
            this._product = product;
            this._order = order;
            this._client = client;
            this._creditCard = creditCard;
            this._transaction = transaction;
        }

        public ObjectResult Result(Controller _controller)
        {
            var product = new
            {
                id = _product!.id,
                quantity = _order!.quantity
            };
            if (_client != null)
            {
                var shipping = new
                {
                    email = _client.email,
                    country = _client.country,
                    address = _client.address,
                    postal_code = _client.postal_code,
                    city = _client.city,
                    province = _client.province
                };
                if(_creditCard != null && _transaction !=null)
                {
                    var card = new
                    {
                        name = _creditCard!.name,
                        number = _creditCard!.number,
                        expiration_year = _creditCard!.expiration_year,
                        cvv = _creditCard!.cvv,
                        expiration_month = _creditCard!.expiration_month
                    };
                    var transaction = new
                    {
                        id = _transaction!.idString,
                        success = _transaction!.success,
                        amount_charged = _transaction!.amount_charged
                    };
                    var obj = new
                    {
                        id = _order.id,
                        total_price = _order!.total_price,
                        paid = _order!.paid,
                        shipping_price = _order.shipping_price,
                        product = product,
                        credit_card = card,
                        transaction = transaction,
                        shipping_information = shipping
                    };
                    return _controller.Ok(obj);
                }
                else
                {
                    var card = new { };
                    var transaction = new { };
                    var obj = new
                    {
                        id = _order.id,
                        total_price = _order!.total_price,
                        paid = _order!.paid,
                        shipping_price = _order.shipping_price,
                        product = product,
                        credit_card = card,
                        transaction = transaction,
                        shipping_information = shipping
                    };
                    return _controller.Ok(obj);
                }
            }
            else
            {
                var shipping = new { };
                var card = new { };
                var transaction = new { };
                var obj = new
                {
                    id = _order.id,
                    total_price = _order!.total_price,
                    paid = _order!.paid,
                    shipping_price = _order.shipping_price,
                    product = product,
                    credit_card = card,
                    transaction = transaction,
                    shipping_information = shipping
                };
                return _controller.Ok(obj);
            }
        }
    }
}
