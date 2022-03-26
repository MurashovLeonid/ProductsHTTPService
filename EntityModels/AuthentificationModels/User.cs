using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrandsHTTPService.EntityModels.AuthentificationModels
{
    public class User
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }
}
