using Microsoft.AspNetCore.Mvc;
using DataModel;
using Service;
using Model;


namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("categories")]
        public List<CategoryDto> GetCategories()
        {
            return categoryService.GetCategories();
        }


        [HttpGet("categoriesPaged")]
        public ActionResult<PagedResult<CategoryDto>> GetCategoriesPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var result = categoryService.GetCategoriesPaged(page, pageSize, search);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetById(int id)
        {
            var category = categoryService.GetCategoryById(id);

            if (category == null)
                return NotFound(); // Devuelve 404 si no existe

            return Ok(category); // Devuelve 200 con el objeto si sí existe
        }

        [HttpPost("add")]
        public bool Create([FromBody] CategoryDto categoryDto)
        {
            var createdCategory = categoryService.AddCategory(categoryDto);
            return createdCategory;
        }

        [HttpPut("edit")]
        public bool UpdateCategory(CategoryDto categoryDto)
        {
            return categoryService.UpdateCategory(categoryDto);
        }


        [HttpDelete("delete/{id}")]
        public bool Delete(int id)
        {
            var result = categoryService.DeleteCategory(id);
            return result;
        }


    }
}
