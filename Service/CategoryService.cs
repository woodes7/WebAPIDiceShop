using Data;
using Mapster;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        // private IContextFactory contextFactory;
        private readonly DiceShopContext diceShopContext;

        public CategoryService(DiceShopContext diceShopContext)
        {
            this.diceShopContext = diceShopContext;
        }
        public List<CategoryDto> GetCategories()
        {
            try
            {
                var categoriesDto = diceShopContext.Categories
                    .ProjectToType<CategoryDto>()
                    .ToList();

                return categoriesDto;
            }
            catch (Exception ex)
            {
                // Puedes registrar el error o lanzar uno personalizado
                Console.WriteLine($"Error al obtener categorías: {ex.Message}");
                return new List<CategoryDto>(); // Devuelve lista vacía en caso de error
            }
        }

        public PagedResult<CategoryDto> GetCategoriesPaged(int page, int pageSize, string search)
        {
            var query = diceShopContext.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            var totalCount = query.Count();

            var items = query
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<CategoryDto>()
                .ToList();

            return new PagedResult<CategoryDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }


        public CategoryDto GetCategoryById(int id)
        {
            return diceShopContext.Categories.Find(id).Adapt<CategoryDto>();
        }

        public bool AddCategory(CategoryDto categoryDto)
        {
            var categoryEntity = categoryDto.Adapt<Category>();
            diceShopContext.Categories.Add(categoryEntity);
            return diceShopContext.SaveChanges() > 0;

        }

        public bool DeleteCategory(int id)
        {

            var category = diceShopContext.Categories.Find(id);
            if (category == null) return false;

            diceShopContext.Categories.Remove(category);
            return diceShopContext.SaveChanges() > 0;

        }

        public bool UpdateCategory(CategoryDto categoryDto)
        {
            var category = diceShopContext.Categories.FirstOrDefault(c => c.Id == categoryDto.Id);
            if (category == null) return false;

            categoryDto.Adapt(category);
            diceShopContext.Update(category);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}
