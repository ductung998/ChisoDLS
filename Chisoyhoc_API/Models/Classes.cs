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
                     int _nhiptim, double _thannhiet, int _hatThu, int _hatTruong, bool _tha, bool _dtd, bool _suytim,
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

        public double CalculateBMI()
        {
            double kqBMI = cannang / (chieucao * chieucao);
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
        public double BSA { get; set; }

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
            return BSA = Math.Sqrt(chieucao * cannang) / 3600;
        }
        public double kqBSA_Dub()
        {
            return BSA = 0.007184 * Math.Pow(chieucao, 0.725) * Math.Pow(cannang, 0.425);
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
            alpha_CKD = (gioitinh == "nam") ? -0.241 : -0.302;
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
        public double eCrCl { get; set; }

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
            eCrCl = (140 - tuoi) * cannang / (72 * CreatininSerum) * gioitinhCoefficient;
            return eCrCl;
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
}