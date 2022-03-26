using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsHTTPService.DTOModels
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string BrandName { get; set; }
        public string RfSize { get; set; }
    }
}
