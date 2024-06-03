using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper
        )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto product)
        {
            var categoryFound = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId) ?? throw AppException.CategoryNotFound(product.CategoryId);
            if (product.Price < 0)
            {
                throw AppException.ProductPriceLessThan0();
            }
            if (product.Inventory < 0)
            {
                throw AppException.ProductInventoryLessThan0();
            }
            var productCreate = new Product(
                product.Name,
                product.Description,
                categoryFound,
                product.Price,
                product.Inventory
            );

            foreach (string image in product.Images)
            {
                productCreate.Images.Add(new Image(productCreate.Id, image));
            }
            var newProduct = await _productRepository.CreateProductAsync(productCreate) ?? throw AppException.CreateProductFail();

            return _mapper.Map<ProductReadDto>(newProduct);
        }

        public async Task<bool> DeleteProductByIdAsync(Guid id)
        {
            _ = await _productRepository.GetProductByIdAsync(id) ?? throw AppException.ProductNotFound(id);
            return await _productRepository.DeleteProductByIdAsync(id);
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<ProductReadDto> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id) ?? throw AppException.NotFound($"Product with ID {id} not found.");
            return _mapper.Map<ProductReadDto>(product);
        }

        public async Task<ProductReadDto> UpdateProductByIdAsync(Guid id, ProductUpdateDto product)
        {
            var productFound = await _productRepository.GetProductByIdAsync(id);
            if (productFound == null)
            {
                throw new ArgumentException("product not found");
            }
            // TODO could refactor this to a helper function
            if (product.Price != null)
            {
                if (product.Price < 0)
                {
                    throw AppException.ProductNotFound(productFound.Id);
                }
                productFound.Price = (decimal)product.Price;
            }
            if (product.Inventory != null)
            {
                if (product.Inventory < 0)
                {
                    throw AppException.ProductInventoryLessThan0();
                }

                productFound.Inventory = (int)product.Inventory;
            }
            if (product.CategoryId != null)
            {
                var categoryFound = await _categoryRepository.GetCategoryByIdAsync((Guid)product.CategoryId);
                if (categoryFound == null)
                {
                    throw AppException.CategoryNotFound(product.CategoryId);
                }
                productFound.Category = categoryFound;
            }
            if (product.Description != null)
            {
                productFound.Description = product.Description;
            }
            if (product.Name != null)
            {
                productFound.Name = product.Name;
            }
            if (product.Images != null)
            {
                productFound.Images.Clear();

                productFound.Images.AddRange(product.Images.Select(url => new Image { Url = url }));
            }

            await _productRepository.UpdateProductAsync(productFound);
            return _mapper.Map<ProductReadDto>(productFound);
        }
    }
}