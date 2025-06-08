using DataModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IProductService
    {

        public Task<PagedResult<ProductDto>> GetProductsPagedAsync(int pageNumber, int pageSize, string? search = null, int? categoryId = null);

        public ProductDto GetProductDto(int id);

        public bool AddProductDto(ProductDto productDto);

        public bool UpdateProductDto(ProductDto productDto);

        public bool DeleteProductDto(int id);
    }
}
