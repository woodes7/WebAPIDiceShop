using DataModel;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;


namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController
    {
        private IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }


        [HttpGet("products")]

        public async Task<PagedResult<ProductDto>> GetProdutcDto(int pageNumber, int pageSize, string? search = null, int? categoryId = null)
        {
            return await productService.GetProductsPagedAsync(pageNumber, pageSize, search, categoryId);
        }

        [HttpGet("{id}")]
        public ProductDto getProductById(int id) {

            return this.productService.GetProductDto(id);
        }

        [HttpPost("add")]
        public bool PostProduct(ProductDto productDto)
        {
            var createrProduct = productService.AddProductDto(productDto);
            return createrProduct;
        }

        [HttpPut("edit")]
        public bool EditProduct(ProductDto productDto)
        {
            return productService.UpdateProductDto(productDto);
        }

        [HttpDelete("delete/{id}")]
        public bool DeleteProduct(int id)
        {
            return productService.DeleteProductDto(id); 
        }
    }
}
