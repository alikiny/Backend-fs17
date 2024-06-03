using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Ecommerce.Service.src.ServiceAbstraction;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly User testAdmin = new User(
                    "admin",
                    "test",
                    UserRole.Admin,
                    "avatarlink",
                    "admin@mail.com",
                    "secure"
                );

        private readonly User testUser = new User(
            "user",
            "test",
            UserRole.User,
            "avatarlink",
            "user@mail.com",
            "secure"
        );

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenEmailDoesNotExist()
        {
            // Arrange
            var userCreateDto = new UserCreateDto { Email = "test@example.com", FirstName = "John", LastName = "Doe", Password = "password" };
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", FirstName = "John", LastName = "Doe" };
            var userWithRoleDto = new UserWithRoleDto { Id = user.Id, Email = "test@example.com", FirstName = "John", LastName = "Doe", RoleText = "User" };

            _userRepoMock.Setup(repo => repo.UserExistsByEmailAsync(user.Email)).ReturnsAsync(false);
            _userRepoMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<User>(userCreateDto)).Returns(user);
            _mapperMock.Setup(mapper => mapper.Map<UserWithRoleDto>(user)).Returns(userWithRoleDto);

            // Act
            var result = await _userService.CreateUserAsync(userCreateDto);

            // Assert
            Assert.Equal(userWithRoleDto, result);
            _userRepoMock.Verify(repo => repo.UserExistsByEmailAsync(user.Email), Times.Once);
            _userRepoMock.Verify(repo => repo.CreateUserAsync(user), Times.Once);
        }
    }
}