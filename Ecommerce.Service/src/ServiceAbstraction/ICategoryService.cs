
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto);
        Task<CategoryReadDto> UpdateCategoryAsync(Guid id, CategoryUpdateDto categoryDto);
        Task<CategoryReadDto> GetCategoryByIdAsync(Guid categoryId);
        Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync();
        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }
}
