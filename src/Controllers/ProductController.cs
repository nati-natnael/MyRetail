using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Net;

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
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                return Ok(await _productManager.GetProductAsync(id));
            }
            catch (Exception)
            {
                return NotFound($"Product not found: {id}");
            }
        }

        [HttpPut("{id:int}/price/{price:double}")]
        public async Task<IActionResult> Put(long id, double price)
        {
            try
            {
                return Ok(await _productManager.UpdateProductPriceAsync(id, price));
            }
            catch (Exception)
            {
                return NotFound($"Product not found: {id}");
            }
        }
    }
}
