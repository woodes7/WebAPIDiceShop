using DataModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICategoryService 
    {
        public List<CategoryDto> GetCategories();
        public PagedResult<CategoryDto> GetCategoriesPaged(int page, int pageSize, string search);
        public CategoryDto GetCategoryById(int id);
        public bool AddCategory(CategoryDto categoryDto);
        public bool DeleteCategory(int id);
        public bool UpdateCategory(CategoryDto categoryDto);
    }
}
