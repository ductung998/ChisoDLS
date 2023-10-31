using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chisoyhoc_API
{
    #region Users
    public class Users
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
        public bool IsDead { get; set; }
    }
    #endregion

    #region Chỉ số y học parent
    public class Chisoyhoc
    {
        protected int IDChiso { get; set; }
        protected string Tenchiso { get; set; }

        public Chisoyhoc()
        {

        }

        public Chisoyhoc(int _IDChiso, string _Tenchiso)
        {
            IDChiso = _IDChiso;
            Tenchiso = _Tenchiso;
        }
    }


    public class Congthuc : Chisoyhoc
    {
        public Congthuc()
        {

        }
    }

    public class Thangdiem : Chisoyhoc
    {
        public Thangdiem()
        {

        }
    }
    #endregion

    #region Chỉ số y học chi tiết - Công thức
    public class IBW : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public IBW()
        {

        }
        public IBW(string _gioitinh, double _chieucao)
        {
            gioitinh = _gioitinh;
            chieucao = _chieucao;
        }
        public double kqIBW()
        {
            double ibwkq;
            if (gioitinh == "Nam")
            {
                ibwkq = 50 + (0.91 * (chieucao - 152.4));
            }
            else
            {
                ibwkq = 45.4 + (0.91 * (chieucao - 152.4));
            }
            return ibwkq;
        }
    }


    #endregion
}