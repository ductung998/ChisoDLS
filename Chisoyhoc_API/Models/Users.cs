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
    }
    #endregion

    #region Chỉ số y học
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

    public class Thangdiem : Chisoyhoc
    {
        public Thangdiem()
        {

        }

        public Thangdiem(List<bien> _bien)
        {

        }

        protected class bien
        {
            public bien()
            {

            }

            public bien(string _tenbien, List<string> _giatri)
            {

            }

            protected class giatribien
            {
                public giatribien(string _giatribien)
                {

                }
            }
        }
    }

    #endregion
}