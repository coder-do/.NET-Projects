using Microsoft.AspNetCore.Mvc;
using SolarCoffee.Services.Product;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpPost]
        public ActionResult CreateProduct([FromBody] ProductModel product)
        {
            _logger.LogInformation("Creating a product");

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = ProductMapper.SerializeProductModel(product);
            var res = _productService.CreateProduct(newProduct);
            return Ok(res);
        }

        [HttpGet]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Getting all products");
            var products = _productService.GetAllProducts();

            var productModels = products
                .Select(p => ProductMapper.SerializeProductModel(p));

            return Ok(productModels);
        }

        [HttpPatch("{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation($"Archiving product {id}");

            var res = _productService.ArchiveProduct(id);

            return Ok(res);
        }
    }
}
