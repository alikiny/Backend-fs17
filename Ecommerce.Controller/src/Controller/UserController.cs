using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace WebDemo.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [Authorize(Roles = "Admin")]// authentication middleware would be invoked if user send get request to this endpoint
        [HttpGet("")] // define endpoint: /users?page=1&pageSize=10
        public async Task<IEnumerable<UserWithRoleDto>> GetAllUsersAsync([FromQuery] QueryOptions options)
        {
            return await _userService.GetAllUsersAsync(options);
        }

        // only admin can get user profile by id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")] // http://localhost:5233/api/v1/users/{id} headers: Authorization: Bearer {token}
        public async Task<UserWithRoleDto> GetUserByIdAsync([FromRoute] Guid id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        // user needs to be logged in to check her own profile
        [Authorize]
        [HttpGet("user_profile")]//http://localhost:5233/api/v1/users/user_profile Headers: Authorization: Bearer {token}
        public async Task<ActionResult<UserWithRoleDto>> GetUserProfileAsync()
        {
            // Retrieve the user's claims from the HttpContext
            var claims = HttpContext.User;

            // Extract the user ID from the claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Get the user's profile using the user ID
            var userProfile = await _userService.GetUserByIdAsync(userId);

            // Return the user's profile
            return Ok(userProfile);
        }

        // Endpoint to create a new user
        [AllowAnonymous] // Allow access to everyone
        [HttpPost("")]// http://localhost:5233/api/v1/users
        public async Task<ActionResult<UserWithRoleDto>> CreateUser([FromBody] UserCreateDto userDto)
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return Ok(createdUser);
        }

        [HttpDelete("{id}")] // http://localhost:5233/api/v1/users/{id} headers: Authorization: Bearer {token}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            await _userService.DeleteUserByIdAsync(id);
            return NoContent();
        }

        [HttpDelete("delete_me")]//http://localhost:5233/api/v1/users/delete_me Headers: Authorization: Bearer {token}
        [Authorize]
        public async Task<IActionResult> DeleteCurrentUserAsync()
        {
            // Retrieve the user's ID from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? throw AppException.UserNotFound();
            var userId = Guid.Parse(userIdClaim.Value);
            await _userService.DeleteUserByIdAsync(userId);
            return NoContent();
        }

        [HttpPut("{id}")] // http://localhost:5233/api/v1/users/{id} headers: Authorization: Bearer {token}
        [Authorize]
        public async Task<ActionResult<UserWithRoleDto>> UpdateUserAsync(Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            // Retrieve the user's ID from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? throw AppException.UserNotFound();

            var userId = Guid.Parse(userIdClaim.Value);

            // Check if the user is updating their own account
            if (userId != id)
                throw AppException.Forbidden("You are not authorized to update this user.");

            // Update the user
            return await _userService.UpdateUserByIdAsync(id, userUpdateDto);
        }
    }
}