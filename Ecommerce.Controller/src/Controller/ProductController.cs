using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()] // http://localhost:5233/api/v1/products Headers: Authorization: Bearer {token}
        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto productCreate)
        {
            return await _productService.CreateProductAsync(productCreate);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")] // http://localhost:5233/api/v1/products Headers: Authorization: Bearer {token}
        public async Task<ProductReadDto> UpdateProductAsync(Guid id, ProductUpdateDto productUpdate)
        {
            return await _productService.UpdateProductByIdAsync(id, productUpdate);
        }

        [AllowAnonymous]
        [HttpGet("{id}")] // http://localhost:5233/api/v1/products/{id}
        public async Task<ActionResult<ProductReadDto>> GetProductByIdAsync([FromRoute] Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet("")] // http://localhost:5233/api/v1/products
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProductsAsync([FromQuery] QueryOptions options)
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] // http://localhost:5233/api/v1/products/{id}
        public async Task<bool> DeleteProductByIdAsync([FromRoute] Guid id)
        {
            return await _productService.DeleteProductByIdAsync(id);
        }
    }
}
