using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Chisoyhoc_API.Models;
using System.Text.RegularExpressions;


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

        [HttpGet]
        [Route("api/chisoyhoc/congthuc/{machiso}/{input}")]
        public List<string> congthuc(string machiso, string input)
        {
            KetnoiDB db = new KetnoiDB();

            List<string> kq = new List<string>();

            kq.Add(machiso);
            kq.Add(db.GetTenchiso(machiso));

            //Thay - thanh . (5-0 to 5.0)
            input = input.Replace("-", ".");

            db.Xulycongthuc(machiso, input);
            
            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/danhsachchiso")]
        public List<Dataclass.DSchisoyhoc> laychiso()
        {
            KetnoiDB db = new KetnoiDB();
            return db.GetDSchisoyhoc();
        }
        [HttpGet]
        [Route("api/chisoyhoc/danhsachbien/{machiso}")]
        public List<Dataclass.DSbienCSYH> danhsachbien(string machiso)
        {
            KetnoiDB db = new KetnoiDB();
            return db.GetDSbien(machiso);
        }
    }
}
