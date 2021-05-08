using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductManager
{
    public Task<Product> GetProduct(long id);
}