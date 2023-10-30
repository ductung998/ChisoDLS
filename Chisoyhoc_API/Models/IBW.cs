using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chisoyhoc_API.Models
{
    public class IBW
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public IBW()
        {

        }
        public IBW(string _gioitinh, int _chieucao)
        {
            gioitinh = _gioitinh;
            chieucao = _chieucao;
        }
        public double kqIBW()
        {
            if (gioitinh == "nam")
            {
                double ibwnam = 50 + (0.91 * (chieucao - 152.4));
                return ibwnam;
            }
            else
            {
                double ibwnu = 45.4 + (0.91 * (chieucao - 152.4));
                return ibwnu;
            }
        }
    }
}