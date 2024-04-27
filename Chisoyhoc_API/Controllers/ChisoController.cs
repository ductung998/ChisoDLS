using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassChung;
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

            kq.AddRange(db.Xulycongthuc(machiso, input));
            
            return kq;
        }

        [HttpGet]
        [Route("api/chisoyhoc/thangdiem/{machiso}/{input}")]
        public List<string> thangdiem(string machiso, string input)
        {
            KetnoiDB db = new KetnoiDB();

            List<string> kq = new List<string>();

            kq.Add(machiso);
            kq.Add(db.GetTenchiso(machiso));

            //Thay - thanh . (5-0 to 5.0)
            input = input.Replace("-", ".");

            kq.AddRange(db.Xulycongthuc(machiso, input));

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
            List<DSBienCSYH> kq = db.GetDSbienCSYH(DSbien);
            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/NCKH/DSbienGop/{input}")]
        public List<DSBienCSYH> DSbienGop(string input)
        {
            KetnoiDB db = new KetnoiDB();
            List<Bien> DSbien = db.GetDSBienGop(input);
            List<DSBienCSYH> kq = db.GetDSbienCSYH(DSbien);
            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/NCKH/DS_KQ_NCKH/{input}")]
        public List<string> DS_KQ_NCKH(string input)
        {
            KetnoiDB db = new KetnoiDB();
            List<string> kq = db.GetDSKQNCKH(input);
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
            List<GiatribienDT> kq = db.GetGiatribienDT(idbien);
            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/CSYHtheoBien/{input}")]
        public List<string> DSbienDT(string input)
        {
            KetnoiDB db = new KetnoiDB();
            List<string> kq = db.GetDSCSYHtheoIDBien(input);
            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/soluongbien/{machiso}")]
        public int soluongbien(string machiso)
        {
            KetnoiDB db = new KetnoiDB();
            int kq = db.GetDSbien(machiso).Count();
            return kq;
        }
        [HttpGet]
        [Route("api/chisoyhoc/soluonggiatri/{machiso}")]
        public List<int> soluongGTbienDT(string machiso)
        {
            KetnoiDB db = new KetnoiDB();
            List<int> kq = db.GetDSsoluongGT(machiso);

            return kq;
        }
    }
}
