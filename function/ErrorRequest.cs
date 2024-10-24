using Microsoft.AspNetCore.Mvc;
using WebApiMagazin.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiMagazin.function
{
    public class ErrorRequest
    {
        private OrderController _orderController;
        private string? code;
        private string? name;

        public ErrorRequest(OrderController orderController, string? code, string? name)
        {
            this._orderController = orderController;
            this.code = code;
            this.name = name;
        }
        public ObjectResult Result()
        {
            var obj = new
            {
                errors = new
                {
                   code = this.code,
                   name = this.name
                }
            };
            return this._orderController.StatusCode(StatusCodes.Status422UnprocessableEntity, obj);
        }
    }
}
