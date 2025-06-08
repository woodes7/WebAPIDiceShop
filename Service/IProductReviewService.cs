using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IProductReviewService
    {
        public List<ProductreviewDto> GetProductReviewDto();

        public ProductreviewDto GetProductReviewDto(int id);

        public bool AddProductreviewDto(ProductreviewDto productreviewDto);

        public bool UpdateProductreviewDto(ProductreviewDto productreviewDto);

        public bool DeleteProductreviewDto(int id);
    }
}
