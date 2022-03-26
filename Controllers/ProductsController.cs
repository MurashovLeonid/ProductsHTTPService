using Microsoft.AspNetCore.Mvc;
using ProductsHTTPService.Abstracts;
using ProductsHTTPService.DTOModels;
using ProductsHTTPService.EntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ProductsHTTPService.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        public const string HTTPRequestApi = "https://localhost:44331/Brands/IsAllowableSizeExist";
        private readonly ProductsContext _context;
        private readonly IProductsService _productsService;

        public ProductsController(ProductsContext context, IProductsService productsService)
        {
            _productsService = productsService;
            _context = context;

        }

        // GET: /Products/All
        [HttpGet("All")]
        public async Task<IActionResult> GetProductsAsync()
        {
            return Ok(await _productsService.GetProductAsync(_context));
        }

        // GET: /Products/List
        [HttpGet("List")]
        public async Task<IActionResult> GetProductsByIdAsync([FromBody]ProductListDTO product)
        {
            return Ok(await _productsService.GetProductsByIdAsync(_context, product));
        }

        // POST: /Products/Edit
        [HttpPost("Edit")]
        public async Task<IActionResult> EditProductAsync([Bind("ProductId, ProductBrandName, ProductRfSize")] ProductDTO product)
        {

            return new JsonResult(await _productsService.EditProductAsync(_context, product));
        }

        // POST: /Products/Delete
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteProductAsync([FromBody] ProductListDTO product)
        {

            return new JsonResult(await _productsService.DeleteProductAsync(_context, product));
        }

        // POST: /Products/Post
        [HttpPost("Post")]
        public async Task<IActionResult>PostProduct([Bind("BrandName, RfSize")] ProductDTO product)
        {
            HttpWebRequest request = WebRequest.Create(HTTPRequestApi) as HttpWebRequest;

            string json = JsonConvert.SerializeObject(product);

            request.Method = "Post";

            byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

            request.ContentType = "application/json";

            request.ContentLength = body.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(body, 0, body.Length);
            }
            WebResponse response = await request.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {

                using (StreamReader reader = new StreamReader(stream))
                {
                    
                    string responseJson = reader.ReadToEnd();
                    IsAllowedSizeExistDTO allowedsizeExist = JsonConvert.DeserializeObject<IsAllowedSizeExistDTO>(responseJson);
                    if (allowedsizeExist.IsSizeExist)
                    {
                        
                        Product prod = new Product { ProductBrandName = product.BrandName, ProductRfSize = product.RfSize, IsDeleted = false };
                        _context.Products.Add(prod);
                        await _context.SaveChangesAsync();
                        return Ok(new { mes = "Запись нового товара прошла успешно" });
                       
                    }
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new { mes = "Данный размер отсутствует у бренда"});
        
        }
    }
}
