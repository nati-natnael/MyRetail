using System.Threading.Tasks;

public class ProductManager : IProductManager
{
    private readonly IProductRepository _productRepository;

    public ProductManager(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> GetProductAsync(long id)
    {
        Product product = await _productRepository.GetProductAsync(id);

        if (product != null)
        {
            product.Price = await _productRepository.GetProductPriceAsync(id);
        }

        return product;
    }

    public async Task<bool> UpdateProductPriceAsync(long id, double price)
    {
        return await _productRepository.UpdateProductPriceAsync(id, price);
    }
}