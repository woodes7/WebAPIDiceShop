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
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public ProductService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public bool AddProductDto(ProductDto productDto)
        {
            var productEntity = productDto.Adapt<Product>();

            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            diceShopContext.Products.Add(productEntity);
            return diceShopContext.SaveChanges() > 0;
        }


        public bool DeleteProductDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var product = diceShopContext.Products.Find(id);
            if (product == null) return false;

            diceShopContext.Products.Remove(product);
            return diceShopContext.SaveChanges() > 0;
        }

        public ProductDto GetProductDto(int id)
        {

            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var product = diceShopContext.Products.AsNoTracking().Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            var productDto = product.Adapt<ProductDto>();
            return productDto;
        }

        public async Task<PagedResult<ProductDto>> GetProductsPagedAsync(int pageNumber, int pageSize, string? search = null, int? categoryId = null)
        {
            try
            {
                using var diceShopContext = diceShopContextFactory.CreateDbContext();

                var query = diceShopContext.Products.AsQueryable();

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
            using var diceShopContext = diceShopContextFactory.CreateDbContext();

            var product = diceShopContext.Products.FirstOrDefault(c => c.Id == productDto.Id);
            if (product == null) return false;
            productDto.Adapt(product);
            diceShopContext.Update(product);
            return diceShopContext.SaveChanges() > 0;
        }



    }
}
