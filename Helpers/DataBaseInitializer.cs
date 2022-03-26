using BrandsHTTPService.EntityModels.AuthentificationModels;
using ProductsHTTPService.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsHTTPService.Helpers
{
    public class DataBaseInitializer
    {
        public static void UserInitialize(UserContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return;
            }
            var user = new User { UserId = 1, UserEmail = "lenja-mur95@yandex.ru", UserPassword = "87654321" };
            context.Users.Add(user);
            context.SaveChanges();
        }
        public static void ProductsInitialize(ProductsContext context)
        {
            context.Database.EnsureCreated();
            if (context.Products.Any())
            {
                return;
            }
            List<Product> products = new List<Product> 
            {
                new Product { ProductId = 1, ProductBrandName = "Nike", ProductRfSize = "1"},
                
                new Product { ProductId = 3, ProductBrandName = "Adidas", ProductRfSize = "3"},
                
                new Product { ProductId = 5, ProductBrandName = "Puma", ProductRfSize = "6"}
                              
            };

            foreach(var product in products)
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

            context.SaveChanges();
        }
       
    }
}
