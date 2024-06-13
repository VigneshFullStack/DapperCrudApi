using Dapper;
using DapperCrudApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperCrudApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var products = await db.QueryAsync<Product>("GetProducts", commandType: CommandType.StoredProcedure);
                return products;
                //return await db.QueryAsync<Product>("SELECT * FROM Products");
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { Id = id };
                return await db.QueryFirstOrDefaultAsync<Product>("GetProductById", parameters, commandType: CommandType.StoredProcedure);
                //return await db.QueryFirstOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<int> AddProduct(Product product)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { product.Name, product.Price, product.Stock };
                var result = await db.QuerySingleAsync<int>("AddProduct", parameters, commandType: CommandType.StoredProcedure);
                return result;
                //var sql = "INSERT INTO Products (Name, Price, Stock) VALUES (@Name, @Price, @Stock)";
                //return await db.ExecuteAsync(sql, product);
            }
        }

        public async Task<int> UpdateProduct(Product product)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { product.Id, product.Name, product.Price, product.Stock };
                return await db.ExecuteAsync("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);
                //var sql = "UPDATE Products SET Name = @Name, Price = @Price, Stock = @Stock WHERE Id = @Id";
                //return await db.ExecuteAsync(sql, product);
            }
        }

        public async Task<int> UpdateProductPartial(int id, JsonPatchDocument<Product> patchDoc)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var product = await GetProductById(id);
                if(product  == null)
                {
                    return 0;
                }
                patchDoc.ApplyTo(product);
                var parameters = new { product.Id, product.Name, product.Price, product.Stock };
                return await db.ExecuteAsync("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);
                //var sql = "UPDATE Products SET Name = @Name, Price = @Price, Stock = @Stock WHERE Id = @Id";
                //return await db.ExecuteAsync(sql, product);
            }
        }

        public async Task<int> DeleteProduct(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { Id = id };
                return await db.ExecuteAsync("DeleteProduct", parameters, commandType: CommandType.StoredProcedure);
                //var sql = "DELETE FROM Products WHERE Id = @Id";
                //return await db.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
