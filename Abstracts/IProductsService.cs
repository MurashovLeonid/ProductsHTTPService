using Microsoft.AspNetCore.Mvc;
using ProductsHTTPService.DTOModels;
using ProductsHTTPService.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsHTTPService.Abstracts
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDTO>> GetProductAsync(ProductsContext context);
        Task<IEnumerable<ProductDTO>> GetProductsByIdAsync(ProductsContext context, ProductListDTO product);
        Task<IActionResult> EditProductAsync(ProductsContext context, ProductDTO product);
        Task<IActionResult> DeleteProductAsync(ProductsContext context, ProductListDTO product);

    }
}
