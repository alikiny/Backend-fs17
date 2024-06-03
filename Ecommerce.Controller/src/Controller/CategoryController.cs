using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()] // POST /api/v1/categories authentication header with bearer token
        public async Task<ActionResult<Category>> CreateCategoryAsync(
            CategoryCreateDto categoryCreate
        )
        {
            var category = await _categoryService.CreateCategoryAsync(categoryCreate);
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]// PUT /api/v1/categories/{id} authentication header with bearer token
        public async Task<CategoryReadDto> UpdateCategory(
            [FromRoute] Guid id,
            CategoryUpdateDto categoryUpdate
        )
        {
            return await _categoryService.UpdateCategoryAsync(id, categoryUpdate);
        }

        [AllowAnonymous]
        [HttpGet("{id}")] // GET /api/v1/categories/{id}
        public async Task<ActionResult<CategoryReadDto>> RetrieveCategoryByIdAsync(
            [FromRoute] Guid id
        )
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        [AllowAnonymous]
        [HttpGet()] // GET /api/v1/categories
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> ListAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] // DELETE /api/v1/categories/{id} authentication header with bearer token
        public async Task<ActionResult<bool>> DeleteCategory([FromRoute] Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok(true);
        }
    }
}
