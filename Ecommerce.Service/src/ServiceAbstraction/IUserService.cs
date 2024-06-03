using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.Common;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IUserService
    {
        Task<UserWithRoleDto> CreateUserAsync(UserCreateDto user);
        Task<UserWithRoleDto> UpdateUserByIdAsync(Guid id, UserUpdateDto user);
        Task<UserWithRoleDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserWithRoleDto>> GetAllUsersAsync(QueryOptions options);
        Task<bool> DeleteUserByIdAsync(Guid id);
    }
}
