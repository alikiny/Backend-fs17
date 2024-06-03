using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src;
using Ecommerce.Service.src.DTO;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class ProductRepo : IProductRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<Product> _products;
        private readonly DbSet<Image> _image;

        public ProductRepo(EcommerceDbContext context)
        {
            _context = context;
            _products = _context.Products;
            _image = _context.Images;
        }

        /* EF core work flow
        - snapshot -> create a snapshot of the current state of the entity
        - saved changes -> update actual database
         */
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductByIdAsync(Guid id)
        {
            await _products.Where(p => p.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsyncWithFilter(QueryOptions? options)
        {
            var searchKey = options?.SearchKey ?? "";
            var skipFrom = (options?.StartingAfter == null ? options?.StartingAfter : 0) + 1;
            var sortBy = options?.SortBy ?? AppConstants.DEFAULT_SORT_BY;

            IEnumerable<Product> products;

            if (options?.SortOrder == SortOrder.ASC)
            {
                products = await _products
                    .Where(p => p.Name.Contains(searchKey))
                    .Skip(skipFrom ?? 1)
                    .Take(options?.Limit ?? AppConstants.PER_PAGE)
                    .OrderBy(p => sortBy)
                    .ToListAsync();
            }
            else
            {
                products = await _products
                    .Where(p => p.Name.Contains(searchKey))
                    .Skip(skipFrom ?? 1)
                    .Take(options?.Limit ?? AppConstants.PER_PAGE)
                    .OrderByDescending(p => sortBy)
                    .ToListAsync();
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }


        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.Include(p => p.Category)
                .Include(p => p.Images).SingleOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product productUpdate)
        {

            productUpdate.UpdatedAt = DateTimeOffset.UtcNow;
            //delete all image that have product_id as null in database
            var imagesToDelete = await _image.Where(i => i.ProductId == null).ToListAsync();
            _image.RemoveRange(imagesToDelete);

            await _context.SaveChangesAsync();
            return await _products.SingleAsync(p => p.Id == productUpdate.Id);
        }

    }
}
