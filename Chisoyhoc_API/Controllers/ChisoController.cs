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

        [HttpGet]
        [Route("api/chisoyhoc/congthuc/{idchiso}/{input}")]
        public string congthuc(string idchiso, string input)
        {
            string kq = idchiso + ": ";
            input = input.Replace("-", ".");
            string[] inputs = input.Split('_');
            switch (idchiso.ToString())
            {
                case "IBW":
                    {
                        IBW IBWCal = new IBW(inputs[0], double.Parse(inputs[1]));
                        kq = kq + Math.Round(IBWCal.kqIBW(),2).ToString();
                        
                        break;
                    }
                case "AdjBW":
                    {
                        AdjBW AdjBWCal = new AdjBW(inputs[0], double.Parse(inputs[1]), double.Parse(inputs[2]));
                        kq = kq + Math.Round(AdjBWCal.kqAdjBW(),2).ToString();
                        break;
                    }
                case "BMI":
                    {
                        BMI BMICal = new BMI(double.Parse(inputs[0]), double.Parse(inputs[1]));
                        kq = kq + Math.Round(BMICal.kqBMI(),2).ToString();
                        break;
                    }
            }
            return kq;
        }
    }
}
