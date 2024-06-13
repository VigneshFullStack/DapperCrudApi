using DapperCrudApi.Models;
using DapperCrudApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            await _productRepository.AddProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productRepository.UpdateProduct(product);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateProductPartial(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if(patchDoc == null)
            {
                return BadRequest();
            }

            var result = await _productRepository.UpdateProductPartial(id, patchDoc);
            if(result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteProduct(id);
            return NoContent();
        }
    }
}
