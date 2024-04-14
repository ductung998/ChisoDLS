using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassChung;

namespace Chisoyhoc_MVC.Controllers
{
    public class TrangchuController : Controller
    {
        CSDL_CSYH_ServerDataContext db = new CSDL_CSYH_ServerDataContext();
        //CSDL_PMChisoyhocDataContext db = new CSDL_PMChisoyhocDataContext();
        KetnoiDB dbclass = new KetnoiDB();
        // GET: Trangchu
        public ActionResult Index(string strSearch)
        {
            List<chisoyhoc> obj = db.chisoyhocs.ToList();
            if (!String.IsNullOrEmpty(strSearch))
            {
                obj = obj.Where(x => x.tenchiso.ToUpper().Contains(strSearch.ToUpper())).ToList();
            }
            return View(obj);
        }

        //CSDL
        public ActionResult Trangtinh(string id)
        {
            string input = "";
            object viewModelModel = null;
            if (id != null)
            {
                ViewBag.Id = id;
                ViewBag.Tenchiso = dbclass.GetTenchiso(id);
                List<Bien> DSbien = dbclass.GetDSbien(id);
                List<string> bienNames = new List<string>();
                foreach (var bien in DSbien)
                {
                    bienNames.Add(bien.tenbien);
                }

                for (int i = 0; i < bienNames.Count(); i++)
                {
                    string a = bienNames[i].Trim();
                    input += Request.Form[a] + "_";
                }
                input = input.TrimEnd('_');

                viewModelModel = Initchiso(id);
            }
            else
            {
                viewModelModel = null;
            }
            if (Request.HttpMethod == "GET")
            {
                return View(viewModelModel);
            }
            else if (Request.HttpMethod == "POST")
            {
                KetnoiDB dbkn = new KetnoiDB();
                List<string> kq = new List<string>();
                kq.Add(id);
                kq.Add(dbclass.GetTenchiso(id));
                input = input.Replace("-", ".");
                ViewBag.input = input;
                kq.AddRange(dbclass.Xulycongthuc(id, input));

                #region Nhiều kết quả
                if (id.Substring(0, 1) == "C")
                {
                    if (id == "C_A05")
                    {
                        string result24h = kq[2];
                        string result8h = kq[3];
                        string result16h = kq[4];
                        var jsonResult = new
                        {
                            Result24h = result24h,
                            Result8h = result8h,
                            Result16h = result16h
                        };
                        return Json(jsonResult);
                    }
                    else if (id == "C_A07")
                    {
                        string kqaag = kq[2];
                        string kqaagnormal = kq[3];
                        var jsonResult = new
                        {
                            Kqaag = kqaag,
                            Kqaagnormal = kqaagnormal
                        };
                        // Return the JSON object
                        return Json(jsonResult);
                    }
                    else if (id == "C_A09")
                    {
                        string kqBSA_Dub = kq[2];
                        string kqBSA_Mos = kq[3];
                        var jsonResult = new
                        {
                            KqBSA_Dub = kqBSA_Dub,
                            KqBSA_Mos = kqBSA_Mos
                        };
                        return Json(jsonResult);
                    }
                    else if (id == "C_C12")
                    {
                        string kqcc12_1 = kq[2];
                        string kqcc12_2 = kq[3];
                        string kqcc12_3 = kq[4];
                        string kqcc12_4 = kq[5];
                        var jsonResult = new
                        {
                            Kqcc12_1 = kqcc12_1,
                            Kqcc12_2 = kqcc12_2,
                            Kqcc12_3 = kqcc12_3,
                            Kqcc12_4 = kqcc12_4
                        };
                        return Json(jsonResult);
                    }
                    else
                    {
                        string ketqua = kq[2];
                        string diengiai = kq[3];
                        var jsonResult = new
                        {
                            Ketqua = ketqua,
                            Diengiai = diengiai
                        };
                        return Json(jsonResult);
                    }

                }
                else
                {
                    if (id == "T_A04")
                    {
                        string ketquachinh = kq[2];
                        string ketquaphu = kq[3];
                        string diengiai = kq[4] + ":\n" + kq[5];
                        var jsonResult = new
                        {
                            Ketquachinh = ketquachinh,
                            Ketquaphu = ketquaphu,
                            Diengiai = diengiai
                        };
                        return Json(jsonResult);
                    }
                    else if (id == "T_A14" || id == "T_B29" || id == "T_B32")
                    {
                        string ketquathieu = kq[2];
                        string diengiaithieu = kq[3];
                        var jsonResult = new
                        {
                            Ketquathieu = ketquathieu,
                            Diengiaithieu = diengiaithieu
                        };
                        return Json(jsonResult);
                    }
                    else if (id == "T_B01")
                    {
                        string ketquaapache = kq[2];
                        string diengiaiapache = kq[3] + ": " + kq[4] + "\n" + kq[5] + ": " + kq[6] + "\n" + kq[7] + ": " + kq[8];
                        var jsonResult = new
                        {
                            Ketquaapache = ketquaapache,
                            Diengiaiapache = diengiaiapache
                        };
                        return Json(jsonResult);
                    }
                    else if (id == "T_C26")
                    {
                        string ketquatc26 = kq[2];
                        string diengiaitc26 = "Nguy cơ mắc biến cố tim mạch trong 10 năm: " + kq[3];
                        var jsonResult = new
                        {
                            Ketquatc26 = ketquatc26,
                            Diengiaitc26 = diengiaitc26
                        };
                        return Json(jsonResult);
                    }
                    else if (id == "T_C27")
                    {
                        string ketquatc27 = kq[2];
                        string diengiaitc27 = "Nguy cơ mắc biến cố tim mạch trong 10 năm ở người bệnh đái tháo đường: " + kq[3];
                        var jsonResult = new
                        {
                            Ketquatc27 = ketquatc27,
                            Diengiaitc27 = diengiaitc27
                        };
                        return Json(jsonResult);
                    }
                    else
                    {
                        string ketqua = kq[2];
                        string diengiai = kq[3] + ":\n" + kq[4];
                        var jsonResult = new
                        {
                            Ketqua = ketqua,
                            Diengiai = diengiai
                        };
                        return Json(jsonResult);
                    }

                }
                #endregion
            }
            return HttpNotFound();
        }

