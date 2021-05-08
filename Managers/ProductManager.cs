using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductManager : IProductManager
{
    private readonly IMyRetailRepository _myRetailRepository;

    public ProductManager(IMyRetailRepository myRetailRepository)
    {
        _myRetailRepository = myRetailRepository;
    }

    public async Task<Product> GetProduct(long id)
    {
        Product product = await _myRetailRepository.GetProduct(id);

        Price price = _myRetailRepository.GetProductPrice(id);

        product.Price = price;

        return product;
    }
}