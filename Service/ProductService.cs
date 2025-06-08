using Data;
using DataModel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class ProductService : IProductService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContext;
        public ProductService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContext = contextFactory;
        }

        public bool AddProductDto(ProductDto productDto)
        {
            var productEntity = productDto.Adapt<Product>();

            using var context = diceShopContext.CreateDbContext();
            context.Products.Add(productEntity);
            return context.SaveChanges() > 0;
        }


        public bool DeleteProductDto(int id)
        {
            using var context = diceShopContext.CreateDbContext();
            var product = context.Products.Find(id);
            if (product == null) return false;

            context.Products.Remove(product);
            return context.SaveChanges() > 0;
        }

        public ProductDto GetProductDto(int id)
        {
            using var context = diceShopContext.CreateDbContext();
            var product = context.Products.AsNoTracking().Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            var productDto = product.Adapt<ProductDto>();
            return productDto;
        }

        public async Task<PagedResult<ProductDto>> GetProductsPagedAsync(int pageNumber, int pageSize, string? search = null, int? categoryId = null)
        {
            try
            {
                using var context = diceShopContext.CreateDbContext();

                var query = context.Products.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
                }

                if (categoryId != null)
                {
                    query = query.Where(p => p.CategoryId == categoryId.Value);
                }

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderBy(p => p.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectToType<ProductDto>()
                    .ToListAsync();
                return new PagedResult<ProductDto>
                {
                    Items = items,
                    TotalCount = totalCount
                };


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos: {ex.Message}");
                return new PagedResult<ProductDto>();
            }
        }






        public bool UpdateProductDto(ProductDto productDto)
        {
            using var context = diceShopContext.CreateDbContext();

            var product = context.Products.FirstOrDefault(c => c.Id == productDto.Id);
            if (product == null) return false;
            productDto.Adapt(product);
            context.Update(product);
            return context.SaveChanges() > 0;
        }



    }
}
