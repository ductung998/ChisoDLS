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
        [Route("api/chisoyhoc/DSchiso")]
        public List<DSchisoyhoc> DSchiso()
        {
            KetnoiDB db = new KetnoiDB();
            return db.GetDSchisoyhoc();
        }
        [HttpGet]
        [Route("api/chisoyhoc/DSidbien/{machiso}")]
        public List<int> DSidbien(string machiso)
        {
            KetnoiDB db = new KetnoiDB();
            return db.GetDSIDbien(machiso);
        }
        [HttpGet]
        [Route("api/chisoyhoc/DSbien/{machiso}")]
        public List<DSBienCSYH> DSbien(string machiso)
        {
            KetnoiDB db = new KetnoiDB();
            List<Bien> DSbien = db.GetDSbien(machiso);
            List<DSBienCSYH> kq = new List<DSBienCSYH>();
            foreach (Bien i in DSbien)
            {
                kq.Add(new DSBienCSYH(i.idbien, i.tenbien, i.tendaydu, i.idloaibien, i.idbiengoc));
            }

            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/GetbienLT/{idbien}")]
        public BienLT_CSYH GetbienLT(int idbien)
        {
            KetnoiDB db = new KetnoiDB();
            BienLT bienLT = db.GetbienLT(idbien);
            BienLT_CSYH kq = new BienLT_CSYH(bienLT);
            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/GetbienDT/{idbien}")]
        public BienDT_CSYH GetbienDT(int idbien)
        {
            KetnoiDB db = new KetnoiDB();
            BienDT bienDT = db.GetbienDT(idbien);
            BienDT_CSYH kq = new BienDT_CSYH(bienDT);
            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/DatabienDT/{idbien}")]
        public List<GiatribienDT> DSbienDT(int idbien)
        {
            KetnoiDB db = new KetnoiDB();
            List<GiatribienDT> kq = db.GetGTbienDT(idbien);
            return kq;
        }
    }
}
