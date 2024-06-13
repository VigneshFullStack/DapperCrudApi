using DapperCrudApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace DapperCrudApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<int> AddProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<int> UpdateProductPartial(int id, JsonPatchDocument<Product> patchDoc);
        Task<int> DeleteProduct(int id);
    }
}
