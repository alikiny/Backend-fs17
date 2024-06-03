using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src;
using Ecommerce.Service.src.DTO;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserReadDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();
        CreateMap<User, UserWithRoleDto>()
            .ForMember(dest => dest.RoleText, opt => opt.MapFrom(src => GetRoleText(src.Role)));

        // Product mappings
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();
        CreateMap<Product, ProductReadDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category)) // Assuming Category is a direct property
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)); // Assuming Images is a collection of Image entities

        // Address mappings
        CreateMap<Address, AddressReadDto>();
        CreateMap<AddressCreateDto, Address>();
        CreateMap<AddressUpdateDto, Address>();

        // Order mappings
        CreateMap<OrderCreateDto, Order>();
        CreateMap<OrderUpdateDto, Order>();

        // Review mappings
        CreateMap<Review, ReviewReadDto>();
        CreateMap<ReviewCreateDto, Review>();
        CreateMap<ReviewUpdateDto, Review>();

        // Category mappings
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();

        // Image mappings
        CreateMap<Image, ImageReadDto>();
    }

    private string GetRoleText(UserRole role)
    {
        switch (role)
        {
            case UserRole.Admin:
                return "admin";
            case UserRole.User:
                return "user";
            // Add more cases as needed
            default:
                return "Unknown";
        }
    }
}
