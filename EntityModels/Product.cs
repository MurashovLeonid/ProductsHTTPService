using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsHTTPService.EntityModels
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductBrandName { get; set; }
        
        public string ProductRfSize { get; set; }
        public bool IsDeleted { get; set; }

    }
}
