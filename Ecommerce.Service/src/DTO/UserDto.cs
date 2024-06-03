using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.DTO
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Avatar { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public class UserReadDto
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public UserRole Role { get; }
        public string Avatar { get; }
        public string Email { get; }

        public UserReadDto(Guid id, string firstName, string lastName, UserRole role, string avatar, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Avatar = avatar;
            Email = email;
        }
    }

    public class UserWithRoleDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleText { get; set; } // Role text instead of UserRole enum
        public string Avatar { get; set; }
        public string Email { get; set; }
    }

    public class UserUpdateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Avatar { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
