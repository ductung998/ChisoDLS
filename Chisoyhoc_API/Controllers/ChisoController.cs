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

            input = input.Replace("-", ".");

            string[] inputs = input.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

            if (machiso.Substring(0, 1) == "C")
            {
                #region C_A
                if (machiso.Substring(0, 3) == "C_A")
                {
                    switch (machiso)
                    {
                        case "C_A01":
                            {
                                IBW IBWCal = new IBW(inputs[0],
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(IBWCal.kqIBW(), 2).ToString());
                                break;
                            }
                        case "C_A02":
                            {
                                AdjBW AdjBWCal = new AdjBW(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(AdjBWCal.kqAdjBW(), 2).ToString());
                                break;
                            }
                        case "C_A03":
                            {
                                LBW LBWCal = new LBW(inputs[0], double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(LBWCal.KqLBW(), 2).ToString());
                                break;
                            }
                        case "C_A04":
                            {
                                AlcoholSerum AlcoholSerumCal = new AlcoholSerum(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(AlcoholSerumCal.kqAlcoholSerum(), 2).ToString());
                                break;
                            }
                        case "C_A05":
                            {
                                Budichbong BudichbongCal = new Budichbong(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(BudichbongCal.kqVdich24h(), 2).ToString());
                                kq.Add(Math.Round(BudichbongCal.kqtocdotruyen8h(), 2).ToString());
                                kq.Add(Math.Round(BudichbongCal.kqtocdotruyen16h(), 2).ToString());
                                break;
                            }
                        case "C_A06":
                            {
                                BMI BMICal = new BMI(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(BMICal.kqBMI(), 2).ToString());
                                break;
                            }
                        case "C_A07":
                            {
                                AaG AaGCal = new AaG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[5]),
                                    double.Parse(inputs[6]));
                                kq.Add(Math.Round(AaGCal.kqAaG(), 2).ToString());
                                kq.Add(Math.Round(AaGCal.kqAaGnormal(), 2).ToString());
                                break;
                            }
                        case "C_A08":
                            {
                                CalciSerum_Adj CalciSerum_AdjCal = new CalciSerum_Adj(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(CalciSerum_AdjCal.kqCalciSerum_Adj(), 2).ToString());
                                break;
                            }
                        case "C_A09":
                            {
                                BSA BSACal = new BSA(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(BSACal.kqBSA_Dub(), 2).ToString());
                                kq.Add(Math.Round(BSACal.kqBSA_Mos(), 2).ToString());
                                break;
                            }
                        case "C_A10":
                            {
                                SAG SAGCal = new SAG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(SAGCal.kqSAG(), 2).ToString());
                                break;
                            }
                        case "C_A11":
                            {
                                SOG SOGCal = new SOG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(SOGCal.kqSOG(), 2).ToString());
                                break;
                            }
                        case "C_A12":
                            {
                                StOG StOGCal = new StOG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(StOGCal.kqStOG(), 2).ToString());
                                break;
                            }
                        case "C_A13":
                            {
                                UAG UAGCal = new UAG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(UAGCal.kqUAG(), 2).ToString());
                                break;
                            }
                        case "C_A14":
                            {
                                UOG UOGCal = new UOG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(UOGCal.kqUOG(), 2).ToString());
                                break;
                            }
                        case "C_A15":
                            {
                                eGFR_CKD eGFR_CKDCal = new eGFR_CKD(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    inputs[2],
                                    double.Parse(inputs[3]),
                                    inputs[4]);
                                eGFR_MDRD eGFR_MDRDCal = new eGFR_MDRD(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    inputs[2],
                                    double.Parse(inputs[3]),
                                    inputs[4]);
                                kq.Add(Math.Round(eGFR_CKDCal.kqeGFR_CKD(), 2).ToString());
                                kq.Add(Math.Round(eGFR_MDRDCal.kqeGFR_MDRD(), 2).ToString());
                                break;
                            }
                        case "C_A16":
                            {
                                eCrCl eCrClCal = new eCrCl(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(eCrClCal.kqeCrCl(), 2).ToString());
                                break;
                            }
                        case "C_A17":
                            {
                                FEMg FEMgCal = new FEMg(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(FEMgCal.kqFEMg(), 2).ToString());
                                break;
                            }
                        case "C_A18":
                            {
                                FENa FENaCal = new FENa(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(FENaCal.kqFENa(), 2).ToString());
                                break;
                            }
                        case "C_A19":
                            {
                                KtVDaugirdas KtVDaugirdasCal = new KtVDaugirdas(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(KtVDaugirdasCal.kqKtVDaugirdas(), 2).ToString());
                                break;
                            }
                        case "C_A20":
                            {
                                RRF_Kru RRF_KruCal = new RRF_Kru(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(RRF_KruCal.kqRRF_Kru(), 2).ToString());
                                break;
                            }
                        case "C_A21": //2
                            {
                                ACR ACRCal = new ACR(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(ACRCal.kqACR(), 2).ToString());
                                break;
                            }
                        case "C_A22": //2
                            {
                                PCR PCRCal = new PCR(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(PCRCal.kqPCR(), 2).ToString());
                                break;
                            }
                        case "C_A23": //5
                            {
                                eAER eAERCal = new eAER(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    inputs[3],
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(eAERCal.kqeAER(), 2).ToString());
                                break;
                            }
                        case "C_A24": //5
                            {
                                ePER ePERCal = new ePER(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    inputs[3],
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(ePERCal.kqePER(), 2).ToString());
                                break;
                            }
                        case "C_A25": //3
                            {
                                TocDoTruyen TocDoTruyenCal = new TocDoTruyen(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(TocDoTruyenCal.kqTocDoTruyen(), 2).ToString());
                                break;
                            }
                        case "C_A26": //3
                            {
                                CrCl24h CrCl24hCal = new CrCl24h(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(CrCl24hCal.kqCrCl24h(), 2).ToString());
                                break;
                            }
                        case "C_A27": //7
                            {
                                eGFR_Schwartz eGFR_SchwartzCal = new eGFR_Schwartz(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    KetnoiDB.str_to_bool(inputs[4]),
                                    inputs[5],
                                    KetnoiDB.str_to_bool(inputs[6]));
                                kq.Add(Math.Round(eGFR_SchwartzCal.kqeGFR_Schwartz(), 2).ToString());
                                break;
                            }
                        case "C_A28": //17
                            {
                                MPM0 MPM0Cal = new MPM0(int.Parse(inputs[0]),
                                    int.Parse(inputs[1]),
                                    int.Parse(inputs[2]),
                                    KetnoiDB.str_to_bool(inputs[3]),
                                    KetnoiDB.str_to_bool(inputs[4]),
                                    KetnoiDB.str_to_bool(inputs[5]),
                                    int.Parse(inputs[6]),
                                    KetnoiDB.str_to_bool(inputs[7]),
                                    KetnoiDB.str_to_bool(inputs[8]),
                                    KetnoiDB.str_to_bool(inputs[9]),
                                    KetnoiDB.str_to_bool(inputs[10]),
                                    KetnoiDB.str_to_bool(inputs[11]),
                                    KetnoiDB.str_to_bool(inputs[12]),
                                    KetnoiDB.str_to_bool(inputs[13]),
                                    KetnoiDB.str_to_bool(inputs[14]),
                                    KetnoiDB.str_to_bool(inputs[15]),
                                    KetnoiDB.str_to_bool(inputs[16]));
                                kq.Add(Math.Round(MPM0Cal.kqMPM0(), 2).ToString());
                                break;
                            }
                    }
                }
                #endregion
                #region C_B
                else if (machiso.Substring(0, 3) == "C_B")
                {

                }
                #endregion
                #region C_C
                else
                {

                }
                #endregion
            }
            else
            {
                #region T_A
                if (machiso.Substring(0, 3) == "T_A")
                {

                }
                #endregion
                #region T_B
                else if (machiso.Substring(0, 3) == "T_B")
                {

                }
                #endregion
                #region T_C
                else
                {

                }
                #endregion
            }
            
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
