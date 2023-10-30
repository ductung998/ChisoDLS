using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chisoyhoc_API.Controllers
{
    public class ChisoController : ApiController
    {
        [HttpGet]
        [Route("api/dangnhap/{account}/{password}")]
        public List<string> dangnhap(string account, string password)
        {
            List<string> kq = new List<string> { account, password };
            return kq;
        }

        [HttpGet]
        [Route("api/dangxuat/{account}/{password}")]
        public List<string> dangxuat(string account, string password)
        {
            List<string> kq = new List<string> { account, password };
            return kq;

        }
    }
}
