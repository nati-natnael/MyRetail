using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductManager
{
    public Task<Product> GetProductAsync(long id);
    public Task<bool> UpdateProductPriceAsync(long id, double price);
}