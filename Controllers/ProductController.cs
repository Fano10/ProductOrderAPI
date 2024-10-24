using Microsoft.AspNetCore.Mvc;
using WebApiMagazin.Data;

namespace WebApiMagazin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private ContextDb contextDb;
        public ProductController(ContextDb contextDb)
        {
            this.contextDb = contextDb;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var result = contextDb.Product.ToList();
            return this.Ok(result);
        }
    }
}