        #region initchiso
        public object Initchiso(string _id)
        {
            List<chisoyhoc> obj = db.chisoyhocs.Where(x => x.machiso.Contains(_id.ToUpper())).ToList();
            object viewModelModel = null;
            if (_id.Substring(0, 1) == "C")
            {
                #region C_A
                if (_id.Substring(0, 3) == "C_A")
                {
                    switch (_id)
                    {
                        case "C_A01":
                            {
                                IBW model = new IBW();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A02":
                            {
                                AdjBW model = new AdjBW();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A03":
                            {
                                LBW model = new LBW();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A04":
                            {
                                AlcoholSerum model = new AlcoholSerum();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A05":
                            {
                                Budichbong model = new Budichbong();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A06":
                            {
                                BMI model = new BMI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A07":
                            {
                                AaG model = new AaG();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A08":
                            {
                                CalciSerum_Adj model = new CalciSerum_Adj();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A09":
                            {
                                BSA model = new BSA();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A10":
                            {
                                SAG model = new SAG();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A11":
                            {
                                SOG model = new SOG();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A12":
                            {
                                StOG model = new StOG();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A13":
                            {
                                UAG model = new UAG();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A14":
                            {
                                UOG model = new UOG();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A15":
                            {
                                eGFR_CKD model = new eGFR_CKD();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A16":
                            {
                                eCrCl model = new eCrCl();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A17":
                            {
                                FEMg model = new FEMg();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A18":
                            {
                                FENa model = new FENa();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A19":
                            {
                                KtVDaugirdas model = new KtVDaugirdas();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A20":
                            {
                                RRF_Kru model = new RRF_Kru();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A21":
                            {
                                ACR model = new ACR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A22":
                            {
                                PCR model = new PCR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A23":
                            {
                                eAER model = new eAER();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A24":
                            {
                                ePER model = new ePER();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A25":
                            {
                                TocDoTruyen model = new TocDoTruyen();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A26":
                            {
                                CrCl24h model = new CrCl24h();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A27":
                            {
                                eGFR_Schwartz model = new eGFR_Schwartz();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A28":
                            {
                                MPM0 model = new MPM0();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_A29":
                            {
                                eGFR_MDRD model = new eGFR_MDRD();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                    }
                }
                #endregion
                #region C_B
                else if (_id.Substring(0, 3) == "C_B")
                {
                    switch (_id)
                    {
                        //case "C_B01":
                        //    {
                        //        DLCO_Adj model = new DLCO_Adj();
                        //        TryUpdateModel(model);
                        //        viewModelModel = Tuple.Create(obj, (object)model);
                        //        break;
                        //    }
                        //case "C_B02":
                        //    {
                        //        MAP model = new MAP();
                        //        TryUpdateModel(model);
                        //        viewModelModel = Tuple.Create(obj, (object)model);
                        //        break;
                        //    }
                        case "C_B03":
                            {
                                PostFEV1 model = new PostFEV1();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B04":
                            {
                                AEC model = new AEC();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B05":
                            {
                                ANC model = new ANC();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B06":
                            {
                                MIPI model = new MIPI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B07":
                            {
                                RPI model = new RPI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B08":
                            {
                                sTfR model = new sTfR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B09":
                            {
                                BMR model = new BMR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B10":
                            {
                                CDC_chieucao model = new CDC_chieucao();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B11":
                            {
                                CDC_cannang model = new CDC_cannang();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B12":
                            {
                                CDC_chuvi model = new CDC_chuvi();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B13":
                            {
                                Vbudich model = new Vbudich();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B14":
                            {
                                PELD_Old model = new PELD_Old();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B15":
                            {
                                WHO_suyDD model = new WHO_suyDD();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B16":
                            {
                                ePER model = new ePER();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B17":
                            {
                                OxyIndex model = new OxyIndex();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B18":
                            {
                                EED model = new EED();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B19":
                            {
                                EER model = new EER();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B20":
                            {
                                CDC_BMI model = new CDC_BMI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B21":
                            {
                                Noikhiquan model = new Noikhiquan();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B22":
                            {
                                PEF model = new PEF();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_B23":
                            {
                                PELD_New model = new PELD_New();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                    }
                }
                #endregion
                #region C_C
                else
                {
                    switch (_id)
                    {
                        case "C_C01":
                            {
                                NatriSerum_Adj model = new NatriSerum_Adj();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C02":
                            {
                                CardiacOutput model = new CardiacOutput();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C03":
                            {
                                FEPO4 model = new FEPO4();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C04":
                            {
                                LDL model = new LDL();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C05":
                            {
                                FIB4 model = new FIB4();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C06":
                            {
                                TSAT model = new TSAT();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C07":
                            {
                                APRI model = new APRI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C08":
                            {
                                MELD model = new MELD();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C09":
                            {
                                MELDNa model = new MELDNa();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C10":
                            {
                                PVR model = new PVR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C11":
                            {
                                PVRI model = new PVRI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C12":
                            {
                                AdjECG model = new AdjECG();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C13":
                            {
                                SVR model = new SVR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C14":
                            {
                                WBCCFS_Adj model = new WBCCFS_Adj();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C15":
                            {
                                Hauphauxogan model = new Hauphauxogan();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "C_C16":
                            {
                                MESA_SCORE model = new MESA_SCORE();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        //case "C_C17":
                        //    {

                        //    }
                    }
                }
                #endregion


            }
            else
            {
                #region T_A
                if (_id.Substring(0, 3) == "T_A")
                {
                    switch (_id)
                    {
                        case "T_A01":
                            {
                                GRACE model = new GRACE();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A02":
                            {
                                COWS model = new COWS();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A03":
                            {
                                qSOFA model = new qSOFA();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A04":
                            {
                                VNTM model = new VNTM();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A05":
                            {
                                MalHyperthermia model = new MalHyperthermia();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A06":
                            {
                                PSI model = new PSI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A07":
                            {
                                VCSS model = new VCSS();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A08":
                            {
                                BISAP model = new BISAP();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A09":
                            {
                                Blatchford model = new Blatchford();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A10":
                            {
                                Rockall model = new Rockall();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A11":
                            {
                                ChildPugh model = new ChildPugh();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A12":
                            {
                                CLIFSOFA model = new CLIFSOFA();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A13":
                            {
                                HBCrohn model = new HBCrohn();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A14":
                            {
                                GlasgowComa model = new GlasgowComa();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A15":
                            {
                                Ranson model = new Ranson();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A16":
                            {
                                IVPO model = new IVPO();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A17":
                            {
                                PUMayoClinic model = new PUMayoClinic();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_A18":
                            {
                                CDAICrohn model = new CDAICrohn();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                    }
                }
                #endregion
                #region T_B
                else if (_id.Substring(0, 3) == "T_B")
                {
                    switch (_id)
                    {
                        case "T_B01":
                            {
                                APACHE2 model = new APACHE2();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B02":
                            {
                                BODECOPD model = new BODECOPD();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B03":
                            {
                                CURB65 model = new CURB65();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B04":
                            {
                                Light model = new Light();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B05":
                            {
                                GenevaDVT model = new GenevaDVT();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B06":
                            {
                                GenevaPE model = new GenevaPE();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B07":
                            {
                                WellsDVT model = new WellsDVT();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B08":
                            {
                                NEWS2 model = new NEWS2();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B09":
                            {
                                PaduaVTE model = new PaduaVTE();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B10":
                            {
                                WellsPE model = new WellsPE();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B11":
                            {
                                SOFA model = new SOFA();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B12":
                            {
                                VTEBLEED model = new VTEBLEED();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B13":
                            {
                                HeparinIT model = new HeparinIT();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B14":
                            {
                                HASBLED model = new HASBLED();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B15":
                            {
                                DIPSSPlusPMS model = new DIPSSPlusPMS();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B16":
                            {
                                IPSHodgkin model = new IPSHodgkin();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B17":
                            {
                                GIPSSXotuy model = new GIPSSXotuy();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B18":
                            {
                                IPSNonHodgkin model = new IPSNonHodgkin();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B19":
                            {
                                Khorana model = new Khorana();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B20":
                            {
                                MDACC model = new MDACC();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B21":
                            {
                                MDSRLsinhtuy model = new MDSRLsinhtuy();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B22":
                            {
                                Sokal model = new Sokal();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B23":
                            {
                                APGAR model = new APGAR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B24":
                            {
                                PUCAI model = new PUCAI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B25":
                            {
                                WestleyCroup model = new WestleyCroup();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B26":
                            {
                                CMMLMayoClinic model = new CMMLMayoClinic();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B27":
                            {
                                EUTOS model = new EUTOS();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B28":
                            {
                                PASRuotthua model = new PASRuotthua();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B29":
                            {
                                GlasgowNhiB2 model = new GlasgowNhiB2();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B30":
                            {
                                STOPBangS model = new STOPBangS();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B31":
                            {
                                IPSSRLoansantuy model = new IPSSRLoansantuy();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_B32":
                            {
                                GlasgowNhiO2 model = new GlasgowNhiO2();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                    }
                }
                #endregion
                #region T_C
                else
                {
                    switch (_id)
                    {
                        case "T_C01":
                            {
                                FraminghamE model = new FraminghamE();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C02":
                            {
                                ACCAHA model = new ACCAHA();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C03":
                            {
                                CHA2DS2VASc model = new CHA2DS2VASc();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C04":
                            {
                                TIMINonST model = new TIMINonST();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C05":
                            {
                                ARISCAT model = new ARISCAT();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C06":
                            {
                                IPSSTienliet model = new IPSSTienliet();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C07":
                            {
                                ABCD2 model = new ABCD2();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C08":
                            {
                                ESS model = new ESS();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C09":
                            {
                                NIH model = new NIH();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C10":
                            {
                                RoPE model = new RoPE();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C11":
                            {
                                FraminghamS model = new FraminghamS();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C12":
                            {
                                GAD7 model = new GAD7();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C13":
                            {
                                PHQ9 model = new PHQ9();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C14":
                            {
                                Caprini model = new Caprini();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C15":
                            {
                                Eckardt model = new Eckardt();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C16":
                            {
                                LAR model = new LAR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C17":
                            {
                                MESS model = new MESS();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C18":
                            {
                                Braden model = new Braden();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C19":
                            {
                                VSD_Obs model = new VSD_Obs();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C20":
                            {
                                Villalta model = new Villalta();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C21":
                            {
                                RA_CDAI model = new RA_CDAI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C22":
                            {
                                RA_SDAI model = new RA_SDAI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C23":
                            {
                                DAS28CRP model = new DAS28CRP();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C24":
                            {
                                DAS28ESR model = new DAS28ESR();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C25":
                            {
                                ISI model = new ISI();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C26":
                            {
                                SCORE2 model = new SCORE2();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C27":
                            {
                                SCORE2_DM model = new SCORE2_DM();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C28":
                            {
                                SCORED model = new SCORED();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C29":
                            {
                                TIMIST model = new TIMIST();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                        case "T_C30":
                            {
                                VSD_Ref model = new VSD_Ref();
                                TryUpdateModel(model);
                                viewModelModel = Tuple.Create(obj, (object)model);
                                break;
                            }
                    }
                }
                #endregion
            }
            return viewModelModel;
        }
        #endregion
    }
}