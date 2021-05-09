using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MyRetail.Controllers
{
    [ApiController]
    [Route("/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductManager productManager, ILogger<ProductController> logger)
        {
            _productManager = productManager;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<Product> Get(long id)
        {
            return await _productManager.GetProductAsync(id);
        }

        [HttpPut("{id:int}/price/{price:double}")]
        public async Task Put(long id, decimal price)
        {
            await _productManager.UpdateProductPriceAsync(id, price);
        }
    }
}
