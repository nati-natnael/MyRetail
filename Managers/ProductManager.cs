using System.Threading.Tasks;

public class ProductManager : IProductManager
{
    private readonly IProductRepository _productRepository;

    public ProductManager(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> GetProduct(long id)
    {
        Product product = await _productRepository.GetProduct(id);

        Price price = _productRepository.GetProductPrice(id);

        product.Price = price;

        return product;
    }

    public async Task<bool> UpdateProductPrice(long id, decimal price)
    {
        return await _productRepository.UpdateProductPrice(id, price);
    }
}