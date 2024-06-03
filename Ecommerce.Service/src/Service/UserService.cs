using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Ecommerce.Service.src.Validation;

namespace Ecommerce.Service.src.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserWithRoleDto> CreateUserAsync(UserCreateDto userDto)
        {
            // Map the UserCreateDto to a User entity
            var user = _mapper.Map<User>(userDto);
            // Ensure CreatedAt is set to UTC
            user.CreatedAt = DateTimeOffset.UtcNow;

            // Check if a user with the given email already exists
            var isUserExistWithEmail = await _userRepo.UserExistsByEmailAsync(user.Email);

            if (isUserExistWithEmail)
            {
                throw AppException.UserCredentialErrorEmailAlreadyExist(user.Email);
            }

            // Create the user asynchronously
            var createdUser = await _userRepo.CreateUserAsync(user);

            // Map the created User entity to UserReadDto and return it
            return _mapper.Map<UserWithRoleDto>(createdUser);
        }


        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            _ = await _userRepo.GetUserByIdAsync(id) ?? throw AppException.UserNotFound($"User with ID {id} not found.");
            return await _userRepo.DeleteUserByIdAsync(id);
        }

        public async Task<IEnumerable<UserWithRoleDto>> GetAllUsersAsync(QueryOptions options)
        {
            var users = await _userRepo.GetAllUsersAsync(options);

            return _mapper.Map<IEnumerable<UserWithRoleDto>>(users);
        }

        public async Task<UserWithRoleDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepo.GetUserByIdAsync(id) ?? throw AppException.UserNotFound($"User with ID {id} not found.");
            return _mapper.Map<UserWithRoleDto>(user);
        }

        public async Task<UserWithRoleDto> UpdateUserByIdAsync(Guid id, UserUpdateDto userDto)
        {
            _ = await _userRepo.GetUserByIdAsync(id) ?? throw AppException.UserNotFound($"User with ID {id} not found.");

            var user = await _userRepo.GetUserByIdAsync(id);

            // Update user properties based on DTO
            if (!string.IsNullOrEmpty(userDto.Email))
            {
                user.Email = userDto.Email;
            }

            if (!string.IsNullOrEmpty(userDto.Avatar))
            {
                user.Avatar = userDto.Avatar;
            }

            if (!string.IsNullOrEmpty(userDto.FirstName))
            {
                user.FirstName = userDto.FirstName;
            }

            if (!string.IsNullOrEmpty(userDto.LastName))
            {
                user.LastName = userDto.LastName;
            }

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = userDto.Password;
            }

            // Save changes to repository
            await _userRepo.UpdateUserByIdAsync(user);

            // Map the updated user to UserReadDto

            return _mapper.Map<UserWithRoleDto>(user);
        }
    }
}
