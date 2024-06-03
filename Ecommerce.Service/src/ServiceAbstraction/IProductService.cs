using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.Common;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IProductService
    {
        Task<ProductReadDto> CreateProductAsync(ProductCreateDto product);
        Task<ProductReadDto> UpdateProductByIdAsync(Guid id, ProductUpdateDto product);
        Task<ProductReadDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductReadDto>> GetAllProductsAsync();
        Task<bool> DeleteProductByIdAsync(Guid id);
    }
}
