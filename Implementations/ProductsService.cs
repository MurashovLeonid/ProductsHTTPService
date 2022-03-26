using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsHTTPService.Abstracts;
using ProductsHTTPService.DTOModels;
using ProductsHTTPService.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsHTTPService.Implementations
{
    public class ProductsService : IProductsService
    {
       public async Task<IEnumerable<ProductDTO>> GetProductAsync(ProductsContext context)
       {
            var products = await context.Products?
               .AsNoTracking()
               .Where(x => x.IsDeleted != true)
               .Select(x =>
               new ProductDTO
               {
                   ProductId = x.ProductId,
                   BrandName = x.ProductBrandName,
                   RfSize = x.ProductRfSize
               })
              .ToListAsync();

            return products;
       }
       public async Task<IEnumerable<ProductDTO>> GetProductsByIdAsync(ProductsContext context, ProductListDTO product)
       {
            var products = new List<ProductDTO>();
            if (product.ProductId.Any())
            {
                var validModelIdArray = product.ProductId.Where(x => x > 0).ToArray();
                products = await context.Products?
                .AsNoTracking()
                .Where(x => validModelIdArray.Contains(x.ProductId) && x.IsDeleted != true)
                .Select(x => new ProductDTO
                {
                    ProductId = x.ProductId,
                    BrandName = x.ProductBrandName,
                    RfSize = x.ProductRfSize
                })
                .ToListAsync();
            }
            return products;
       }
       public async Task<IActionResult> EditProductAsync(ProductsContext context, ProductDTO product)
       {
            try
            {
                if (product.ProductId < 0)
                {
                    return new JsonResult(new { mes = "ID товара не может быть меньше 0!" });
                }
                var _product = await context.Products.Where(x => x.ProductId == product.ProductId).FirstAsync();
                if (String.IsNullOrEmpty(_product.ProductId.ToString()))
                {
                    return new JsonResult(new { mes = "В базе данных нет бренда с указанным ID" });
                }
                _product.ProductBrandName = product.BrandName;
                _product.ProductRfSize = product.RfSize;
                await context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(new { mes = "Во время выполнения обновления произошла ошибка!" });
            }
            return new JsonResult(new { mes = "Обновление названия бренда успешно завершено!" });
       }
        public async Task<IActionResult> DeleteProductAsync(ProductsContext context, ProductListDTO products)
        {
            for (int i = 0; i <= products.ProductId.Length - 1; i++)
            {
                if (products.ProductId[i] < 0)
                {
                    continue;
                }
                var product = await context.Products.Where(x => x.ProductId == products.ProductId[i]).FirstAsync();

                if (product.IsDeleted == true)
                {
                    continue;
                }
                else
                {
                    product.IsDeleted = true;
                }
                context.SaveChanges();
            }
            await context.SaveChangesAsync();
            return new JsonResult(new { mes = "Операция удаления выполнена" });
        }


    }
}
