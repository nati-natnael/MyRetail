using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

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
            try
            {
                return await _productManager.GetProductAsync(id);
            }
            catch (Exception)
            {
                throw new Exception($"Product not found: {id}");
            }
        }

        [HttpPut("{id:int}/price/{price:double}")]
        public async Task Put(long id, decimal price)
        {
            try
            {
                await _productManager.UpdateProductPriceAsync(id, price);
            }
            catch (Exception)
            {
                throw new Exception($"Price update failed on product: {id}");
            }
        }
    }
}
