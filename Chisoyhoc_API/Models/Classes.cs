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
    #region Nguoi benh
    public class Nguoibenh
    {
        public string IDNB { get; set; }
        public string hoten { get; set; }
        public string gioitinh { get; set; }
        public DateTime ngaysinh { get; set; }
        public double tuoi { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public int nhiptim { get; set; }
        public double thannhiet { get; set; }
        public int HATThu { get; set; }
        public int HATTruong { get; set; }
        public bool hutthuoc { get; set; }
        public bool THA { get; set; }
        public bool DTD { get; set; }
        public bool suytim { get; set; }
        public bool ungthu { get; set; }
        public bool NMCT { get; set; }
        public bool dotquytim { get; set; }
        public bool thieumaunao { get; set; }
        public List<Xetnghiem> Xetnghiem { get; set; }
        public Nguoibenh()
        {

        }
        public Nguoibenh(string _idnb, string _hoten, string _gioitinh, DateTime _ngaysinh, double _chieucao, double _cannang,
                     int _nhiptim, double _thannhiet, int _hatThu, int _hatTruong, bool _hutthuoc, bool _tha, bool _dtd, bool _suytim,
                     bool _ungthu, bool _nmct, bool _dotquytim, bool _thieumaunao)
        {
            IDNB = _idnb;
            hoten = _hoten;
            gioitinh = _gioitinh.ToLower();
            ngaysinh = _ngaysinh;
            tinhtuoi();
            chieucao = _chieucao;
            cannang = _cannang;
            nhiptim = _nhiptim;
            thannhiet = _thannhiet;
            HATThu = _hatThu;
            HATTruong = _hatTruong;
            hutthuoc = _hutthuoc;
            THA = _tha;
            DTD = _dtd;
            suytim = _suytim;
            ungthu = _ungthu;
            NMCT = _nmct;
            dotquytim = _dotquytim;
            thieumaunao = _thieumaunao;
        }
        public void tinhtuoi()
        {
            DateTime currentDate = DateTime.Now;
            int tuoi = currentDate.Year - ngaysinh.Year;

            if (currentDate.Month < ngaysinh.Month || (currentDate.Month == ngaysinh.Month && currentDate.Day < ngaysinh.Day))
            {
                tuoi--;
            }
        }
    }
    #endregion
    #region Xet nghiem
    public class Xetnghiem
    {
        public string IDXN { get; set; }
        public double creatininSerum { get; set; }
        public double creatininUrine { get; set; }
        public double AST { get; set; }
        public double ALT { get; set; }
        public double BUN { get; set; }
        public double bilirubin { get; set; }
        public double totalCholesterol { get; set; }
        public double triglyceride { get; set; }
        public double LDL { get; set; }
        public double HDL { get; set; }
        public double RBC { get; set; }
        public double Hb { get; set; }
        public double Hct { get; set; }
        public double platelet { get; set; }
        public double WBC { get; set; }
        public double WBC_EOS { get; set; }
        public double WBC_BAS { get; set; }
        public double WBC_NEU { get; set; }
        public double WBC_MONO { get; set; }
        public double WBC_LYMPHO { get; set; }
        public double natriSerum { get; set; }
        public double kaliSerum { get; set; }
        public double calciSerum { get; set; }
        public double cloSerum { get; set; }
        public double HCO3Serum { get; set; }
        public double pHSerum { get; set; }
        public Xetnghiem()
        {

        }
        public Xetnghiem(string _idxn, double _creatininSerum, double _creatininUrine, double _ast, double _alt,
                     double _bun, double _bilirubin, double _totalCholesterol, double _triglyceride, double _ldl,
                     double _hdl, double _rbc, double _hb, double _hct, double _platelet, double _wbc,
                     double _wbcEos, double _wbcBas, double _wbcNeu, double _wbcMono, double _wbcLympho,
                     double _natriSerum, double _kaliSerum, double _canxiSerum, double _cloSerum,
                     double _hco3Serum, double _phSerum)
        {
            IDXN = _idxn;
            creatininSerum = _creatininSerum;
            creatininUrine = _creatininUrine;
            AST = _ast;
            ALT = _alt;
            BUN = _bun;
            bilirubin = _bilirubin;
            totalCholesterol = _totalCholesterol;
            triglyceride = _triglyceride;
            LDL = _ldl;
            HDL = _hdl;
            RBC = _rbc;
            Hb = _hb;
            Hct = _hct;
            platelet = _platelet;
            WBC = _wbc;
            WBC_EOS = _wbcEos;
            WBC_BAS = _wbcBas;
            WBC_NEU = _wbcNeu;
            WBC_MONO = _wbcMono;
            WBC_LYMPHO = _wbcLympho;
            natriSerum = _natriSerum;
            kaliSerum = _kaliSerum;
            calciSerum = _canxiSerum;
            cloSerum = _cloSerum;
            HCO3Serum = _hco3Serum;
            pHSerum = _phSerum;
        }
    }
    #endregion
    #region Thang diem pho bien
    public class Thangdiemphobien
    {
        public string ID_Thangdiemphobien { get; set; }
        public int GlascowComa { get; set; }
        public int ECOG { get; set; }
        public Thangdiemphobien()
        {

        }
        public Thangdiemphobien(string _id_Thangdiemphobien, int _glascowComa, int _ecog)
        {
            ID_Thangdiemphobien = _id_Thangdiemphobien;
            GlascowComa = _glascowComa;
            ECOG = _ecog;
        }
    }
    #endregion
    #region Chỉ số y học parent
    public class Chisoyhoc
    {       
        public string IDChiso { get; set; }
        public string Tenchiso { get; set; }
        public string mucdich { get; set; }
        public string ungdung { get; set; }
        public string phuongphap { get; set; }
        public string diengiai { get; set; }
        public string ghichu { get; set; }
        public string TLTK { get; set; }
        public Chisoyhoc()
        {

        }

        public Chisoyhoc(string _IDChiso, string _Tenchiso, string _Mucdich, string _Ungdung, string _Phuongphap,
            string _Diengiai, string _Ghichu, string _TLTK)
        {
            IDChiso = _IDChiso;
            Tenchiso = _Tenchiso;
            mucdich = _Mucdich;
            ungdung = _Ungdung;
            phuongphap = _Phuongphap;
            diengiai = _Diengiai;
            ghichu = _Ghichu;
            TLTK = _TLTK;
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
            gioitinh = _gioitinh.ToLower();
            chieucao = _chieucao;
        }
        public IBW(Nguoibenh NB)
        {
            gioitinh = NB.gioitinh;
            chieucao = NB.chieucao;
        }
        public double kqIBW()
        {
            double ibwkq;
            if (gioitinh == "nam")
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
    public class AdjBW : Congthuc
    {
        public string gioitinh { get; set; }
        public double cannang { get; set; }
        public double chieucao { get; set; }
        public AdjBW()
        {

        }
        public AdjBW(string _gioitinh, double _chieucao, double _cannang)
        {
            gioitinh = _gioitinh.ToLower();
            chieucao = _chieucao;
            cannang = _cannang;
        }
        public AdjBW(Nguoibenh NB)
        {
            gioitinh = NB.gioitinh;
            chieucao = NB.chieucao;
            cannang = NB.cannang;
        }
        public double kqAdjBW()
        {
            double kqAdjBW;
            IBW IBW_ob = new IBW(gioitinh, chieucao);
            double IBW = IBW_ob.kqIBW();
            kqAdjBW = IBW + 0.4 * (cannang - IBW);
            return kqAdjBW;
        }
    }

    public class BMI : Congthuc
    {
        public double cannang { get; set; }
        public double chieucao { get; set; }

        public BMI()
        {

        }

        public BMI(double _chieucao, double _cannang)
        {
            chieucao = _chieucao;
            cannang = _cannang;
        }

        public BMI(Nguoibenh NB)
        {
            chieucao = NB.chieucao;
            cannang = NB.cannang;
        }

        public double kqBMI()
        {
            double kqBMI = cannang / (chieucao * chieucao / 10000);
            return kqBMI;
        }
    }
    public class AaG : Congthuc
    {
        public double FiO2 { get; set; }
        public double docaouoctinh { get; set; }
        public double thannhiet { get; set; }
        public double pCO2 { get; set; }
        public double Hesohohap { get; set; }
        public double tuoi { get; set; }
        public double PaO2 { get; set; }

        public AaG()
        {

        }
        public AaG(Nguoibenh NB)
        {
            tuoi = NB.tuoi;
            thannhiet = NB.thannhiet;
            FiO2 = 0;
            docaouoctinh = 0;
            pCO2 = 0;
            Hesohohap = 0;
            PaO2 = 0;
        }

        public AaG(double _FiO2, double _docaouoctinh, double _thannhiet, double _pCO2, double _Hesohohap, double _tuoi, double _PaO2)
        {
            FiO2 = _FiO2;
            docaouoctinh = _docaouoctinh;
            thannhiet = _thannhiet;
            pCO2 = _pCO2;
            Hesohohap = _Hesohohap;
            tuoi = _tuoi;
            PaO2 = _PaO2;
        }

        public double CalculateAaG()
        {
            double pKhiquyen = 760 * Math.Exp(docaouoctinh);
            double pH2O = 47 * Math.Exp((thannhiet - 37) / 18.4);
            double AaG = FiO2 * (pKhiquyen - pH2O) - (pCO2 / Hesohohap) + pCO2 * FiO2 * (1 - Hesohohap) / Hesohohap - PaO2;
            return AaG;
        }

        public double CalculateAaGnormal()
        {
            double AaGnormal = 2.5 + (0.21 * tuoi);
            return AaGnormal;
        }
    }
    public class CalciSerum_Adj : Congthuc
    {
        public double albuminSerumNorm { get; set; }
        public double albuminSerum { get; set; }
        public double calciSerum { get; set; }

        public CalciSerum_Adj()
        {

        }
        public CalciSerum_Adj(Xetnghiem XN)
        {
            calciSerum = XN.calciSerum;
            albuminSerumNorm = 4;
            albuminSerum = 0;
        }

        public CalciSerum_Adj(double _normAlbumin, double _albuminSerum, double _calciSerum)
        {
            albuminSerumNorm = _normAlbumin;
            albuminSerum = _albuminSerum;
            calciSerum = _calciSerum;
        }

        public double kqCalciSerum_Adj()
        {
            return 0.8 * (albuminSerumNorm - albuminSerum) + calciSerum;
        }
    }
    public class BSA : Congthuc
    {
        public double chieucao { get; set; }
        public double cannang { get; set; }

        public BSA()
        {

        }
        public BSA(Nguoibenh NB)
        {
            chieucao = NB.chieucao;
            cannang = NB.cannang;
        }

        public BSA(double _chieucao, double _cannang)
        {
            chieucao = _chieucao;
            cannang = _cannang;
        }

        public double kqBSA_Mos()
        {
            return Math.Sqrt(chieucao * cannang) / 3600;
        }
        public double kqBSA_Dub()
        {
            return 0.007184 * Math.Pow(chieucao, 0.725) * Math.Pow(cannang, 0.425);
        }
    }
    public class eGFR_CKD : Congthuc
    {
        public string gioitinh { get; set; }
        public double CreatininSerum { get; set; }
        public double tuoi { get; set; }
        public double hesogioitinh_CKD { get; set; }
        public double alpha_CKD { get; set; }
        public double kappa_CKD { get; set; }
        public double eGFR { get; set; }

        public eGFR_CKD()
        {

        }
        public eGFR_CKD(Nguoibenh NB, Xetnghiem XN)
        {
            gioitinh = NB.gioitinh;
            CreatininSerum = XN.creatininSerum;
            tuoi = NB.tuoi;
            SetCoefficients();
        }

        public eGFR_CKD(string _gioitinh, double _CreatininSerum, double _tuoi)
        {
            gioitinh = _gioitinh.ToLower();
            CreatininSerum = _CreatininSerum;
            tuoi = _tuoi;
            SetCoefficients();
        }

        private void SetCoefficients()
        {
            hesogioitinh_CKD = (gioitinh == "nam") ? 1.0 : 1.012;
            alpha_CKD = (gioitinh == "nam") ? -0.302 : -0.241;
            kappa_CKD = (gioitinh == "nam") ? 0.9 : 0.7;
        }

        public double kqeGFR_CKD()
        {
            double minTerm = Math.Min(CreatininSerum / kappa_CKD, 1.0);
            double maxTerm = Math.Max(CreatininSerum / kappa_CKD, 1.0);

            eGFR = 142 * Math.Pow(minTerm, alpha_CKD) * Math.Pow(maxTerm, -1.2) * Math.Pow(0.9938, tuoi) * hesogioitinh_CKD;
            return eGFR;
        }
    }
    public class eGFR_MDRD : Congthuc
    {
        public double CreatininSerum { get; set; }
        public double tuoi { get; set; }
        public string chungtoc { get; set; }
        public string gioitinh { get; set; }
        public double eGFR { get; set; }

        public eGFR_MDRD()
        {

        }
        public eGFR_MDRD(Nguoibenh NB, Xetnghiem XN)
        {
            CreatininSerum = XN.creatininSerum;
            tuoi = NB.tuoi;
            gioitinh = NB.gioitinh;
            chungtoc = "người châu á";
        }

        public eGFR_MDRD(double _CreatininSerum, double _tuoi, string _chungtoc, string _gioitinh)
        {
            CreatininSerum = _CreatininSerum;
            tuoi = _tuoi;
            chungtoc = _chungtoc.ToLower();
            gioitinh = _gioitinh.ToLower();
        }

        public double kqeGFR_MDRD()
        {
            double chungtocCoefficient = (chungtoc == "người da đen") ? 1.212 : 1.0;
            double gioitinhCoefficient = (gioitinh == "nam") ? 1.0 : 0.742;

            eGFR = 175 * Math.Pow(CreatininSerum, -1.154) * Math.Pow(tuoi, -0.203) * chungtocCoefficient * gioitinhCoefficient;
            return eGFR;
        }
    }
    public class eCrCl : Congthuc
    {
        public double tuoi { get; set; }
        public double cannang { get; set; }
        public double CreatininSerum { get; set; }
        public string gioitinh { get; set; }

        public eCrCl()
        {

        }
        public eCrCl(Nguoibenh NB, Xetnghiem XN)
        {
            tuoi = NB.tuoi;
            cannang = NB.cannang;
            CreatininSerum = XN.creatininSerum;
            gioitinh = NB.gioitinh;
        }

        public eCrCl(double _tuoi, double _cannang, double _CreatininSerum, string _gioitinh)
        {
            tuoi = _tuoi;
            cannang = _cannang;
            CreatininSerum = _CreatininSerum;
            gioitinh = _gioitinh.ToLower();
        }

        public double kqeCrCl()
        {
            double gioitinhCoefficient = (gioitinh == "nam") ? 1.0 : 0.85;
            double kq = (140 - tuoi) * cannang / (72 * CreatininSerum) * gioitinhCoefficient;
            return kq;
        }
    }
    public class KtVDaugirdas : Congthuc
    {
        public double BUNsauloc { get; set; }
        public double BUNtruocloc { get; set; }
        public double tglocmau { get; set; }
        public double Vlocmau { get; set; }
        public double cannangsaulocmau { get; set; }
        public double KtVDaugirdasResult { get; set; }

        public KtVDaugirdas()
        {

        }

        public KtVDaugirdas(double _BUNsauloc, double _BUNtruocloc, double _tglocmau, double _Vlocmau, double _cannangsaulocmau)
        {
            BUNsauloc = _BUNsauloc;
            BUNtruocloc = _BUNtruocloc;
            tglocmau = _tglocmau;
            Vlocmau = _Vlocmau;
            cannangsaulocmau = _cannangsaulocmau;
        }

        public double kqKtVDaugirdas()
        {
            KtVDaugirdasResult = -Math.Log((BUNsauloc / BUNtruocloc) - (0.008 * tglocmau)) + ((4 - (3.5 * BUNsauloc / BUNtruocloc)) * Vlocmau / cannangsaulocmau);
            return KtVDaugirdasResult;
        }
    }
    public class RRF_Kru : Congthuc
    {
        public double VUrineRRF { get; set; }
        public double UreUrine { get; set; }
        public double IntervalRRF { get; set; }
        public double BUN1RRF { get; set; }
        public double BUN2RRF { get; set; }
        public double RRF_KruResult { get; set; }

        public RRF_Kru()
        {

        }

        public RRF_Kru(double _VUrineRRF, double _UreUrine, double _IntervalRRF, double _BUN1RRF, double _BUN2RRF)
        {
            VUrineRRF = _VUrineRRF;
            UreUrine = _UreUrine;
            IntervalRRF = _IntervalRRF;
            BUN1RRF = _BUN1RRF;
            BUN2RRF = _BUN2RRF;
        }

        public double kqRRF_Kru()
        {
            RRF_KruResult = VUrineRRF * UreUrine / IntervalRRF / ((BUN1RRF + BUN2RRF) / 2);
            return RRF_KruResult;
        }
    }
    public class eAER : Congthuc
    {
        public double AlbuminUrine { get; set; }
        public double CreatininUrine { get; set; }
        public string gioitinh { get; set; }
        public string chungtoc { get; set; }
        public double tuoi { get; set; }
        public double eAERResult { get; set; }

        public eAER()
        {

        }
        public eAER(Nguoibenh NB, Xetnghiem XN)
        {
            AlbuminUrine = 0;
            CreatininUrine = XN.creatininUrine;
            gioitinh = NB.gioitinh;
            chungtoc = "người châu á";
            tuoi = NB.tuoi;
        }
        public eAER(double _AlbuminUrine, double _CreatininUrine, string _gioitinh, string _chungtoc, double _tuoi)
        {
            AlbuminUrine = _AlbuminUrine;
            CreatininUrine = _CreatininUrine;
            gioitinh = _gioitinh.ToLower();
            chungtoc = _chungtoc.ToLower().ToLower();
            tuoi = _tuoi;
        }

        public double kqeAER()
        {
            double baseValue;

            if (gioitinh == "nam")
            {
                if (chungtoc == "người da đen")
                {
                    baseValue = 1413.9 + (23.2 * tuoi) - (0.3 * tuoi * tuoi);
                }
                else
                {
                    baseValue = 1307.3 + (23.1 * tuoi) - (0.3 * tuoi * tuoi);
                }
            }
            else
            {
                if (chungtoc == "người da đen")
                {
                    baseValue = 1148.6 + (15.6 * tuoi) - (0.3 * tuoi * tuoi);
                }
                else
                {
                    baseValue = 1051.3 + (5.3 * tuoi) - (0.1 * tuoi * tuoi);
                }
            }

            eAERResult = AlbuminUrine / CreatininUrine * baseValue;
            return eAERResult;
        }
    }
    public class eGFR_Schwartz : Congthuc
    {
        public string loaiXNcreatinin { get; set; }
        public bool benhthanman { get; set; }
        public double tuoi { get; set; }
        public bool sinhnon { get; set; }
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double CreatininSerum { get; set; }
        public double eGFR_SchwartzResult { get; set; }

        public eGFR_Schwartz()
        {

        }
        public eGFR_Schwartz(Nguoibenh NB, Xetnghiem XN)
        {
            loaiXNcreatinin = "jaffe";
            benhthanman = false;
            tuoi = NB.tuoi;
            sinhnon = false;
            gioitinh = NB.gioitinh;
            chieucao = NB.chieucao;
            CreatininSerum = XN.creatininSerum;
        }
        public eGFR_Schwartz(string _loaiXNcreatinin, bool _benhthanman, double _tuoi, bool _sinhnon, string _gioitinh, double _chieucao, double _CreatininSerum)
        {
            loaiXNcreatinin = _loaiXNcreatinin.ToLower();
            benhthanman = _benhthanman;
            tuoi = _tuoi;
            sinhnon = _sinhnon;
            gioitinh = _gioitinh.ToLower();
            chieucao = _chieucao;
            CreatininSerum = _CreatininSerum;
        }

        public double kqeGFR_Schwartz()
        {
            double factor;

            if (loaiXNcreatinin == "jaffe" && benhthanman == true)
            {
                factor = 0.413;
            }
            else if (tuoi < 1)
            {
                factor = (sinhnon == true) ? 0.33 : 0.45;
            }
            else if (tuoi < 13)
            {
                factor = 0.55;
            }
            else
            {
                factor = (gioitinh == "nam") ? 0.7 : 0.55;
            }

            eGFR_SchwartzResult = factor * chieucao / CreatininSerum;
            return eGFR_SchwartzResult;
        }
    }
    public class NatriSerum_Adj : Congthuc
    {
        public double NatriSerum { get; set; }
        public double GlucoseSerum { get; set; }
        public double NatriSerum_AdjResult { get; set; }

        public NatriSerum_Adj()
        {

        }
        public NatriSerum_Adj(Xetnghiem XN)
        {
            NatriSerum = XN.natriSerum;
            GlucoseSerum = 0;
        }

        public NatriSerum_Adj(double _NatriSerum, double _GlucoseSerum)
        {
            NatriSerum = _NatriSerum;
            GlucoseSerum = _GlucoseSerum;
        }

        public double kqNatriSerum_Adj()
        {
            NatriSerum_AdjResult = NatriSerum + (2 * (GlucoseSerum - 100) / 100);
            return NatriSerum_AdjResult;
        }
    }
    public class CardiacOutput : Congthuc
    {
        public double oxytieuthu { get; set; }
        public double Hb { get; set; }
        public double O2Sat { get; set; }
        public double PaO2 { get; set; }
        public double O2vSat { get; set; }
        public double PvO2 { get; set; }
        public double CardiacOutputResult { get; set; }

        public CardiacOutput()
        {

        }
        public CardiacOutput(Xetnghiem XN)
        {
            oxytieuthu = 0;
            Hb = XN.Hb;
            O2Sat = 0;
            PaO2 = 0;
            O2vSat = 0;
            PvO2 = 0;
        }

        public CardiacOutput(double _oxytieuthu, double _Hb, double _O2Sat, double _PaO2, double _O2vSat, double _PvO2)
        {
            oxytieuthu = _oxytieuthu;
            Hb = _Hb;
            O2Sat = _O2Sat;
            PaO2 = _PaO2;
            O2vSat = _O2vSat;
            PvO2 = _PvO2;
        }

        public double kqCardiacOutput()
        {
            CardiacOutputResult = oxytieuthu / (((Hb * 13.4 * O2Sat / 100) + (PaO2 * 0.031)) - (Hb * 13.4 * O2vSat / 100) + (PvO2 * 0.031));
            return CardiacOutputResult;
        }
    }
    public class LDL : Congthuc
    {
        public double TotalCholesterol { get; set; }
        public double Triglycerid { get; set; }
        public double HDL { get; set; }
        public double LDLResult { get; set; }

        public LDL()
        {

        }
        public LDL(Xetnghiem XN)
        {
            TotalCholesterol = XN.totalCholesterol;
            Triglycerid = XN.triglyceride;
            HDL = XN.HDL;
        }

        public LDL(double _TotalCholesterol, double _Triglycerid, double _HDL)
        {
            TotalCholesterol = _TotalCholesterol;
            Triglycerid = _Triglycerid;
            HDL = _HDL;
        }

        public double kqLDL()
        {
            LDLResult = TotalCholesterol - (Triglycerid / 5) - HDL;
            return LDLResult;
        }
    }
    public class FIB4 : Congthuc
    {
        public double tuoi { get; set; }
        public double AST { get; set; }
        public double tieucau { get; set; }
        public double ALT { get; set; }
        public double FIB4Result { get; set; }

        public FIB4()
        {

        }
        public FIB4(Nguoibenh NB, Xetnghiem XN)
        {
            tuoi = NB.tuoi;
            AST = XN.AST;
            tieucau = XN.platelet;
            ALT = XN.ALT;
        }
        public FIB4(double _tuoi, double _AST, double _tieucau, double _ALT)
        {
            tuoi = _tuoi;
            AST = _AST;
            tieucau = _tieucau;
            ALT = _ALT;
        }

        public double kqFIB4()
        {
            FIB4Result = tuoi * AST / (0.001 * tieucau * Math.Sqrt(ALT));
            return FIB4Result;
        }
    }
    public class APRI : Congthuc
    {
        public double AST { get; set; }
        public double ASTNormUL { get; set; }
        public double tieucau { get; set; }
        public double APRIResult { get; set; }

        public APRI()
        {

        }
        public APRI(Xetnghiem XN)
        {
            AST = XN.AST;
            ASTNormUL = 0;
            tieucau = XN.platelet;
        }
        public APRI(double _AST, double _ASTNormUL, double _tieucau)
        {
            AST = _AST;
            ASTNormUL = _ASTNormUL;
            tieucau = _tieucau;
        }

        public double kqAPRI()
        {
            APRIResult = 100 * ((AST / ASTNormUL) / (tieucau / 1000));
            return APRIResult;
        }
    }
    public class MELD : Congthuc
    {
        public double tansuatlocmau1tuan { get; set; }
        public double thoigianlocmau1tuan { get; set; }
        public double CreatininSerum { get; set; }
        public double BilirubinSerum { get; set; }
        public double INR { get; set; }
        public double MELDResult { get; set; }

        public MELD()
        {

        }
        public MELD(Xetnghiem XN)
        {
            tansuatlocmau1tuan = 0;
            thoigianlocmau1tuan = 0;
            CreatininSerum = XN.creatininSerum;
            BilirubinSerum = XN.bilirubin;
            INR = 0;
        }
        public MELD(double _tansuatlocmau1tuan, double _thoigianlocmau1tuan, double _CreatininSerum, double _BilirubinSerum, double _INR)
        {
            tansuatlocmau1tuan = _tansuatlocmau1tuan;
            thoigianlocmau1tuan = _thoigianlocmau1tuan;
            CreatininSerum = _CreatininSerum;
            BilirubinSerum = _BilirubinSerum;
            INR = _INR;
        }

        public double kqMELD()
        {
            double creatinineTerm = (tansuatlocmau1tuan >= 2 || thoigianlocmau1tuan >= 24) ? 4 : CreatininSerum;

            MELDResult = 9.57 * Math.Log(creatinineTerm) + 3.78 * Math.Log(BilirubinSerum) + 11.2 * Math.Log(INR) + 6.43;
            return MELDResult;
        }
    }
    public class MELDNa : Congthuc
    {
        public double tansuatlocmau1tuan { get; set; }
        public double thoigianlocmau1tuan { get; set; }
        public double CreatininSerum { get; set; }
        public double BilirubinSerum { get; set; }
        public double INR { get; set; }
        public double NatriSerum { get; set; }
        public double MELDNaResult { get; set; }

        public MELDNa()
        {

        }
        public MELDNa(Xetnghiem XN)
        {
            tansuatlocmau1tuan = 0;
            thoigianlocmau1tuan = 0;
            CreatininSerum = XN.creatininSerum;
            BilirubinSerum = XN.bilirubin;
            INR = 0;
            NatriSerum = XN.natriSerum;
        }
        public MELDNa(double _tansuatlocmau1tuan, double _thoigianlocmau1tuan, double _CreatininSerum, double _BilirubinSerum, double _INR, double _NatriSerum)
        {
            tansuatlocmau1tuan = _tansuatlocmau1tuan;
            thoigianlocmau1tuan = _thoigianlocmau1tuan;
            CreatininSerum = _CreatininSerum;
            BilirubinSerum = _BilirubinSerum;
            INR = _INR;
            NatriSerum = _NatriSerum;
        }

        public double kqMELDNa()
        {
            MELD meld = new MELD(tansuatlocmau1tuan, thoigianlocmau1tuan, CreatininSerum, BilirubinSerum, INR);
            double meldValue = meld.kqMELD();

            MELDNaResult = (meldValue <= 11) ? meldValue : meldValue + ((1.32 * (137 - NatriSerum)) - (0.033 * meldValue * (137 - NatriSerum)));
            return MELDNaResult;
        }
    }
    public class AdjECG : Congthuc
    {
        public double QT_ECG { get; set; }
        public double RR_ECG { get; set; }
        public double nhiptim { get; set; }

        public AdjECG()
        {

        }
        public AdjECG(Nguoibenh NB)
        {
            QT_ECG = 0;
            RR_ECG = 0;
            nhiptim = NB.nhiptim;
        }

        public AdjECG(double _QT_ECG, double _RR_ECG, double _nhiptim)
        {
            QT_ECG = _QT_ECG;
            RR_ECG = _RR_ECG;
            nhiptim = _nhiptim;
        }

        public double kqAdjQT_Bazett()
        {
            return QT_ECG / Math.Sqrt(RR_ECG / 1000);
        }

        public double kqAdjQT_Fridericia()
        {
            return QT_ECG / Math.Pow((RR_ECG / 1000), 1.0 / 3.0);
        }

        public double kqAdjQT_Framingham()
        {
            return QT_ECG + (0.154 * (1000 - RR_ECG));
        }

        public double kqAdjQT_Hodges()
        {
            return QT_ECG + (1.75 * (nhiptim - 60));
        }
    }
    public class WBCCFS_Adj : Congthuc
    {
        public double WBC_CFS { get; set; }
        public double RBC_CFS { get; set; }
        public double WBC { get; set; }
        public double RBC { get; set; }

        public double kqWBCCFS_Adj { get; private set; }

        public WBCCFS_Adj()
        {

        }
        public WBCCFS_Adj(Xetnghiem XN)
        {
            WBC_CFS = 0;
            RBC_CFS = 0;
            WBC = XN.WBC;
            RBC = XN.RBC;
        }
        public WBCCFS_Adj(double _WBC_CFS, double _RBC_CFS, double _WBC, double _RBC)
        {
            WBC_CFS = _WBC_CFS;
            RBC_CFS = _RBC_CFS;
            WBC = _WBC;
            RBC = _RBC;
        }
        public double GetkqWBCCFS_Adj()
        {
            kqWBCCFS_Adj = WBC_CFS - ((WBC * RBC_CFS) / (RBC * 1000000));
            return kqWBCCFS_Adj;
        }
    }
    #endregion
    #region Chỉ số y học chi tiết - Thang điểm
    public class SCORE2_DM : Thangdiem
    {
        public string gioitinh { get; set; }
        public int nhomtuoi { get; set; }
        public int nhomDM_Age { get; set; }
        public int nhomSmoking { get; set; }
        public int nhomHATT { get; set; }
        public int nhomTotalCholesterol { get; set; }
        public int nhomHDL { get; set; }
        public int nhomHbA1C { get; set; }
        public int nhomEGFR { get; set; }
        public int nhomvungnguyco { get; set; }
        public int[] diemNam { get; set; }
        public int[] diemNu { get; set; }
        public int[] nguycoNam { get; set; }
        public int[] nguycoNu { get; set; }
        public string[] PLnguycoNam { get; set; }
        public string[] PLnguycoNu { get; set; }
        public int diem_start_index { get; set; }
        public int nguyco_start_index { get; set; }
        public SCORE2_DM()
        {
            init_SCORE2_DM();
        }
        public SCORE2_DM(Nguoibenh NB, Xetnghiem XN)
        {
            init_SCORE2_DM();
            checktuoi(NB.tuoi);
            checkSmoking(NB.hutthuoc);
            checkHATT(NB.HATThu);
            checkTotalCholesterol(XN.totalCholesterol);
            checkHDL(XN.HDL);
            checkEGFR(NB.gioitinh, XN.creatininSerum, NB.tuoi);
        }
        public SCORE2_DM(string _gioitinh, double _tuoi, double _namDM, bool _smoking, double _HATT, double _TotalCholesterol,
            double _HDL, double _HbA1C, double _creatininSerum, string _vungnguyco)
        {
            init_SCORE2_DM();
            gioitinh = _gioitinh;
            checktuoi(_tuoi);
            double DM_Age = _tuoi - (DateTime.Now.Year - _namDM);
            checkDM_Age(DM_Age);
            checkSmoking(_smoking);
            checkHATT(_HATT);
            checkTotalCholesterol(_TotalCholesterol);
            checkHDL(_HDL);
            checkHbA1C(_HbA1C);
            checkEGFR(_gioitinh, _creatininSerum, _tuoi);
            checkvungnguyco(_vungnguyco);
        }
        private void init_SCORE2_DM()
        {
            int[] _diemNam = {3, 2, 1, 0, 0, 0, 0, 0, -9, -2, -1, 1, 3, 6, -4, -3, -1, 1, 3, 2, 0, -1, 1, 2, 4, 5, 7, 8, 4, 1, -1, 3, 2, 1, 0, 0, 0, 0, 0, -5, 2, -1, 1, 3, 5, -4, -2, -1, 1, 3, 1, 0, -1, 1, 2, 3, 5, 6, 7, 4, 1, -1, 3, 2, 1, 0, 0, 0, 0, 0, 0, 6, -1, 1, 3, 4, -3, -2, -1, 1, 2, 1, 0, -1, 0, 2, 3, 4, 5, 6, 3, 1, -1, 3, 2, 1, 0, 0, -1, 0, 0, 4, 9, -1, 1, 2, 4, -3, -2, -1, 1, 2, 1, 0, -1, 0, 2, 3, 4, 5, 6, 3, 1, 0, 3, 2, 1, 0, 0, -1, -2, 0, 9, 13, -1, 1, 2, 3, -3, -2, -1, 1, 2, 1, 0, -1, 0, 1, 2, 3, 4, 5, 3, 1, 0, 3, 2, 1, 0, 0, -1, -2, -3, 13, 17, 0, 0, 1, 2, -2, -1, 0, 0, 1, 1, 0, -1, 0, 1, 2, 3, 4, 4, 2, 1, 0};
            diemNam = _diemNam;
            int[] _diemNu = { 4, 3, 2, 0, 0, 0, 0, 0, -11, -1, -1, 1, 3, 5, -5, -3, -1, 1, 3, 2, 0, -2, 1, 3, 5, 7, 9, 9, 5, 2, -1, 4, 3, 2, 1, 0, 0, 0, 0, -6, 3, -1, 1, 3, 5, -4, -2, -1, 1, 3, 2, 0, -2, 1, 2, 4, 6, 8, 8, 5, 1, -1, 4, 3, 2, 1, -1, 0, 0, 0, 0, 8, -1, 1, 3, 4, -4, -2, -1, 1, 3, 2, 0, -2, 1, 2, 4, 5, 7, 7, 4, 1, -1, 4, 3, 2, 1, -1, -2, 0, 0, 5, 12, -1, 1, 2, 4, -3, -2, -1, 1, 2, 2, 0, -1, 1, 2, 3, 5, 6, 6, 3, 1, 0, 4, 3, 2, 1, -1, -2, -3, 0, 11, 16, -1, 1, 2, 3, -3, -2, -1, 1, 2, 2, 0, -1, 0, 2, 3, 4, 5, 5, 3, 1, 0, 4, 3, 2, 1, -1, -2, -3, -4, 16, 21, -1, 1, 2, 3, -2, -1, 0, 0, 1, 1, 0, -1, 0, 1, 2, 3, 4, 4, 2, 1, 0 };
            diemNu = _diemNu;
            //Nguy co nam: -15++ (tổng 49)
            int[] _nguycoNam = { 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 7, 7, 8, 8, 9, 10, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 22, 23, 25, 27, 28, 30, 32, 34, 36, 38, 41, 43, 45, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 7, 7, 8, 9, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 22, 24, 26, 28, 30, 32, 34, 36, 38, 41, 43, 46, 49, 51, 54, 57, 60, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 5, 5, 5, 6, 7, 7, 8, 9, 9, 10, 11, 12, 13, 15, 16, 17, 19, 21, 22, 24, 26, 28, 31, 33, 36, 39, 41, 44, 48, 51, 54, 57, 61, 64, 68, 71, 74, 78, 4, 4, 4, 5, 5, 5, 6, 6, 7, 7, 8, 9, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19, 20, 22, 23, 25, 27, 29, 31, 33, 35, 38, 40, 43, 45, 48, 51, 54, 57, 60, 63, 66, 69, 72, 75, 78, 80, 83, 85 };
            nguycoNam = _nguycoNam;
            //Nguy co nu: -14++ (tổng 53)
            int[] _nguycoNu = { 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 6, 6, 7, 7, 7, 8, 9, 9, 10, 10, 11, 12, 13, 14, 15, 15, 17, 18, 19, 20, 21, 23, 24, 26, 27, 29, 31, 1, 1, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 25, 27, 29, 31, 33, 35, 37, 39, 42, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 5, 5, 5, 6, 7, 7, 8, 9, 9, 10, 11, 12, 13, 15, 16, 17, 19, 20, 22, 24, 26, 28, 31, 33, 36, 38, 41, 44, 47, 50, 54, 57, 61, 64, 67, 71, 3, 4, 4, 4, 5, 5, 5, 6, 6, 7, 8, 8, 9, 10, 10, 11, 12, 13, 14, 15, 16, 18, 19, 21, 22, 24, 26, 28, 30, 32, 34, 36, 39, 41, 44, 47, 50, 52, 55, 58, 61, 65, 68, 71, 74, 76, 79, 82, 84 };
            nguycoNu = _nguycoNu;
            //PL Nguy co nam: -15++ (tổng 49)
            string[] _PLnguycoNam = {"T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC"};
            PLnguycoNam = _PLnguycoNam;
            //PL Nguy co nu: -14++ (tổng 53)
            string[] _PLnguycoNu = {"T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC"};
            PLnguycoNu = _PLnguycoNu;
        }
        private void checktuoi(double _tuoi)
        {
            //Nhom tuoi
            if (_tuoi < 45)
                nhomtuoi = 0;
            else if (_tuoi < 50)
                nhomtuoi = 1;
            else if (_tuoi < 55)
                nhomtuoi = 2;
            else if (_tuoi < 60)
                nhomtuoi = 3;
            else if (_tuoi < 65)
                nhomtuoi = 4;
            else
                nhomtuoi = 5;
        }
        private void checkDM_Age(double DM_Age)
        {
            //DM_age
            if (DM_Age < 35)
                nhomDM_Age = 0;
            else if (DM_Age < 40)
                nhomDM_Age = 1;
            else if (DM_Age < 45)
                nhomDM_Age = 2;
            else if (DM_Age < 50)
                nhomDM_Age = 3;
            else if (DM_Age < 55)
                nhomDM_Age = 4;
            else if (DM_Age < 60)
                nhomDM_Age = 5;
            else if (DM_Age < 65)
                nhomDM_Age = 6;
            else
                nhomDM_Age = 7;
        }
        private void checkSmoking(bool _smoking)
        {
            //Smoking
            if (_smoking)
                nhomSmoking = 1;
            else
                nhomSmoking = 0;
        }
        private void checkHATT(double _HATT)
        {
            //HATT
            if (_HATT < 120)
                nhomHATT = 0;
            else if (_HATT < 140)
                nhomHATT = 1;
            else if (_HATT < 160)
                nhomHATT = 2;
            else
                nhomHATT = 3;
        }
        private void checkTotalCholesterol(double _TotalCholesterol)
        {
            //TCho
            if (_TotalCholesterol < 4)
                nhomTotalCholesterol = 0;
            else if (_TotalCholesterol < 5)
                nhomTotalCholesterol = 1;
            else if (_TotalCholesterol < 6)
                nhomTotalCholesterol = 2;
            else if (_TotalCholesterol < 7)
                nhomTotalCholesterol = 3;
            else
                nhomTotalCholesterol = 4;
        }
        private void checkHDL(double _HDL)
        {
            //HDL
            if (_HDL < 1)
                nhomHDL = 0;
            else if (_HDL < 1.5)
                nhomHDL = 1;
            else
                nhomHDL = 2;
        }
        private void checkHbA1C(double _HbA1C)
        {
            //HbA1C
            if (_HbA1C < 40)
                nhomHbA1C = 0;
            else if (_HbA1C < 50)
                nhomHbA1C = 1;
            else if (_HbA1C < 60)
                nhomHbA1C = 2;
            else if (_HbA1C < 70)
                nhomHbA1C = 3;
            else
                nhomHbA1C = 4;
        }
        private void checkEGFR(string _gioitinh, double _creatininSerum, double _tuoi)
        {
            //eGFR
            eGFR_CKD eGFR_CKD_temp = new eGFR_CKD(_gioitinh, _creatininSerum, _tuoi);
            double kqeGFR = eGFR_CKD_temp.kqeGFR_CKD();
            if (kqeGFR < 45)
                nhomEGFR = 0;
            else if (kqeGFR < 60)
                nhomEGFR = 1;
            else if (kqeGFR < 90)
                nhomEGFR = 2;
            else
                nhomEGFR = 3;
        }
        private void checkvungnguyco(string _vungnguyco)
        {
            //Vùng nguy cơ
            if (_vungnguyco == "Thấp")
                nhomvungnguyco = 0;
            else if (_vungnguyco == "Trung bình")
                nhomvungnguyco = 1;
            else if (_vungnguyco == "Cao")
                nhomvungnguyco = 2;
            else
                nhomvungnguyco = 3;
        }
        public int kqSCORE2_DM()
        {
            //Mỗi nhóm tuổi có 31 giá trị cho 7 biến: DM_Age (8); Smoking (2); HATT (4); TotalCho (5)
            //HDL (3); HbA1C (5); eGFR (4)
            //Dữ liệu ở init_SCORE2_DM();
            diem_start_index = 31 * nhomtuoi;
            int kq;
            if (gioitinh == "Nam")
            {
                kq = diemNam[diem_start_index + nhomDM_Age] +
                    diemNam[diem_start_index + 8 + nhomSmoking] +
                    diemNam[diem_start_index + 8 + 2 + nhomHATT] +
                    diemNam[diem_start_index + 8 + 2 + 4 + nhomTotalCholesterol] +
                    diemNam[diem_start_index + 8 + 2 + 4 + 5 + nhomHDL] +
                    diemNam[diem_start_index + 8 + 2 + 4 + 5 + 3 + nhomHbA1C] +
                    diemNam[diem_start_index + 8 + 2 + 4 + 5 + 3 + 5 + nhomEGFR];
            }
            else
            {
                kq = diemNu[diem_start_index + nhomDM_Age] +
                    diemNu[diem_start_index + 8 + nhomSmoking] +
                    diemNu[diem_start_index + 8 + 2 + nhomHATT] +
                    diemNu[diem_start_index + 8 + 2 + 4 + nhomTotalCholesterol] +
                    diemNu[diem_start_index + 8 + 2 + 4 + 5 + nhomHDL] +
                    diemNu[diem_start_index + 8 + 2 + 4 + 5 + 3 + nhomHbA1C] +
                    diemNu[diem_start_index + 8 + 2 + 4 + 5 + 3 + 5 + nhomEGFR];
            }
            return kq;
        }
        public int kqNguycoSCORE2_DM()
        {
            int diem = kqSCORE2_DM();
            int kq;

            if (gioitinh == "Nam")
            {
                nguyco_start_index = 49 * nhomvungnguyco;
                kq = nguycoNam[nguyco_start_index + diem + 15];
            }
            else
            {
                nguyco_start_index = 53 * nhomvungnguyco;
                kq = nguycoNu[nguyco_start_index + diem + 14];
            }
            return kq;
        }
        public string kqPLNguycoSCORE2_DM()
        {
            int diem = kqSCORE2_DM();
            string kq;

            if (gioitinh == "Nam")
            {
                nguyco_start_index = 49 * nhomvungnguyco;
                kq = PLnguycoNam[nguyco_start_index + diem + 15];
            }
            else
            {
                nguyco_start_index = 53 * nhomvungnguyco;
                kq = PLnguycoNu[nguyco_start_index + diem + 14];
            }
            if (kq == "T")
                kq = "Thấp";
            else if (kq == "TB")
                kq = "Trung bình";
            else if (kq == "C")
                kq = "Cao";
            else
                kq = "Rất cao";
            return kq;
        }
    }
    public class SCORE2 : Thangdiem
    {
        public int nhomgioitinh { get; set; }
        public int nhomtuoi { get; set; }
        public int nhomSmoking { get; set; }
        public int nhomHATT { get; set; }
        public int nhomNonHDL { get; set; }
        public int nhomvungnguyco { get; set; }
        public int[] diem { get; set; }
        public string[] PLnguyco { get; set; }
        public int diem_start_index { get; set; }
        public SCORE2()
        {
            init_SCORE2();
        }
        public SCORE2(Nguoibenh NB, Xetnghiem XN)
        {
            init_SCORE2();
            checkgioitinh(NB.gioitinh);
            checktuoi(NB.tuoi);
            checkSmoking(NB.hutthuoc);
            checkHATT(NB.HATThu);
            checkNonHDL(XN.HDL, XN.totalCholesterol);
        }
        public SCORE2(string _gioitinh, double _tuoi, bool _smoking, double _HATT, double _TotalCholesterol,
            double _HDL, string _vungnguyco)
        {
            init_SCORE2();
            checkgioitinh(_gioitinh);
            checktuoi(_tuoi);
            checkSmoking(_smoking);
            checkHATT(_HATT);
            checkNonHDL(_HDL,_TotalCholesterol);
            checkvungnguyco(_vungnguyco);
        }
        private void init_SCORE2()
        {
            //Vùng nguy cơ: 640; Giới tính: 320; Hút thuốc: 160; Tuổi: 16; NonHDL: 4; HATT: 1
            int[] _diem = {1, 1, 1, 2, 1, 1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 3, 1, 1, 2, 2, 1, 2, 2, 3, 1, 2, 2, 3, 1, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 4, 2, 2, 3, 4, 2, 3, 3, 4, 2, 3, 3, 4, 2, 3, 4, 5, 3, 3, 4, 5, 3, 3, 4, 5, 3, 4, 5, 6, 3, 4, 5, 6, 4, 4, 5, 7, 4, 5, 6, 7, 5, 5, 7, 8, 5, 6, 7, 8, 5, 6, 7, 9, 5, 6, 7, 9, 6, 7, 9, 10, 6, 7, 9, 11, 6, 8, 10, 12, 7, 8, 10, 12, 9, 11, 13, 15, 10, 11, 13, 15, 10, 12, 14, 16, 11, 13, 15, 17, 15, 16, 18, 20, 15, 17, 19, 21, 16, 18, 20, 22, 17, 19, 21, 23, 23, 24, 26, 28, 24, 25, 27, 29, 25, 26, 28, 30, 26, 27, 29, 31, 2, 2, 3, 4, 2, 3, 3, 4, 2, 3, 4, 5, 2, 3, 4, 6, 2, 3, 4, 5, 2, 3, 4, 5, 3, 4, 5, 6, 3, 4, 5, 7, 3, 4, 5, 6, 3, 4, 5, 7, 4, 5, 6, 7, 4, 5, 6, 8, 4, 5, 6, 8, 4, 5, 7, 8, 5, 6, 7, 9, 5, 6, 8, 10, 5, 6, 8, 10, 6, 7, 8, 10, 6, 7, 9, 11, 6, 8, 9, 11, 7, 8, 10, 12, 7, 9, 10, 12, 7, 9, 11, 13, 8, 9, 11, 13, 9, 11, 14, 17, 10, 12, 15, 18, 10, 13, 15, 19, 11, 14, 16, 20, 13, 15, 18, 21, 14, 16, 19, 22, 15, 17, 20, 23, 15, 18, 21, 24, 18, 20, 23, 25, 19, 21, 24, 26, 20, 22, 25, 28, 21, 23, 26, 29, 25, 27, 29, 31, 26, 28, 30, 32, 27, 29, 31, 33, 28, 30, 32, 34, 1, 2, 2, 3, 2, 2, 3, 4, 2, 3, 3, 5, 2, 3, 4, 5, 2, 2, 3, 4, 2, 3, 4, 5, 3, 3, 4, 6, 3, 4, 5, 6, 3, 3, 4, 5, 3, 4, 5, 6, 3, 4, 5, 7, 3, 5, 6, 8, 4, 4, 5, 7, 4, 5, 6, 7, 4, 5, 7, 8, 5, 6, 8, 9, 5, 6, 7, 8, 5, 6, 8, 9, 6, 7, 8, 10, 6, 8, 9, 11, 6, 8, 9, 11, 7, 8, 10, 12, 7, 9, 11, 12, 8, 10, 11, 13, 8, 10, 12, 15, 8, 11, 13, 16, 9, 12, 14, 18, 10, 13, 16, 19, 12, 14, 16, 19, 13, 15, 18, 21, 15, 18, 21, 24, 17, 20, 23, 27, 17, 19, 21, 23, 20, 22, 25, 27, 24, 26, 29, 32, 28, 31, 34, 37, 25, 26, 28, 29, 30, 32, 33, 35, 36, 38, 40, 42, 43, 45, 47, 49, 3, 3, 5, 6, 3, 4, 5, 7, 4, 5, 6, 8, 5, 6, 8, 10, 3, 4, 6, 7, 4, 5, 7, 8, 5, 6, 8, 10, 5, 7, 9, 11, 4, 6, 7, 9, 5, 6, 8, 10, 6, 7, 9, 11, 7, 8, 10, 13, 6, 7, 9, 10, 6, 8, 10, 12, 7, 9, 11, 13, 8, 10, 12, 15, 7, 9, 10, 13, 8, 10, 11, 14, 9, 10, 13, 15, 10, 11, 14, 17, 9, 11, 13, 15, 10, 12, 14, 16, 11, 13, 15, 17, 11, 13, 16, 19, 12, 14, 18, 22, 13, 16, 19, 24, 14, 17, 21, 26, 15, 19, 23, 28, 15, 18, 21, 24, 17, 20, 23, 27, 19, 23, 26, 31, 22, 26, 30, 34, 19, 22, 24, 26, 23, 25, 28, 31, 27, 30, 33, 36, 31, 34, 38, 41, 25, 26, 27, 29, 30, 32, 33, 35, 36, 38, 40, 42, 43, 45, 47, 49, 1, 1, 1, 2, 1, 1, 2, 2, 1, 1, 2, 3, 1, 2, 2, 3, 1, 2, 2, 3, 1, 2, 2, 3, 1, 2, 3, 3, 2, 2, 3, 4, 2, 2, 3, 4, 2, 2, 3, 4, 2, 3, 4, 5, 2, 3, 4, 5, 3, 3, 4, 5, 3, 3, 4, 6, 3, 4, 5, 6, 3, 4, 5, 7, 4, 5, 6, 7, 4, 5, 6, 8, 4, 5, 7, 8, 5, 6, 7, 9, 5, 7, 8, 10, 6, 7, 9, 10, 6, 7, 9, 11, 6, 8, 9, 12, 7, 9, 11, 13, 7, 9, 11, 14, 8, 10, 12, 15, 8, 11, 13, 16, 12, 14, 16, 19, 12, 15, 17, 20, 13, 15, 18, 21, 14, 16, 19, 23, 19, 21, 24, 27, 20, 22, 25, 28, 21, 24, 27, 30, 22, 25, 28, 31, 30, 32, 35, 37, 32, 34, 36, 39, 33, 35, 38, 40, 34, 37, 39, 42, 2, 3, 3, 5, 2, 3, 4, 5, 2, 3, 5, 6, 3, 4, 5, 7, 3, 3, 5, 6, 3, 4, 5, 7, 3, 4, 6, 8, 4, 5, 6, 9, 3, 5, 6, 8, 4, 5, 6, 8, 4, 6, 7, 9, 5, 6, 8, 10, 5, 6, 8, 10, 5, 7, 8, 11, 6, 7, 9, 11, 6, 8, 10, 12, 6, 8, 10, 12, 7, 8, 11, 13, 7, 9, 11, 14, 8, 10, 12, 15, 9, 10, 13, 15, 9, 11, 13, 16, 9, 12, 14, 17, 10, 12, 15, 18, 12, 15, 18, 22, 13, 16, 19, 23, 13, 17, 20, 25, 14, 18, 22, 26, 17, 20, 24, 27, 18, 21, 25, 29, 19, 22, 26, 30, 20, 24, 28, 32, 24, 27, 30, 34, 25, 28, 32, 35, 27, 30, 33, 37, 28, 31, 35, 39, 34, 36, 39, 41, 25, 28, 40, 43, 37, 39, 42, 44, 38, 41, 43, 46, 2, 2, 3, 4, 2, 3, 4, 5, 2, 3, 4, 6, 3, 4, 5, 7, 2, 3, 4, 5, 3, 4, 5, 6, 3, 4, 5, 7, 4, 5, 6, 8, 3, 4, 5, 7, 4, 5, 6, 8, 4, 5, 7, 9, 5, 6, 8, 10, 4, 5, 7, 9, 5, 6, 8, 10, 6, 7, 9, 11, 6, 8, 10, 12, 6, 7, 9, 11, 7, 8, 10, 12, 7, 9, 11, 13, 8, 10, 12, 15, 8, 10, 12, 14, 9, 11, 13, 15, 10, 12, 14, 17, 10, 13, 15, 18, 10, 12, 15, 19, 11, 13, 17, 21, 12, 15, 18, 23, 13, 16, 20, 25, 15, 17, 21, 24, 17, 20, 23, 27, 19, 23, 27, 31, 22, 26, 30, 35, 22, 25, 27, 30, 26, 29, 32, 35, 31, 34, 37, 41, 36, 40, 43, 47, 32, 34, 36, 37, 39, 41, 43, 45, 47, 49, 51, 53, 55, 57, 59, 62, 3, 4, 6, 8, 4, 5, 7, 9, 5, 6, 8, 11, 6, 8, 10, 13, 4, 5, 7, 9, 5, 7, 8, 11, 6, 8, 10, 13, 7, 9, 12, 15, 5, 7, 9, 11, 6, 8, 10, 13, 7, 9, 12, 15, 8, 11, 14, 17, 7, 9, 11, 14, 8, 10, 13, 16, 9, 11, 14, 17, 10, 13, 16, 20, 9, 11, 14, 17, 10, 13, 15, 18, 11, 14, 17, 20, 12, 15, 18, 22, 12, 14, 17, 20, 13, 15, 18, 22, 14, 17, 20, 23, 15, 18, 21, 25, 15, 19, 23, 28, 16, 20, 25, 31, 18, 22, 28, 34, 20, 24, 30, 36, 19, 23, 27, 31, 22, 26, 30, 35, 25, 29, 34, 39, 29, 33, 38, 44, 25, 28, 31, 34, 30, 33, 36, 40, 35, 38, 42, 46, 40, 44, 48, 53, 32, 34, 35, 37, 39, 41, 43, 45, 46, 48, 51, 53, 55, 57, 59, 61, 1, 1, 1, 2, 1, 1, 2, 3, 1, 1, 2, 3, 1, 2, 2, 4, 1, 2, 2, 3, 1, 2, 3, 4, 2, 2, 3, 4, 2, 2, 4, 5, 2, 3, 3, 5, 2, 3, 4, 5, 2, 3, 4, 6, 3, 4, 5, 7, 3, 4, 5, 7, 3, 4, 6, 8, 4, 5, 7, 9, 4, 5, 7, 10, 5, 6, 8, 11, 5, 7, 9, 11, 6, 7, 9, 12, 6, 8, 10, 13, 8, 10, 12, 15, 8, 10, 13, 16, 8, 11, 14, 17, 9, 11, 14, 18, 11, 14, 17, 21, 12, 15, 18, 22, 13, 16, 17, 24, 14, 17, 20, 25, 18, 22, 25, 29, 19, 23, 27, 31, 20, 24, 28, 32, 22, 25, 29, 34, 29, 32, 36, 40, 31, 34, 38, 42, 32, 36, 39, 44, 34, 37, 41, 45, 44, 47, 50, 53, 46, 49, 52, 55, 48, 51, 54, 57, 50, 52, 55, 58, 2, 3, 4, 6, 2, 4, 5, 7, 3, 4, 6, 9, 3, 5, 7, 10, 3, 4, 6, 8, 3, 5, 7, 10, 4, 6, 8, 11, 5, 6, 9, 13, 4, 6, 8, 11, 5, 7, 9, 13, 6, 8, 10, 14, 6, 9, 12, 16, 6, 8, 11, 15, 7, 9, 12, 16, 8, 10, 14, 18, 8, 11, 15, 20, 9, 12, 15, 20, 10, 13, 16, 21, 11, 14, 18, 23, 11, 15, 19, 25, 13, 16, 21, 26, 14, 17, 22, 27, 14, 18, 23, 29, 15, 19, 24, 30, 19, 23, 28, 33, 20, 24, 29, 35, 21, 26, 31, 37, 22, 27, 33, 39, 26, 31, 35, 41, 28, 32, 37, 43, 29, 34, 39, 45, 31, 36, 41, 47, 36, 40, 44, 49, 38, 42, 46, 51, 40, 44, 48, 53, 41, 46, 50, 55, 49, 52, 55, 58, 51, 53, 56, 59, 52, 55, 58, 61, 54, 57, 60, 63, 1, 2, 3, 4, 2, 2, 3, 5, 2, 3, 4, 6, 3, 4, 5, 7, 2, 3, 4, 5, 2, 3, 5, 6, 3, 4, 6, 8, 4, 5, 7, 9, 3, 4, 5, 7, 3, 5, 6, 8, 4, 5, 7, 10, 5, 6, 9, 11, 4, 6, 7, 9, 5, 6, 8, 11, 6, 7, 10, 12, 7, 9, 11, 14, 6, 8, 10, 13, 7, 9, 11, 14, 8, 10, 13, 16, 9, 11, 14, 18, 9, 11, 14, 17, 10, 12, 15, 18, 11, 13, 16, 20, 12, 15, 18, 22, 12, 15, 19, 23, 14, 17, 20, 25, 15, 18, 22, 27, 16, 20, 24, 29, 18, 21, 24, 28, 20, 24, 27, 32, 23, 27, 31, 35, 26, 30, 34, 39, 26, 29, 31, 34, 30, 33, 36, 40, 35, 38, 42, 45, 40, 44, 47, 51, 36, 38, 40, 42, 43, 45, 47, 49, 51, 53, 55, 57, 58, 61, 63, 65, 3, 4, 6, 8, 4, 5, 7, 10, 5, 7, 9, 13, 6, 8, 11, 16, 4, 6, 8, 10, 5, 7, 9, 13, 6, 8, 11, 15, 7, 10, 14, 18, 6, 7, 10, 13, 7, 9, 12, 15, 8, 10, 14, 18, 9, 12, 16, 21, 8, 10, 13, 16, 9, 11, 15, 19, 10, 13, 17, 21, 12, 15, 19, 24, 10, 13, 16, 20, 12, 15, 18, 23, 13, 16, 20, 25, 15, 18, 23, 28, 14, 17, 21, 25, 15, 19, 23, 28, 17, 20, 25, 30, 18, 22, 27, 32, 18, 22, 27, 33, 20, 24, 29, 35, 22, 26, 32, 38, 23, 28, 34, 41, 23, 27, 31, 35, 26, 30, 34, 39, 29, 34, 38, 44, 33, 37, 43, 48, 29, 32, 35, 38, 34, 37, 40, 44, 39, 42, 46, 50, 44, 48, 52, 56, 36, 38, 40, 41, 43, 45, 47, 49, 50, 52, 54, 56, 58, 60, 62, 65, 2, 3, 4, 5, 2, 3, 4, 6, 2, 3, 5, 7, 3, 4, 6, 8, 3, 4, 5, 7, 3, 4, 6, 8, 4, 5, 7, 9, 4, 6, 8, 10, 4, 6, 8, 10, 5, 6, 9, 11, 5, 7, 9, 12, 6, 8, 11, 14, 7, 8, 11, 14, 7, 9, 12, 15, 8, 10, 13, 17, 9, 11, 14, 18, 10, 12, 16, 20, 11, 13, 17, 21, 11, 14, 18, 22, 12, 15, 19, 24, 15, 18, 22, 27, 16, 19, 23, 28, 16, 20, 24, 30, 17, 21, 26, 31, 26, 29, 33, 37, 27, 30, 34, 38, 28, 31, 35, 39, 29, 32, 36, 41, 34, 37, 41, 44, 35, 39, 42, 46, 36, 40, 43, 47, 37, 41, 45, 48, 44, 47, 50, 53, 45, 48, 51, 54, 47, 49, 52, 55, 48, 51, 54, 57, 56, 58, 60, 62, 57, 59, 61, 63, 58, 60, 62, 64, 60, 61, 63, 65, 5, 7, 9, 13, 6, 8, 11, 15, 6, 9, 12, 17, 7, 10, 14, 19, 7, 9, 12, 16, 8, 10, 14, 18, 9, 12, 15, 21, 10, 13, 17, 23, 9, 12, 16, 21, 10, 13, 18, 23, 11, 15, 19, 25, 13, 17, 22, 28, 13, 16, 21, 26, 14, 18, 23, 28, 15, 19, 24, 31, 16, 21, 26, 33, 17, 22, 27, 33, 18, 23, 29, 35, 20, 25, 30, 37, 21, 26, 32, 39, 23, 28, 34, 41, 24, 30, 36, 42, 26, 31, 37, 44, 27, 33, 39, 46, 34, 39, 43, 48, 36, 40, 44, 49, 37, 41, 46, 51, 38, 43, 47, 52, 42, 46, 49, 53, 43, 47, 51, 55, 44, 48, 52, 56, 46, 49, 53, 58, 50, 53, 56, 59, 51, 54, 57, 60, 53, 56, 59, 62, 54, 57, 60, 63, 59, 61, 63, 65, 60, 62, 64, 66, 61, 63, 65, 67, 63, 65, 66, 68, 3, 4, 5, 7, 4, 5, 6, 9, 4, 6, 8, 11, 5, 7, 10, 13, 4, 5, 7, 9, 5, 6, 8, 11, 6, 8, 10, 13, 7, 9, 12, 16, 6, 7, 10, 12, 7, 9, 11, 14, 8, 10, 13, 16, 9, 12, 15, 19, 8, 10, 13, 16, 9, 11, 14, 18, 10, 13, 16, 20, 12, 15, 18, 23, 11, 14, 17, 20, 12, 15, 19, 23, 14, 17, 20, 25, 15, 18, 22, 27, 15, 18, 22, 26, 17, 20, 24, 28, 18, 21, 26, 30, 19, 23, 27, 32, 25, 28, 32, 35, 26, 30, 33, 37, 28, 31, 35, 39, 29, 33, 36, 40, 31, 34, 37, 40, 33, 36, 39, 42, 36, 39, 42, 45, 38, 41, 44, 48, 38, 40, 42, 44, 41, 43, 46, 48, 45, 47, 49, 52, 48, 51, 53, 56, 46, 47, 48, 49, 50, 52, 53, 54, 55, 56, 58, 59, 60, 61, 63, 64, 6, 8, 11, 14, 7, 10, 13, 17, 9, 12, 16, 20, 11, 14, 19, 24, 8, 10, 13, 17, 9, 12, 16, 20, 11, 14, 18, 24, 13, 17, 22, 28, 10, 13, 17, 21, 12, 15, 19, 24, 14, 17, 22, 28, 16, 20, 25, 31, 13, 17, 21, 25, 15, 19, 23, 28, 17, 21, 26, 32, 19, 24, 29, 35, 17, 21, 25, 31, 19, 23, 28, 33, 21, 25, 31, 36, 23, 28, 33, 40, 22, 26, 31, 36, 24, 28, 33, 39, 26, 30, 36, 42, 28, 33, 38, 44, 31, 35, 39, 43, 33, 36, 41, 45, 34, 38, 42, 47, 36, 40, 44, 49, 36, 39, 42, 45, 38, 41, 44, 48, 41, 44, 47, 51, 43, 47, 50, 54, 40, 43, 45, 47, 44, 46, 49, 51, 48, 50, 52, 55, 51, 54, 56, 59, 46, 47, 48, 49, 50, 52, 53, 54, 55, 56, 58, 59, 60, 61, 63, 64};
            diem = _diem;
            string[] _PLnguyco = {};
            PLnguyco = _PLnguyco;
        }
        private void checkgioitinh(string _gioitinh)
        {
            if (_gioitinh == "Nam")
                nhomgioitinh = 1;
            else
                nhomgioitinh = 0;
        }
        private void checktuoi(double _tuoi)
        {
            //Nhom tuoi
            if (_tuoi < 45)
                nhomtuoi = 0;
            else if (_tuoi < 50)
                nhomtuoi = 1;
            else if (_tuoi < 55)
                nhomtuoi = 2;
            else if (_tuoi < 60)
                nhomtuoi = 3;
            else if (_tuoi < 65)
                nhomtuoi = 4;
            else if (_tuoi < 70)
                nhomtuoi = 5;
            else if (_tuoi < 75)
                nhomtuoi = 6;
            else if (_tuoi < 80)
                nhomtuoi = 7;
            else if (_tuoi < 85)
                nhomtuoi = 8;
            else
                nhomtuoi = 9;
        }
        private void checkSmoking(bool _smoking)
        {
            //Smoking
            if (_smoking)
                nhomSmoking = 1;
            else
                nhomSmoking = 0;
        }
        private void checkHATT(double _HATT)
        {
            //HATT
            if (_HATT < 120)
                nhomHATT = 0;
            else if (_HATT < 140)
                nhomHATT = 1;
            else if (_HATT < 160)
                nhomHATT = 2;
            else
                nhomHATT = 3;
        }
        private void checkNonHDL(double _HDL, double _TotalCholesterol)
        {
            //HDL
            if (_TotalCholesterol - _HDL < 4)
                nhomNonHDL = 0;
            else if (_TotalCholesterol - _HDL < 5)
                nhomNonHDL = 1;
            else if (_TotalCholesterol - _HDL < 6)
                nhomNonHDL = 2;
            else
                nhomNonHDL = 3;
        }
        private void checkvungnguyco(string _vungnguyco)
        {
            //Vùng nguy cơ
            if (_vungnguyco == "Thấp")
                nhomvungnguyco = 0;
            else if (_vungnguyco == "Trung bình")
                nhomvungnguyco = 1;
            else if (_vungnguyco == "Cao")
                nhomvungnguyco = 2;
            else
                nhomvungnguyco = 3;
        }
        public int kqSCORE2()
        {
            //Mỗi biến có 1 trọng số: theo dữ liệu ở init_SCORE2_DM();
            diem_start_index = 640 * nhomvungnguyco + 320 * nhomgioitinh + 160 * nhomSmoking +
                16 * nhomtuoi + 4 * nhomNonHDL + nhomHATT;
            int kq = diem[diem_start_index];
            return kq;
        }
        public string kqPLNguycoSCORE2()
        {
            diem_start_index = 640 * nhomvungnguyco + 320 * nhomgioitinh + 160 * nhomSmoking +
                16 * nhomtuoi + 4 * nhomNonHDL + nhomHATT;
            string kq = PLnguyco[diem_start_index];
            if (kq == "T")
                kq = "Thấp";
            else if (kq == "TB")
                kq = "Trung bình";
            else if (kq == "C")
                kq = "Cao";
            else
                kq = "Rất cao";
            return kq;
        }
    }
    #endregion
}