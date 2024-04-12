using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace ClassChung
{
    #region Tuong tac
    public class Tuongtac
    {
        private readonly string pathtoTemp;
        public Tuongtac(string _pathtoTemp)
        {
            pathtoTemp = _pathtoTemp;
        }
        public class ReadCSV
        {
            public DataTable readCSV;

            public ReadCSV(string fileName, bool firstRowContainsFieldNames = true)
            {
                readCSV = GenerateDataTable(fileName, firstRowContainsFieldNames);
            }

            private static DataTable GenerateDataTable(string fileName, bool firstRowContainsFieldNames = true)
            {
                DataTable result = new DataTable();

                if (fileName == "")
                {
                    return result;
                }

                string delimiters = ",";
                string extension = Path.GetExtension(fileName);

                if (extension.ToLower() == "txt")
                    delimiters = "\t";
                else if (extension.ToLower() == "csv")
                    delimiters = ",";

                using (TextFieldParser tfp = new TextFieldParser(fileName))
                {
                    tfp.SetDelimiters(delimiters);

                    // Get The Column Names
                    if (!tfp.EndOfData)
                    {
                        string[] fields = tfp.ReadFields();

                        for (int i = 0; i < fields.Count(); i++)
                        {
                            if (firstRowContainsFieldNames)
                                result.Columns.Add(fields[i]);
                            else
                                result.Columns.Add("Col" + i);
                        }

                        // If first line is data then add it
                        if (!firstRowContainsFieldNames)
                            result.Rows.Add(fields);
                    }

                    // Get Remaining Rows from the CSV
                    while (!tfp.EndOfData)
                        result.Rows.Add(tfp.ReadFields());
                }

                return result;
            }
        }
        public void exceltoCSV(string pathfile)
        {
            string excelpath = pathfile;
            string csvpath = Path.GetDirectoryName(pathfile) + "\\" + Path.GetFileNameWithoutExtension(excelpath) + ".csv";

            Microsoft.Office.Interop.Excel.Application a = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook b = a.Workbooks.Open(pathfile);

            b.SaveAs(csvpath, Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV);
            a.Quit();
        }
        public void CSVtoexcel(string pathfile)
        {
            string csvpath = pathfile;
            string excelpath = Path.GetDirectoryName(pathfile) + "\\" + Path.GetFileNameWithoutExtension(csvpath) + ".xlsx";
            Microsoft.Office.Interop.Excel.Application a = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook b = a.Workbooks.Open(pathfile);

            b.SaveAs(excelpath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault);
            a.Quit();
        }
        public DataTable tinhketqua(DataTable input)
        {
            KetnoiDB db = new KetnoiDB();

            DataColumn ketqua = new DataColumn("ketqua", typeof(string));
            int colnum = input.Columns.Count;
            int rownum = input.Rows.Count;

            input.Columns.Add(ketqua);

            foreach (DataRow item in input.Rows)
            {
                string machiso = item[0].ToString();
                string input_mod = "";
                for (int i = 1; i < colnum; i++)
                {
                    if (i == colnum - 1)
                        input_mod = input_mod + item[i];
                    else
                        input_mod = input_mod + item[i] + "_";
                }

                List<string> ketquaxuly = db.Xulycongthuc(machiso, input_mod);
                item[colnum] = ketquaxuly[0];
            }

            return input;
        }
        public static void DatatableToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
    }

    #endregion
    #region Ket not CSDL
    public class KetnoiDB
    {
        public KetnoiDB()
        {
            //Khởi tạo kết nối với CSDL
            initDB();
        }
        public CSDL_PMChisoyhocDataContext db;
        
        public List<chisoyhoc> DSchiso;
        public void initDB()
        {
            //Tạo kết nối LINQ to SQL
            db = new CSDL_PMChisoyhocDataContext();
        }
        public List<DSchisoyhoc> GetDSchisoyhoc()
        {
            //Lấy toàn bộ danh sách chỉ số y học
            DSchiso = (from data in db.chisoyhocs
                       select data).ToList();
            
            List<DSchisoyhoc> DSchisoyhoc = new List<DSchisoyhoc>();
            foreach (chisoyhoc chiso in DSchiso)
            {
                DSchisoyhoc chisokq = new DSchisoyhoc(chiso.machiso, chiso.tenchiso);
                DSchisoyhoc.Add(chisokq);
            }
            return DSchisoyhoc;
        }
        public Chisoyhoc GetCSYHtheoIDchiso(string _machiso)
        {
            //Lấy chỉ số y học theo mã chỉ số (ID)
            Chisoyhoc kq = new Chisoyhoc();

            chisoyhoc i = (from data in db.chisoyhocs
                           where data.machiso == _machiso
                           select data).FirstOrDefault();
            kq.SetChisoyhoc(_machiso, i.tenchiso, i.mucdich, i.ungdung, i.phuongphap, i.diengiaiketqua, i.ghichu, i.tltk);
            return kq;
        }
        public string GetTenchiso(string _machiso)
        {
            //Lấy TÊN chỉ số y học theo mã chỉ số (ID)
            string tenchiso = (from data in db.chisoyhocs
                               where data.machiso == _machiso
                               select data.tenchiso).FirstOrDefault();
            return tenchiso;
        }
        public string GetMachiso(string _tenchiso)
        {
            //Lấy mã chỉ số (ID) chỉ số y học theo tên
            string machiso = (from data in db.chisoyhocs
                              where data.tenchiso == _tenchiso
                              select data.machiso).FirstOrDefault();
            return machiso;
        }
        public List<int> GetDSIDbien(string _machiso)
        {
            //Lấy danh sách ID biến theo mã chỉ số (1 chỉ số có nhiều biến) => List
            List<int> DSIDbien = (from data in db.r_chiso_biens
                                  where data.machiso == _machiso
                                  select data.IDBien).ToList();
            return DSIDbien;
        }
        public List<Bien> GetDSbien(string _IDchiso)
        {
            //Lấy danh sách Bien theo mã chỉ số (1 chỉ số có nhiều biến) => List
            List<Bien> kq = new List<Bien>();

            List<int> DSIDbien = GetDSIDbien(_IDchiso);

            foreach (int i in DSIDbien)
            {
                chiso_DSbien j = (from data in db.chiso_DSbiens
                                  where data.IDbien == i
                                  select data).FirstOrDefault();
                kq.Add(new Bien(j.IDbien, j.tenbien, j.tendaydu, j.IDPhanloaibien, j.IDbiengoc));
            }
            return kq;
        }

        public List<int> GetDSsoluongGT(string _IDchiso)
        {
            //Lấy danh sách số lượng giá trị biến định tính theo mã chỉ số (1 chỉ số có nhiều biến)
            //1 biến định tính có tối thiểu 2 giá trị (VD: Có/Không)
            List<int> kq = new List<int>();

            List<Bien> DSbien = GetDSbien(_IDchiso);

            foreach (Bien i in DSbien)
            {
                try
                {
                    kq.Add(GetGiatribienDT(i.idbien).Count());
                }
                catch
                {
                    continue;
                }
            }
            return kq;
        }
        public List<DSBienCSYH> GetDSbienCSYH(List<Bien> input)
        {
            //Trả về danh sách biến để hiển thị, chỉ gồm một số thuộc tính của Bien
            //DSBienCSYH là class hiển thị đơn thuần
            List<DSBienCSYH> kq = new List<DSBienCSYH>();
            foreach (Bien i in input)
            {
                kq.Add(new DSBienCSYH(i.idbien, i.tenbien, i.tendaydu, i.idloaibien, i.idbiengoc));
            }
            return kq;
        }
        public Bien Getbien(int _idbien)
        {
            //Trả về Bien có idbien tương ứng
            chiso_DSbien checkbiengoc = (from data in db.chiso_DSbiens
                                         where data.IDbien == _idbien
                                         select data).FirstOrDefault();
            chiso_DSbien biengoc;
            if (checkbiengoc.IDbiengoc == 0)
            {
                biengoc = checkbiengoc;
            }
            else
            {
                biengoc = (from data in db.chiso_DSbiens
                           where data.IDbien == checkbiengoc.IDbiengoc
                           select data).FirstOrDefault();
            }
            Bien kq = new Bien(biengoc);
            return kq;
        }
        public BienLT GetbienLT(int _idbien)
        {
            //Trả về BienLT với IDbien tương ứng (BienLT là class con của Bien, có thêm một số thuộc tính)
            chiso_DSbienLT bienLTgoc = (from data in db.chiso_DSbienLTs
                                        where data.ID_Bien == _idbien
                                        select data).FirstOrDefault();
            BienLT kq = new BienLT(Getbien(_idbien), bienLTgoc.donvichuan, bienLTgoc.IDphanloaidonvi);
            return kq;
        }
        public BienDT GetbienDT(int _idbien)
        {
            //Trả về BienLT với IDbien tương ứng (BienDT là class con của Bien, có thêm một số thuộc tính)
            chiso_DSbienDT bienDTgoc = (from data in db.chiso_DSbienDTs
                                        where data.IDBien == _idbien
                                        select data).FirstOrDefault();
            List<GiatribienDT> DSgiatri = GetGiatribienDT(_idbien);

            BienDT kq = new BienDT(Getbien(_idbien), DSgiatri.Count(), bienDTgoc.xuly);
            kq.initBienDT();

            return kq;
        }
        public List<GiatribienDT> GetGiatribienDT(int _idbien)
        {
            //Trả về List GiatribienDT (giá trị biến định tính theo idbien
            //GiatribienDT gồm thứ tự, giá trị, điểm, giới hạn
            List<chiso_DSbienDT> bienDTgoc = (from data in db.chiso_DSbienDTs
                                              where data.IDBien == _idbien
                                              select data).ToList();
            List<GiatribienDT> kq = new List<GiatribienDT>();

            foreach (chiso_DSbienDT i in bienDTgoc)
            {
                GiatribienDT them = new GiatribienDT(i.thutu, i.giatri, i.diem, i.limit);
                if (!kq.Contains(them))
                {
                    kq.Add(them);
                }
            }

            return kq;
        }
        public string GetCSYHtheoIDBien(string _idbien)
        {
            //Trả về chuỗi các Mã chỉ số y học có sử dụng biến theo idbien truyền vào
            //(kể cả biến gốc và biến được chuyển từ biến gốc (vd khoảng tuổi)
            //CHuỗi trả về dưới dạng Mã chỉ số 1-Mã chỉ số 2
            string kq = "";
            List<string> them = (from data in db.r_chiso_biens
                                 where data.IDBien == int.Parse(_idbien)
                                 select data.machiso).ToList();
            foreach (string i in them)
                kq = kq + i + "-";
            kq = kq.Substring(0, kq.Length - 1);
            return kq;
        }
        public List<string> GetDSCSYHtheoIDBien(string _input)
        {
            //Trả về List các Mã chỉ số y học có sử dụng biến theo idbien truyền vào
            //(kể cả biến gốc và biến được chuyển từ biến gốc (vd khoảng tuổi)
            //CHuỗi trả về dưới dạng Mã chỉ số 1-Mã chỉ số 2
            List<string> kq = new List<string>();

            List<string> input = _input.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (string idbien in input)
            {
                kq.Add(GetCSYHtheoIDBien(idbien));
            }
            return kq;
        }
        public List<string> GetDatabienDT(int _idbien)
        {
            //Trả về danh sách giá trị của biến định tính theo idbien tương ứng (tối thiểu 2)
            List<chiso_DSbienDT> bienDTgoc = (from data in db.chiso_DSbienDTs
                                              where data.IDBien == _idbien
                                              select data).ToList();
            List<string> kq = new List<string>();

            foreach (chiso_DSbienDT i in bienDTgoc)
            {
                kq.Add(i.giatri);
            }
            return kq;
        }
        public List<Bien> GetDSBiengoc(List<Bien> input)
        {
            //Trả về danh sách Biến gốc từ danh sách Bien đưa vào
            //VD: nhóm tuổi sẽ có biến gốc là Tuổi => gom về biến Tuổi
            List<int> idbiencheck = new List<int>();
            List<Bien> kq = new List<Bien>();
            foreach (Bien i in input)
            {
                if (idbiencheck.Contains(i.idbien) || idbiencheck.Contains(i.idbiengoc))
                {
                    continue;
                }
                else
                {
                    idbiencheck.Add(i.idbien);
                    kq.Add(Getbien(i.idbien));
                }
            }
            kq = kq.OrderBy(x => x.idbien).ToList();
            return kq;
        }
        #region NCKH
        public List<Bien> GetDSBienGop(string _input)
        {
            //Truyền vào danh sách machiso dưới dạng machiso 1_machiso 2, gộp các biến sử dụng vào thành List
            //Một số biến được quy về biến gốc
            List<Bien> listdem = new List<Bien>();
            List<string> input = _input.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string machiso in input)
            {
                listdem.AddRange(GetDSbien(machiso));
            }

            List<Bien> kq = GetDSBiengoc(listdem);
            return kq;
        }
        public List<NCKH> GetNCKH(string _input)
        {
            //Truyền vào danh sách machiso dưới dạng machiso 1_machiso 2, tạo datatable (B1) để tạo file CSV
            List<NCKH> kq = new List<NCKH>();

            List<string> input = _input.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string machiso in input)
            {
                kq.Add(new NCKH(machiso, GetDSbienCSYH(GetDSbien(machiso))));
            }
            return kq;
        }
        public List<string> GetdiengiaiNCKH(string _input)
        {
            List<NCKH> listNCKH = GetNCKH(_input);
            List<string> kq = new List<string>();

            return kq;
        }
        public DataTable BangDSBienGop(string _input)
        {
            //Truyền vào danh sách machiso dưới dạng machiso 1_machiso 2, tạo datatable (B1) để tạo file CSV
            DataTable kq = new DataTable();

            List<DSBienCSYH> danhsachbien = GetDSbienCSYH(GetDSBienGop(_input));
            foreach (DSBienCSYH i in danhsachbien)
            {
                kq.Columns.Add(i.idbien.ToString(),typeof(string));
            }

            return kq;
        }
        public class NCKH
        {
            public string machiso { get; set; }
            public List<DSBienCSYH> dsidbien { get; set; }
            public NCKH()
            {
                
            }
            public NCKH(string _machiso, List<DSBienCSYH> _dsidbien)
            {
                machiso = _machiso;
                dsidbien = _dsidbien;
            }
        }
        #endregion
        public List<string> Xulycongthuc(string machiso, string input)
        {
            // truyền vào machiso và input, trả về kết quả dạng List<string>
            //Input dạng: [Giá trị biến 1]_[Giá trị biến 2]_[Giá trị biến 3]_...
            List<string> kq = new List<string>();

            //Tách input thành List theo ký tự _
            List<string> inputs = input.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //Chia nhỏ & check rồi xử lý
            if (machiso.Substring(0, 1) == "C")
            {
                #region C_A
                if (machiso.Substring(0, 3) == "C_A")
                {
                    switch (machiso)
                    {
                        case "C_A01": //2 IBW
                            {
                                IBW IBWCal = new IBW(inputs[0],
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(IBWCal.kqIBW(), 2).ToString());
                                kq.Add(IBWCal.kqIBW_diengiai());
                                break;
                            }
                        case "C_A02": //3 AdjBW
                            {
                                AdjBW AdjBWCal = new AdjBW(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(AdjBWCal.kqAdjBW(), 2).ToString());
                                kq.Add(AdjBWCal.kqAdjBW_diengiai());
                                break;
                            }
                        case "C_A03": //3 LBW
                            {
                                LBW LBWCal = new LBW(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(LBWCal.kqLBW(), 2).ToString());
                                kq.Add(LBWCal.kqLBW_diengiai());
                                break;
                            }
                        case "C_A04": //3 AlcoholSerum
                            {
                                AlcoholSerum AlcoholSerumCal = new AlcoholSerum(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(AlcoholSerumCal.kqAlcoholSerum(), 2).ToString());
                                kq.Add(AlcoholSerumCal.kqAlcoholSerum_diengiai());
                                break;
                            }
                        case "C_A05"://2 Budichbong
                            {
                                Budichbong BudichbongCal = new Budichbong(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(BudichbongCal.kqVdich24h(), 2).ToString());
                                kq.Add(Math.Round(BudichbongCal.kqtocdotruyen8h(), 2).ToString());
                                kq.Add(Math.Round(BudichbongCal.kqtocdotruyen16h(), 2).ToString());
                                kq.Add(BudichbongCal.kqVdich24h_diengiai());
                                break;
                            }
                        case "C_A06": //BMI
                            {
                                BMI BMICal = new BMI(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(BMICal.kqBMI(), 2).ToString());
                                kq.Add(BMICal.kqBMI_diengiai());
                                break;
                            }
                        case "C_A07"://7 AaG
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
                                kq.Add(AaGCal.kqAaG_diengiai());
                                break;
                            }
                        case "C_A08":
                            {
                                CalciSerum_Adj CalciSerum_AdjCal = new CalciSerum_Adj(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(CalciSerum_AdjCal.kqCalciSerum_Adj(), 2).ToString());
                                kq.Add(CalciSerum_AdjCal.kqCalciSerum_Adj_diengiai());
                                break;
                            }
                        case "C_A09":
                            {
                                BSA BSACal = new BSA(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(BSACal.kqBSA_Dub(), 2).ToString());
                                kq.Add(Math.Round(BSACal.kqBSA_Mos(), 2).ToString());
                                kq.Add(BSACal.kqBSA_diengiai());
                                break;
                            }
                        case "C_A10"://4
                            {
                                SAG SAGCal = new SAG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(SAGCal.kqSAG(), 2).ToString());
                                kq.Add(SAGCal.kqSAG_diengiai());
                                break;
                            }
                        case "C_A11"://4
                            {
                                SOG SOGCal = new SOG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(SOGCal.kqSOG(), 2).ToString());
                                kq.Add(SOGCal.kqSOG_diengiai());
                                break;
                            }
                        case "C_A12":
                            {
                                StOG StOGCal = new StOG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(StOGCal.kqStOG(), 2).ToString());
                                kq.Add(StOGCal.kqStOG_diengiai());
                                break;
                            }
                        case "C_A13":
                            {
                                UAG UAGCal = new UAG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(UAGCal.kqUAG(), 2).ToString());
                                kq.Add(UAGCal.kqUAG_diengiai());
                                break;
                            }
                        case "C_A14"://5
                            {
                                UOG UOGCal = new UOG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(UOGCal.kqUOG(), 2).ToString());
                                kq.Add(UOGCal.kqUOG_diengiai());
                                break;
                            }
                        case "C_A15"://5 CKD 5 MDRD
                            {
                                eGFR_CKD eGFR_CKDCal = new eGFR_CKD(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(eGFR_CKDCal.kqeGFR_CKD(), 2).ToString());
                                kq.Add(eGFR_CKDCal.kqeGFR_CKD_diengiai());
                                break;
                            }
                        case "C_A16": //4
                            {
                                eCrCl eCrClCal = new eCrCl(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(eCrClCal.kqeCrCl(), 2).ToString());
                                kq.Add(eCrClCal.kqeCrCl_diengiai());
                                break;
                            }
                        case "C_A17": //4
                            {
                                FEMg FEMgCal = new FEMg(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(FEMgCal.kqFEMg(), 2).ToString());
                                kq.Add(FEMgCal.kqFEMg_diengiai());
                                break;
                            }
                        case "C_A18"://4
                            {
                                FENa FENaCal = new FENa(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(FENaCal.kqFENa(), 2).ToString());
                                kq.Add(FENaCal.kqFENa_diengiai());
                                break;
                            }
                        case "C_A19": //5
                            {
                                KtVDaugirdas KtVDaugirdasCal = new KtVDaugirdas(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(KtVDaugirdasCal.kqKtVDaugirdas(), 2).ToString());
                                kq.Add(KtVDaugirdasCal.kqKtVDaugirdas_diengiai());
                                break;
                            }
                        case "C_A20"://5
                            {
                                RRF_Kru RRF_KruCal = new RRF_Kru(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(RRF_KruCal.kqRRF_Kru(), 2).ToString());
                                kq.Add(RRF_KruCal.kqRRF_Kru_diengiai());
                                break;
                            }
                        case "C_A21": //2
                            {
                                ACR ACRCal = new ACR(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(ACRCal.kqACR(), 2).ToString());
                                kq.Add(ACRCal.kqACR_diengiai());
                                break;
                            }
                        case "C_A22": //2
                            {
                                PCR PCRCal = new PCR(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(PCRCal.kqPCR(), 2).ToString());
                                kq.Add(PCRCal.kqPCR_diengiai());
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
                                kq.Add(eAERCal.kqeAER_diengiai());
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
                                kq.Add(ePERCal.kqePER_diengiai());
                                break;
                            }
                        case "C_A25": //3
                            {
                                TocDoTruyen TocDoTruyenCal = new TocDoTruyen(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(TocDoTruyenCal.kqTocDoTruyen(), 2).ToString());
                                kq.Add(TocDoTruyenCal.kqTocDoTruyen_diengiai());
                                break;
                            }
                        case "C_A26": //3
                            {
                                CrCl24h CrCl24hCal = new CrCl24h(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(CrCl24hCal.kqCrCl24h(), 2).ToString());
                                kq.Add(CrCl24hCal.kqCrCl24h_diengiai());
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
                                kq.Add(eGFR_SchwartzCal.kqeGFR_Schwartz_diengiai());
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
                                kq.Add(MPM0Cal.kqMPM0_diengiai());
                                break;
                            }
                        case "C_A29"://5 MDRD
                            {
                                eGFR_MDRD eGFR_MDRDCal = new eGFR_MDRD(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    inputs[3]);
                                kq.Add(Math.Round(eGFR_MDRDCal.kqeGFR_MDRD(), 2).ToString());
                                kq.Add(eGFR_MDRDCal.kqeGFR_MDRD_diengiai());
                                break;
                            }
                    }
                }
                #endregion
                #region C_B
                else if (machiso.Substring(0, 3) == "C_B")
                {
                    switch (machiso)
                    {
                        case "C_B01": //4
                            {
                                DLCO_Adj DLCO_AdjCal = new DLCO_Adj(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(DLCO_AdjCal.kqDLCO_Adj(), 2).ToString());
                                kq.Add(DLCO_AdjCal.kqDLCO_Adj_diengiai());
                                break;
                            }
                        case "C_B02": //2
                            {
                                MAP MAPCal = new MAP(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(MAPCal.kqMAP(), 2).ToString());
                                kq.Add(MAPCal.kqMAP_diengiai());
                                break;
                            }
                        case "C_B03": //7
                            {
                                PostFEV1 PostFEV1Cal = new PostFEV1(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    KetnoiDB.str_to_bool(inputs[4]));
                                kq.Add(Math.Round(PostFEV1Cal.kqPostFEV1(), 2).ToString());
                                kq.Add(PostFEV1Cal.kqPostFEV1_diengiai());
                                break;
                            }
                        case "C_B04": //2
                            {
                                AEC AECCal = new AEC(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(AECCal.kqAEC(), 2).ToString());
                                kq.Add(AECCal.kqAEC_diengiai());
                                break;
                            }
                        case "C_B05": //2
                            {
                                ANC ANCCal = new ANC(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(ANCCal.kqANC(), 2).ToString());
                                kq.Add(ANCCal.kqANC_diengiai());
                                break;
                            }
                        case "C_B06": //5
                            {
                                MIPI MIPICal = new MIPI(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    int.Parse(inputs[4]));
                                kq.Add(Math.Round(MIPICal.kqMIPI(), 2).ToString());
                                kq.Add(MIPICal.kqMIPI_diengiai());
                                break;
                            }
                        case "C_B07": //2
                            {
                                RPI RPICal = new RPI(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(RPICal.kqRPI(), 2).ToString());
                                kq.Add(RPICal.kqRPI_diengiai());
                                break;
                            }
                        case "C_B08": //2
                            {
                                sTfR sTfRCal = new sTfR(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(sTfRCal.kqsTfR(), 2).ToString());
                                kq.Add(sTfRCal.kqsTfR_diengiai());
                                break;
                            }
                        case "C_B09": //4
                            {
                                BMR BMRCal = new BMR(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(BMRCal.kqBMR_HB(), 2).ToString());
                                kq.Add(Math.Round(BMRCal.kqBMR_Scho(), 2).ToString());
                                kq.Add(BMRCal.kqBMR_diengiai());
                                break;
                            }
                        case "C_B10": //3
                            {
                                CDC_chieucao CDC_chieucaoCal = new CDC_chieucao(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(CDC_chieucaoCal.kqCDC_chieucao_diengiai());
                                kq.Add(Math.Round(CDC_chieucaoCal.kqCDC_chieucao_zscore(), 2).ToString());
                                break;
                            }
                        case "C_B11": //3
                            {
                                CDC_cannang CDC_cannangCal = new CDC_cannang(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(CDC_cannangCal.kqCDC_cannang_diengiai());
                                kq.Add(Math.Round(CDC_cannangCal.kqCDC_cannang_zscore(), 2).ToString());
                                break;
                            }
                        case "C_B12": //3
                            {
                                CDC_chuvi CDC_chuviCal = new CDC_chuvi(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(CDC_chuviCal.kqCDC_chuvi_diengiai());
                                kq.Add(Math.Round(CDC_chuviCal.kqCDC_chuvi_zscore(), 2).ToString());
                                break;
                            }
                        case "C_B13": //1
                            {
                                Vbudich VbudichCal = new Vbudich(double.Parse(inputs[0]));
                                kq.Add(Math.Round(VbudichCal.kqVdich24h(), 2).ToString());
                                kq.Add(Math.Round(VbudichCal.kqtocdotruyen24h(), 2).ToString());
                                kq.Add(Math.Round(VbudichCal.kqVdich_theogio(), 2).ToString());
                                kq.Add(VbudichCal.kqVdich24h_diengiai());
                                break;
                            }
                        case "C_B14": //8
                            {
                                PELD_Old PELD_OldCal = new PELD_Old(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[5]),
                                    double.Parse(inputs[6]),
                                    KetnoiDB.str_to_bool(inputs[7]));
                                kq.Add(Math.Round(PELD_OldCal.kqPELD_Old(), 2).ToString());
                                kq.Add(PELD_OldCal.kqPELD_Old_diengiai());
                                break;
                            }
                        case "C_B15": //4
                            {
                                WHO_suyDD WHO_suyDDCal = new WHO_suyDD(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(WHO_suyDDCal.kqWHO_suyDD_diengiai());
                                kq.Add(Math.Round(WHO_suyDDCal.kqWHO_chieucao_zscore(), 2).ToString());
                                kq.Add(Math.Round(WHO_suyDDCal.kqWHO_cannang_zscore(), 2).ToString());
                                break;
                            }
                        case "C_B16": //2
                            {
                                ePER_PNCT ePER_PNCTCal = new ePER_PNCT(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(ePER_PNCTCal.kqePER_PNCT(), 2).ToString());
                                kq.Add(ePER_PNCTCal.kqePER_PNCT_diengiai());
                                break;
                            }
                        case "C_B17": //6
                            {
                                OxyIndex OxyIndexCal = new OxyIndex(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[5]));
                                kq.Add(Math.Round(OxyIndexCal.kqOxyIndex(), 2).ToString());
                                kq.Add(OxyIndexCal.kqOxyIndex_diengiai());
                                break;
                            }
                        case "C_B18": //4
                            {
                                EED EEDCal = new EED(numbertodatetime(inputs[0]),
                                    numbertodatetime(inputs[1]),
                                    int.Parse(inputs[2]),
                                    KetnoiDB.str_to_bool(inputs[3]));
                                kq.Add(datetimetonumber(EEDCal.kqEED()));
                                kq.Add(EEDCal.kqTuoithai().ToString());
                                kq.Add(EEDCal.kqEED_diengiai());
                                break;
                            }
                        case "C_B19": //5
                            {
                                EER EERCal = new EER(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    inputs[4]);
                                kq.Add(Math.Round(EERCal.kqEER(), 2).ToString());
                                kq.Add(EERCal.kqEER_diengiai());
                                break;
                            }
                        case "C_B20": //3
                            {
                                CDC_BMI CDC_BMICal = new CDC_BMI(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(CDC_BMICal.kqCDC_BMI_diengiai());
                                break;
                            }
                        case "C_B21": //2
                            {
                                Noikhiquan NoikhiquanCal = new Noikhiquan(double.Parse(inputs[0]),
                                    KetnoiDB.str_to_bool(inputs[1]));
                                kq.Add(Math.Round(NoikhiquanCal.kqNoikhiquan(), 2).ToString());
                                kq.Add(NoikhiquanCal.kqNoikhiquan_diengiai());
                                break;
                            }
                        case "C_B22": //3
                            {
                                PEF PEFCal = new PEF(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(PEFCal.kqPEF(), 2).ToString());
                                kq.Add(PEFCal.kqPEF_diengiai());
                                break;
                            }
                        case "C_B23": //10
                            {
                                PELD_New PELD_NewCal = new PELD_New(inputs[0],
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[5]),
                                    double.Parse(inputs[6]),
                                    numbertodatetime(inputs[7]),
                                    numbertodatetime(inputs[8]),
                                    KetnoiDB.str_to_bool(inputs[9]));
                                kq.Add(Math.Round(PELD_NewCal.kqPELD_New(), 2).ToString());
                                kq.Add(PELD_NewCal.kqPELD_New_diengiai());
                                break;
                            }
                    }
                }
                #endregion
                #region C_C
                else
                {
                    switch (machiso)
                    {
                        case "C_C01": //2
                            {
                                NatriSerum_Adj NatriSerum_AdjCal = new NatriSerum_Adj(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(NatriSerum_AdjCal.kqNatriSerum_Adj(), 2).ToString());
                                kq.Add(NatriSerum_AdjCal.kqNatriSerum_Adj_diengiai());
                                break;
                            }
                        case "C_C02": //6
                            {
                                CardiacOutput CardiacOutputCal = new CardiacOutput(
                                    double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[5]));
                                kq.Add(Math.Round(CardiacOutputCal.kqCardiacOutput(), 2).ToString());
                                kq.Add(CardiacOutputCal.kqCardiacOutput_diengiai());
                                break;
                            }
                        case "C_C03": //4
                            {
                                FEPO4 FEPO4Cal = new FEPO4(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(FEPO4Cal.kqFEPO4(), 2).ToString());
                                kq.Add(FEPO4Cal.kqFEPO4_diengiai());
                                break;
                            }
                        case "C_C04": //3
                            {
                                LDL LDLCal = new LDL(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(LDLCal.kqLDL(), 2).ToString());
                                kq.Add(LDLCal.kqLDL_diengiai());
                                break;
                            }
                        case "C_C05": //4
                            {
                                FIB4 FIB4Cal = new FIB4(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(FIB4Cal.kqFIB4(), 2).ToString());
                                kq.Add(FIB4Cal.kqFIB4_diengiai());
                                break;
                            }
                        case "C_C06": //2
                            {
                                TSAT TSATCal = new TSAT(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]));
                                kq.Add(Math.Round(TSATCal.kqTSAT(), 2).ToString());
                                kq.Add(TSATCal.kqTSAT_diengiai());
                                break;
                            }
                        case "C_C07": //3
                            {
                                APRI APRICal = new APRI(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(APRICal.kqAPRI(), 2).ToString());
                                kq.Add(APRICal.kqAPRI_diengiai());
                                break;
                            }
                        case "C_C08": // 5 MELD
                            {
                                MELD MELDCal = new MELD(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(MELDCal.kqMELD(), 2).ToString());
                                kq.Add(MELDCal.kqMELD_diengiai());
                                break;
                            }
                        case "C_C09": // 6 MELDNa
                            {
                                MELDNa MELDNaCal = new MELDNa(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[5]));
                                kq.Add(Math.Round(MELDNaCal.kqMELDNa(), 2).ToString());
                                kq.Add(MELDNaCal.kqMELDNa_diengiai());
                                break;
                            }
                        case "C_C10": // 3 PVR
                            {
                                PVR PVRCal = new PVR(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(PVRCal.kqPVR(), 2).ToString());
                                kq.Add(PVRCal.kqPVR_diengiai());
                                break;
                            }
                        case "C_C11": //5 PVRI
                            {
                                PVRI PVRICal = new PVRI(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[5]));
                                kq.Add(Math.Round(PVRICal.kqPVRI(), 2).ToString());
                                kq.Add(PVRICal.kqPVRI_diengiai());
                                break;
                            }
                        case "C_C12": //3 AdjECG
                            {
                                AdjECG AdjECGCal = new AdjECG(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]));
                                kq.Add(Math.Round(AdjECGCal.kqAdjQT_Bazett(), 2).ToString());
                                kq.Add(Math.Round(AdjECGCal.kqAdjQT_Framingham(), 2).ToString());
                                kq.Add(Math.Round(AdjECGCal.kqAdjQT_Fridericia(), 2).ToString());
                                kq.Add(Math.Round(AdjECGCal.kqAdjQT_Hodges(), 2).ToString());
                                kq.Add(AdjECGCal.kqAdjQT_diengiai());
                                break;
                            }
                        case "C_C13": //4 SVR
                            {
                                SVR SVRCal = new SVR(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(SVRCal.kqSVR(), 2).ToString());
                                kq.Add(SVRCal.kqSVR_diengiai());
                                break;
                            }
                        case "C_C14": //4 WBCCFS_Adj
                            {
                                WBCCFS_Adj WBCCFS_AdjCal = new WBCCFS_Adj(double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]));
                                kq.Add(Math.Round(WBCCFS_AdjCal.kqWBCCFS_Adj(), 2).ToString());
                                kq.Add(WBCCFS_AdjCal.kqWBCCFS_Adj_diengiai());
                                break;
                            }
                        case "C_C15": //5 Hauphauxogan + tu vong 7n, 30n, 90n
                            {
                                Hauphauxogan HauphauxoganCal = new Hauphauxogan(
                                    double.Parse(inputs[0]),
                                    double.Parse(inputs[1]),
                                    double.Parse(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]));
                                kq.Add(Math.Round(HauphauxoganCal.kqHauphauxogan(), 2).ToString());
                                kq.Add(Math.Round(HauphauxoganCal.kqhauphau7n(), 2).ToString());
                                kq.Add(Math.Round(HauphauxoganCal.kqhauphau30n(), 2).ToString());
                                kq.Add(Math.Round(HauphauxoganCal.kqhauphau90n(), 2).ToString());
                                kq.Add(HauphauxoganCal.kqhauphau_diengiai());
                                break;
                            }
                        case "C_C16": //12 MESA Score, CAC & khong CAC
                            {
                                if (inputs.Count() == 11)
                                {
                                    MESA_SCORE MESA_SCORECal = new MESA_SCORE(
                                        inputs[0],
                                        double.Parse(inputs[1]),
                                        double.Parse(inputs[2]),
                                        double.Parse(inputs[3]),
                                        double.Parse(inputs[4]),
                                        KetnoiDB.str_to_bool(inputs[5]),
                                        KetnoiDB.str_to_bool(inputs[6]),
                                        KetnoiDB.str_to_bool(inputs[7]),
                                        inputs[8],
                                        KetnoiDB.str_to_bool(inputs[9]),
                                        KetnoiDB.str_to_bool(inputs[10]));
                                    kq.Add(Math.Round(MESA_SCORECal.kqMESA_SCORE_nonCAC(), 2).ToString());
                                    kq.Add(MESA_SCORECal.kqMESA_diengiai());
                                }
                                else
                                {
                                    MESA_SCORE MESA_SCORECal = new MESA_SCORE(
                                        inputs[0],
                                        double.Parse(inputs[1]),
                                        double.Parse(inputs[2]),
                                        double.Parse(inputs[3]),
                                        double.Parse(inputs[4]),
                                        KetnoiDB.str_to_bool(inputs[5]),
                                        KetnoiDB.str_to_bool(inputs[6]),
                                        KetnoiDB.str_to_bool(inputs[7]),
                                        inputs[8],
                                        KetnoiDB.str_to_bool(inputs[9]),
                                        KetnoiDB.str_to_bool(inputs[10]),
                                        double.Parse(inputs[11]));
                                    kq.Add(Math.Round(MESA_SCORECal.kqMESA_SCORE_CAC(), 2).ToString());
                                }

                                break;
                            }
                    }
                }
                #endregion
            }
            else
            {
                #region T_A
                if (machiso.Substring(0, 3) == "T_A")
                {
                    switch (machiso)
                    {
                        case "T_A01": //GRACE 9 var: 7 7 7 7 2 2 2 2 2
                            {
                                GRACE GRACECal = new GRACE(input);
                                kq.Add(GRACECal.kqGRACE().ToString());
                                kq.AddRange(GRACECal.kqGRACE_diengiai());
                                break;
                            }
                        case "T_A02": //COWS 11 var: 4 5 4 4 4 4 5 4 4 4 3
                            {
                                COWS COWSCal = new COWS(input);
                                kq.Add(COWSCal.kqCOWS().ToString());
                                kq.AddRange(COWSCal.kqCOWS_diengiai());
                                break;
                            }
                        case "T_A03": //qSOFA 3:var: 2 2 2
                            {
                                qSOFA qSOFACal = new qSOFA(input);
                                kq.Add(qSOFACal.kqqSOFA().ToString());
                                kq.AddRange(qSOFACal.kqqSOFA_diengiai());
                                break;
                            }
                        case "T_A04": //VNTM 8 var: 3,5,2,2,2,7,5,2
                            {
                                VNTM VNTMCal = new VNTM(input);
                                kq.Add(VNTMCal.kqVNTM_Chinh().ToString()); //tieu chuan chinh
                                kq.Add(VNTMCal.kqVNTM_Phu().ToString()); //tieu chuan phu
                                kq.AddRange(VNTMCal.kqVNTM_diengiai());
                                break;
                            }
                        case "T_A05": //MalHyperthermia 10 var: 3,7,7,3,3,2,2,2,2,2
                            {
                                MalHyperthermia MalHyperthermiaCal = new MalHyperthermia(input);
                                kq.Add(MalHyperthermiaCal.kqMalHyperthermia().ToString());
                                kq.AddRange(MalHyperthermiaCal.kqMalHyperthermia_diengiai());
                                break;
                            }
                        case "T_A06": //PSI 21 var: 2,0 (tuoi),2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2
                            {
                                PSI PSICal = new PSI(input);
                                kq.Add(PSICal.kqPSI().ToString());
                                kq.AddRange(PSICal.kqPSI_diengiai());
                                break;
                            }
                        case "T_A07": //VCSS 10 var: 4,4,4,4,4,4,4,4,4,4
                            {
                                VCSS VCSSCal = new VCSS(input);
                                kq.Add(VCSSCal.kqVCSS().ToString());
                                kq.AddRange(VCSSCal.kqVCSS_diengiai());
                                break;
                            }
                        case "T_A08": //BISAP 10 var: 2,2,2,2,2,2,2,2,2,2
                            {
                                BISAP BISAPCal = new BISAP(input);
                                kq.Add(BISAPCal.kqBISAP().ToString());
                                kq.AddRange(BISAPCal.kqBISAP_diengiai());
                                break;
                            }
                        case "T_A09": //Blatchford 8 var: 5,6,4,2,2,2,2,2
                            {
                                Blatchford BlatchfordCal = new Blatchford(input);
                                kq.Add(BlatchfordCal.kqBlatchford().ToString());
                                kq.AddRange(BlatchfordCal.kqBlatchford_diengiai());
                                break;
                            }
                        case "T_A10": //Rockall 7 var: 3,2,2,3,3,3,2
                            {
                                Rockall RockallCal = new Rockall(input);
                                kq.Add(RockallCal.kqRockall().ToString());
                                kq.AddRange(RockallCal.kqRockall_diengiai());
                                break;
                            }
                        case "T_A11": //ChildPugh 5 var: 5,3,3,3,3
                            {
                                ChildPugh ChildPughCal = new ChildPugh(input);
                                kq.Add(ChildPughCal.kqChildPugh().ToString());
                                kq.AddRange(ChildPughCal.kqChildPugh_diengiai());
                                break;
                            }
                        case "T_A12": //CLIF-SOFA 10 var: 0,0,0,5,5,5,2,3,5,5
                            {
                                CLIFSOFA CLIFSOFACal = new CLIFSOFA(input);
                                kq.Add(CLIFSOFACal.kqCLIFSOFA().ToString());
                                kq.AddRange(CLIFSOFACal.kqCLIFSOFA_diengiai());
                                break;
                            }
                        case "T_A13": //HBCrohn	12 var: 5,4,0,4,2,2,2,2,2,2,2,2
                            {
                                HBCrohn HBCrohnCal = new HBCrohn(input);
                                kq.Add(HBCrohnCal.kqHBCrohn().ToString());
                                kq.AddRange(HBCrohnCal.kqHBCrohn_diengiai());
                                break;
                            }
                        case "T_A14": //GlasgowComa 3 var: 4,5,6
                            {
                                GlasgowComa GCSCal = new GlasgowComa(input);
                                kq.Add(GCSCal.kqGlasgowComa().ToString());
                                kq.AddRange(GCSCal.kqGlasgowComa_diengiai());
                                break;
                            }
                        case "T_A15": //Ranson 11 var: 2,2,2,2,2,2,2,2,2,2,2
                            {
                                Ranson RansonCal = new Ranson(input);
                                kq.Add(RansonCal.kqRanson().ToString());
                                kq.AddRange(RansonCal.kqRanson_diengiai());
                                break;
                            }
                        case "T_A16": //IVPO 6 var: 2,2,2,2,2,2
                            {
                                IVPO IVPOCal = new IVPO(input);
                                kq.Add(IVPOCal.kqIVPO().ToString());
                                kq.AddRange(IVPOCal.kqIVPO_diengiai());
                                break;
                            }
                        case "T_A17": //PUMayoClinic 4 var: 4,4,4,4
                            {
                                PUMayoClinic PUMayoClinicCal = new PUMayoClinic(input);
                                kq.Add(PUMayoClinicCal.kqPUMayoClinic().ToString());
                                kq.AddRange(PUMayoClinicCal.kqPUMayoClinic_diengiai());
                                break;
                            }
                        case "T_A18": //CDAICrohn 15 var: 0,0,0,0,0,2,4,5,2,2,2,2,2,2,3
                            {
                                CDAICrohn CDAICrohnCal = new CDAICrohn(input);
                                kq.Add(CDAICrohnCal.kqCDAICrohn().ToString());
                                kq.AddRange(CDAICrohnCal.kqCDAICrohn_diengiai());
                                break;
                            }
                    }
                }
                #endregion
                #region T_B
                else if (machiso.Substring(0, 3) == "T_B")
                {
                    switch (machiso)
                    {
                        case "T_B01": //APACHE2 22 var: 5,4,4,4,2,4,4,2,5,5,5,5,4,4,4,0,5,2,3,2,35,22
                            {
                                APACHE2 APACHE2Cal = new APACHE2(input);
                                kq.Add(APACHE2Cal.kqAPACHE2().ToString());
                                kq.AddRange(APACHE2Cal.kqAPACHE2_diengiai());
                                break;
                            }
                        case "T_B02": //BODECOPD 4 var: 4,4,4,2
                            {
                                BODECOPD BODECOPDCal = new BODECOPD(input);
                                kq.Add(BODECOPDCal.kqBODECOPD().ToString());
                                kq.AddRange(BODECOPDCal.kqBODECOPD_diengiai());
                                break;
                            }
                        case "T_B03": //CURB-65 7 var: 2,2,2,2,2,2,2
                            {
                                CURB65 CURB65Cal = new CURB65(input);
                                kq.Add(CURB65Cal.kqCURB65().ToString());
                                kq.AddRange(CURB65Cal.kqCURB65_diengiai());
                                break;
                            }
                        case "T_B04": //Light 8 var: 0,0,0,0,0,2,2,2
                            {
                                Light LightCal = new Light(input);
                                kq.Add(LightCal.kqLight().ToString());
                                kq.AddRange(LightCal.kqLight_diengiai());
                                break;
                            }
                        case "T_B05": //GenevaDVT 19 var: 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2
                            {
                                GenevaDVT GenevaDVTCal = new GenevaDVT(input);
                                kq.Add(GenevaDVTCal.kqGenevaDVT().ToString());
                                kq.AddRange(GenevaDVTCal.kqGenevaDVT_diengiai());
                                break;
                            }
                        case "T_B06": //GenevaPE 9 var: 2,2,2,2,2,2,2,3,2
                            {
                                GenevaPE GenevaPECal = new GenevaPE(input);
                                kq.Add(GenevaPECal.kqGenevaPE().ToString());
                                kq.AddRange(GenevaPECal.kqGenevaPE_diengiai());
                                break;
                            }
                        case "T_B07": //WellsDVT 11 var: 2,2,2,2,2,2,2,2,2,2,2
                            {
                                WellsDVT WellsDVTCal = new WellsDVT(input);
                                kq.Add(WellsDVTCal.kqWellsDVT().ToString());
                                kq.AddRange(WellsDVTCal.kqWellsDVT_diengiai());
                                break;
                            }
                        case "T_B08": //NEWS2 10 var: 4,0,2,2,4,4,2,4,4,4
                            {
                                NEWS2 NEWS2Cal = new NEWS2(input);
                                kq.Add(NEWS2Cal.kqNEWS2().ToString());
                                kq.AddRange(NEWS2Cal.kqNEWS2_diengiai());
                                break;
                            }
                        case "T_B09": //PaduaVTE 14 var: 2,2,2,2,2,2,2,2,2,2,2,2,2,2
                            {
                                PaduaVTE PaduaVTECal = new PaduaVTE(input);
                                kq.Add(PaduaVTECal.kqPaduaVTE().ToString());
                                kq.AddRange(PaduaVTECal.kqPaduaVTE_diengiai());
                                break;
                            }
                        case "T_B10": //WellsPE 7 var: 2,2,2,2,2,2,2
                            {
                                WellsPE WellsPECal = new WellsPE(input);
                                kq.Add(WellsPECal.kqWellsPE().ToString());
                                kq.AddRange(WellsPECal.kqWellsPE_diengiai());
                                break;
                            }
                        case "T_B11": //SOFA 10 var: 0,0,2,5,5,5,2,3,5,5
                            {
                                SOFA SOFACal = new SOFA(input);
                                kq.Add(SOFACal.kqSOFA().ToString());
                                kq.AddRange(SOFACal.kqSOFA_diengiai());
                                break;
                            }

                        case "T_B12": //VTE-BLEED 6 var: 2,2,2,2,3,2
                            {
                                VTEBLEED VTEBLEEDCal = new VTEBLEED(input);
                                kq.Add(VTEBLEEDCal.kqVTEBLEED().ToString());
                                kq.AddRange(VTEBLEEDCal.kqVTEBLEED_diengiai());
                                break;
                            }

                        case "T_B13": //HIT 8 var: 3,3,0,0,3,3,3,3
                            {
                                HeparinIT HeparinITCal = new HeparinIT(input);
                                kq.Add(HeparinITCal.kqHeparinIT().ToString());
                                kq.AddRange(HeparinITCal.kqHeparinIT_diengiai());
                                break;
                            }

                        case "T_B14": //HAS-BLED 8 var: 2,2,2,2,2,2,2,2
                            {
                                HASBLED HASBLEDCal = new HASBLED(input);
                                kq.Add(HASBLEDCal.kqHASBLED().ToString());
                                kq.AddRange(HASBLEDCal.kqHASBLED_diengiai());
                                break;
                            }

                        case "T_B15": //DIPSS-PlusPMS 10 var: 2,2,2,2,2,2,2,2,2,2
                            {
                                DIPSSPlusPMS DIPSSPlusPMSCal = new DIPSSPlusPMS(input);
                                kq.Add(DIPSSPlusPMSCal.kqDIPSS().ToString());
                                kq.AddRange(DIPSSPlusPMSCal.kqDIPSS_diengiai());
                                kq.Add(DIPSSPlusPMSCal.kqDIPSSPlus().ToString());
                                kq.AddRange(DIPSSPlusPMSCal.kqDIPSSPlus_diengiai());
                                break;
                            }
                        case "T_B16": //IPSHodgkin 7 var: 2,2,2,4,2,2,2
                            {
                                IPSHodgkin IPSHodgkinCal = new IPSHodgkin(input);
                                kq.Add(IPSHodgkinCal.kqIPSHodgkin().ToString());
                                kq.AddRange(IPSHodgkinCal.kqIPSHodgkin_diengiai());
                                break;
                            }

                        case "T_B17": //GIPSSXotuy 5 var: 3,2,2,2,2
                            {
                                GIPSSXotuy GIPSSXotuyCal = new GIPSSXotuy(input);
                                kq.Add(GIPSSXotuyCal.kqGIPSSXotuy().ToString());
                                kq.AddRange(GIPSSXotuyCal.kqGIPSSXotuy_diengiai());
                                break;
                            }

                        case "T_B18": //IPSNonHodgkin 5 var: 2,2,4,2,2
                            {
                                IPSNonHodgkin IPSNonHodgkinCal = new IPSNonHodgkin(input);
                                kq.Add(IPSNonHodgkinCal.kqIPSNonHodgkin().ToString());
                                kq.AddRange(IPSNonHodgkinCal.kqIPSNonHodgkin_diengiai());
                                break;
                            }

                        case "T_B19": //Khorana 6 var: 3,2,2,2,2,2
                            {
                                Khorana KhoranaCal = new Khorana(input);
                                kq.Add(KhoranaCal.kqKhorana().ToString());
                                kq.AddRange(KhoranaCal.kqKhorana_diengiai());
                                break;
                            }

                        case "T_B20": //MDACC 8 var: 3,2,2,4,2,2,3,2
                            {
                                MDACC MDACCCal = new MDACC(input);
                                kq.Add(MDACCCal.kqMDACC().ToString());
                                kq.AddRange(MDACCCal.kqMDACC_diengiai());
                                break;
                            }

                        case "T_B21": //MDSRLsinhtuy 6 var: 4,3,2,2,2,2
                            {
                                MDSRLsinhtuy MDSRLsinhtuyCal = new MDSRLsinhtuy(input);
                                kq.Add(MDSRLsinhtuyCal.kqMDSRLsinhtuy().ToString());
                                kq.AddRange(MDSRLsinhtuyCal.kqMDSRLsinhtuy_diengiai());
                                break;
                            }

                        case "T_B22": //Sokal 4 var: 0,0,0,0
                            {
                                Sokal SokalCal = new Sokal(input);
                                kq.Add(SokalCal.kqSokal().ToString());
                                kq.AddRange(SokalCal.kqSokal_diengiai());
                                break;
                            }

                        case "T_B23": //APGAR 5 var: 3,3,3,3,3
                            {
                                APGAR APGARCal = new APGAR(input);
                                kq.Add(APGARCal.kqAPGAR().ToString());
                                kq.AddRange(APGARCal.kqAPGAR_diengiai());
                                break;
                            }

                        case "T_B24": //PUCAI 6 var: 3,4,3,4,2,3
                            {
                                PUCAI PUCAICal = new PUCAI(input);
                                kq.Add(PUCAICal.kqPUCAI().ToString());
                                kq.AddRange(PUCAICal.kqPUCAI_diengiai());
                                break;
                            }
                        case "T_B25": //WestleyCroup 5 var: 2,3,3,3,4
                            {
                                WestleyCroup WestleyCroupCal = new WestleyCroup(input);
                                kq.Add(WestleyCroupCal.kqWestleyCroup().ToString());
                                kq.AddRange(WestleyCroupCal.kqWestleyCroup_diengiai());
                                break;
                            }

                        case "T_B26": //CMMLMayoClinic 6 var: 0,0,2,2,2,2
                            {
                                CMMLMayoClinic CMMLMayoClinicCal = new CMMLMayoClinic(input);
                                kq.Add(CMMLMayoClinicCal.kqCMMLMayoClinic().ToString());
                                kq.AddRange(CMMLMayoClinicCal.kqCMMLMayoClinic_diengiai());
                                break;
                            }

                        case "T_B27": //EUTOS 4 var: 0,0,0,0
                            {
                                EUTOS EUTOSCal = new EUTOS(input);
                                kq.Add(EUTOSCal.kqEUTOS().ToString());
                                kq.AddRange(EUTOSCal.kqEUTOS_diengiai());
                                break;
                            }

                        case "T_B28": //PASRuotthua 8 var: 2,2,2,2,2,2,2,2
                            {
                                PASRuotthua PASRuotthuaCal = new PASRuotthua(input);
                                kq.Add(PASRuotthuaCal.kqPASRuotthua().ToString());
                                kq.AddRange(PASRuotthuaCal.kqPASRuotthua_diengiai());
                                break;
                            }

                        case "T_B29": //GlasgowNhiB2 3 var: 4,5,6
                            {
                                GlasgowNhiB2 GlasgowNhiB2Cal = new GlasgowNhiB2(input);
                                kq.Add(GlasgowNhiB2Cal.kqGlasgowNhiB2().ToString());
                                kq.AddRange(GlasgowNhiB2Cal.kqGlasgowNhiB2_diengiai());
                                break;
                            }
                        case "T_B30": //STOP-BangS 8 var: 2,2,2,2,2,2,2,2
                            {
                                STOPBangS STOPBangSCal = new STOPBangS(input);
                                kq.Add(STOPBangSCal.kqSTOP().ToString());
                                kq.Add(STOPBangSCal.kqBang().ToString());
                                kq.AddRange(STOPBangSCal.kqSTOPBangS_diengiai());
                                break;
                            }

                        case "T_B31": //IPSS-RLoansantuy 5 var: 5,4,3,3,2
                            {
                                IPSSRLoansantuy IPSSRLoansantuyCal = new IPSSRLoansantuy(input);
                                kq.Add(IPSSRLoansantuyCal.kqIPSSRLoansantuy().ToString());
                                kq.AddRange(IPSSRLoansantuyCal.kqIPSSRLoansantuy_diengiai());
                                break;
                            }
                        case "T_B32": //GlasgowNhiO2 3 var: 4,5,6
                            {
                                GlasgowNhiO2 GlasgowNhiO2Cal = new GlasgowNhiO2(input);
                                kq.Add(GlasgowNhiO2Cal.kqGlasgowNhiO2().ToString());
                                kq.AddRange(GlasgowNhiO2Cal.kqGlasgowNhiO2_diengiai());
                                break;
                            }
                    }
                }
                #endregion
                #region T_C
                else
                {
                    switch (machiso)
                    {
                        case "T_C01": //FraminghamE 8 var: 2,0,0,0,0,2,2,2
                            {
                                FraminghamE FraminghamECal = new FraminghamE(input);
                                kq.Add(FraminghamECal.kqFraminghamE().ToString());
                                kq.AddRange(FraminghamECal.kqFraminghamE_diengiai());
                                break;
                            }

                        case "T_C02": //ACCAHA 9 var: 5,2,0,0,0,0,2,2,2
                            {
                                ACCAHA ACCAHACal = new ACCAHA(input);
                                kq.Add(ACCAHACal.kqACCAHA().ToString());
                                kq.AddRange(ACCAHACal.kqACCAHA_diengiai());
                                break;
                            }

                        case "T_C03": //CHA2DS2-VASc 7 var: 2,3,2,2,2,2,2
                            {
                                CHA2DS2VASc CHA2DS2VAScCal = new CHA2DS2VASc(input);
                                kq.Add(CHA2DS2VAScCal.kqCHA2DS2VASc().ToString());
                                kq.AddRange(CHA2DS2VAScCal.kqCHA2DS2VASc_diengiai());
                                break;
                            }

                        case "T_C04": //TIMINonST 12 var: 2,2,2,2,2,2,2,2,2,2,2,2
                            {
                                TIMINonST TIMINonSTCal = new TIMINonST(input);
                                kq.Add(TIMINonSTCal.kqTIMINonST().ToString());
                                kq.AddRange(TIMINonSTCal.kqTIMINonST_diengiai());
                                break;
                            }

                        case "T_C29": //TIMIST 11 var: 3,2,2,2,2,2,4,2,2,2,2
                            {
                                TIMIST TIMISTCal = new TIMIST(input);
                                kq.Add(TIMISTCal.kqTIMIST().ToString());
                                kq.AddRange(TIMISTCal.kqTIMIST_diengiai());
                                break;
                            }

                        case "T_C05": //ARISCAT 8 var: 3,3,2,2,2,2,2,3
                            {
                                ARISCAT ARISCATCal = new ARISCAT(input);
                                kq.Add(ARISCATCal.kqARISCAT().ToString());
                                kq.AddRange(ARISCATCal.kqARISCAT_diengiai());
                                break;
                            }

                        case "T_C06": //IPSSTienliet 7 var: 6,6,6,6,6,6,6
                            {
                                IPSSTienliet IPSSTienlietCal = new IPSSTienliet(input);
                                kq.Add(IPSSTienlietCal.kqIPSSTienliet().ToString());
                                kq.AddRange(IPSSTienlietCal.kqIPSSTienliet_diengiai());
                                break;
                            }

                        case "T_C07": //ABCD2 6 var: 2,2,2,3,3,2
                            {
                                ABCD2 ABCD2Cal = new ABCD2(input);
                                kq.Add(ABCD2Cal.kqABCD2().ToString());
                                kq.AddRange(ABCD2Cal.kqABCD2_diengiai());
                                break;
                            }

                        case "T_C08": //ESS 8 var: 4,4,4,4,4,4,4,4
                            {
                                ESS ESSCal = new ESS(input);
                                kq.Add(ESSCal.kqESS().ToString());
                                kq.AddRange(ESSCal.kqESS_diengiai());
                                break;
                            }

                        case "T_C09": //NIH 15 var: 4,3,3,3,4,4,6,6,6,6,4,3,4,4,3
                            {
                                NIH NIHCal = new NIH(input);
                                kq.Add(NIHCal.kqNIH().ToString());
                                kq.AddRange(NIHCal.kqNIH_diengiai());
                                break;
                            }

                        case "T_C10": //RoPE 6 var: 2,2,2,2,2,6
                            {
                                RoPE RoPECal = new RoPE(input);
                                kq.Add(RoPECal.kqRoPE().ToString());
                                kq.AddRange(RoPECal.kqRoPE_diengiai());
                                break;
                            }
                        case "T_C11": //FraminghamS 10 var: 0,0,2,2,2,2,2,2,2,3
                            {
                                FraminghamS FraminghamSCal = new FraminghamS(input);
                                kq.Add(FraminghamSCal.kqFraminghamS().ToString());
                                kq.AddRange(FraminghamSCal.kqFraminghamS_diengiai());
                                break;
                            }

                        case "T_C12": //GAD7 7 var: 4,4,4,4,4,4,4
                            {
                                GAD7 GAD7Cal = new GAD7(input);
                                kq.Add(GAD7Cal.kqGAD7().ToString());
                                kq.AddRange(GAD7Cal.kqGAD7_diengiai());
                                break;
                            }

                        case "T_C13": //PHQ9 9 var: 4,4,4,4,4,4,4,4,4
                            {
                                PHQ9 PHQ9Cal = new PHQ9(input);
                                kq.Add(PHQ9Cal.kqPHQ9().ToString());
                                kq.AddRange(PHQ9Cal.kqPHQ9_diengiai());
                                break;
                            }

                        case "T_C14": //Caprini 34 var: 4,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0
                            {
                                Caprini CapriniCal = new Caprini(input);
                                kq.Add(CapriniCal.kqCaprini().ToString());
                                kq.AddRange(CapriniCal.kqCaprini_diengiai());
                                break;
                            }

                        case "T_C15": //Eckardt 4 var: 4,4,4,4
                            {
                                Eckardt EckardtCal = new Eckardt(input);
                                kq.Add(EckardtCal.kqEckardt().ToString());
                                kq.AddRange(EckardtCal.kqEckardt_diengiai());
                                break;
                            }

                        case "T_C16": //LAR 5 var: 3,2,4,3,3
                            {
                                LAR LARCal = new LAR(input);
                                kq.Add(LARCal.kqLAR().ToString());
                                kq.AddRange(LARCal.kqLAR_diengiai());
                                break;
                            }

                        case "T_C17": //MESS 4 var: 3,4,6,3
                            {
                                MESS MESSCal = new MESS(input);
                                kq.Add(MESSCal.kqMESS().ToString());
                                kq.AddRange(MESSCal.kqMESS_diengiai());
                                break;
                            }

                        case "T_C18": //Braden 6 var: 4,4,4,4,4,3
                            {
                                Braden BradenCal = new Braden(input);
                                kq.Add(BradenCal.kqBraden().ToString());
                                kq.AddRange(BradenCal.kqBraden_diengiai());
                                break;
                            }

                        case "T_C19": //VSD_Obs 8 var: 2,2,2,2,2,2,2,2
                            {
                                VSD_Obs VSD_ObsCal = new VSD_Obs(input);
                                kq.Add(VSD_ObsCal.kqVSD_Obs().ToString());
                                kq.AddRange(VSD_ObsCal.kqVSD_Obs_diengiai());
                                break;
                            }

                        case "T_C30": //VSD-Ref 9 var: 2,2,2,2,2,2,2,2,2
                            {
                                VSD_Ref VSD_RefCal = new VSD_Ref(input);
                                kq.Add(VSD_RefCal.kqVSD_Ref().ToString());
                                kq.AddRange(VSD_RefCal.kqVSD_Ref_diengiai());
                                break;
                            }
                        case "T_C20": //Villalta	12 var: 4,4,4,4,4,4,4,4,4,4,4,2
                            {
                                Villalta VillaltaCal = new Villalta(input);
                                kq.Add(VillaltaCal.kqVillalta().ToString());
                                kq.AddRange(VillaltaCal.kqVillalta_diengiai());
                                break;
                            }

                        case "T_C21": //RA-CDAI 60 var: 0, 0, 56 var, 0, 0
                            {
                                RA_CDAI RA_CDAICal = new RA_CDAI(input);
                                kq.Add(RA_CDAICal.kqRA_CDAI().ToString());
                                kq.AddRange(RA_CDAICal.kqRA_CDAI_diengiai());
                                break;
                            }

                        case "T_C22": //RA-SDAI 61 var: 0, 0, 56 var, 0, 0, 0
                            {
                                RA_SDAI RA_SDAICal = new RA_SDAI(input);
                                kq.Add(RA_SDAICal.kqRA_SDAI().ToString());
                                kq.AddRange(RA_SDAICal.kqRA_SDAI_diengiai());
                                break;
                            }

                        case "T_C23": //DAS28CRP 60 var: 0, 56 var, 0, 0, 0
                            {
                                DAS28CRP DAS28CRPCal = new DAS28CRP(input);
                                kq.Add(DAS28CRPCal.kqDAS28CRP().ToString());
                                kq.AddRange(DAS28CRPCal.kqDAS28CRP_diengiai());
                                break;
                            }

                        case "T_C24": //DAS28ESR 60 var: 0, 56 var, 0, 0, 0
                            {
                                DAS28ESR DAS28ESRCal = new DAS28ESR(input);
                                kq.Add(DAS28ESRCal.kqDAS28ESR().ToString());
                                kq.AddRange(DAS28ESRCal.kqDAS28ESR_diengiai());
                                break;
                            }

                        case "T_C25": //ISI	7 var: 5,5,5,5,5,5,5
                            {
                                ISI ISICal = new ISI(input);
                                kq.Add(ISICal.kqISI().ToString());
                                kq.AddRange(ISICal.kqISI_diengiai());
                                break;
                            }
                        case "T_C26": //SCORE2
                            {
                                SCORE2 SCORE2Cal = new SCORE2(double.Parse(inputs[0]),
                                    inputs[1],
                                    KetnoiDB.str_to_bool(inputs[2]),
                                    double.Parse(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[6]),
                                    inputs[7]);

                                kq.Add(SCORE2Cal.kqSCORE2().ToString());
                                kq.Add(SCORE2Cal.kqSCORE2_diengiai());
                                break;
                            }
                        case "T_C27": //SCORE2_DM 10 var
                            {
                                SCORE2_DM SCORE2_DMCal = new SCORE2_DM(double.Parse(inputs[0]),
                                    inputs[1],
                                    double.Parse(inputs[2]),
                                    KetnoiDB.str_to_bool(inputs[3]),
                                    double.Parse(inputs[4]),
                                    double.Parse(inputs[5]),
                                    double.Parse(inputs[6]),
                                    double.Parse(inputs[7]),
                                    double.Parse(inputs[8]),
                                    inputs[9]);

                                kq.Add(SCORE2_DMCal.kqSCORE2_DM().ToString());
                                kq.Add(SCORE2_DMCal.kqPLNguycoSCORE2_DM());
                                break;
                            }
                        case "T_C28": //SCORED	9 var: 4,2,2,2,2,2,2,2,2
                            {
                                SCORED SCOREDCal = new SCORED(input);
                                kq.Add(SCOREDCal.kqSCORED().ToString());
                                kq.AddRange(SCOREDCal.kqSCORED_diengiai());
                                break;
                            }
                    }
                }
                #endregion
            }
            return kq;
        }
        public double GetDiembienDT(int _idbien, int _thutu)
        {
            //Trả về điểm của biến định tính với idbien và thứ tự nhập tương ứng
            double kq = (from data in db.chiso_DSbienDTs
                         where data.IDBien == _idbien && data.thutu == _thutu
                         select data.diem).FirstOrDefault();
            return kq;
        }
        public chiso_DSBienKQ GetBienKQ(string _machiso)
        {
            //Trả về biến kết quả theo mã chỉ số tương ứng để đánh giá điểm đã tính
            chiso_DSBienKQ kq = (from data in db.chiso_DSBienKQs
                                 where data.machiso == _machiso
                                 select data).FirstOrDefault();
            return kq;
        }
        public List<chiso_GTBienKQ> GetGTBienKQ(int idbienkq)
        {
            //Trả về danh sách giá trị của biến kết quả theo idbienkq cung cấp (để dò diễn giải, đánh giá)
            List<chiso_GTBienKQ> kq = (from data in db.chiso_GTBienKQs
                                       where data.IDBienKQ == idbienkq
                                       select data).ToList();
            return kq;
        }
        public List<string> GetDiengiaiKQ(string _machiso, double _diem)
        {
            //Dò diễn giải và đánh giá theo mã chỉ số và điểm tương ứng
            //Dùng khi BienKQ CHỈ CÓ 1 idbienkq
            List<string> kq = new List<string>();

            chiso_DSBienKQ Bienkq = GetBienKQ(_machiso);
            List<chiso_GTBienKQ> DSGiatribienkq = GetGTBienKQ(Bienkq.IDBienKQ);

            kq.Add(Bienkq.TendayduKQ);

            foreach (chiso_GTBienKQ i in DSGiatribienkq)
            {
                if (_diem >= i.DiemLL && _diem <= i.DiemUL)
                    kq.Add(i.Diengiai);
            }

            return kq;
        }
        public List<string> GetDiengiaiKQ_2(string _machiso, double _diem, int _idbienkq)
        {
            //Dò diễn giải và đánh giá theo mã chỉ số và điểm tương ứng
            //Dùng khi BienKQ CÓ 2 idbienkq riêng (vd chỉ số DIPSSPLus có DIPSS và DIPSS Plus)
            List<string> kq = new List<string>();

            chiso_DSBienKQ Bienkq = GetBienKQ(_machiso);
            List<chiso_GTBienKQ> DSGiatribienkq = GetGTBienKQ(_idbienkq);

            kq.Add(Bienkq.TendayduKQ);

            foreach (chiso_GTBienKQ i in DSGiatribienkq)
            {
                if (_diem >= i.DiemLL && _diem <= i.DiemUL)
                    kq.Add(i.Diengiai);
            }

            return kq;
        }
        public int phannhomDTDL(int _idbien, double _giatri)
        {
            //Trả về thứ tự nhập của biến ĐỊNH TÍNH ĐỊNH LƯỢNG từ idbien và giá trị ĐỊNH LƯỢNG
            List<GiatribienDT> dataDT = GetGiatribienDT(_idbien);
            int thutu = 0;
            foreach (GiatribienDT i in dataDT)
            {
                if (GetkqinputGioihan(i.limit, _giatri))
                    thutu = i.thutu;
            }
            return thutu;
        }
        public static bool GetkqinputGioihan(string _gioihan, double _input)
        {
            //Xử lý chuỗi giới hạn
            //Trả về true nếu input nằm trong CÁC khoảng giới hạn (tra từ CSDL, tự động)
            bool kq = false;
            if (_gioihan.Contains('U'))
            {
                List<Gioihan> dscheck = GetListGioihan(_gioihan);
                foreach (Gioihan check in dscheck)
                {
                    kq = kq || checkinputGioihan(check, _input);
                }
            }
            else
            {
                Gioihan check = Getgioihan(_gioihan);
                kq = checkinputGioihan(check, _input);
            }
            return kq;
        }
        public static bool checkinputGioihan(Gioihan _gioihan, double _input)
        {
            //Trả về true nếu input nằm trong khoảng giới hạn (truyền vào từ method xử lý chuỗi ở trên)
            bool kq;

            if (_gioihan.equalLL && _gioihan.equalUL)
                kq = _input >= _gioihan.LL && _input <= _gioihan.UL;
            else if (_gioihan.equalLL)
                kq = _input >= _gioihan.LL && _input < _gioihan.UL;
            else if (_gioihan.equalUL)
                kq = _input > _gioihan.LL && _input <= _gioihan.UL;
            else
                kq = _input > _gioihan.LL && _input < _gioihan.UL;

            return kq;
        }
        private static List<Gioihan> GetListGioihan(string _gioihan)
        {
            //Xử lý chuỗi ĐA giới hạn thành từng khoảng giới hạn
            string[] tachGioihan = _gioihan.Split('U');

            List<Gioihan> kq = new List<Gioihan>();

            foreach (string i in tachGioihan)
            {
                kq.Add(Getgioihan(i));
            }

            return kq;
        }
        private static Gioihan Getgioihan(string _gioihan)
        {
            //Xử lý chuỗi ĐƠN giới hạn thành từng khoảng giới hạn, được gọi từ method xử lý chuỗi đa
            string[] parts = _gioihan.Trim(new char[] { '(', '[', ')', ']' }).Split(':');

            if (parts.Length == 2)
            {
                double lowerLimit = double.Parse(parts[0]);
                double upperLimit = double.Parse(parts[1]);

                bool equalLL = _gioihan[0] == '[';
                bool equalUL = _gioihan[_gioihan.Length - 1] == ']';

                return new Gioihan(lowerLimit, upperLimit, equalLL, equalUL);
            }
            else
            {
                throw new ArgumentException("Lỗi khoảng giới hạn: " + _gioihan);
            }
        }
        public static bool str_to_bool(string input)
        {
            //Chuyển chuỗi input thành boolean, bao gồm cả giá trị 1, 0 hoặc Y, N
            if (input.ToLower() == "true" || input.ToLower() == "1" || input.ToLower() == "y")
                return true;
            else if (input.ToLower() == "false" || input.ToLower() == "0" || input.ToLower() == "n")
                return false;
            else
                throw new ArgumentException("Invalid boolean input: " + input);
        }
        public static string datetimetonumber(DateTime input)
        {
            //Chuyển giá trị biến THỜI GIAN nhập vào thành CHUỖI
            //Chuỗi xử lý là số ngày các với ngày 1/1/0001
            string kq;
            DateTime referenceDate = new DateTime(1900, 1, 1);

            kq = (input - referenceDate).TotalDays.ToString();
            return kq;
        }
        public static DateTime numbertodatetime(string input)
        {
            //Chuyển giá trị biến CHUỖI trở lại thành biến THỜI GIAN
            DateTime kq;
            DateTime referenceDate = new DateTime(1900, 1, 1);

            kq = referenceDate.AddDays(double.Parse(input));
            return kq;
        }
        public int checkngay(int nam, int thang)
        {
            int kq = DateTime.DaysInMonth(nam, thang);

            return kq;
        }
        public double quydoidonvi(double input, string donvifrom, string donvito)
        {
            string check = donvifrom + "_" + donvito;
            double kq;
            switch (check)
            {
                case "g_kg":
                    kq = input * 1 / 1000;
                    break;
                case "kg_g":
                    kq =  input * 1000;
                    break;
                case "cm_m":
                    kq =  input * 1 / 100;
                    break;
                case "m_cm":
                    kq =  input * 100;
                    break;
                case "cm^2_m^2":
                    kq =  input * 1 / 10000;
                    break;
                case "m^2_cm^2":
                    kq =  input * 10000;
                    break;
                case "mL_L":
                    kq =  input * 1 / 1000;
                    break;
                case "L_mL":
                    kq =  input * 1000;
                    break;
                case "dL_L":
                    kq =  input * 1 / 10;
                    break;
                case "L_dL":
                    kq =  input * 10;
                    break;
                case "mL_dL":
                    kq =  input * 1 / 100;
                    break;
                case "dL_mL":
                    kq =  input * 100;
                    break;
                case "mmH2O_mmHg":
                    kq =  input * 1 / 13.5951;
                    break;
                case "mmHg_mmH2O":
                    kq =  input * 13.5951;
                    break;
                case "mL/phút_L/phút":
                    kq =  input * 1 / 1000;
                    break;
                case "L/phút_mL/phút":
                    kq =  input * 1000;
                    break;
                case "mg/dL_g/dL":
                    kq =  input * 1 / 1000;
                    break;
                case "g/dL_mg/dL":
                    kq =  input * 1000;
                    break;
                case "mg/L_g/L":
                    kq =  input * 1 / 1000;
                    break;
                case "g/L_mg/L":
                    kq =  input * 1000;
                    break;
                case "microg/dL_mg/dL":
                    kq =  input * 1 / 1000;
                    break;
                case "mg/dL_microg/dL":
                    kq =  input * 1000;
                    break;
                default:
                    kq =  input;
                    break;
            }
            return Math.Round(kq,2);
        }
        public double quydoidonviXN(double input, string chat, string donvifrom, string donvito)
        {
            string check = chat.ToLower() + "_" + donvifrom + "_" + donvito;
            double kq;
            switch (check)
            {
                case "calci_mg/dL_mmol/L":
                    kq = input * 0.2495;
                    break;
                case "calci_mmol/L_mg/dL":
                    kq = input * 1 / 0.2495;
                    break;

                case "glucose_mg/dL_mmol/L":
                    kq = input * 0.055;
                    break;
                case "glucose_mmol/L_mg/dL":
                    kq = input * 1 / 0.055;
                    break;

                case "BUN_mg/dL_mmol/L":
                    kq = input * 0.357;
                    break;
                case "BUN_mmol/L_mg/dL":
                    kq = input * 1 / 0.357;
                    break;

                case "creatinin_mg/dL_mcmol/L":
                    kq = input * 88.42;
                    break;
                case "creatinin_mcmol/L_mg/dL":
                    kq = input * 1 / 88.42;
                    break;

                case "bilirubin_mg/dL_mcmol/L":
                    kq = input * 17.10;
                    break;
                case "bilirubin_mcmol/L_mg/dL":
                    kq = input * 1 / 17.10;
                    break;

                case "protein_mg/dL_g/L":
                    kq = input * 10.0;
                    break;
                case "protein_g/L_mg/dL":
                    kq = input * 1 / 10.0;
                    break;

                case "albumin_g/dL_g/L":
                    kq = input * 10.0; 
                    break;
                case "albumin_g/L_g/dL":
                    kq = input * 1 / 10.0; 
                    break;

                case "magie_mg/dL_mmol/L":
                    kq = input * 0.41152;
                    break;
                case "magie_mmol/L_mg/dL":
                    kq = input * 1 / 0.41152;
                    break;

                case "phosphat_mg/dL_mmol/L":
                    kq = input * 0.323;
                    break;
                case "phosphat_mmol/L_mg/dL":
                    kq = input * 1 / 0.323;
                    break;

                case "ferrous_microg/dL_mcmol/L":
                    kq = input * 0.179; 
                    break;
                case "ferrous_mcmol/L_microg/dL":
                    kq = input * 1 / 0.179;
                    break;

                case "hdl_mg/dL_mmol/L":
                    kq = input * 1 / 38.67;
                    break;
                case "hdl_mmol/L_mg/dL":
                    kq = input * 38.67;
                    break;

                case "ldl_mg/dL_mmol/L":
                    kq = input * 1 / 38.67;
                    break;
                case "ldl_mmol/L_mg/dL":
                    kq = input * 38.67;
                    break;

                case "totalcholesterol_mg/dL_mmol/L":
                    kq = input * 1 / 38.67;
                    break;
                case "totalcholesterol_mmol/L_mg/dL":
                    kq = input * 38.67;
                    break;

                case "triglyceride_mg/dL_mmol/L":
                    kq = input * 1 / 88.57;
                    break;
                case "triglyceride_mmol/L_mg/dL":
                    kq = input * 88.57;
                    break;

                case "hb_g/dL_mmol/L":
                    kq = input * 0.6206;
                    break;
                case "hb_mmol/L_g/dL":
                    kq = input * 1 / 0.6206;
                    break;

                default:
                    kq = input;
                    break;
            }
            return Math.Round(kq, 2);
        }
    }
    #endregion
    #region Data class
    public class DSchisoyhoc
    {
        //CLASS HIỂN THỊ
        public string machiso { get; set; }
        public string tenchiso { get; set; }
        public DSchisoyhoc(string _machiso, string _tenchiso)
        {
            machiso = _machiso;
            tenchiso = _tenchiso;
        }
    }
    public class DSBienCSYH
    {
        //CLASS HIỂN THỊ
        public int idbien { get; set; }
        public string tenbien { get; set; }
        public string tendaydu { get; set; }
        public int idloaibien { get; set; }
        public int idbiengoc { get; set; }
        public DSBienCSYH()
        {

        }
        public DSBienCSYH(int _idbien, string _tenbien, string _tendaydu, int _idloaibien, int _idbiengoc)
        {
            idbien = _idbien;
            tenbien = _tenbien;
            tendaydu = _tendaydu;
            idloaibien = _idloaibien;
            idbiengoc = _idbiengoc;
        }
        public void setDSBienCSYH(int _idbien, string _tenbien, string _tendaydu, int _idloaibien, int _idbiengoc)
        {
            idbien = _idbien;
            tenbien = _tenbien;
            tendaydu = _tendaydu;
            idloaibien = _idloaibien;
            idbiengoc = _idbiengoc;
        }
    }
    public class BienLT_CSYH : DSBienCSYH
    {
        //CLASS HIỂN THỊ
        public string donvichuan { get; set; } //LT
        public int IDloaidonvi { get; set; } //LT
        public BienLT_CSYH(BienLT _bienLT)
        {
            setDSBienCSYH(_bienLT.idbien, _bienLT.tenbien, _bienLT.tendaydu, _bienLT.idloaibien, _bienLT.idbiengoc);
            donvichuan = _bienLT.donvichuan;
            IDloaidonvi = _bienLT.IDloaidonvi;
        }
    }
    public class BienDT_CSYH : DSBienCSYH
    {
        //CLASS HIỂN THỊ
        public int sogiatri { get; set; } //DT
        public BienDT_CSYH(BienDT _bienDT)
        {
            setDSBienCSYH(_bienDT.idbien, _bienDT.tenbien, _bienDT.tendaydu, _bienDT.idloaibien, _bienDT.idbiengoc);
            sogiatri = _bienDT.sogiatri;
        }
    }
    public class Bien
    {
        //CLASS LƯU TRỮ VÀ XỬ LÝ
        public int idbien { get; set; }
        public string tenbien { get; set; }
        public string tendaydu { get; set; }
        public int idloaibien { get; set; }
        public int idbiengoc { get; set; }
        public string donvichuan { get; set; } //LT
        public int IDloaidonvi { get; set; } //LT
        public bool bienxuly { get; set; } //DT
        public List<GiatribienDT> giatribien { get; set; } //DT
        public KetnoiDB db;

        public Bien()
        {
            initDB();
        }
        public void initDB()
        {
            db = new KetnoiDB();
        }
        public Bien(int _idbien, string _tenbien, string _tendaydu, int _idloaibien, int _idbiengoc)
        {
            idbien = _idbien;
            tenbien = _tenbien;
            tendaydu = _tendaydu;
            idloaibien = _idloaibien;
            idbiengoc = _idbiengoc;

            initDB();
        }
        public Bien(chiso_DSbien _bien)
        {
            idbien = _bien.IDbien;
            tenbien = _bien.tenbien;
            tendaydu = _bien.tendaydu;
            idloaibien = _bien.IDPhanloaibien;
            idbiengoc = _bien.IDbiengoc;

            initDB();
        }
        public void setBien(int _idbien, string _tenbien, string _tendaydu, int _idloaibien, int _idbiengoc)
        {
            idbien = _idbien;
            tenbien = _tenbien;
            tendaydu = _tendaydu;
            idloaibien = _idloaibien;
            idbiengoc = _idbiengoc;
        }

    }
    public class BienLT : Bien
    {
        public BienLT(Bien _bien, string _donvichuan, int _IDphanloaidonvi)
        {
            initDB();
            setBien(_bien.idbien, _bien.tenbien, _bien.tendaydu, _bien.idloaibien, _bien.idbiengoc);
            donvichuan = _donvichuan;
            IDloaidonvi = _IDphanloaidonvi;
        }
    }
    public class BienDT : Bien
    {
        public int sogiatri { get; set; }
        public BienDT(Bien _bien, int _sogiatri, bool _bienxuly)
        {
            initDB();
            setBien(_bien.idbien, _bien.tenbien, _bien.tendaydu, _bien.idloaibien, _bien.idbiengoc);
            sogiatri = _sogiatri;
            bienxuly = _bienxuly;
        }
        public void initBienDT()
        {
            giatribien = db.GetGiatribienDT(idbien);
        }
    }
    public class GiatribienDT
    {
        public int thutu { get; set; }
        public string giatri { get; set; }
        public double diem { get; set; }
        public string limit { get; set; }
        public GiatribienDT()
        {

        }
        public GiatribienDT(int _thutu, string _giatri, double _diem, string _limit)
        {
            thutu = _thutu;
            giatri = _giatri;
            diem = _diem;
            limit = _limit;
        }
    }
    public class Gioihan
    {
        public double LL { get; set; }
        public bool equalLL { get; set; }
        public double UL { get; set; }
        public bool equalUL { get; set; }

        public Gioihan(double _LL, double _UL, bool _equalLL, bool _equalUL)
        {
            LL = _LL;
            equalLL = _equalLL;
            UL = _UL;
            equalUL = _equalUL;
        }
    }
    #endregion
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
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public int nhiptim { get; set; }
        public int nhiptho { get; set; }
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
                     int _nhiptim, int _nhiptho, double _thannhiet, int _hatThu, int _hatTruong, bool _hutthuoc, bool _tha, bool _dtd, bool _suytim,
                     bool _ungthu, bool _nmct, bool _dotquytim, bool _thieumaunao)
        {
            IDNB = _idnb;
            hoten = _hoten;
            gioitinh = _gioitinh.ToLower();
            ngaysinh = _ngaysinh;
            tinhtuoi_nam();
            chieucao = _chieucao;
            cannang = _cannang;
            nhiptim = _nhiptim;
            nhiptho = _nhiptho;
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
        public void capnhat(string _hoten, string _gioitinh, DateTime _ngaysinh, double _chieucao, double _cannang,
             int _nhiptim, int _nhiptho, double _thannhiet, int _hatThu, int _hatTruong, bool _hutthuoc, bool _tha, bool _dtd, bool _suytim,
             bool _ungthu, bool _nmct, bool _dotquytim, bool _thieumaunao)
        {
            hoten = _hoten;
            gioitinh = _gioitinh.ToLower();
            ngaysinh = _ngaysinh;
            tinhtuoi_nam();
            chieucao = _chieucao;
            cannang = _cannang;
            nhiptim = _nhiptim;
            nhiptho = _nhiptho;
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
        public double tinhtuoi_nam()
        {
            DateTime currentDate = DateTime.Now;
            int tuoithuc = currentDate.Year - ngaysinh.Year;

            if (currentDate.Month < ngaysinh.Month || (currentDate.Month == ngaysinh.Month && currentDate.Day < ngaysinh.Day))
            {
                tuoithuc--;
            }
            return tuoithuc;
        }
        public double tinhtuoi_thang()
        {
            DateTime currentDate = DateTime.Now;

            int years = currentDate.Year - ngaysinh.Year;
            int months = currentDate.Month - ngaysinh.Month;

            if (currentDate.Day < ngaysinh.Day)
            {
                months--;
            }

            return years * 12 + months;
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
        public double albumin { get; set; }
        public double proteinSerum { get; set; }
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
        public double WBC_EOS_tyle { get; set; }
        public double WBC_BAS_tyle { get; set; }
        public double WBC_NEU_tyle { get; set; }
        public double WBC_MONO_tyle { get; set; }
        public double WBC_LYMPHO_tyle { get; set; }
        public double natriSerum { get; set; }
        public double kaliSerum { get; set; }
        public double calciSerum { get; set; }
        public double cloSerum { get; set; }
        public double HCO3Serum { get; set; }
        public double pHSerum { get; set; }
        public double glucoseSerum { get; set; }
        public double natriUrine { get; set; }
        public double kaliUrine { get; set; }
        public double cloUrine { get; set; }
        public double ureUrine { get; set; }
        public double glucoseUrine { get; set; }
        public double PO2 { get; set; }
        public double PaO2 { get; set; }
        public double PvO2 { get; set; }
        public double PCO2 { get; set; }
        public double PaCO2 { get; set; }
        public double PvCO2 { get; set; }
        public double FiO2 { get; set; }
        public double SpO2 { get; set; }
        public double INR { get; set; }
        public Xetnghiem()
        {

        }
        public Xetnghiem(string _idxn, double _creatininSerum, double _creatininUrine, double _ast, double _alt,
            double _bun, double _albumin, double _proteinSerum, double _bilirubin, double _totalCholesterol, double _triglyceride, double _ldl,
            double _hdl, double _rbc, double _hb, double _hct, double _platelet, double _wbc,
            double _wbcEos, double _wbcBas, double _wbcNeu, double _wbcMono, double _wbcLympho,
            double _wbcEos_tyle, double _wbcBas_tyle, double _wbcNeu_tyle, double _wbcMono_tyle, double _wbcLympho_tyle,
            double _natriSerum, double _kaliSerum, double _canxiSerum, double _cloSerum,
            double _hco3Serum, double _phSerum, double _glucoseSerum, double _natriUrine,
            double _kaliUrine, double _cloUrine, double _ureUrine, double _glucoseUrine,
            double _PO2, double _PaO2, double _PvO2, double _PCO2, double _PaCO2, double _PvCO2,
            double _FiO2, double _SpO2, double _INR)
        {
            IDXN = _idxn;
            creatininSerum = _creatininSerum;
            creatininUrine = _creatininUrine;
            AST = _ast;
            ALT = _alt;
            BUN = _bun;
            albumin = _albumin;
            proteinSerum = _proteinSerum;
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
            WBC_EOS_tyle = _wbcEos_tyle;
            WBC_BAS_tyle = _wbcBas_tyle;
            WBC_NEU_tyle = _wbcNeu_tyle;
            WBC_MONO_tyle = _wbcMono_tyle;
            WBC_LYMPHO_tyle = _wbcLympho_tyle;
            natriSerum = _natriSerum;
            kaliSerum = _kaliSerum;
            calciSerum = _canxiSerum;
            cloSerum = _cloSerum;
            HCO3Serum = _hco3Serum;
            pHSerum = _phSerum;
            glucoseSerum = _glucoseSerum;
            natriUrine = _natriUrine;
            kaliUrine = _kaliUrine;
            cloUrine = _cloUrine;
            ureUrine = _ureUrine;
            glucoseUrine = _glucoseUrine;
            PO2 = _PO2;
            PaO2 = _PaO2;
            PvO2 = _PvO2;
            PCO2 = _PCO2;
            PaCO2 = _PaCO2;
            PvCO2 = _PvCO2;
            FiO2 = _FiO2;
            SpO2 = _SpO2;
            INR = _INR;
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
        protected KetnoiDB db;
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
        public void SetChisoyhoc(string _IDChiso, string _Tenchiso, string _Mucdich, string _Ungdung, string _Phuongphap,
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
        public void SetCSYH_CSYH(Chisoyhoc _input)
        {
            IDChiso = _input.IDChiso;
            Tenchiso = _input.Tenchiso;
            mucdich = _input.mucdich;
            ungdung = _input.ungdung;
            phuongphap = _input.phuongphap;
            diengiai = _input.diengiai;
            ghichu = _input.ghichu;
            TLTK = _input.TLTK;
        }
        protected void initDB()
        {
            db = new KetnoiDB();
        }
        protected void initchiso(string machiso)
        {
            initDB();
            //SetCSYH_CSYH(db.GetCSYHtheoIDchiso(machiso));
            IDChiso = machiso;
        }
        public double z_score(double X, double L, double M, double S)
        {
            double Z = (L == 0) ?
                (Math.Pow((X / M), L) - 1) / (L * S) :
                (Math.Log(X / M) / S);
            return Z;
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
        protected List<GiatribienDT> listGiatribienDT;
        protected List<BiendiemCSYH> DStinhdiem;
        public Thangdiem()
        {
            initDB();
        }
        protected void initTongdiem(string _input)
        {
            initDB();
            DStinhdiem = new List<BiendiemCSYH>();
            List<Bien> laydsbien = db.GetDSbien(IDChiso);
            List<DSBienCSYH> laydsbien2 = db.GetDSbienCSYH(laydsbien);

            foreach (DSBienCSYH i in laydsbien2)
            {
                DStinhdiem.Add(new BiendiemCSYH(i.idbien, i.idloaibien));
            }
            //Comment vì URL quá dài khi truyền cả IDbien và giá trị
            /*            List<string> inputRieng = _input.Split(new[] { "__" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (string i in inputRieng)
                        {
                            List<string> input = i.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            for (int j = 0; j < DStinhdiem.Count(); j++)
                            {
                                if (DStinhdiem[j].idbien == int.Parse(input[0]))
                                {
                                    if (DStinhdiem[j].idloaibien == 1)
                                        DStinhdiem[j].giatri = int.Parse(input[1]);
                                    else
                                        DStinhdiem[j].thutunhap = int.Parse(input[1]);
                                }
                            }
                        }*/
            List<string> input = _input.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < DStinhdiem.Count(); i++)
            {
                if (DStinhdiem[i].idloaibien == 1)
                    DStinhdiem[i].giatri = int.Parse(input[i]);
                else
                    DStinhdiem[i].thutunhap = int.Parse(input[i]);
            }

        }
        public List<tachInput> tachInputbienDT()
        {
            List<tachInput> kq = new List<tachInput>();

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                kq.Add(new tachInput(i.idbien, i.thutunhap));
            }
            return kq;
        }
        protected void tinhTongdiem()
        {
            foreach (BiendiemCSYH biendiem in DStinhdiem)
            {
                if (biendiem.idloaibien == 1)
                    biendiem.diemketqua = 0;
                else if (biendiem.thutunhap == 0)
                    biendiem.diemketqua = 0;
                else
                    biendiem.diemketqua = db.GetDiembienDT(biendiem.idbien, biendiem.thutunhap);
            }
        }
        protected class BiendiemCSYH
        {
            public int idbien { get; set; }
            public int idloaibien { get; set; }
            public int thutunhap { get; set; }
            public double diemketqua { get; set; }
            public double giatri { get; set; }
            public BiendiemCSYH(int _idbien, int _idloaibien)
            {
                idbien = _idbien;
                idloaibien = _idloaibien;
                thutunhap = 0;
                diemketqua = 0;
                giatri = 0;
            }
        }
        public class tachInput
        {
            public int idbien { get; set; }
            public int thutunhap { get; set; }
            public tachInput(int _idbien, int _thutunhap)
            {
                idbien = _idbien;
                thutunhap = _thutunhap;
            }
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
            //init("C_A01");
        }
        public IBW(string _gioitinh, double _chieucao)
        {
            gioitinh = _gioitinh.ToLower();
            chieucao = _chieucao;
            //init("C_A01");
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
        public string kqIBW_diengiai()
        {
            return "";
        }
    }
    public class AdjBW : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public AdjBW()
        {
            //init("C_A02");
        }
        public AdjBW(string _gioitinh, double _chieucao, double _cannang)
        {
            gioitinh = _gioitinh.ToLower();
            chieucao = _chieucao;
            cannang = _cannang;
            //init("C_A02");
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
        public string kqAdjBW_diengiai()
        {
            return "";
        }
    }
    public class LBW : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }

        public LBW()
        {
            //init("C_A03");
        }
        public LBW(Nguoibenh nb)
        {
            gioitinh = nb.gioitinh;
            cannang = nb.cannang;
            chieucao = nb.chieucao;
            //init("C_A03");
        }
        public LBW(string _gioitinh, double _chieucao, double _cannang)
        {
            gioitinh = _gioitinh.ToLower();
            cannang = _cannang;
            chieucao = _chieucao;
            //init("C_A03");
        }

        public double kqLBW()
        {
            double lbw = (gioitinh == "nam")
                ? (0.32810 * cannang) + (0.33929 * chieucao) - 29.5336
                : (0.29569 * cannang) + (0.41813 * chieucao) - 43.2933;

            return lbw;
        }
        public string kqLBW_diengiai()
        {
            return "";
        }
    }
    public class AlcoholSerum : Congthuc
    {
        public double cannang { get; set; }
        public double AlcoholVolume { get; set; }
        public double AlcoholConcentration { get; set; }
        public AlcoholSerum()
        {
            //init("C_A04");
        }
        public AlcoholSerum(Nguoibenh nb)
        {
            cannang = nb.cannang;
            //init("C_A04");
        }
        public AlcoholSerum(double _cannang, double _AlcoholVolume, double _AlcoholConcentration)
        {
            AlcoholVolume = _AlcoholVolume;
            AlcoholConcentration = _AlcoholConcentration;
            cannang = _cannang;
        }

        public double kqAlcoholSerum()
        {
            double kq = ((AlcoholVolume * AlcoholConcentration) * 0.8) / (cannang * 0.6);
            return kq;
        }
        public string kqAlcoholSerum_diengiai()
        {
            return "";
        }
    }
    public class Budichbong : Congthuc
    {
        public double cannang { get; set; }
        public double Tylebong { get; set; }

        public Budichbong()
        {
            //init("C_A05");
        }
        public Budichbong(Nguoibenh nb)
        {
            //init("C_A05");
            cannang = nb.cannang;
        }
        public Budichbong(double _cannang, double _Tylebong)
        {
            cannang = _cannang;
            Tylebong = _Tylebong;
        }

        public double kqVdich24h()
        {
            double kq = 4 * cannang * Tylebong;
            return kq;
        }
        public double kqtocdotruyen8h()
        {
            double Vdich24h = kqVdich24h();
            double kq = Vdich24h / 16;
            return kq;
        }
        public double kqtocdotruyen16h()
        {
            double Vdich24h = kqVdich24h();
            double kq = Vdich24h / 32;
            return kq;
        }
        public string kqVdich24h_diengiai()
        {
            return "";
        }
    }
    public class BMI : Congthuc
    {
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public BMI()
        {
            //init("C_A06");
        }

        public BMI(double _chieucao, double _cannang)
        {
            chieucao = _chieucao;
            cannang = _cannang;
            //init("C_A06");
        }

        public BMI(Nguoibenh NB)
        {
            chieucao = NB.chieucao;
            cannang = NB.cannang;
            //init("C_A06");
        }

        public double kqBMI()
        {
            double kqBMI = cannang / (chieucao * chieucao / 10000);
            return kqBMI;
        }
        public string kqBMI_diengiai()
        {
            double BMI = kqBMI();
            if (BMI < 18.5)
                return "BMI < 18,5: Thiếu cân";
            else if (BMI < 25)
                return "BMI trong khoảng 18,5 - 25: Cân nặng bình thường";
            else if (BMI < 30)
                return "BMI trong khoảng 25 - 30: Thừa cân";
            else
                return "BMI > 30: Béo phì";
        }
    }
    public class AaG : Congthuc
    {
        public double tuoi { get; set; }
        public double thannhiet { get; set; }
        public double FiO2 { get; set; }
        public double pCO2 { get; set; }
        public double PaO2 { get; set; }
        public double docaouoctinh { get; set; }
        public double Hesohohap { get; set; }
        public AaG()
        {
            //init("C_A07");
        }
        public AaG(Nguoibenh NB, Xetnghiem XN)
        {
            tuoi = NB.tinhtuoi_nam();
            thannhiet = NB.thannhiet;
            FiO2 = XN.FiO2;
            docaouoctinh = 0;
            pCO2 = XN.PCO2;
            Hesohohap = 0;
            PaO2 = XN.PaO2;
            //init("C_A07");
        }

        public AaG(double _tuoi, double _thannhiet, double _FiO2, double _pCO2, double _PaO2, double _docaouoctinh, double _Hesohohap)
        {
            FiO2 = _FiO2;
            docaouoctinh = _docaouoctinh;
            thannhiet = _thannhiet;
            pCO2 = _pCO2;
            Hesohohap = _Hesohohap;
            tuoi = _tuoi;
            PaO2 = _PaO2;
            //init("C_A07");
        }

        public double kqAaG()
        {
            double pKhiquyen = 760 * Math.Exp(docaouoctinh);
            double pH2O = 47 * Math.Exp((thannhiet - 37) / 18.4);
            double AaG = FiO2 * (pKhiquyen - pH2O) - (pCO2 / Hesohohap) + pCO2 * FiO2 * (1 - Hesohohap) / Hesohohap - PaO2;
            return AaG;
        }

        public double kqAaGnormal()
        {
            double AaGnormal = 2.5 + (0.21 * tuoi);
            return AaGnormal;
        }
        public string kqAaG_diengiai()
        {
            return "";
        }
    }
    public class CalciSerum_Adj : Congthuc
    {
        public double calciSerum { get; set; }
        public double albuminSerumNorm { get; set; }
        public double albuminSerum { get; set; }

        public CalciSerum_Adj()
        {
            //init("C_A08");
        }
        public CalciSerum_Adj(Xetnghiem XN)
        {
            calciSerum = XN.calciSerum;
            albuminSerumNorm = 4;
            albuminSerum = 0;
            //init("C_A08");
        }

        public CalciSerum_Adj(double _calciSerum, double _normAlbumin, double _albuminSerum)
        {
            albuminSerumNorm = _normAlbumin;
            albuminSerum = _albuminSerum;
            calciSerum = _calciSerum;
            //init("C_A08");
        }

        public double kqCalciSerum_Adj()
        {
            return 0.8 * (albuminSerumNorm - albuminSerum) + calciSerum;
        }
        public string kqCalciSerum_Adj_diengiai()
        {
            double ketqua = kqCalciSerum_Adj();
            if (ketqua > 4 && ketqua < 4.4)
                return "Nồng độ calci nằm trong khoảng bình thường (4 - 4,4g/dL)";
            else
                return "Nồng độ calci nằm ngoài khoảng bình thường (4 - 4,4g/dL)";
        }
    }
    public class BSA : Congthuc
    {
        public double chieucao { get; set; }
        public double cannang { get; set; }

        public BSA()
        {
            //init("C_A09");
        }
        public BSA(Nguoibenh NB)
        {
            chieucao = NB.chieucao;
            cannang = NB.cannang;
            //init("C_A09");
        }

        public BSA(double _chieucao, double _cannang)
        {
            chieucao = _chieucao;
            cannang = _cannang;
            //init("C_A09");
        }

        public double kqBSA_Mos()
        {
            return Math.Sqrt(chieucao * cannang) / 3600;
        }
        public double kqBSA_Dub()
        {
            return 0.007184 * Math.Pow(chieucao, 0.725) * Math.Pow(cannang, 0.425);
        }
        public string kqBSA_diengiai()
        {
            return "";
        }
    }
    public class SAG : Congthuc //C_A10
    {
        public double NatriSerum { get; set; }
        public double KaliSerum { get; set; }
        public double CloSerum { get; set; }
        public double HCO3Serum { get; set; }

        public SAG()
        {
            //init("C_A10");

        }
        public SAG(Xetnghiem XN)
        {
            NatriSerum = XN.natriSerum;
            KaliSerum = XN.kaliSerum;
            CloSerum = XN.cloSerum;
            HCO3Serum = XN.HCO3Serum;
            //init("C_A10");
        }
        public SAG(double _NatriSerum, double _KaliSerum, double _CloSerum, double _HCO3Serum)
        {
            NatriSerum = _NatriSerum;
            KaliSerum = _KaliSerum;
            CloSerum = _CloSerum;
            HCO3Serum = _HCO3Serum;
            //init("C_A10");
        }

        public double kqSAG()
        {
            double kq = NatriSerum + KaliSerum - CloSerum - HCO3Serum;
            return kq;
        }
        public string kqSAG_diengiai()
        {
            return "";
        }
    }
    public class SOG : Congthuc //C_A11
    {
        public double NatriSerum { get; set; }
        public double BUN { get; set; }
        public double GlucoseSerum { get; set; }
        public double OsmSerum { get; set; }

        public SOG()
        {
            //init("C_A11");
        }
        public SOG(Xetnghiem XN)
        {
            NatriSerum = XN.natriSerum;
            BUN = XN.BUN;
            GlucoseSerum = XN.glucoseSerum;
            //init("C_A11");
        }
        public SOG(double _NatriSerum, double _BUN, double _GlucoseSerum, double _OsmSerum)
        {
            OsmSerum = _OsmSerum;
            NatriSerum = _NatriSerum;
            BUN = _BUN;
            GlucoseSerum = _GlucoseSerum;
            //init("C_A11");
        }

        public double kqSOG()
        {
            double kq = OsmSerum - (2 * NatriSerum + BUN / 2.5 + GlucoseSerum / 18);
            return kq;
        }
        public string kqSOG_diengiai()
        {
            return "";
        }
    }
    public class StOG : Congthuc //C_A12
    {
        public double OsmStool { get; set; }
        public double NatriStool { get; set; }
        public double KaliStool { get; set; }

        public StOG()
        {
            //init("C_A12");

        }
        public StOG(double _OsmStool, double _NatriStool, double _KaliStool)
        {
            OsmStool = _OsmStool;
            NatriStool = _NatriStool;
            KaliStool = _KaliStool;
            //init("C_A12");
        }

        public double kqStOG()
        {
            double kq = OsmStool - 2 * (NatriStool + KaliStool);
            return kq;
        }
        public string kqStOG_diengiai()
        {
            double ketqua = kqStOG();
            if (ketqua > 50 && ketqua < 100)
                return "Khoảng trống Osmol phânnằm trong khoảng bình thường (50 - 100 mOsm/kg)";
            else
                return "Khoảng trống Osmol phânnằm ngoài khoảng bình thường (50 - 100 mOsm/kg)";
        }
    }
    public class UAG : Congthuc
    {
        public double NatriUrine { get; set; }
        public double CloUrine { get; set; }
        public double KaliUrine { get; set; }
        public UAG()
        {
            //init("C_A13");
        }
        public UAG(Xetnghiem XN)
        {
            NatriUrine = XN.natriUrine;
            KaliUrine = XN.kaliUrine;
            CloUrine = XN.cloUrine;
            //init("C_A13");
        }
        public UAG(double _NatriUrine, double _CloUrine, double _KaliUrine)
        {
            NatriUrine = _NatriUrine;
            KaliUrine = _KaliUrine;
            CloUrine = _CloUrine;
            //init("C_A13");
        }
        public double kqUAG()
        {
            double kq = NatriUrine + KaliUrine - CloUrine;
            return kq;
        }
        public string kqUAG_diengiai()
        {
            return "";
        }
    }
    public class UOG : Congthuc
    {
        public double NatriUrine { get; set; }
        public double KaliUrine { get; set; }
        public double UreUrine { get; set; }
        public double GlucoseUrine { get; set; }
        public double OsmUrine { get; set; }

        public UOG()
        {
            //init("C_A14");
        }
        public UOG(Xetnghiem XN)
        {
            NatriUrine = XN.natriUrine;
            KaliUrine = XN.kaliUrine;
            UreUrine = XN.ureUrine;
            GlucoseUrine = XN.glucoseUrine;
            //init("C_A14");
        }
        public UOG(double _NatriUrine, double _KaliUrine, double _UreUrine, double _GlucoseUrine, double _OsmUrine)
        {
            OsmUrine = _OsmUrine;
            NatriUrine = _NatriUrine;
            KaliUrine = _KaliUrine;
            UreUrine = _UreUrine;
            GlucoseUrine = _GlucoseUrine;
            //init("C_A14");
        }

        public double kqUOG()
        {
            double kq = OsmUrine - (2 * NatriUrine + 2 * KaliUrine + UreUrine / 2.8 + GlucoseUrine / 18);
            return kq;
        }
        public string kqUOG_diengiai()
        {
            return "";

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
            //init("C_A15");
        }
        public eGFR_CKD(Nguoibenh NB, Xetnghiem XN)
        {
            gioitinh = NB.gioitinh;
            CreatininSerum = XN.creatininSerum;
            tuoi = NB.tinhtuoi_nam();
            SetCoefficients();
            //init("C_A15");
        }

        public eGFR_CKD(string _gioitinh, double _tuoi, double _CreatininSerum)
        {
            gioitinh = _gioitinh.ToLower();
            CreatininSerum = _CreatininSerum;
            tuoi = _tuoi;
            SetCoefficients();
            //init("C_A15");
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
        public string kqeGFR_CKD_diengiai()
        {
            if (eGFR >= 90)
                return "Giai đoạn 1: eGFR bình thường hoặc cao (eGFR > 90 mL/phút) (KDIGO 2012)";
            else if (eGFR >= 60)
                return "Giai đoạn 2: Bệnh thận mạn nhẹ (eGFR = 60-89 mL/phút) (KDIGO 2012)";
            else if (eGFR >= 450)
                return "Giai đoạn 3A: Bệnh thận mạn trung bình (eGFR = 45-59 mL/phút) (KDIGO 2012)";
            else if (eGFR >= 30)
                return "Giai đoạn 3B: Bệnh thận mạn trung bình (eGFR = 30-44 mL/phút) (KDIGO 2012)";
            else if (eGFR >= 15)
                return "Giai đoạn 4: Bệnh thận mạn nặng (eGFR = 15-29 mL/phút)(KDIGO 2012)";
            else
                return "Giai đoạn 5: Bệnh thận mạn giai đoạn cuối (GFR <15 mL/phút) (KDIGO 2012)";
        }
    }
    public class eGFR_MDRD : Congthuc //C_A29
    {
        public string gioitinh { get; set; }
        public double tuoi { get; set; }
        public double CreatininSerum { get; set; }
        public string chungtoc { get; set; }
        public double eGFR { get; set; }

        public eGFR_MDRD()
        {
            //init("C_A15");
        }
        public eGFR_MDRD(Nguoibenh NB, Xetnghiem XN)
        {
            CreatininSerum = XN.creatininSerum;
            tuoi = NB.tinhtuoi_nam();
            gioitinh = NB.gioitinh;
            chungtoc = "người châu á";
            //init("C_A15");
        }

        public eGFR_MDRD(string _gioitinh, double _tuoi, double _CreatininSerum, string _chungtoc)
        {
            CreatininSerum = _CreatininSerum;
            tuoi = _tuoi;
            chungtoc = _chungtoc.ToLower();
            gioitinh = _gioitinh.ToLower();
            //init("C_A15");
        }
        public double kqeGFR_MDRD()
        {
            double chungtocCoefficient = (chungtoc == "người da đen") ? 1.212 : 1.0;
            double gioitinhCoefficient = (gioitinh == "nam") ? 1.0 : 0.742;

            eGFR = 175 * Math.Pow(CreatininSerum, -1.154) * Math.Pow(tuoi, -0.203) * chungtocCoefficient * gioitinhCoefficient;
            return eGFR;
        }
        public string kqeGFR_MDRD_diengiai()
        {
            if (eGFR >= 90)
                return "Giai đoạn 1: eGFR bình thường hoặc cao (eGFR > 90 mL/phút) (KDIGO 2012)";
            else if (eGFR >= 60)
                return "Giai đoạn 2: Bệnh thận mạn nhẹ (eGFR = 60-89 mL/phút) (KDIGO 2012)";
            else if (eGFR >= 45)
                return "Giai đoạn 3A: Bệnh thận mạn trung bình (eGFR = 45-59 mL/phút) (KDIGO 2012)";
            else if (eGFR >= 30)
                return "Giai đoạn 3B: Bệnh thận mạn trung bình (eGFR = 30-44 mL/phút) (KDIGO 2012)";
            else if (eGFR >= 15)
                return "Giai đoạn 4: Bệnh thận mạn nặng (eGFR = 15-29 mL/phút)(KDIGO 2012)";
            else
                return "Giai đoạn 5: Bệnh thận mạn giai đoạn cuối (GFR <15 mL/phút) (KDIGO 2012)";
        }
    }
    public class eCrCl : Congthuc
    {
        public string gioitinh { get; set; }
        public double cannang { get; set; }
        public double tuoi { get; set; }
        public double CreatininSerum { get; set; }

        public eCrCl()
        {
            //init("C_A16");
        }
        public eCrCl(Nguoibenh NB, Xetnghiem XN)
        {
            tuoi = NB.tinhtuoi_nam();
            cannang = NB.cannang;
            CreatininSerum = XN.creatininSerum;
            gioitinh = NB.gioitinh;
            //init("C_A16");
        }

        public eCrCl(string _gioitinh, double _cannang, double _tuoi, double _CreatininSerum)
        {
            tuoi = _tuoi;
            cannang = _cannang;
            CreatininSerum = _CreatininSerum;
            gioitinh = _gioitinh.ToLower();
            //init("C_A16");
        }

        public double kqeCrCl()
        {
            double gioitinhCoefficient = (gioitinh == "nam") ? 1.0 : 0.85;
            double kq = (140 - tuoi) * cannang / (72 * CreatininSerum) * gioitinhCoefficient;
            return kq;
        }
        public string kqeCrCl_diengiai()
        {
            return "";
        }
    }
    public class FEMg : Congthuc
    {
        public double CreatininSerum { get; set; }
        public double CreatininUrine { get; set; }
        public double MagieUrine { get; set; }
        public double MagieSerum { get; set; }

        public FEMg()
        {
            //init("C_A17");
        }
        public FEMg(Xetnghiem XN)
        {
            CreatininSerum = XN.creatininSerum;
            CreatininUrine = XN.creatininUrine;
            //init("C_A17");
        }
        public FEMg(double _CreatininSerum, double _CreatininUrine, double _MagieUrine, double _MagieSerum)
        {
            MagieUrine = _MagieUrine;
            CreatininSerum = _CreatininSerum;
            MagieSerum = _MagieSerum;
            CreatininUrine = _CreatininUrine;
            //init("C_A17");
        }
        public double kqFEMg()
        {
            double kq = (MagieUrine * CreatininSerum) / (MagieSerum * CreatininUrine);
            return kq;
        }
        public string kqFEMg_diengiai()
        {
            return "";
        }
    }
    public class FENa : Congthuc
    {
        public double NatriSerum { get; set; }
        public double NatriUrine { get; set; }
        public double CreatininSerum { get; set; }
        public double CreatininUrine { get; set; }

        public FENa()
        {
            //init("C_A18");
        }
        public FENa(Xetnghiem XN)
        {
            NatriUrine = XN.natriUrine;
            CreatininSerum = XN.creatininSerum;
            NatriSerum = XN.natriSerum;
            CreatininUrine = XN.creatininUrine;
            //init("C_A18");
        }
        public FENa(double _NatriSerum, double _NatriUrine, double _CreatininSerum, double _CreatininUrine)
        {
            NatriUrine = _NatriUrine;
            CreatininSerum = _CreatininSerum;
            NatriSerum = _NatriSerum;
            CreatininUrine = _CreatininUrine;
            //init("C_A18");
        }
        public double kqFENa()
        {
            double kq = (NatriUrine * CreatininSerum) / (NatriSerum * CreatininUrine);
            return kq;
        }
        public string kqFENa_diengiai()
        {
            return "";
        }
    }
    public class KtVDaugirdas : Congthuc
    {
        public double BUNtruocloc { get; set; }
        public double BUNsauloc { get; set; }
        public double tglocmau { get; set; }
        public double Vlocmau { get; set; }
        public double cannangsaulocmau { get; set; }

        public KtVDaugirdas()
        {
            //init("C_A19");
        }

        public KtVDaugirdas(double _BUNtruocloc, double _BUNsauloc, double _tglocmau, double _Vlocmau, double _cannangsaulocmau)
        {
            BUNsauloc = _BUNsauloc;
            BUNtruocloc = _BUNtruocloc;
            tglocmau = _tglocmau;
            Vlocmau = _Vlocmau;
            cannangsaulocmau = _cannangsaulocmau;
            //init("C_A19");
        }

        public double kqKtVDaugirdas()
        {
            double kq = -Math.Log((BUNsauloc / BUNtruocloc) - (0.008 * tglocmau)) +
                ((4 - (3.5 * BUNsauloc / BUNtruocloc)) * Vlocmau / cannangsaulocmau);
            return kq;
        }
        public string kqKtVDaugirdas_diengiai()
        {
            return "";
        }
    }
    public class RRF_Kru : Congthuc
    {
        public double UreUrine { get; set; }
        public double VUrineRRF { get; set; }
        public double IntervalRRF { get; set; }
        public double BUN1RRF { get; set; }
        public double BUN2RRF { get; set; }
        public double RRF_KruResult { get; set; }

        public RRF_Kru()
        {
            //init("C_A20");
        }

        public RRF_Kru(double _UreUrine, double _VUrineRRF, double _IntervalRRF, double _BUN1RRF, double _BUN2RRF)
        {
            VUrineRRF = _VUrineRRF;
            UreUrine = _UreUrine;
            IntervalRRF = _IntervalRRF;
            BUN1RRF = _BUN1RRF;
            BUN2RRF = _BUN2RRF;
            //init("C_A20");
        }

        public double kqRRF_Kru()
        {
            RRF_KruResult = VUrineRRF * UreUrine / IntervalRRF / ((BUN1RRF + BUN2RRF) / 2);
            return RRF_KruResult;
        }
        public string kqRRF_Kru_diengiai()
        {
            return "";
        }
    }
    public class ACR : Congthuc
    {
        public double CreatininUrine { get; set; }
        public double AlbuminUrine { get; set; }

        public ACR()
        {
            //init("C_A21");
        }
        public ACR(Xetnghiem XN)
        {
            CreatininUrine = XN.creatininUrine;
            //init("C_A21");
        }
        public ACR(double _CreatininUrine, double _AlbuminUrine)
        {
            AlbuminUrine = _AlbuminUrine;
            CreatininUrine = _CreatininUrine;
            //init("C_A21");
        }

        public double kqACR()
        {
            double kq = AlbuminUrine / CreatininUrine;
            return kq;
        }
        public string kqACR_diengiai()
        {
            double ketqua = kqACR();
            if (ketqua < 30)
                return "Tỷ lệ albumin/creatinine trong nước tiểu bình thường: <30 mg/g";
            else if (ketqua <300)
                return "Tăng albumin niệu trung bình hay microalbumin niệu: từ 30 - 300 mg/g";
            else
                return "Tăng albumin niệu nghiêm trọng hay macroalbumin niệu: >300 mg/g";
        }
    }
    public class PCR : Congthuc
    {
        public double ProteinUrine { get; set; }
        public double CreatininUrine { get; set; }

        public PCR()
        {
            //init("C_A22");
        }
        public PCR(Xetnghiem XN)
        {
            CreatininUrine = XN.creatininUrine;
            //init("C_A22");
        }
        public PCR(double _ProteinUrine, double _CreatininUrine)
        {
            ProteinUrine = _ProteinUrine;
            CreatininUrine = _CreatininUrine;
            //init("C_A22");
        }
        public double kqPCR()
        {
            double kq = ProteinUrine / CreatininUrine;
            return kq;
        }
        public string kqPCR_diengiai()
        {
            double ketqua = kqPCR();
            if (ketqua < 0.2)
                return "PCR trong nước tiểu bình thường: < 0,2 mg/mg";
            else if (ketqua < 3.5)
                return "PCR trong nước tiểu bất thường: 0,2 - 3,5 mg/mg";
            else
                return "Biểu hiện thận hư: > 3,5 mg/mg";
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
            //init("C_A23");
        }
        public eAER(Nguoibenh NB, Xetnghiem XN)
        {
            AlbuminUrine = 0;
            CreatininUrine = XN.creatininUrine;
            gioitinh = NB.gioitinh;
            chungtoc = "người châu á";
            tuoi = NB.tinhtuoi_nam();
            //init("C_A23");
        }
        public eAER(string _gioitinh, double _tuoi, double _CreatininUrine,
            string _chungtoc, double _AlbuminUrine)
        {
            AlbuminUrine = _AlbuminUrine;
            CreatininUrine = _CreatininUrine;
            gioitinh = _gioitinh.ToLower();
            chungtoc = _chungtoc.ToLower();
            tuoi = _tuoi;
            //init("C_A23");
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
        public string kqeAER_diengiai()
        {

            if (eAERResult < 30)
                return "Albumin niệu ở mức bình thường: < 30 mg/ngày";
            else if (eAERResult < 300)
                return "Tăng albumin niệu nhẹ: 30 - 300 mg/ngày";
            else
                return "Tăng albumin niệu nghiêm trọng: > 300mg/ngày";
        }
    }
    public class ePER : Congthuc //C_A24
    {
        public string gioitinh { get; set; }
        public double tuoi { get; set; }
        public double creatininUrine { get; set; }
        public string chungtoc { get; set; }
        public double proteinUrine { get; set; }

        public ePER()
        {
            //init("C_A24");
        }
        public ePER(Nguoibenh nb, Xetnghiem xn)
        {
            gioitinh = nb.gioitinh;
            tuoi = nb.tinhtuoi_nam();
            creatininUrine = xn.creatininUrine;
            //init("C_A24");
        }
        public ePER(string _gioitinh, double _tuoi, double _creatininUrine,
            string _chungtoc, double _proteinUrine)
        {
            gioitinh = _gioitinh.ToLower();
            tuoi = _tuoi;
            creatininUrine = _creatininUrine;
            chungtoc = _chungtoc.ToLower();
            proteinUrine = _proteinUrine;
            //init("C_A24");
        }
        public double kqePER()
        {
            double ePER = proteinUrine / creatininUrine * ((gioitinh == "nam") ?
                         ((chungtoc == "người da đen") ?
                         1413.9 + (23.2 * tuoi) - (0.3 * tuoi * tuoi) :
                         1307.3 + (23.1 * tuoi) - (0.3 * tuoi * tuoi)) :
                         ((chungtoc == "người da đen") ?
                         1148.6 + (15.6 * tuoi) - (0.3 * tuoi * tuoi) :
                         1051.3 + (5.3 * tuoi) - (0.1 * tuoi * tuoi)));
            return ePER;
        }
        public string kqePER_diengiai()
        {
            double ePER = kqePER();
            if (ePER < 150)
                return "ePER < 150 mg/ngày: Bình thường";
            else if (ePER < 3500)
                return "ePER từ 150 - 3.500mg/ngày: Tăng protein niệu";
            else
                return "ePER > 3.500 mg/ngày:Thận hư & tăng protein niệu";
        }
    }
    public class TocDoTruyen : Congthuc
    {
        public double VdichTruyen { get; set; }
        public double HesoGiot { get; set; }
        public double ThoiGianTruyen { get; set; }

        public TocDoTruyen()
        {
            //init("C_A25");

        }
        public TocDoTruyen(double _VdichTruyen, double _HesoGiot, double _ThoiGianTruyen)
        {
            VdichTruyen = _VdichTruyen;
            HesoGiot = _HesoGiot;
            ThoiGianTruyen = _ThoiGianTruyen;
            //init("C_A25");
        }
        public double kqTocDoTruyen()
        {
            double kq = VdichTruyen * HesoGiot / ThoiGianTruyen;
            return kq;
        }
        public string kqTocDoTruyen_diengiai()
        {
            return "";
        }
    }
    public class CrCl24h : Congthuc
    {
        public double CreatininSerum { get; set; }
        public double CreatininUrine { get; set; }
        public double VUrine24h { get; set; }

        public CrCl24h()
        {
            //init("C_A26");

        }
        public CrCl24h(Xetnghiem XN)
        {
            CreatininUrine = XN.creatininUrine;
            CreatininSerum = XN.creatininSerum;
            //init("C_A26");
        }
        public CrCl24h(double _CreatininSerum, double _CreatininUrine, double _VUrine24h)
        {
            CreatininUrine = _CreatininUrine;
            VUrine24h = _VUrine24h;
            CreatininSerum = _CreatininSerum;
            //init("C_A26");
        }
        public double kqCrCl24h()
        {
            double kq = CreatininUrine * VUrine24h / CreatininSerum / 1440;
            return kq;
        }
        public string kqCrCl24h_diengiai()
        {
            return "";
        }
    }
    public class eGFR_Schwartz : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double tuoi { get; set; }
        public double CreatininSerum { get; set; }
        public bool sinhnon { get; set; }
        public string loaiXNcreatinin { get; set; }
        public bool benhthanman { get; set; }
        public double eGFR_SchwartzResult { get; set; }

        public eGFR_Schwartz()
        {
            //init("C_A27");
        }
        public eGFR_Schwartz(Nguoibenh NB, Xetnghiem XN)
        {
            loaiXNcreatinin = "jaffe";
            benhthanman = false;
            tuoi = NB.tinhtuoi_nam();
            sinhnon = false;
            gioitinh = NB.gioitinh;
            chieucao = NB.chieucao;
            CreatininSerum = XN.creatininSerum;
            //init("C_A27");
        }
        public eGFR_Schwartz(string _gioitinh, double _chieucao, double _tuoi, double _CreatininSerum, bool _sinhnon, string _loaiXNcreatinin, bool _benhthanman)
        {
            gioitinh = _gioitinh.ToLower();
            chieucao = _chieucao;
            tuoi = _tuoi;
            CreatininSerum = _CreatininSerum;
            sinhnon = _sinhnon;
            loaiXNcreatinin = _loaiXNcreatinin.ToLower();
            benhthanman = _benhthanman;
            //init("C_A27");
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
        public string kqeGFR_Schwartz_diengiai()
        {
            return "";
        }
    }
    public class MPM0 : Congthuc
    {
        public int tuoi { get; set; }
        public int nhiptim { get; set; }
        public int sbp { get; set; }
        public bool strokenao { get; set; }
        public bool suythanman { get; set; }
        public bool honme { get; set; }
        public int glasgowcoma { get; set; }
        public bool xogan { get; set; }
        public bool ungthu { get; set; }
        public bool suythancap { get; set; }
        public bool loannhip { get; set; }
        public bool xhth { get; set; }
        public bool khoinoiso { get; set; }
        public bool hoisuctim { get; set; }
        public bool thongkhicohoc { get; set; }
        public bool yeucauphauthuat { get; set; }
        public bool capcuutimphoi { get; set; }
        public bool fullcode { get; set; }
        public double nhiptimF { get; set; }
        public double sbpF { get; set; }
        public double strokenaoF { get; set; }
        public double suythanmanF { get; set; }
        public double honmeF { get; set; }
        public double glasgowcomaF { get; set; }
        public double xoganF { get; set; }
        public double ungthuF { get; set; }
        public double suythancapF { get; set; }
        public double loannhipF { get; set; }
        public double xhthF { get; set; }
        public double khoinoisoF { get; set; }
        public double hoisuctimF { get; set; }
        public double thongkhicohocF { get; set; }
        public double yeucauphauthuatF { get; set; }
        public double capcuutimphoiF { get; set; }
        public double fullcodeF { get; set; }
        public MPM0()
        {
            //init("C_A28");
        }
        public MPM0(int _tuoi, int _nhiptim, int _sbp, bool _strokenao, bool _suythanman, bool _honme,
                int _glasgowcoma, bool _xogan, bool _ungthu, bool _suythancap, bool _loannhip,
                bool _xhth, bool _khoinoiso, bool _hoisuctim, bool _thongkhicohoc,
                bool _yeucauphauthuat, bool _fullcode)
        {
            tuoi = _tuoi;
            honme = _honme;
            glasgowcoma = _glasgowcoma;
            nhiptim = _nhiptim;
            sbp = _sbp;
            suythanman = _suythanman;
            xogan = _xogan;
            ungthu = _ungthu;
            suythancap = _suythancap;
            loannhip = _loannhip;
            strokenao = _strokenao;
            xhth = _xhth;
            khoinoiso = _khoinoiso;
            hoisuctim = _hoisuctim;
            thongkhicohoc = _thongkhicohoc;
            yeucauphauthuat = _yeucauphauthuat;
            fullcode = _fullcode;

            checkMPM0();
            //init("C_A28");
        }
        public void checkMPM0()
        {
            tuoi = tuoi;
            honmeF = (honme || glasgowcoma < 5) ? 2.050514 : 0;
            nhiptimF = (nhiptim >= 150) ? 0.433188 : 0;
            sbpF = (sbp <= 90) ? 1.451005 : 0;
            suythanmanF = (suythanman) ? 0.5395209 : 0;
            xoganF = (xogan) ? 2.070695 : 0;
            ungthuF = (ungthu) ? 3.204902 : 0;
            suythancapF = (suythancap) ? 0.8412274 : 0;
            loannhipF = (loannhip) ? 0.8219612 : 0;
            strokenaoF = (strokenao) ? 0.4107686 : 0;
            xhthF = (xhth) ? -0.165253 : 0;
            khoinoisoF = (khoinoiso) ? 1.855276 : 0;
            hoisuctimF = (hoisuctim) ? 1.497258 : 0;
            thongkhicohocF = (thongkhicohoc) ? 0.821648 : 0;
            yeucauphauthuatF = (yeucauphauthuat) ? 0.9097936 : 0;

            if (honmeF + nhiptimF + sbpF + suythanmanF + xoganF + ungthuF + suythancapF + loannhipF +
                             strokenaoF + xhthF + khoinoisoF + hoisuctimF + thongkhicohocF + yeucauphauthuatF == 0)
            {
                fullcodeF = (capcuutimphoi) ? -0.7969783 : 0;
            }
        }
        public List<double> getFactor()
        {
            List<double> kq = new List<double>();
            kq.AddRange(new List<double>() {nhiptimF, sbpF, strokenaoF, suythanmanF, honmeF,
        glasgowcomaF, xoganF, ungthuF, suythancapF, loannhipF, xhthF,
        khoinoisoF, hoisuctimF, thongkhicohocF, yeucauphauthuatF, capcuutimphoiF, fullcodeF});
            return kq;
        }
        public double kqMPM0()
        {
            double MPM0_F1 = 0;
            if (fullcodeF == 0)
                MPM0_F1 = honmeF + nhiptimF + sbpF + suythanmanF + xoganF + ungthuF + suythancapF + loannhipF +
                             strokenaoF + xhthF + khoinoisoF + hoisuctimF + thongkhicohocF + yeucauphauthuatF;
            else
                MPM0_F1 = -0.4243604;

            double MPM0_F2 = MPM0_F1 + (tuoi * 0.0385582) + fullcodeF - (honmeF * tuoi * 0.0075284) -
                             (sbpF * tuoi * 0.0085197) - (xoganF * tuoi * 0.022433) - (ungthuF * tuoi * 0.0330237) -
                             (loannhipF * tuoi * 0.0101286) - (khoinoisoF * tuoi * 0.0169215) -
                             (hoisuctimF * tuoi * 0.011214) - 5.36283;

            double mortality_MPM0 = 100 * Math.Exp(MPM0_F1) / (1 + Math.Exp(MPM0_F2));

            return mortality_MPM0;
        }
        public string kqMPM0_diengiai()
        {
            return "";
        }
    }
    public class DLCO_Adj : Congthuc
    {
        public string gioitinh { get; set; }
        public double tuoi { get; set; }
        public double Hb { get; set; }
        public double DLCOPredicted { get; set; }
        public DLCO_Adj()
        {

        }
        public DLCO_Adj(Nguoibenh nb, Xetnghiem xn)
        {
            Hb = xn.Hb;
            tuoi = nb.tinhtuoi_nam();
            gioitinh = nb.gioitinh;
            //init("C_B01");
        }

        public DLCO_Adj(string _gioitinh, double _tuoi, double _Hb, double _DLCOPredicted)
        {
            DLCOPredicted = _DLCOPredicted;
            Hb = _Hb;
            tuoi = _tuoi;
            gioitinh = _gioitinh.ToLower();
            //init("C_B01");
        }
        public double kqDLCO_Adj()
        {
            double DLCO_Adj = DLCOPredicted * 0.3348 * (1.7 * Hb / ((gioitinh == "nam" && tuoi > 15) ? 10.22 : 9.38));
            return DLCO_Adj;
        }
        public string kqDLCO_Adj_diengiai()
        {
            return "";
        }
    }
    public class MAP : Congthuc
    {
        public double SBP { get; set; }
        public double DBP { get; set; }
        public MAP()
        {

        }
        public MAP(Nguoibenh NB)
        {
            SBP = NB.HATThu;
            DBP = NB.HATTruong;
            //init("C_B02");
        }

        public MAP(double _SBP, double _DBP)
        {
            SBP = _SBP;
            DBP = _DBP;
            //init("C_B02");
        }
        public double kqMAP()
        {
            double MAP = (1.0 / 3) * SBP + (2.0 / 3) * DBP;
            return MAP;
        }
        public string kqMAP_diengiai()
        {
            return "";
        }
    }
    public class PostFEV1 : Congthuc
    {
        public double preFEV1 { get; set; }
        public double phanthuyCNcatbo { get; set; }
        public double tongphanthuyCN { get; set; }
        public double phansuattuoimau { get; set; }
        public bool phuongphapgiaiphau { get; set; }

        public PostFEV1()
        {
            //init("C_B03");
        }

        public PostFEV1(double _preFEV1, double _phanthuyCNcatbo, double _tongphanthuyCN, double _phansuattuoimau, bool _phuongphapgiaiphau)
        {
            phuongphapgiaiphau = _phuongphapgiaiphau;
            preFEV1 = _preFEV1;
            phanthuyCNcatbo = _phanthuyCNcatbo;
            tongphanthuyCN = _tongphanthuyCN;
            phansuattuoimau = _phansuattuoimau;
            //init("C_B03");
        }
        public double PostFEV1_Dich()
        {
            double PostFEV1 = preFEV1 * (1 - phansuattuoimau);
            return PostFEV1;
        }
        public double PostFEV1_GP()
        {
            double PostFEV1 = preFEV1 * (1 - phanthuyCNcatbo / tongphanthuyCN);
            return PostFEV1;
        }
        public double kqPostFEV1()
        {
            if (phuongphapgiaiphau)
                return PostFEV1_GP();
            else
                return PostFEV1_Dich();
        }
        public string kqPostFEV1_diengiai()
        {
            return "";
        }
    }
    public class PEF : Congthuc
    {
        public double chieucao { get; set; }
        public double tuoi { get; set; }
        public string gioitinh { get; set; }

        public PEF()
        {
            //init("C_B22");
        }
        public PEF(Nguoibenh nb)
        {
            chieucao = nb.chieucao;
            tuoi = nb.tinhtuoi_nam();
            gioitinh = nb.gioitinh;
            //init("C_B22");
        }

        public PEF(string _gioitinh, double _chieucao, double _tuoi)
        {
            chieucao = _chieucao;
            tuoi = _tuoi;
            gioitinh = _gioitinh.ToLower();
            //init("C_B22");
        }

        public double kqPEF()
        {
            if (tuoi < 18)
            {
                // < 18 years old
                return (chieucao - 100) * 5 + 100;
            }
            else if (gioitinh.ToLower() == "nam")
            {
                // Male
                return ((chieucao * 5.48) / 100 + 1.58 - (tuoi * 0.041)) * 60;
            }
            else if (gioitinh.ToLower() == "nu")
            {
                // Female
                return ((chieucao * 3.72) / 100 + 2.24 - (tuoi * 0.03)) * 60;
            }
            else
            {
                return 0.0;
            }
        }
        public string kqPEF_diengiai()
        {
            double PEF_kq = kqPEF();
            if (PEF_kq > 0.8)
                return "Chức năng phổi tốt";
            else if (PEF_kq > 0.5)
                return "Chức năng phổi suy giảm";
            else
                return "Chức năng phổi suy giảm nhiều";
        }
    }
    public class AEC : Congthuc
    {
        public double WBC { get; set; }
        public double WBC_Eos_tyle { get; set; }

        public AEC()
        {
            //init("C_B04");
        }
        public AEC(Xetnghiem XN)
        {
            if (XN.WBC != 0)
                WBC = XN.WBC;
            if (XN.WBC_EOS_tyle != 0)
                WBC_Eos_tyle = XN.WBC_EOS_tyle;
            //init("C_B04");
        }
        public AEC(double _WBC, double _WBC_Eos_tyle)
        {
            WBC = _WBC;
            WBC_Eos_tyle = _WBC_Eos_tyle;
            //init("C_B04");
        }
        public double kqAEC()
        {
            double AEC = WBC * WBC_Eos_tyle;
            return AEC;
        }
        public string kqAEC_diengiai()
        {
            double ketqua = kqAEC();
            if (ketqua < 40)
                return "Giảm bạch cầu ái toan (<40 tế bào/microL)";
            else if (ketqua < 450)
                return "Bạch cầu ái toan ở mức bình thường (40-450 tế bào/microL)";
            else if (ketqua < 1500)
                return "Tăng nhẹ bạch cầu ái toan (450 - 1.500 tế bào/microL)";
            else
                return "Tăng bạch cầu ái toan (>1.500 tế bào/microL)";
        }
    }
    public class ANC : Congthuc
    {
        public double WBC { get; set; }
        public double WBC_Neu_tyle { get; set; }
        public ANC()
        {
            //init("C_B05");
        }
        public ANC(Xetnghiem XN)
        {
            WBC = XN.WBC;
            WBC_Neu_tyle = XN.WBC_NEU_tyle;
            //init("C_B05");
        }
        public ANC(double _WBC, double _WBC_Neu_tyle)
        {
            WBC = _WBC;
            WBC_Neu_tyle = _WBC_Neu_tyle;
            //init("C_B05");
        }
        public double kqANC()
        {
            double ANC = WBC * WBC_Neu_tyle;
            return ANC;
        }
        public string kqANC_diengiai()
        {
            double ketqua = kqANC();
            if (ketqua < 500)
                return "Giảm bạch cầu trung tính nặng (<500 tế bào/microL)";
            else if (ketqua < 1000)
                return "Giảm bạch cầu trung tính trung bình (500 - 1.000 tế bào/microL)";
            else if (ketqua < 1500)
                return "Giảm bạch cầu trung tính nhẹ (1.000 - 1.500 tế bào/microL)";
            else if (ketqua < 8000)
                return "Bạch cầu trung tính trong khoảng bình thường (1.500 - 8.000 tế bào/microL)";
            else
                return "Tăng bạch cầu trung tính (>8.000 tế bào/microL)";
        }
    }
    public class MIPI : Congthuc
    {
        public double tuoi { get; set; }
        public double WBC { get; set; }
        public double LDHSerum { get; set; }
        public double LDHSerum_ULN { get; set; }
        public int ECOG { get; set; }

        public MIPI()
        {
            //init("C_B06");
        }
        public MIPI(Nguoibenh nb, Xetnghiem xn)
        {
            tuoi = nb.tinhtuoi_nam();
            WBC = xn.WBC;
            //init("C_B06");
        }
        public MIPI(double _tuoi, double _WBC, double _LDHSerum, double _LDHSerum_ULN, int _ECOG)
        {
            tuoi = _tuoi;
            ECOG = _ECOG;
            LDHSerum = _LDHSerum;
            LDHSerum_ULN = _LDHSerum_ULN;
            WBC = _WBC;
            //init("C_B06");
        }

        public double kqMIPI()
        {
            double LDHLog = Math.Log10(LDHSerum / LDHSerum_ULN);
            double WBCLog = Math.Log10(WBC);

            return (0.03535 * tuoi) + (ECOG > 1 ? 0.6978 : 0) + (1.367 * LDHLog + 0.9393 * WBCLog);
        }
        public string kqMIPI_diengiai()
        {
            double mipiScore = kqMIPI();

            if (mipiScore < 5.7)
            {
                return "Tiên lượng tốt";
            }
            else if (mipiScore > 6.2)
            {
                return "Tiên lượng xấu";
            }
            else
            {
                return "Tiên lượng trung bình";
            }
        }
    }
    public class RPI : Congthuc //C_B07
    {
        public double Hct { get; set; }
        public double Rec { get; set; }
        public RPI()
        {
            //init("C_B07");            
        }
        public RPI(Xetnghiem xn)
        {
            Hct = xn.Hct;
            //init("C_B07");
        }
        public RPI(double _Hct, double _Rec)
        {
            Hct = _Hct;
            Rec = _Rec;
            //init("C_B07");
        }
        public double kqRPI()
        {
            return (Hct / 45) * Rec / (Hct >= 0.4 ? 1 : (Hct >= 0.3 ? 1.5 : (Hct >= 0.2 ? 2 : 2.5)));
        }

        public string kqRPI_diengiai()
        {
            double rpi = kqRPI();

            if (rpi > 3)
            {
                return "Phản ứng bình thường của tủy xương đối với tình trạng thiếu máu";
            }
            else if (rpi < 2)
            {
                return "Tủy xương kém đáp ứng với tình trạng thiếu máu";
            }
            else
            {
                return "Tủy xương có đáp ứng với tình trạng thiếu máu";
            }
        }
    }
    public class sTfR : Congthuc
    {
        public double sTfRdoduoc { get; set; }
        public double Ferritin { get; set; }

        public sTfR()
        {
            //init("C_B08");
        }
        public sTfR(double _sTfRdoduoc, double _Ferritin)
        {
            sTfRdoduoc = _sTfRdoduoc;
            Ferritin = _Ferritin;
            //init("C_B08");
        }

        public double kqsTfR()
        {
            return sTfRdoduoc / Math.Log10(Ferritin);
        }
        public string kqsTfR_diengiai()
        {
            return "";
        }
    }
    public class BMR : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public double tuoi { get; set; }

        public BMR()
        {
            //init("C_B09");
        }
        public BMR(Nguoibenh nb)
        {
            gioitinh = nb.gioitinh;
            cannang = nb.cannang;
            chieucao = nb.chieucao;
            tuoi = nb.tinhtuoi_nam();
            //init("C_B09");
        }
        public BMR(string _gioitinh, double _chieucao, double _cannang, double _tuoi)
        {
            gioitinh = _gioitinh.ToLower();
            cannang = _cannang;
            chieucao = _chieucao;
            tuoi = _tuoi;
            //init("C_B09");
        }

        public double kqBMR_HB()
        {
            return (gioitinh == "nam")
                ? (66 + (13.7 * cannang) + (5 * chieucao) - (6.8 * tuoi))
                : (655 + (9.6 * cannang) + (1.8 * chieucao) - (4.7 * tuoi));
        }
        public double kqBMR_Scho()
        {
            if (gioitinh == "nam")
            {
                return (tuoi < 10)
                    ? ((22.706 * cannang) + 504.3)
                    : ((17.686 * cannang) + 658.2);
            }
            else
            {
                return (tuoi < 10)
                    ? ((20.315 * cannang) + 485.9)
                    : ((13.384 * cannang) + 692.6);
            }
        }
        public string kqBMR_diengiai()
        {
            return "";
        }
    }
    public class CDC_chieucao : Congthuc
    {
        public string gioitinh { get; set; }
        public double tuoi { get; set; }
        public double chieucao { get; set; }
        public double[] datachieucao { get; set; }
        public double[] dataLMS_Nam { get; set; }
        public double[] dataLMS_Nu { get; set; }
        public CDC_chieucao()
        {
            //init("C_B10");
        }
        public CDC_chieucao(string _gioitinh, double _chieucao, double _tuoi)
        {
            gioitinh = _gioitinh.ToLower();
            tuoi = _tuoi;
            chieucao = _chieucao;
            //init("C_B10");
        }
        public CDC_chieucao(Nguoibenh nb)
        {
            gioitinh = nb.gioitinh;
            tuoi = nb.tinhtuoi_thang();
            chieucao = nb.chieucao;
            //init("C_B10");
        }
        private void initCDC_chieucao()
        {
            double[] _datachieucao = { 45.57, 48.56, 52.73, 55.77, 58.24, 60.34, 62.18, 63.84, 65.36, 66.75, 68.06, 69.28, 70.43, 71.53, 72.57, 73.57, 74.53, 75.45, 76.34, 77.20, 78.03, 78.83, 79.61, 80.37, 81.11, 81.09, 81.83, 82.56, 83.28, 83.98, 84.67, 85.35, 86.01, 86.67, 87.32, 87.95, 88.58, 89.20, 89.77, 90.33, 90.89, 91.43, 91.97, 92.50, 93.03, 93.55, 94.06, 94.57, 95.08, 95.58, 96.08, 96.58, 97.07, 97.56, 98.05, 98.54, 99.03, 99.51, 100.00, 100.48, 100.97, 101.45, 101.94, 102.42, 102.91, 103.39, 103.88, 104.37, 104.86, 105.35, 105.84, 106.33, 106.82, 107.31, 107.80, 108.29, 108.79, 109.28, 109.77, 110.26, 110.76, 111.25, 111.74, 112.23, 112.71, 113.20, 113.68, 114.16, 114.64, 115.12, 115.59, 116.06, 116.53, 116.99, 117.45, 117.91, 118.36, 118.81, 119.25, 119.69, 120.12, 120.55, 120.97, 121.39, 121.80, 122.21, 122.61, 123.01, 123.40, 123.79, 124.18, 124.56, 124.93, 125.30, 125.67, 126.04, 126.40, 126.75, 127.11, 127.46, 127.81, 128.16, 128.51, 128.86, 129.21, 129.55, 129.90, 130.25, 130.60, 130.95, 131.31, 131.67, 132.03, 132.40, 132.77, 133.15, 133.53, 133.92, 134.32, 134.73, 135.14, 135.56, 135.99, 136.43, 136.88, 137.33, 137.80, 138.28, 138.76, 139.26, 139.77, 140.28, 140.81, 141.34, 141.89, 142.44, 143.00, 143.56, 144.13, 144.71, 145.29, 145.87, 146.46, 147.05, 147.64, 148.23, 148.81, 149.39, 149.97, 150.55, 151.11, 151.67, 152.22, 152.76, 153.29, 153.81, 154.32, 154.81, 155.30, 155.76, 156.22, 156.66, 157.08, 157.49, 157.89, 158.27, 158.63, 158.98, 159.32, 159.64, 159.94, 160.24, 160.51, 160.78, 161.03, 161.27, 161.50, 161.72, 161.92, 162.12, 162.30, 162.48, 162.64, 162.80, 162.95, 163.09, 163.22, 163.34, 163.46, 163.57, 163.67, 163.77, 163.86, 163.95, 164.03, 164.10, 164.18, 164.24, 164.31, 164.37, 164.42, 164.47, 164.52, 164.57, 164.61, 164.65, 164.69, 164.73, 164.76, 164.79, 164.82, 164.85, 164.87, 164.90, 164.92, 164.94, 164.96, 164.98, 165.00, 165.02, 165.03, 165.04, 49.99, 52.70, 56.63, 59.61, 62.08, 64.22, 66.13, 67.86, 69.46, 70.95, 72.35, 73.67, 74.92, 76.12, 77.26, 78.37, 79.43, 80.45, 81.44, 82.41, 83.34, 84.25, 85.13, 86.00, 86.84, 86.86, 87.65, 88.42, 89.18, 89.91, 90.63, 91.33, 92.02, 92.70, 93.36, 94.01, 94.65, 95.27, 95.91, 96.55, 97.17, 97.79, 98.40, 99.00, 99.60, 100.19, 100.78, 101.36, 101.94, 102.51, 103.08, 103.65, 104.21, 104.77, 105.33, 105.88, 106.43, 106.99, 107.54, 108.08, 108.63, 109.18, 109.72, 110.26, 110.81, 111.35, 111.89, 112.43, 112.97, 113.51, 114.05, 114.59, 115.12, 115.66, 116.20, 116.73, 117.27, 117.80, 118.33, 118.87, 119.40, 119.93, 120.46, 120.98, 121.51, 122.03, 122.55, 123.07, 123.59, 124.10, 124.62, 125.13, 125.63, 126.14, 126.64, 127.14, 127.63, 128.12, 128.61, 129.10, 129.58, 130.06, 130.53, 131.00, 131.46, 131.93, 132.38, 132.84, 133.29, 133.73, 134.18, 134.62, 135.05, 135.48, 135.91, 136.33, 136.76, 137.17, 137.59, 138.00, 138.41, 138.82, 139.23, 139.64, 140.04, 140.45, 140.85, 141.26, 141.66, 142.07, 142.48, 142.89, 143.31, 143.73, 144.15, 144.58, 145.02, 145.46, 145.91, 146.37, 146.83, 147.31, 147.79, 148.29, 148.79, 149.31, 149.84, 150.38, 150.93, 151.50, 152.07, 152.66, 153.26, 153.87, 154.50, 155.13, 155.76, 156.41, 157.06, 157.72, 158.38, 159.03, 159.69, 160.35, 161.00, 161.65, 162.29, 162.92, 163.54, 164.14, 164.74, 165.31, 165.88, 166.42, 166.95, 167.46, 167.96, 168.43, 168.89, 169.32, 169.74, 170.14, 170.52, 170.88, 171.23, 171.55, 171.86, 172.16, 172.43, 172.70, 172.95, 173.18, 173.40, 173.61, 173.81, 173.99, 174.17, 174.33, 174.49, 174.63, 174.77, 174.90, 175.02, 175.13, 175.24, 175.34, 175.44, 175.53, 175.61, 175.69, 175.77, 175.84, 175.90, 175.97, 176.03, 176.08, 176.13, 176.19, 176.23, 176.28, 176.32, 176.36, 176.40, 176.44, 176.47, 176.50, 176.53, 176.56, 176.59, 176.62, 176.64, 176.67, 176.69, 176.71, 176.73, 176.75, 176.77, 176.79, 176.81, 176.83, 176.84, 176.85, 54.31, 57.00, 60.96, 64.01, 66.55, 68.77, 70.75, 72.56, 74.24, 75.80, 77.27, 78.66, 79.99, 81.25, 82.46, 83.63, 84.75, 85.84, 86.89, 87.91, 88.90, 89.86, 90.80, 91.72, 92.61, 92.63, 93.53, 94.41, 95.26, 96.08, 96.88, 97.66, 98.42, 99.16, 99.87, 100.58, 101.26, 101.93, 102.59, 103.25, 103.89, 104.54, 105.17, 105.81, 106.43, 107.06, 107.68, 108.30, 108.91, 109.52, 110.13, 110.74, 111.35, 111.95, 112.55, 113.16, 113.76, 114.36, 114.96, 115.56, 116.16, 116.76, 117.35, 117.95, 118.55, 119.15, 119.75, 120.35, 120.94, 121.54, 122.14, 122.74, 123.33, 123.93, 124.53, 125.12, 125.72, 126.31, 126.90, 127.50, 128.09, 128.68, 129.27, 129.85, 130.44, 131.02, 131.60, 132.18, 132.76, 133.34, 133.91, 134.48, 135.04, 135.61, 136.17, 136.72, 137.28, 137.83, 138.37, 138.92, 139.45, 139.99, 140.52, 141.05, 141.57, 142.09, 142.61, 143.12, 143.62, 144.13, 144.63, 145.12, 145.61, 146.10, 146.59, 147.07, 147.55, 148.02, 148.50, 148.97, 149.44, 149.91, 150.37, 150.84, 151.30, 151.77, 152.23, 152.70, 153.16, 153.63, 154.10, 154.58, 155.06, 155.54, 156.03, 156.52, 157.02, 157.52, 158.04, 158.56, 159.09, 159.62, 160.17, 160.73, 161.29, 161.87, 162.45, 163.05, 163.66, 164.27, 164.90, 165.53, 166.17, 166.82, 167.47, 168.13, 168.79, 169.46, 170.12, 170.78, 171.45, 172.10, 172.75, 173.40, 174.03, 174.66, 175.27, 175.87, 176.45, 177.02, 177.57, 178.11, 178.63, 179.13, 179.61, 180.07, 180.51, 180.93, 181.34, 181.72, 182.09, 182.44, 182.78, 183.09, 183.39, 183.68, 183.94, 184.20, 184.44, 184.67, 184.88, 185.09, 185.28, 185.46, 185.63, 185.80, 185.95, 186.09, 186.23, 186.36, 186.48, 186.60, 186.71, 186.81, 186.91, 187.00, 187.09, 187.18, 187.26, 187.33, 187.40, 187.47, 187.53, 187.60, 187.65, 187.71, 187.76, 187.81, 187.86, 187.91, 187.95, 187.99, 188.03, 188.07, 188.11, 188.14, 188.17, 188.21, 188.24, 188.27, 188.29, 188.32, 188.35, 188.37, 188.39, 188.42, 188.44, 188.46, 188.48, 188.50, 188.52, 188.53, 45.58, 47.96, 51.48, 54.18, 56.43, 58.40, 60.16, 61.77, 63.26, 64.65, 65.96, 67.19, 68.37, 69.49, 70.57, 71.61, 72.61, 73.58, 74.51, 75.42, 76.30, 77.16, 78.00, 78.82, 79.61, 79.65, 80.44, 81.23, 82.00, 82.74, 83.47, 84.17, 84.84, 85.49, 86.11, 86.70, 87.26, 87.81, 88.34, 88.87, 89.40, 89.92, 90.44, 90.95, 91.47, 91.98, 92.49, 93.01, 93.52, 94.04, 94.56, 95.08, 95.60, 96.13, 96.65, 97.18, 97.71, 98.24, 98.78, 99.31, 99.85, 100.39, 100.93, 101.47, 102.01, 102.55, 103.09, 103.64, 104.18, 104.72, 105.26, 105.80, 106.33, 106.87, 107.40, 107.93, 108.46, 108.99, 109.51, 110.03, 110.54, 111.05, 111.56, 112.06, 112.56, 113.05, 113.54, 114.03, 114.50, 114.98, 115.44, 115.90, 116.36, 116.81, 117.25, 117.69, 118.12, 118.54, 118.96, 119.38, 119.79, 120.19, 120.58, 120.97, 121.36, 121.74, 122.12, 122.49, 122.86, 123.22, 123.58, 123.93, 124.29, 124.64, 124.99, 125.34, 125.69, 126.04, 126.39, 126.74, 127.09, 127.45, 127.82, 128.18, 128.56, 128.94, 129.33, 129.73, 130.15, 130.57, 131.01, 131.46, 131.92, 132.40, 132.90, 133.41, 133.93, 134.47, 135.03, 135.60, 136.18, 136.78, 137.38, 137.99, 138.61, 139.22, 139.84, 140.46, 141.06, 141.66, 142.25, 142.83, 143.39, 143.93, 144.45, 144.95, 145.43, 145.89, 146.32, 146.73, 147.12, 147.49, 147.83, 148.15, 148.45, 148.73, 148.99, 149.23, 149.46, 149.67, 149.86, 150.04, 150.21, 150.36, 150.51, 150.64, 150.76, 150.88, 150.98, 151.08, 151.18, 151.26, 151.34, 151.42, 151.48, 151.55, 151.61, 151.67, 151.72, 151.77, 151.82, 151.86, 151.90, 151.94, 151.98, 152.01, 152.05, 152.08, 152.11, 152.14, 152.16, 152.19, 152.21, 152.23, 152.26, 152.28, 152.30, 152.32, 152.33, 152.35, 152.37, 152.38, 152.40, 152.41, 152.43, 152.44, 152.45, 152.46, 152.48, 152.49, 152.50, 152.51, 152.52, 152.53, 152.54, 152.55, 152.56, 152.56, 152.57, 152.58, 152.59, 152.59, 152.60, 152.61, 152.61, 152.62, 152.63, 152.63, 152.64, 152.64, 152.65, 152.65, 49.29, 51.68, 55.29, 58.09, 60.46, 62.54, 64.41, 66.12, 67.71, 69.19, 70.59, 71.92, 73.19, 74.40, 75.56, 76.68, 77.76, 78.80, 79.81, 80.80, 81.76, 82.69, 83.60, 84.48, 85.35, 85.40, 86.29, 87.16, 88.00, 88.81, 89.58, 90.33, 91.05, 91.74, 92.40, 93.03, 93.63, 94.21, 94.80, 95.37, 95.95, 96.52, 97.08, 97.65, 98.21, 98.78, 99.34, 99.90, 100.47, 101.03, 101.60, 102.17, 102.74, 103.31, 103.89, 104.46, 105.04, 105.62, 106.20, 106.79, 107.37, 107.96, 108.54, 109.13, 109.72, 110.31, 110.90, 111.49, 112.08, 112.66, 113.25, 113.84, 114.42, 115.01, 115.59, 116.16, 116.74, 117.31, 117.88, 118.45, 119.01, 119.57, 120.13, 120.68, 121.22, 121.76, 122.30, 122.83, 123.35, 123.87, 124.39, 124.90, 125.40, 125.90, 126.39, 126.87, 127.35, 127.83, 128.29, 128.76, 129.22, 129.67, 130.11, 130.56, 131.00, 131.43, 131.86, 132.29, 132.71, 133.13, 133.55, 133.97, 134.38, 134.80, 135.22, 135.63, 136.05, 136.48, 136.90, 137.33, 137.77, 138.21, 138.66, 139.12, 139.58, 140.06, 140.54, 141.04, 141.55, 142.07, 142.60, 143.14, 143.70, 144.26, 144.84, 145.42, 146.02, 146.62, 147.23, 147.84, 148.46, 149.07, 149.68, 150.29, 150.89, 151.49, 152.07, 152.64, 153.19, 153.73, 154.25, 154.76, 155.24, 155.70, 156.14, 156.56, 156.96, 157.34, 157.70, 158.04, 158.36, 158.66, 158.94, 159.21, 159.46, 159.69, 159.91, 160.11, 160.30, 160.48, 160.64, 160.80, 160.94, 161.08, 161.21, 161.33, 161.44, 161.54, 161.64, 161.73, 161.82, 161.90, 161.97, 162.05, 162.11, 162.18, 162.23, 162.29, 162.34, 162.39, 162.44, 162.49, 162.53, 162.57, 162.61, 162.64, 162.68, 162.71, 162.74, 162.77, 162.80, 162.83, 162.85, 162.88, 162.90, 162.92, 162.95, 162.97, 162.99, 163.00, 163.02, 163.04, 163.06, 163.07, 163.09, 163.10, 163.12, 163.13, 163.14, 163.16, 163.17, 163.18, 163.19, 163.20, 163.21, 163.22, 163.23, 163.24, 163.25, 163.26, 163.27, 163.28, 163.28, 163.29, 163.30, 163.30, 163.31, 163.32, 163.32, 163.33, 163.34, 163.34, 53.77, 55.96, 59.39, 62.15, 64.53, 66.65, 68.57, 70.36, 72.02, 73.59, 75.07, 76.48, 77.84, 79.14, 80.39, 81.59, 82.76, 83.90, 85.00, 86.07, 87.11, 88.13, 89.13, 90.10, 91.05, 91.13, 92.12, 93.08, 94.01, 94.90, 95.76, 96.58, 97.36, 98.12, 98.84, 99.53, 100.19, 100.83, 101.47, 102.11, 102.75, 103.38, 104.01, 104.64, 105.27, 105.90, 106.53, 107.16, 107.79, 108.42, 109.06, 109.69, 110.33, 110.97, 111.61, 112.25, 112.89, 113.54, 114.18, 114.83, 115.48, 116.13, 116.78, 117.43, 118.08, 118.73, 119.39, 120.04, 120.69, 121.33, 121.98, 122.63, 123.27, 123.91, 124.55, 125.18, 125.81, 126.44, 127.06, 127.68, 128.30, 128.91, 129.51, 130.11, 130.70, 131.29, 131.88, 132.45, 133.03, 133.59, 134.15, 134.70, 135.25, 135.79, 136.33, 136.86, 137.38, 137.90, 138.41, 138.92, 139.42, 139.92, 140.41, 140.90, 141.38, 141.86, 142.34, 142.82, 143.29, 143.77, 144.24, 144.71, 145.19, 145.66, 146.14, 146.62, 147.11, 147.59, 148.09, 148.59, 149.09, 149.60, 150.12, 150.65, 151.19, 151.73, 152.29, 152.85, 153.42, 154.00, 154.58, 155.17, 155.77, 156.38, 156.98, 157.59, 158.20, 158.81, 159.42, 160.02, 160.62, 161.21, 161.79, 162.36, 162.91, 163.46, 163.98, 164.49, 164.99, 165.46, 165.92, 166.36, 166.78, 167.18, 167.56, 167.93, 168.27, 168.60, 168.91, 169.20, 169.47, 169.73, 169.98, 170.21, 170.42, 170.63, 170.82, 171.00, 171.16, 171.32, 171.47, 171.61, 171.74, 171.86, 171.98, 172.08, 172.19, 172.28, 172.37, 172.45, 172.53, 172.61, 172.68, 172.75, 172.81, 172.87, 172.92, 172.98, 173.03, 173.07, 173.12, 173.16, 173.20, 173.24, 173.27, 173.31, 173.34, 173.37, 173.40, 173.43, 173.46, 173.48, 173.51, 173.53, 173.55, 173.57, 173.59, 173.61, 173.63, 173.65, 173.66, 173.68, 173.70, 173.71, 173.72, 173.74, 173.75, 173.76, 173.78, 173.79, 173.80, 173.81, 173.82, 173.83, 173.84, 173.85, 173.86, 173.86, 173.87, 173.88, 173.89, 173.89, 173.90, 173.91, 173.91, 173.92, 173.93, 173.93, 173.94, 173.94, 173.95, 173.95 };
            datachieucao = _datachieucao;
            double[] _dataLMS_Nam = { 1.2670042261, 0.5112376962, -0.45224446, -0.990594599, -1.285837689, -1.43031238, -1.47657547, -1.456837849, -1.391898768, -1.29571459, -1.177919048, -1.045326049, -0.902800887, -0.753908107, -0.601263523, -0.446805039, -0.291974772, -0.13784767, 0.014776155, 0.1653041691, 0.3133018086, 0.4584554707, 0.6005446308, 0.7394389526, 0.8750004465, 1.00720807, 0.837251351, 0.681492975, 0.538779654, 0.407697153, 0.286762453, 0.174489485, 0.069444521, -0.029720564, -0.124251789, -0.215288396, -0.30385434, -0.390918369, -0.254801167, -0.125654535, -0.00316735, 0.11291221, 0.222754969, 0.326530126, 0.42436156, 0.516353108, 0.602595306, 0.683170764, 0.758158406, 0.827636736, 0.891686306, 0.95039153, 1.003830006, 1.05213569, 1.0953669, 1.133652119, 1.167104213, 1.195845353, 1.220004233, 1.239715856, 1.255121285, 1.266367398, 1.273606657, 1.276996893, 1.276701119, 1.272887366, 1.265728536, 1.255402281, 1.242090871, 1.225981067, 1.207263978, 1.186140222, 1.162796198, 1.137442868, 1.110286487, 1.081536236, 1.05140374, 1.020102497, 0.987847213, 0.954853043, 0.921334742, 0.887505723, 0.85357703, 0.819756239, 0.786246296, 0.753244292, 0.720940222, 0.689515708, 0.659142731, 0.629997853, 0.602203984, 0.575908038, 0.55123134, 0.528279901, 0.507143576, 0.487895344, 0.470590753, 0.455267507, 0.441945241, 0.430625458, 0.421291648, 0.413909588, 0.408427813, 0.404778262, 0.402877077, 0.402625561, 0.40391127, 0.406609232, 0.410583274, 0.415687443, 0.421767514, 0.428662551, 0.436206531, 0.44423, 0.45256176, 0.461030578, 0.469466904, 0.477704608, 0.48558272, 0.492947182, 0.499652617, 0.505564115, 0.510559047, 0.514528903, 0.517381177, 0.519041285, 0.519454524, 0.518588072, 0.516433004, 0.513006312, 0.508352901, 0.502547502, 0.495696454, 0.487939275, 0.479449924, 0.470437652, 0.461147305, 0.451858946, 0.442886661, 0.434576385, 0.427302633, 0.421464027, 0.417477538, 0.415771438, 0.416777012, 0.420919142, 0.428606007, 0.440218167, 0.456097443, 0.476536014, 0.501766234, 0.531951655, 0.567179725, 0.607456565, 0.652704121, 0.702759868, 0.757379106, 0.816239713, 0.878947416, 0.945053486, 1.014046108, 1.085383319, 1.158487278, 1.232768816, 1.307628899, 1.382473225, 1.456720479, 1.529810247, 1.601219573, 1.670433444, 1.736995571, 1.800483802, 1.860518777, 1.916765525, 1.968934444, 2.016781776, 2.060109658, 2.098765817, 2.132642948, 2.16167779, 2.185849904, 2.205180153, 2.219728869, 2.2295937, 2.234907144, 2.235833767, 2.232567138, 2.2253265, 2.214353232, 2.199905902, 2.182262864, 2.161704969, 2.138524662, 2.113023423, 2.085490286, 2.0562195, 2.025496648, 1.993598182, 1.960789092, 1.927320937, 1.89343024, 1.859337259, 1.825245107, 1.791339209, 1.757787065, 1.724738292, 1.692324905, 1.660661815, 1.629847495, 1.599964788, 1.571081817, 1.543252982, 1.516519998, 1.490912963, 1.466451429, 1.44314546, 1.420996665, 1.399999187, 1.380140651, 1.361403047, 1.343763564, 1.327195355, 1.311668242, 1.297149359, 1.283603728, 1.270994782, 1.25928483, 1.248435461, 1.23840791, 1.229163362, 1.220663228, 1.212869374, 1.20574431, 1.199251356, 1.19335477, 1.188019859, 1.183213059, 1.178901998, 1.175055543, 1.171643828, 1.16863827, 1.167279219, 49.988884079, 52.695975301, 56.628428552, 59.608953427, 62.077000266, 64.216864104, 66.125314898, 67.860179904, 69.459084582, 70.948039123, 72.345861109, 73.666654103, 74.921297174, 76.118375358, 77.264799111, 78.366223087, 79.427340501, 80.452094919, 81.443836034, 82.405436434, 83.339380627, 84.247833944, 85.132696575, 85.995648803, 86.838175097, 86.86160934, 87.65247282, 88.42326434, 89.17549228, 89.91040853, 90.62907762, 91.33242379, 92.02127167, 92.69637946, 93.35846546, 94.00822923, 94.64636981, 95.27359106, 95.91474929, 96.54734328, 97.17191309, 97.78897727, 98.3990283, 99.00254338, 99.599977, 100.191764, 100.7783198, 101.3600411, 101.9373058, 102.5104735, 103.0798852, 103.645864, 104.208713, 104.7687256, 105.3261638, 105.8812823, 106.4343146, 106.9854769, 107.534968, 108.0829695, 108.6296457, 109.1751441, 109.7195954, 110.2631136, 110.8057967, 111.3477265, 111.8889694, 112.4295761, 112.9695827, 113.5090108, 114.0478678, 114.5861486, 115.1238315, 115.6608862, 116.1972691, 116.732925, 117.2677879, 117.8017819, 118.3348215, 118.8668123, 119.397652, 119.9272309, 120.455433, 120.9821362, 121.5072136, 122.0305342, 122.5519634, 123.0713645, 123.588599, 124.1035312, 124.6160161, 125.1259182, 125.6331012, 126.1374319, 126.6387804, 127.1370217, 127.6320362, 128.1237104, 128.6119383, 129.096622, 129.5776723, 130.0550101, 130.5285669, 130.9982857, 131.4641218, 131.9260439, 132.3840348, 132.838092, 133.2882291, 133.7344759, 134.1768801, 134.6155076, 135.0504433, 135.4817925, 135.9096813, 136.3342577, 136.7556923, 137.1741794, 137.5899378, 138.0032114, 138.4142703, 138.8234114, 139.2309592, 139.6372663, 140.042714, 140.4477127, 140.8527022, 141.2581515, 141.6645592, 142.072452, 142.4823852, 142.8949403, 143.3107241, 143.7303663, 144.1545167, 144.5838414, 145.0190192, 145.4607359, 145.9096784, 146.3665278, 146.8319513, 147.3065929, 147.7910635, 148.2859294, 148.7917006, 149.3088178, 149.8376391, 150.3784267, 150.9313331, 151.4963887, 152.0734897, 152.6623878, 153.2626819, 153.8738124, 154.495058, 155.1255365, 155.7642086, 156.4098858, 157.0612415, 157.7168289, 158.3750929, 159.034399, 159.6930501, 160.3493168, 161.0014586, 161.6477515, 162.2865119, 162.9161202, 163.535045, 164.1418486, 164.7352199, 165.3139755, 165.8770715, 166.4236087, 166.9528354, 167.4641466, 167.9570814, 168.4313175, 168.8866644, 169.3230548, 169.7405351, 170.139255, 170.5194567, 170.881464, 171.2256717, 171.5525345, 171.8625576, 172.1562865, 172.4342983, 172.6971935, 172.9455898, 173.180112, 173.4013896, 173.6100518, 173.8067179, 173.9919998, 174.1664951, 174.3307855, 174.4854344, 174.6309856, 174.7679617, 174.8968634, 175.0181691, 175.1323345, 175.2397926, 175.340954, 175.4362071, 175.5259191, 175.6104358, 175.690083, 175.7651671, 175.8359757, 175.9027788, 175.9658293, 176.0253641, 176.081605, 176.1347593, 176.1850208, 176.2325707, 176.2775781, 176.3202008, 176.3605864, 176.3988725, 176.4351874, 176.469651, 176.5023751, 176.533464, 176.5630153, 176.5911197, 176.6178621, 176.6433219, 176.6675729, 176.6906844, 176.712721, 176.733743, 176.753807, 176.7729657, 176.7912687, 176.8087622, 176.8254895, 176.8414914, 176.8492322, 0.0531121908, 0.0486926838, 0.0441168302, 0.0417955825, 0.0404541256, 0.0396338789, 0.0391238128, 0.0388119944, 0.0386332091, 0.0385468328, 0.0385262623, 0.038553387, 0.0386155012, 0.0387034611, 0.0388105571, 0.0389317838, 0.0390633563, 0.0392023816, 0.0393466285, 0.0394943647, 0.0396442379, 0.0397951891, 0.0399463877, 0.0400971806, 0.0402470597, 0.040395626, 0.040577525, 0.040723122, 0.040833194, 0.040909059, 0.040952433, 0.04096533, 0.040949976, 0.040908737, 0.040844062, 0.040758431, 0.040654312, 0.04053412, 0.040572876, 0.04061691, 0.040666414, 0.040721467, 0.040782045, 0.040848042, 0.040919281, 0.040995524, 0.041076485, 0.041161838, 0.041251224, 0.041344257, 0.041440534, 0.041539635, 0.041641136, 0.041744602, 0.041849607, 0.041955723, 0.042062532, 0.042169628, 0.042276619, 0.042383129, 0.042488804, 0.042593311, 0.042696342, 0.042797615, 0.042896877, 0.042993904, 0.043088503, 0.043180513, 0.043269806, 0.043356287, 0.043439893, 0.043520597, 0.043598407, 0.043673359, 0.043745523, 0.043815003, 0.043881929, 0.043946461, 0.044008785, 0.044069112, 0.044127675, 0.044184725, 0.044240532, 0.044295379, 0.044349559, 0.044403374, 0.04445713, 0.044511135, 0.044565693, 0.044621104, 0.044677662, 0.044735646, 0.044795322, 0.044856941, 0.04492073, 0.044986899, 0.045055632, 0.045127088, 0.045201399, 0.045278671, 0.045358979, 0.045442372, 0.045528869, 0.045618459, 0.045711105, 0.045806742, 0.045905281, 0.046006604, 0.046110573, 0.046217028, 0.04632579, 0.046436662, 0.04654943, 0.046663871, 0.046779748, 0.046896817, 0.047014827, 0.047133525, 0.047252654, 0.047371961, 0.047491194, 0.047610108, 0.047728463, 0.04784603, 0.047962592, 0.048077942, 0.048191889, 0.048304259, 0.048414893, 0.048523648, 0.048630402, 0.04873505, 0.048837504, 0.048937694, 0.049035564, 0.049131073, 0.049224189, 0.049314887, 0.049403145, 0.049488934, 0.049572216, 0.049652935, 0.049731004, 0.0498063, 0.04987865, 0.049947823, 0.050013518, 0.050075353, 0.050132858, 0.050185471, 0.050232532, 0.050273285, 0.050306885, 0.050332406, 0.05034886, 0.050355216, 0.050350423, 0.050333444, 0.050303283, 0.050259018, 0.050199837, 0.050125062, 0.05003418, 0.049926861, 0.049802977, 0.04966261, 0.049506051, 0.049333801, 0.049146553, 0.04894519, 0.048730749, 0.048504404, 0.048267442, 0.04802123, 0.047767192, 0.047506783, 0.047241456, 0.04697265, 0.046701759, 0.046430122, 0.046159004, 0.045889585, 0.045622955, 0.045360101, 0.045101913, 0.044849174, 0.044602566, 0.044362674, 0.044129985, 0.043904897, 0.043687723, 0.043478698, 0.043277987, 0.043085685, 0.042901835, 0.042726424, 0.042559396, 0.042400652, 0.042250063, 0.042107465, 0.041972676, 0.041845488, 0.041725679, 0.041613015, 0.041507249, 0.041408129, 0.041315398, 0.041228796, 0.04114806, 0.041072931, 0.04100315, 0.040938463, 0.040878617, 0.040823368, 0.040772475, 0.040725706, 0.040682834, 0.04064364, 0.040607913, 0.040575448, 0.040546051, 0.040519532, 0.040495713, 0.040474421, 0.040455493, 0.040438773, 0.040424111, 0.040411366, 0.040400405, 0.040391101, 0.040383334, 0.04037699, 0.040371962, 0.040368149, 0.040365456, 0.040363795, 0.04036308, 0.040363233, 0.040364179, 0.04036585, 0.04036818, 0.040369574 };
            dataLMS_Nam = _dataLMS_Nam;
            double[] _dataLMS_Nu = { -1.295960857, -0.809249882, -0.050782985, 0.4768514065, 0.8432996117, 1.0975622571, 1.2725096408, 1.3904288587, 1.466733925, 1.5123019758, 1.534950767, 1.5403908751, 1.5328528917, 1.5155094695, 1.4907650275, 1.460458255, 1.4260060091, 1.3885070954, 1.3488181274, 1.3076096543, 1.2654081486, 1.2226277319, 1.1795943654, 1.1365644483, 1.0937319466, 1.051272912, 1.041951175, 1.012592236, 0.970541909, 0.921129988, 0.868221392, 0.81454413, 0.761957977, 0.711660228, 0.664323379, 0.620285102, 0.57955631, 0.54198094, 0.511429832, 0.482799937, 0.455521041, 0.429150288, 0.403351725, 0.377878239, 0.352555862, 0.327270297, 0.301955463, 0.276583851, 0.251158446, 0.225705996, 0.20027145, 0.174913356, 0.149700081, 0.12470671, 0.100012514, 0.075698881, 0.051847635, 0.02853967, 0.005853853, -0.016133871, -0.037351181, -0.057729947, -0.077206672, -0.09572283, -0.113225128, -0.129665689, -0.145002179, -0.159197885, -0.172221748, -0.184048358, -0.194660215, -0.204030559, -0.212174408, -0.219069129, -0.224722166, -0.229140412, -0.232335686, -0.234324563, -0.235128195, -0.234772114, -0.233286033, -0.230703633, -0.227062344, -0.222403111, -0.216770161, -0.210210748, -0.202774891, -0.194515104, -0.185486099, -0.175744476, -0.165348396, -0.15435722, -0.142831123, -0.130830669, -0.118416354, -0.105648092, -0.092584657, -0.079283065, -0.065797888, -0.0521805, -0.03847825, -0.024733545, -0.010982868, 0.002744306, 0.016426655, 0.030052231, 0.043619747, 0.05713988, 0.070636605, 0.08414848, 0.097729873, 0.111452039, 0.125404005, 0.13969316, 0.154445482, 0.169805275, 0.185934346, 0.203010488, 0.2212252, 0.240780542, 0.261885086, 0.284748919, 0.309577733, 0.336566048, 0.365889711, 0.397699038, 0.432104409, 0.46917993, 0.508943272, 0.551354277, 0.596307363, 0.643626542, 0.693062173, 0.744289752, 0.79691098, 0.85045728, 0.904395871, 0.958138449, 1.011054559, 1.062474568, 1.111727029, 1.158135105, 1.201050821, 1.239852328, 1.274006058, 1.303044695, 1.326605954, 1.344443447, 1.356437773, 1.362602695, 1.363085725, 1.358162799, 1.348227142, 1.333772923, 1.315374704, 1.293664024, 1.269304678, 1.242968236, 1.21531127, 1.186955477, 1.158471522, 1.130367088, 1.103079209, 1.076970655, 1.052329922, 1.029374161, 1.008254396, 0.989062282, 0.971837799, 0.95657215, 0.94324228, 0.931767062, 0.922058291, 0.914012643, 0.907516917, 0.902452436, 0.898698641, 0.896143482, 0.894659668, 0.89413892, 0.894475371, 0.895569834, 0.897330209, 0.899671635, 0.902516442, 0.905793969, 0.909440266, 0.913397733, 0.91761471, 0.922045055, 0.926647697, 0.931386217, 0.93622842, 0.941145943, 0.94611388, 0.95111043, 0.956116576, 0.961115792, 0.966093766, 0.971038162, 0.975938391, 0.980785418, 0.985571579, 0.99029042, 0.994936555, 0.999505539, 1.003993753, 1.0083983, 1.012716921, 1.016947912, 1.021090055, 1.025142554, 1.029104983, 1.032977233, 1.036759475, 1.040452117, 1.044055774, 1.047571238, 1.050999451, 1.054341482, 1.057598512, 1.060771808, 1.063862715, 1.066872639, 1.069803036, 1.072655401, 1.075431258, 1.078132156, 1.080759655, 1.083315329, 1.085800751, 1.088217496, 1.090567133, 1.092851222, 1.095071313, 1.097228939, 1.099325619, 1.101362852, 1.103342119, 1.105264876, 1.107132561, 1.108046193, 49.286396118, 51.683580573, 55.286128126, 58.093819061, 60.459807634, 62.536696555, 64.406327624, 66.118415533, 67.705744192, 69.191236138, 70.591639237, 71.919616727, 73.185010399, 74.395643786, 75.557854397, 76.676858713, 77.757009856, 78.801984056, 79.814918523, 80.798515316, 81.755120921, 82.686788098, 83.59532461, 84.48233206, 85.349236238, 85.3973169, 86.29026318, 87.15714182, 87.9960184, 88.8055115, 89.58476689, 90.33341722, 91.0515436, 91.7396352, 92.39854429, 93.02945392, 93.63382278, 94.21335709, 94.79643239, 95.37391918, 95.94692677, 96.51644912, 97.08337211, 97.6484807, 98.21246579, 98.77593069, 99.33939735, 99.9033122, 100.4680516, 101.033927, 101.6011898, 102.1700358, 102.7406094, 103.3130077, 103.8872839, 104.4634511, 105.0414853, 105.6213287, 106.2028921, 106.7860583, 107.3706841, 107.9566031, 108.5436278, 109.1315521, 109.7201531, 110.3091934, 110.8984228, 111.4875806, 112.0763967, 112.6645943, 113.2518902, 113.8380006, 114.4226317, 115.0054978, 115.5863089, 116.1647782, 116.7406221, 117.3135622, 117.8833259, 118.4496481, 119.0122722, 119.5709513, 120.1254495, 120.6755427, 121.22102, 121.7616844, 122.2973542, 122.827864, 123.3530652, 123.8728276, 124.38704, 124.8956114, 125.398472, 125.895574, 126.3868929, 126.8724284, 127.3522056, 127.8262759, 128.2947187, 128.757642, 129.2151839, 129.6675143, 130.1148354, 130.5573839, 130.995432, 131.4292887, 131.8593015, 132.2858574, 132.7093845, 133.1303527, 133.5492749, 133.9667073, 134.3832499, 134.7995463, 135.2162826, 135.634186, 136.0540223, 136.4765925, 136.9027281, 137.3332846, 137.7691339, 138.2111552, 138.6602228, 139.1171933, 139.5828898, 140.0580848, 140.5434787, 141.0396832, 141.5471945, 142.0663731, 142.59742, 143.1403553, 143.6949981, 144.2609497, 144.8375809, 145.4240246, 146.0191748, 146.621692, 147.2300177, 147.8423918, 148.4568879, 149.0714413, 149.6838943, 150.2920328, 150.8936469, 151.4865636, 152.0686985, 152.6380955, 153.1929631, 153.7317031, 154.2529332, 154.755501, 155.2384904, 155.7012216, 156.1432438, 156.564323, 156.9644258, 157.3436995, 157.7024507, 158.0411233, 158.3602756, 158.6605588, 158.9426964, 159.2074654, 159.455679, 159.688172, 159.9057871, 160.1093647, 160.299733, 160.4776996, 160.6440526, 160.7995428, 160.9448916, 161.0807857, 161.2078755, 161.3267744, 161.4380593, 161.5422726, 161.639917, 161.7314645, 161.8173534, 161.8979913, 161.9737558, 162.0449969, 162.1120386, 162.17518, 162.2346979, 162.2908474, 162.343864, 162.3939652, 162.4413513, 162.4862071, 162.5287029, 162.5689958, 162.6072309, 162.6435418, 162.6780519, 162.7108751, 162.7421168, 162.7718741, 162.8002371, 162.8272889, 162.8531067, 162.8777619, 162.9013208, 162.9238449, 162.9453912, 162.9660131, 162.9857599, 163.0046776, 163.0228094, 163.0401953, 163.0568727, 163.0728768, 163.0882404, 163.1029943, 163.1171673, 163.1307866, 163.1438776, 163.1564644, 163.1685697, 163.1802146, 163.1914194, 163.202203, 163.2125835, 163.2225779, 163.2322024, 163.2414722, 163.2504019, 163.2590052, 163.2672954, 163.2752848, 163.2829854, 163.2904086, 163.297565, 163.304465, 163.3111185, 163.3175349, 163.3237231, 163.3296918, 163.3354491, 163.338251, 0.0500855601, 0.0468185454, 0.0434439, 0.0417161032, 0.0407051733, 0.0400797646, 0.0396868449, 0.0394445547, 0.0393047376, 0.0392371101, 0.0392216648, 0.0392446716, 0.0392964203, 0.0393698746, 0.0394598321, 0.0395623818, 0.0396745415, 0.0397940102, 0.0399189943, 0.0400480838, 0.0401801621, 0.0403143396, 0.040449904, 0.0405862829, 0.0407230154, 0.040859727, 0.041142161, 0.041349399, 0.041500428, 0.041610508, 0.041691761, 0.04175368, 0.041803562, 0.041846882, 0.041887626, 0.041928568, 0.041971514, 0.042017509, 0.042104522, 0.042199507, 0.042300333, 0.042405225, 0.042512706, 0.042621565, 0.042730809, 0.042839638, 0.042947412, 0.043053626, 0.043157889, 0.043259907, 0.043359463, 0.043456406, 0.043550638, 0.043642107, 0.043730791, 0.043816701, 0.043899867, 0.043980337, 0.044058171, 0.04413344, 0.044206218, 0.044276588, 0.044344632, 0.044410436, 0.044474084, 0.044535662, 0.044595254, 0.044652942, 0.044708809, 0.044762936, 0.044815402, 0.044866288, 0.044915672, 0.044963636, 0.045010259, 0.045055624, 0.045099817, 0.045142924, 0.045185036, 0.045226249, 0.045266662, 0.045306383, 0.045345524, 0.045384203, 0.045422551, 0.045460702, 0.045498803, 0.045537012, 0.045575495, 0.045614432, 0.045654016, 0.04569445, 0.045735953, 0.045778759, 0.045823114, 0.04586928, 0.045917535, 0.045968169, 0.04602149, 0.046077818, 0.046137487, 0.046200842, 0.04626824, 0.046340046, 0.046416629, 0.046498361, 0.046585611, 0.046678741, 0.046778099, 0.04688401, 0.046996769, 0.047116633, 0.047243801, 0.047378413, 0.047520521, 0.047670085, 0.047826946, 0.04799081, 0.048161228, 0.04833757, 0.048519011, 0.048704503, 0.048892759, 0.049082239, 0.049271137, 0.049457371, 0.049638596, 0.049812203, 0.049975355, 0.050125012, 0.050257992, 0.050371024, 0.050460835, 0.050524236, 0.050558224, 0.050560083, 0.050527494, 0.050458634, 0.050352269, 0.050207825, 0.050025434, 0.049805967, 0.049551023, 0.049262895, 0.048944504, 0.048599314, 0.048231224, 0.047844442, 0.047443362, 0.04703243, 0.046616026, 0.046198356, 0.04578335, 0.045374597, 0.044975281, 0.044588148, 0.044215488, 0.043859135, 0.04352048, 0.043200497, 0.042899776, 0.042618565, 0.042356812, 0.042114211, 0.041890247, 0.04168424, 0.041495379, 0.041322765, 0.041165437, 0.041022401, 0.040892651, 0.040775193, 0.040669052, 0.040573288, 0.040487005, 0.040409354, 0.040339537, 0.040276811, 0.040220488, 0.040169932, 0.040124562, 0.040083845, 0.040047295, 0.040014473, 0.03998498, 0.039958458, 0.039934584, 0.039913066, 0.039893644, 0.039876087, 0.039860185, 0.039845754, 0.039832629, 0.039820663, 0.039809725, 0.0397997, 0.039790485, 0.039781991, 0.039774136, 0.03976685, 0.03976007, 0.039753741, 0.039747815, 0.039742249, 0.039737004, 0.039732048, 0.039727352, 0.03972289, 0.03971864, 0.039714581, 0.039710697, 0.039706971, 0.039703391, 0.039699945, 0.039696623, 0.039693415, 0.039690313, 0.039687311, 0.039684402, 0.039681581, 0.039678842, 0.039676182, 0.039673596, 0.039671082, 0.039668635, 0.039666254, 0.039663936, 0.039661679, 0.039659481, 0.039657339, 0.039655252, 0.039653218, 0.039651237, 0.039649306, 0.039647424, 0.039645591, 0.039643804, 0.039642063, 0.039640367, 0.039638715, 0.039637105, 0.039636316 };
            dataLMS_Nu = _dataLMS_Nu;
        }
        public int kqCDC_chieucao()
        {
            initCDC_chieucao();

            int stt = 0;
            //Gioi tinh
            if (gioitinh == "nữ")
                stt = 726;
            //Thang tuoi
            stt = stt + Convert.ToInt32(tuoi);

            double phanphoi5 = datachieucao[stt];
            double phanphoi50 = datachieucao[stt + 242];
            double phanphoi95 = datachieucao[stt + 242 * 2];

            if (chieucao < phanphoi5)
            {
                return 1;
            }
            else if (chieucao > phanphoi95)
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
        public string kqCDC_chieucao_diengiai()
        {
            if (kqCDC_chieucao() == 1)
            {
                return "Trẻ thuộc nhóm tầm vóc thấp (p < 5%)";
            }
            else if (kqCDC_chieucao() == 3)
            {
                return "Trẻ thuộc nhóm tầm vóc cao (p > 95%)";
            }
            else
            {
                return "Trẻ thuộc nhóm tầm vóc bình thường (5% < p < 95%)";
            }
        }
        public double kqCDC_chieucao_zscore()
        {
            initCDC_chieucao();

            int stt = 0;
            //Thang tuoi
            stt = stt + Convert.ToInt32(tuoi);

            double L, M, S;
            if (gioitinh == "nam")
            {
                L = dataLMS_Nam[stt];
                M = dataLMS_Nam[stt + 242];
                S = dataLMS_Nam[stt + 242 * 2];
            }
            else
            {
                L = dataLMS_Nu[stt];
                M = dataLMS_Nu[stt + 242];
                S = dataLMS_Nu[stt + 242 * 2];
            }
            return z_score(chieucao, L, M, S);
        }
    }
    public class CDC_cannang : Congthuc
    {
        public string gioitinh { get; set; }
        public double tuoi { get; set; }
        public double cannang { get; set; }
        public double[] datacannang { get; set; }
        public double[] dataLMS_Nam { get; set; }
        public double[] dataLMS_Nu { get; set; }
        public CDC_cannang()
        {
            //init("C_B11");
        }
        public CDC_cannang(string _gioitinh, double _cannang, double _tuoi)
        {
            gioitinh = _gioitinh.ToLower();
            tuoi = _tuoi;
            cannang = _cannang;
            //init("C_B11");
        }
        public CDC_cannang(Nguoibenh nb)
        {
            gioitinh = nb.gioitinh;
            tuoi = nb.tinhtuoi_thang();
            cannang = nb.cannang;
            //init("C_B11");
        }
        private void initCDC_cannang()
        {
            double[] _datacannang = { 2.53, 2.96, 3.77, 4.5, 5.16, 5.74, 6.27, 6.75, 7.17, 7.56, 7.9, 8.21, 8.49, 8.75, 8.98, 9.2, 9.39, 9.58, 9.74, 9.9, 10.05, 10.19, 10.33, 10.45, 10.58, 10.7, 10.82, 10.94, 11.05, 11.17, 11.28, 11.4, 11.51, 11.63, 11.75, 11.86, 11.98, 12.1, 12.22, 12.34, 12.46, 12.59, 12.71, 12.84, 12.96, 13.09, 13.22, 13.35, 13.48, 13.61, 13.74, 13.87, 14.01, 14.14, 14.28, 14.41, 14.55, 14.68, 14.82, 14.96, 15.09, 15.23, 15.37, 15.51, 15.65, 15.79, 15.93, 16.07, 16.22, 16.36, 16.5, 16.65, 16.79, 16.94, 17.08, 17.23, 17.38, 17.53, 17.68, 17.83, 17.98, 18.13, 18.28, 18.43, 18.59, 18.74, 18.9, 19.05, 19.21, 19.37, 19.53, 19.69, 19.85, 20.01, 20.17, 20.33, 20.5, 20.66, 20.83, 20.99, 21.16, 21.33, 21.5, 21.67, 21.84, 22.01, 22.18, 22.36, 22.53, 22.71, 22.89, 23.07, 23.25, 23.43, 23.61, 23.8, 23.98, 24.17, 24.36, 24.55, 24.75, 24.94, 25.14, 25.34, 25.55, 25.75, 25.96, 26.17, 26.39, 26.6, 26.82, 27.04, 27.27, 27.5, 27.73, 27.97, 28.21, 28.45, 28.7, 28.95, 29.21, 29.47, 29.73, 30, 30.27, 30.55, 30.83, 31.12, 31.41, 31.7, 32, 32.31, 32.62, 32.93, 33.25, 33.57, 33.89, 34.22, 34.56, 34.9, 35.24, 35.59, 35.94, 36.29, 36.65, 37.01, 37.37, 37.74, 38.1, 38.48, 38.85, 39.22, 39.6, 39.98, 40.36, 40.74, 41.12, 41.5, 41.88, 42.26, 42.64, 43.02, 43.39, 43.76, 44.14, 44.51, 44.87, 45.23, 45.59, 45.95, 46.3, 46.64, 46.98, 47.32, 47.64, 47.97, 48.28, 48.59, 48.9, 49.19, 49.48, 49.76, 50.04, 50.3, 50.56, 50.81, 51.05, 51.29, 51.51, 51.73, 51.94, 52.15, 52.35, 52.54, 52.72, 52.89, 53.06, 53.23, 53.38, 53.54, 53.68, 53.82, 53.96, 54.09, 54.22, 54.34, 54.46, 54.58, 54.69, 54.8, 54.91, 55.01, 55.1, 55.2, 55.28, 55.36, 55.44, 55.51, 55.56, 55.61, 55.65, 55.66, 3.53, 4, 4.88, 5.67, 6.39, 7.04, 7.63, 8.16, 8.64, 9.08, 9.48, 9.84, 10.16, 10.46, 10.73, 10.98, 11.21, 11.42, 11.62, 11.8, 11.98, 12.14, 12.3, 12.45, 12.6, 12.74, 12.88, 13.02, 13.15, 13.29, 13.43, 13.56, 13.7, 13.84, 13.97, 14.12, 14.26, 14.4, 14.55, 14.7, 14.85, 15, 15.16, 15.32, 15.48, 15.64, 15.81, 15.98, 16.15, 16.32, 16.49, 16.66, 16.84, 17.02, 17.2, 17.38, 17.56, 17.74, 17.93, 18.11, 18.3, 18.49, 18.67, 18.86, 19.05, 19.24, 19.43, 19.62, 19.81, 20, 20.2, 20.39, 20.58, 20.78, 20.97, 21.17, 21.36, 21.56, 21.76, 21.96, 22.16, 22.36, 22.56, 22.76, 22.96, 23.17, 23.37, 23.58, 23.79, 24, 24.21, 24.43, 24.64, 24.86, 25.08, 25.3, 25.53, 25.75, 25.98, 26.21, 26.45, 26.68, 26.92, 27.16, 27.41, 27.66, 27.91, 28.16, 28.42, 28.68, 28.95, 29.21, 29.48, 29.76, 30.04, 30.32, 30.6, 30.89, 31.19, 31.48, 31.78, 32.09, 32.4, 32.71, 33.03, 33.35, 33.67, 34, 34.34, 34.68, 35.02, 35.37, 35.72, 36.07, 36.43, 36.8, 37.17, 37.54, 37.92, 38.3, 38.68, 39.07, 39.47, 39.87, 40.27, 40.67, 41.08, 41.5, 41.92, 42.34, 42.76, 43.19, 43.62, 44.05, 44.49, 44.93, 45.37, 45.81, 46.26, 46.71, 47.16, 47.61, 48.06, 48.51, 48.96, 49.42, 49.87, 50.33, 50.78, 51.23, 51.68, 52.13, 52.58, 53.03, 53.47, 53.91, 54.35, 54.79, 55.22, 55.65, 56.07, 56.49, 56.91, 57.32, 57.72, 58.12, 58.51, 58.9, 59.28, 59.66, 60.03, 60.39, 60.75, 61.1, 61.44, 61.77, 62.1, 62.42, 62.73, 63.03, 63.33, 63.62, 63.9, 64.17, 64.44, 64.7, 64.95, 65.2, 65.43, 65.67, 65.89, 66.11, 66.32, 66.52, 66.72, 66.92, 67.11, 67.29, 67.47, 67.64, 67.81, 67.98, 68.14, 68.3, 68.46, 68.61, 68.76, 68.91, 69.05, 69.19, 69.34, 69.47, 69.61, 69.74, 69.87, 70, 70.12, 70.24, 70.35, 70.46, 70.55, 70.6, 4.34, 4.91, 5.97, 6.92, 7.78, 8.56, 9.26, 9.89, 10.45, 10.97, 11.43, 11.85, 12.23, 12.57, 12.89, 13.18, 13.45, 13.69, 13.92, 14.14, 14.35, 14.54, 14.73, 14.92, 15.1, 15.28, 15.45, 15.63, 15.8, 15.98, 16.16, 16.34, 16.53, 16.72, 16.91, 17.11, 17.31, 17.51, 17.72, 17.93, 18.15, 18.37, 18.6, 18.83, 19.06, 19.3, 19.54, 19.78, 20.03, 20.28, 20.54, 20.8, 21.06, 21.32, 21.58, 21.85, 22.12, 22.4, 22.67, 22.95, 23.23, 23.51, 23.79, 24.08, 24.36, 24.65, 24.94, 25.24, 25.53, 25.83, 26.12, 26.43, 26.73, 27.03, 27.34, 27.65, 27.96, 28.28, 28.59, 28.91, 29.24, 29.56, 29.89, 30.23, 30.56, 30.9, 31.24, 31.59, 31.94, 32.3, 32.66, 33.02, 33.39, 33.76, 34.13, 34.51, 34.9, 35.29, 35.68, 36.08, 36.49, 36.9, 37.31, 37.73, 38.16, 38.59, 39.02, 39.46, 39.91, 40.36, 40.82, 41.28, 41.74, 42.22, 42.69, 43.17, 43.66, 44.15, 44.64, 45.14, 45.65, 46.16, 46.67, 47.19, 47.71, 48.23, 48.76, 49.3, 49.83, 50.37, 50.91, 51.46, 52.01, 52.56, 53.11, 53.66, 54.22, 54.78, 55.34, 55.9, 56.47, 57.03, 57.6, 58.17, 58.74, 59.3, 59.87, 60.44, 61.01, 61.58, 62.15, 62.72, 63.28, 63.85, 64.41, 64.98, 65.54, 66.1, 66.66, 67.22, 67.78, 68.33, 68.89, 69.44, 69.99, 70.53, 71.07, 71.61, 72.15, 72.69, 73.22, 73.75, 74.27, 74.79, 75.31, 75.83, 76.34, 76.84, 77.35, 77.84, 78.34, 78.83, 79.31, 79.79, 80.27, 80.74, 81.2, 81.66, 82.11, 82.56, 83, 83.44, 83.87, 84.29, 84.71, 85.12, 85.52, 85.92, 86.3, 86.68, 87.06, 87.42, 87.78, 88.13, 88.46, 88.8, 89.12, 89.43, 89.73, 90.03, 90.31, 90.59, 90.86, 91.11, 91.36, 91.6, 91.83, 92.05, 92.26, 92.46, 92.65, 92.84, 93.01, 93.18, 93.34, 93.49, 93.64, 93.78, 93.92, 94.05, 94.18, 94.31, 94.44, 94.57, 94.7, 94.83, 94.97, 95.11, 95.27, 95.44, 95.62, 95.71, 2.55, 2.89, 3.55, 4.15, 4.71, 5.22, 5.69, 6.13, 6.53, 6.9, 7.25, 7.56, 7.86, 8.13, 8.38, 8.61, 8.83, 9.03, 9.22, 9.39, 9.56, 9.72, 9.87, 10.01, 10.14, 10.27, 10.4, 10.52, 10.64, 10.76, 10.87, 10.99, 11.1, 11.21, 11.32, 11.43, 11.54, 11.66, 11.77, 11.88, 12, 12.11, 12.23, 12.35, 12.47, 12.59, 12.71, 12.84, 12.96, 13.09, 13.22, 13.35, 13.48, 13.61, 13.75, 13.88, 14.02, 14.15, 14.29, 14.43, 14.57, 14.71, 14.85, 14.99, 15.13, 15.28, 15.42, 15.56, 15.71, 15.85, 16, 16.14, 16.29, 16.44, 16.58, 16.73, 16.88, 17.03, 17.17, 17.32, 17.47, 17.62, 17.77, 17.92, 18.08, 18.23, 18.38, 18.54, 18.69, 18.85, 19.01, 19.16, 19.32, 19.49, 19.65, 19.81, 19.98, 20.15, 20.31, 20.49, 20.66, 20.83, 21.01, 21.19, 21.37, 21.56, 21.74, 21.93, 22.12, 22.32, 22.51, 22.71, 22.91, 23.12, 23.33, 23.54, 23.75, 23.97, 24.19, 24.41, 24.64, 24.87, 25.1, 25.33, 25.57, 25.81, 26.05, 26.3, 26.55, 26.8, 27.06, 27.31, 27.57, 27.84, 28.1, 28.37, 28.64, 28.91, 29.18, 29.46, 29.74, 30.02, 30.3, 30.58, 30.87, 31.15, 31.44, 31.72, 32.01, 32.3, 32.59, 32.88, 33.17, 33.46, 33.74, 34.03, 34.32, 34.61, 34.89, 35.18, 35.46, 35.74, 36.02, 36.3, 36.57, 36.85, 37.12, 37.39, 37.65, 37.91, 38.17, 38.43, 38.68, 38.93, 39.17, 39.41, 39.65, 39.88, 40.11, 40.33, 40.55, 40.77, 40.98, 41.18, 41.38, 41.58, 41.77, 41.95, 42.13, 42.31, 42.48, 42.64, 42.8, 42.96, 43.11, 43.25, 43.39, 43.52, 43.65, 43.78, 43.9, 44.02, 44.13, 44.23, 44.34, 44.44, 44.53, 44.62, 44.71, 44.79, 44.87, 44.95, 45.03, 45.1, 45.17, 45.23, 45.3, 45.36, 45.42, 45.48, 45.53, 45.59, 45.64, 45.69, 45.74, 45.79, 45.84, 45.88, 45.93, 45.97, 46.01, 46.05, 46.09, 46.12, 46.16, 46.19, 46.21, 46.24, 46.26, 46.27, 46.29, 46.29, 3.4, 3.8, 4.54, 5.23, 5.86, 6.44, 6.97, 7.45, 7.9, 8.31, 8.69, 9.04, 9.37, 9.67, 9.94, 10.2, 10.45, 10.67, 10.89, 11.09, 11.28, 11.46, 11.64, 11.81, 11.97, 12.13, 12.29, 12.44, 12.6, 12.75, 12.9, 13.04, 13.19, 13.34, 13.49, 13.64, 13.79, 13.94, 14.09, 14.25, 14.4, 14.56, 14.72, 14.88, 15.04, 15.21, 15.37, 15.54, 15.71, 15.88, 16.05, 16.22, 16.4, 16.57, 16.75, 16.93, 17.11, 17.29, 17.47, 17.65, 17.84, 18.02, 18.21, 18.4, 18.59, 18.78, 18.97, 19.16, 19.35, 19.55, 19.74, 19.94, 20.14, 20.34, 20.54, 20.74, 20.94, 21.15, 21.36, 21.57, 21.78, 21.99, 22.21, 22.43, 22.65, 22.87, 23.09, 23.32, 23.55, 23.78, 24.02, 24.26, 24.5, 24.74, 24.99, 25.24, 25.5, 25.76, 26.02, 26.28, 26.55, 26.83, 27.1, 27.38, 27.67, 27.95, 28.25, 28.54, 28.84, 29.14, 29.45, 29.76, 30.07, 30.39, 30.71, 31.04, 31.37, 31.7, 32.04, 32.38, 32.72, 33.06, 33.41, 33.76, 34.12, 34.47, 34.83, 35.19, 35.55, 35.92, 36.28, 36.65, 37.02, 37.39, 37.76, 38.13, 38.5, 38.88, 39.25, 39.62, 39.99, 40.36, 40.73, 41.1, 41.46, 41.83, 42.19, 42.55, 42.91, 43.26, 43.62, 43.97, 44.31, 44.65, 44.99, 45.33, 45.66, 45.98, 46.31, 46.62, 46.93, 47.24, 47.54, 47.84, 48.13, 48.41, 48.69, 48.96, 49.23, 49.49, 49.75, 49.99, 50.24, 50.47, 50.7, 50.93, 51.14, 51.35, 51.56, 51.76, 51.95, 52.14, 52.32, 52.49, 52.66, 52.82, 52.98, 53.13, 53.28, 53.42, 53.56, 53.69, 53.82, 53.95, 54.07, 54.18, 54.29, 54.4, 54.51, 54.61, 54.71, 54.81, 54.91, 55, 55.09, 55.18, 55.27, 55.36, 55.45, 55.53, 55.62, 55.71, 55.79, 55.88, 55.97, 56.05, 56.14, 56.23, 56.32, 56.41, 56.5, 56.59, 56.69, 56.78, 56.87, 56.97, 57.07, 57.16, 57.26, 57.35, 57.45, 57.54, 57.63, 57.72, 57.8, 57.88, 57.96, 58.03, 58.09, 58.15, 58.2, 58.22, 4.15, 4.63, 5.52, 6.33, 7.08, 7.76, 8.38, 8.95, 9.48, 9.96, 10.4, 10.81, 11.2, 11.55, 11.88, 12.2, 12.49, 12.77, 13.03, 13.29, 13.53, 13.77, 14, 14.23, 14.46, 14.68, 14.9, 15.11, 15.33, 15.55, 15.77, 15.99, 16.21, 16.44, 16.67, 16.9, 17.13, 17.36, 17.6, 17.84, 18.08, 18.33, 18.58, 18.83, 19.08, 19.34, 19.6, 19.86, 20.13, 20.39, 20.66, 20.93, 21.21, 21.48, 21.76, 22.04, 22.32, 22.6, 22.89, 23.17, 23.46, 23.75, 24.04, 24.34, 24.63, 24.93, 25.23, 25.53, 25.83, 26.14, 26.45, 26.76, 27.08, 27.39, 27.71, 28.04, 28.36, 28.69, 29.03, 29.36, 29.7, 30.05, 30.4, 30.75, 31.11, 31.47, 31.84, 32.21, 32.59, 32.97, 33.36, 33.75, 34.15, 34.55, 34.96, 35.38, 35.8, 36.23, 36.66, 37.1, 37.54, 38, 38.45, 38.92, 39.39, 39.86, 40.34, 40.83, 41.32, 41.82, 42.32, 42.83, 43.35, 43.86, 44.39, 44.92, 45.45, 45.99, 46.53, 47.07, 47.62, 48.18, 48.73, 49.29, 49.85, 50.41, 50.98, 51.54, 52.11, 52.68, 53.25, 53.82, 54.39, 54.96, 55.53, 56.09, 56.66, 57.22, 57.78, 58.34, 58.9, 59.45, 60, 60.55, 61.09, 61.63, 62.16, 62.69, 63.21, 63.72, 64.23, 64.73, 65.23, 65.72, 66.2, 66.67, 67.14, 67.59, 68.04, 68.48, 68.92, 69.34, 69.75, 70.16, 70.56, 70.94, 71.32, 71.69, 72.04, 72.39, 72.73, 73.06, 73.38, 73.69, 73.99, 74.28, 74.56, 74.83, 75.09, 75.35, 75.59, 75.83, 76.06, 76.27, 76.48, 76.69, 76.88, 77.07, 77.25, 77.42, 77.59, 77.75, 77.9, 78.05, 78.2, 78.33, 78.47, 78.6, 78.72, 78.84, 78.96, 79.08, 79.19, 79.3, 79.41, 79.52, 79.62, 79.72, 79.83, 79.93, 80.03, 80.13, 80.24, 80.34, 80.44, 80.54, 80.65, 80.75, 80.86, 80.96, 81.07, 81.17, 81.28, 81.39, 81.49, 81.6, 81.71, 81.81, 81.92, 82.02, 82.12, 82.22, 82.32, 82.41, 82.5, 82.59, 82.67, 82.74, 82.81, 82.88, 82.93, 82.95 };
            datacannang = _datacannang;
            double[] _dataLMS_Nam = { 1.815151075, 1.547523128, 1.068795548, 0.695973505, 0.41981509, 0.219866801, 0.077505598, -0.02190761, -0.0894409, -0.1334091, -0.1600954, -0.17429685, -0.1797189, -0.179254, -0.17518447, -0.16932268, -0.1631139, -0.15770999, -0.15402279, -0.15276214, -0.15446658, -0.15952202, -0.16817926, -0.1805668, -0.19670196, -0.216501213, -0.239790488, -0.266315853, -0.295754969, -0.327729368, -0.361817468, -0.397568087, -0.434520252, -0.472188756, -0.510116627, -0.547885579, -0.58507011, -0.621319726, -0.656295986, -0.689735029, -0.721410388, -0.751175223, -0.778904279, -0.804515498, -0.828003255, -0.849380372, -0.86869965, -0.886033992, -0.901507878, -0.915241589, -0.927377772, -0.938069819, -0.94747794, -0.955765694, -0.963096972, -0.969633434, -0.975532355, -0.980937915, -0.986006518, -0.99086694, -0.995644402, -1.000453886, -1.005399668, -1.010575003, -1.016061941, -1.021931241, -1.028242376, -1.035043608, -1.042372125, -1.050254232, -1.058705595, -1.067731529, -1.077321193, -1.087471249, -1.098152984, -1.10933408, -1.120974043, -1.133024799, -1.145431351, -1.158132499, -1.171061612, -1.184141975, -1.197307185, -1.210475099, -1.223565263, -1.236497304, -1.249186293, -1.261555446, -1.273523619, -1.285013783, -1.295952066, -1.306268473, -1.31589753, -1.324778843, -1.332857581, -1.340080195, -1.346412105, -1.351813296, -1.356253969, -1.359710858, -1.362167159, -1.363612378, -1.364042106, -1.363457829, -1.361865669, -1.35928261, -1.355720571, -1.351202536, -1.345754408, -1.339405453, -1.332188093, -1.324137479, -1.315291073, -1.30568824, -1.295369867, -1.284374967, -1.272750864, -1.260539193, -1.247783611, -1.234527763, -1.220815047, -1.206688407, -1.19219015, -1.177361786, -1.162243894, -1.146876007, -1.131296524, -1.115542634, -1.099650267, -1.083654055, -1.067587314, -1.051482972, -1.035367321, -1.019277299, -1.003235326, -0.987269866, -0.971406609, -0.955670107, -0.940083834, -0.924670244, -0.909450843, -0.894446258, -0.879676305, -0.865160071, -0.850915987, -0.836961905, -0.823315176, -0.809992726, -0.797011132, -0.784386693, -0.772135506, -0.760273528, -0.748815968, -0.737780398, -0.727181568, -0.717035494, -0.707358338, -0.698166437, -0.689476327, -0.68130475, -0.673668658, -0.666585194, -0.660069969, -0.654142602, -0.648819666, -0.644118611, -0.640056805, -0.636651424, -0.633919328, -0.631876912, -0.63053994, -0.629923353, -0.630041066, -0.630905733, -0.632528509, -0.634918779, -0.638083884, -0.642028835, -0.646756013, -0.652262297, -0.658551638, -0.665609025, -0.673425951, -0.681987284, -0.691273614, -0.701261055, -0.711921092, -0.723218488, -0.735121189, -0.747580416, -0.760550666, -0.773984558, -0.787817728, -0.801993069, -0.816446409, -0.831110299, -0.845914498, -0.860786514, -0.875652181, -0.890436283, -0.905063185, -0.91945749, -0.933544683, -0.947251765, -0.960507855, -0.973244762, -0.985397502, -0.996904762, -1.007705555, -1.017756047, -1.027002713, -1.035402243, -1.042916356, -1.049511871, -1.055160732, -1.059840019, -1.063531973, -1.066224038, -1.067908908, -1.068589885, -1.068261146, -1.066933756, -1.064620976, -1.061341755, -1.057116957, -1.051988979, -1.04599033, -1.039168248, -1.031579574, -1.023291946, -1.014385118, -1.004952366, -0.995101924, -0.984958307, -0.974663325, -0.964376555, -0.954274945, -0.944551187, -0.935410427, -0.927059784, -0.919718461, -0.91648762, 3.530203168, 4.003106424, 4.879525083, 5.672888765, 6.391391982, 7.041836432, 7.630425182, 8.162951035, 8.644832479, 9.081119817, 9.476500305, 9.835307701, 10.16153567, 10.45885399, 10.7306256, 10.97992482, 11.20955529, 11.4220677, 11.61977698, 11.80477902, 11.9789663, 12.14404334, 12.30154103, 12.45283028, 12.59913494, 12.74154396, 12.88102276, 13.01842382, 13.1544966, 13.28989667, 13.42519408, 13.56088113, 13.69737858, 13.83504622, 13.97418299, 14.1150324, 14.25779618, 14.40262749, 14.54964614, 14.69893326, 14.85054151, 15.00449143, 15.16078454, 15.31940246, 15.48030313, 15.64343309, 15.80872535, 15.97610456, 16.14548194, 16.31676727, 16.4898646, 16.66467529, 16.84109948, 17.01903746, 17.1983908, 17.37906341, 17.56096245, 17.74400082, 17.92809121, 18.11315625, 18.29912286, 18.48592413, 18.67349965, 18.86179576, 19.05076579, 19.24037019, 19.43057662, 19.62136007, 19.8127028, 20.0045944, 20.19703171, 20.39001872, 20.58356862, 20.77769565, 20.97242631, 21.16779192, 21.36383013, 21.56058467, 21.75810506, 21.95644627, 22.15566842, 22.35583862, 22.55702268, 22.75929558, 22.9627344, 23.16741888, 23.37343341, 23.58086145, 23.78979096, 24.00031064, 24.21251028, 24.42648043, 24.642312, 24.86009596, 25.07992303, 25.30188584, 25.52606977, 25.75256528, 25.9814599, 26.2128399, 26.44679027, 26.68339457, 26.92273494, 27.16489199, 27.40994539, 27.65796978, 27.90904433, 28.16324264, 28.42063744, 28.68130005, 28.94530029, 29.21270645, 29.48358527, 29.75800198, 30.03602021, 30.31770417, 30.60311107, 30.89230072, 31.18532984, 31.48225315, 31.78312329, 32.08799062, 32.39690313, 32.7099062, 33.02704244, 33.34835148, 33.67386973, 34.00363017, 34.33766207, 34.67599076, 35.01863732, 35.36561737, 35.71694723, 36.07262569, 36.43265996, 36.79704392, 37.1657671, 37.53881268, 37.91615721, 38.2977703, 38.6836143, 39.07364401, 39.46780643, 39.86604044, 40.26827652, 40.67443658, 41.08443363, 41.49817164, 41.91554528, 42.33643978, 42.76073078, 43.18828419, 43.61895703, 44.0525931, 44.48903027, 44.92809483, 45.36960315, 45.81336172, 46.25916729, 46.70680701, 47.15605863, 47.60669074, 48.05846572, 48.51113138, 48.96443224, 49.41810374, 49.87187409, 50.32546478, 50.77859121, 51.23096332, 51.68228625, 52.13226113, 52.58058583, 53.02695588, 53.47106525, 53.91260737, 54.35127608, 54.78676659, 55.21877657, 55.64701131, 56.07116407, 56.49095862, 56.90610886, 57.31634059, 57.72138846, 58.12099696, 58.51492143, 58.90293208, 59.28479948, 59.66032626, 60.02931704, 60.39158721, 60.74698785, 61.09536847, 61.43660077, 61.77057372, 62.09719399, 62.41638628, 62.72809362, 63.03227756, 63.32891841, 63.61801537, 63.89958662, 64.17366943, 64.44032016, 64.69961427, 64.95164625, 65.1965295, 65.43440186, 65.66540015, 65.88970117, 66.10749114, 66.31897311, 66.52436618, 66.72390443, 66.91783563, 67.10641956, 67.28992603, 67.46863255, 67.64281378, 67.8127675, 67.97877331, 68.14111022, 68.30004741, 68.4558454, 68.60872174, 68.75889263, 68.90653028, 69.05176427, 69.19467288, 69.33527376, 69.47351373, 69.60925782, 69.74227758, 69.87223885, 69.99868896, 70.12104381, 70.23857482, 70.35039626, 70.45546105, 70.55252127, 70.59761453, 0.152385273, 0.146025021, 0.136478767, 0.129677511, 0.124717085, 0.121040119, 0.1182712, 0.116153695, 0.114510349, 0.113217163, 0.11218624, 0.111354536, 0.110676413, 0.110118635, 0.109656941, 0.109273653, 0.10895596, 0.108694678, 0.108483324, 0.108317416, 0.108193944, 0.108110954, 0.108067236, 0.108062078, 0.108095077, 0.108166006, 0.108274706, 0.108421025, 0.10860477, 0.108825681, 0.109083424, 0.109377581, 0.109707646, 0.110073084, 0.110473254, 0.1109074, 0.111374787, 0.111874514, 0.112405687, 0.112967254, 0.11355811, 0.114176956, 0.114822482, 0.115493292, 0.116187777, 0.116904306, 0.117641148, 0.118396541, 0.119168555, 0.11995532, 0.120754916, 0.121565421, 0.122384927, 0.123211562, 0.124043503, 0.124878992, 0.125716348, 0.126554022, 0.127390453, 0.128224294, 0.129054277, 0.129879257, 0.130698212, 0.131510245, 0.132314586, 0.133110593, 0.133897752, 0.134675673, 0.13544409, 0.13620286, 0.136951959, 0.137691478, 0.138421673, 0.139142773, 0.139855242, 0.140559605, 0.141256489, 0.141946613, 0.142630785, 0.143309898, 0.143984924, 0.144656953, 0.145327009, 0.145996289, 0.146666, 0.147337375, 0.148011715, 0.148690256, 0.149374297, 0.150065107, 0.150763933, 0.151471982, 0.152190413, 0.152920322, 0.153662731, 0.154418635, 0.155188768, 0.155973912, 0.156774684, 0.157591579, 0.158424964, 0.159275071, 0.160141995, 0.161025689, 0.161925976, 0.162842452, 0.163774719, 0.164722138, 0.165683945, 0.166659247, 0.167647017, 0.168646104, 0.169655235, 0.170673022, 0.17169797, 0.17272854, 0.173762961, 0.174799493, 0.175836284, 0.176871417, 0.177902912, 0.17892874, 0.17994683, 0.180955078, 0.181951361, 0.182933537, 0.183899465, 0.184847006, 0.185774041, 0.18667847, 0.187558229, 0.18841128, 0.189235738, 0.190029545, 0.190790973, 0.191518224, 0.192209619, 0.192863569, 0.193478582, 0.194053274, 0.194586368, 0.195076705, 0.195523246, 0.195925079, 0.196281418, 0.196591612, 0.19685514, 0.19707162, 0.197240806, 0.197362591, 0.197437004, 0.19746421, 0.197444522, 0.197378345, 0.197266263, 0.197108968, 0.196907274, 0.196662115, 0.196374538, 0.196045701, 0.195676862, 0.19526938, 0.19482473, 0.19434441, 0.193830046, 0.193283319, 0.192705974, 0.192099812, 0.191466681, 0.190808471, 0.190127105, 0.18942453, 0.188702714, 0.187963636, 0.187209281, 0.18644163, 0.185662657, 0.184874323, 0.184078567, 0.183277339, 0.182472427, 0.181665781, 0.18085918, 0.180054395, 0.179253153, 0.178457127, 0.177667942, 0.176887192, 0.176116307, 0.175356814, 0.174610071, 0.173877336, 0.173159953, 0.172459052, 0.171775726, 0.171110986, 0.170465756, 0.169840869, 0.169237063, 0.168654971, 0.168095124, 0.16755794, 0.167043722, 0.166552654, 0.166084798, 0.16564009, 0.165218341, 0.164819236, 0.16444238, 0.164087103, 0.163752791, 0.163438661, 0.163143825, 0.162867311, 0.162608072, 0.162365006, 0.162136973, 0.161922819, 0.161721398, 0.16153153, 0.161352313, 0.161182785, 0.161022184, 0.160869943, 0.160725793, 0.160589574, 0.1604617, 0.160342924, 0.160234478, 0.160138158, 0.160056393, 0.159992344, 0.159949989, 0.159934231, 0.159951004, 0.160007394, 0.160111769, 0.160273918, 0.160505203, 0.160818788, 0.161229617, 0.161476792 };
            dataLMS_Nam = _dataLMS_Nam;
            double[] _dataLMS_Nu = { 1.509187507, 1.357944315, 1.105537708, 0.902596648, 0.734121414, 0.590235275, 0.464391566, 0.352164071, 0.250497889, 0.15724751, 0.070885725, -0.00968493, -0.085258, -0.15640945, -0.22355869, -0.28701346, -0.34699919, -0.40368918, -0.45721877, -0.50770077, -0.55523599, -0.59992113, -0.64185418, -0.6811381, -0.71788283, -0.75220657, -0.78423366, -0.81409582, -0.841935504, -0.867889398, -0.892102647, -0.914718817, -0.935876584, -0.955723447, -0.974383363, -0.991980756, -1.008640742, -1.024471278, -1.039573604, -1.054039479, -1.067946784, -1.081374153, -1.094381409, -1.107021613, -1.119338692, -1.131367831, -1.143135936, -1.15466215, -1.165958392, -1.177029925, -1.187871001, -1.198484073, -1.208853947, -1.218965087, -1.228798212, -1.238330855, -1.247537914, -1.256392179, -1.264864846, -1.272926011, -1.28054514, -1.287691525, -1.294332076, -1.300441561, -1.305989011, -1.310946941, -1.315289534, -1.318992925, -1.322035315, -1.324398133, -1.326064539, -1.327020415, -1.327256387, -1.326763834, -1.325538668, -1.323579654, -1.320888012, -1.317468695, -1.313331446, -1.308487081, -1.302948173, -1.296733913, -1.289863329, -1.282358762, -1.274244931, -1.265548787, -1.256299378, -1.24653066, -1.236266832, -1.225551344, -1.214410914, -1.202884389, -1.191007906, -1.178818621, -1.166354376, -1.153653688, -1.140751404, -1.127684095, -1.114490244, -1.101204848, -1.087863413, -1.074500927, -1.061151213, -1.047847141, -1.034620551, -1.021502197, -1.008521695, -0.995707494, -0.983086844, -0.970685789, -0.958529157, -0.946640568, -0.935042447, -0.923756041, -0.912801445, -0.902197638, -0.891962513, -0.882112919, -0.872664706, -0.863632768, -0.855031092, -0.846872805, -0.839170224, -0.831934903, -0.825177688, -0.818908758, -0.813137675, -0.807873433, -0.803122613, -0.79889771, -0.795203499, -0.792047959, -0.789435274, -0.787374433, -0.785870695, -0.784929893, -0.784557605, -0.78475917, -0.785539703, -0.786904102, -0.788858208, -0.791403051, -0.794546352, -0.79829102, -0.802640891, -0.807599577, -0.813170461, -0.819356692, -0.826161176, -0.833586038, -0.841634949, -0.850307441, -0.859607525, -0.869534339, -0.880088651, -0.891270585, -0.903079458, -0.915513542, -0.928569454, -0.942245864, -0.956537923, -0.971440492, -0.986947308, -1.003050887, -1.019742425, -1.037011698, -1.054846957, -1.073234825, -1.092160195, -1.111606122, -1.131553723, -1.151982079, -1.172868141, -1.19418462, -1.215907492, -1.238005268, -1.260445591, -1.283193626, -1.306212032, -1.329460945, -1.35289798, -1.376478254, -1.400154426, -1.423876772, -1.447593267, -1.471249702, -1.494789826, -1.518155513, -1.541286949, -1.564122852, -1.586600712, -1.608657054, -1.630227728, -1.651248208, -1.67165392, -1.691380583, -1.710364557, -1.728543207, -1.745855274, -1.762241248, -1.777643747, -1.792007891, -1.805281675, -1.817416335, -1.828366707, -1.838091576, -1.846554015, -1.853721704, -1.859567242, -1.864068443, -1.86720861, -1.8689768, -1.869371157, -1.868386498, -1.866033924, -1.862327775, -1.857289195, -1.850946286, -1.84333425, -1.834495505, -1.824479785, -1.813344222, -1.801153404, -1.787979408, -1.773901816, -1.759007704, -1.743391606, -1.72715546, -1.710410733, -1.693267093, -1.67585442, -1.658302847, -1.640747464, -1.623332891, -1.606209374, -1.589533346, -1.573467222, -1.558179166, -1.543846192, -1.530642461, -1.518754013, -1.51336185, 3.39918645, 3.79752846, 4.544776513, 5.230584214, 5.859960798, 6.437587751, 6.967850457, 7.454854109, 7.902436186, 8.314178377, 8.693418423, 9.043261854, 9.366593571, 9.666089185, 9.944226063, 10.20329397, 10.4454058, 10.67250698, 10.88638558, 11.08868151, 11.28089537, 11.46439708, 11.64043402, 11.81013895, 11.97453748, 12.13455523, 12.2910249, 12.44469258, 12.59622335, 12.74620911, 12.89517218, 13.04357164, 13.19180874, 13.34022934, 13.48913319, 13.63877446, 13.78936547, 13.94108332, 14.09407175, 14.24844498, 14.40429169, 14.56167529, 14.72064045, 14.88121352, 15.04340553, 15.20721443, 15.37262729, 15.53962221, 15.70817017, 15.87823668, 16.04978452, 16.2227706, 16.39715363, 16.57289122, 16.74994187, 16.92826587, 17.10782615, 17.28858894, 17.47052444, 17.65360733, 17.83781722, 18.02313904, 18.20956418, 18.3970876, 18.58571243, 18.77544728, 18.966307, 19.15831267, 19.35149163, 19.54587708, 19.74150854, 19.93843145, 20.13669623, 20.33635961, 20.53748298, 20.74013277, 20.94438028, 21.15030093, 21.35797332, 21.56748045, 21.77890902, 21.99234686, 22.20788541, 22.4256177, 22.64563824, 22.86804258, 23.09292679, 23.32038549, 23.55051871, 23.78341652, 24.01917703, 24.25789074, 24.49964778, 24.74453536, 24.99263735, 25.24403371, 25.49880264, 25.7570168, 26.01874261, 26.28404312, 26.55297507, 26.82558904, 27.1019295, 27.38203422, 27.66593402, 27.9536524, 28.24520531, 28.54060085, 28.83983907, 29.14291171, 29.44980208, 29.76048479, 30.0749257, 30.39308176, 30.71490093, 31.0403221, 31.36927506, 31.7016805, 32.03744999, 32.37648607, 32.71868225, 33.06392318, 33.4120847, 33.76303402, 34.1166299, 34.47272283, 34.83115524, 35.19176177, 35.55437176, 35.91879976, 36.28486194, 36.65236365, 37.02110818, 37.39088668, 37.76148905, 38.1326991, 38.50429603, 38.87605489, 39.24774707, 39.61914076, 39.98999994, 40.36009244, 40.72917544, 41.09701099, 41.46335907, 41.82797963, 42.19063313, 42.55108107, 42.90908653, 43.2644155, 43.61683402, 43.9661169, 44.31203579, 44.65437319, 44.99291356, 45.32744704, 45.65777013, 45.98368656, 46.30500858, 46.62155183, 46.93314404, 47.23962058, 47.54082604, 47.83661466, 48.12685082, 48.41140938, 48.69017613, 48.9630481, 49.22993391, 49.49075409, 49.74544132, 49.99394068, 50.23620985, 50.47222213, 50.70195581, 50.92540942, 51.14259229, 51.3535268, 51.55824831, 51.75680513, 51.94925841, 52.13568193, 52.31616197, 52.49079703, 52.65969757, 52.82298572, 52.9807949, 53.13326946, 53.28056425, 53.42284417, 53.5602837, 53.69306637, 53.82138422, 53.94543725, 54.06543278, 54.18158486, 54.29411356, 54.40324431, 54.50920717, 54.61223603, 54.71256787, 54.81044184, 54.90609842, 54.99977846, 55.09172217, 55.18216811, 55.271352, 55.35950558, 55.44685531, 55.53362107, 55.62001464, 55.70623826, 55.79247939, 55.87892356, 55.96573022, 56.05304601, 56.14099882, 56.22969564, 56.3192203, 56.40963105, 56.50095811, 56.59320107, 56.68632619, 56.78026364, 56.87490465, 56.97009856, 57.06564989, 57.16131528, 57.25679821, 57.35175792, 57.44578172, 57.53840429, 57.62910094, 57.7172758, 57.80226553, 57.88333502, 57.95967458, 58.0303973, 58.09453209, 58.15103575, 58.1987714, 58.21897289, 0.142106724, 0.138075916, 0.131733888, 0.126892697, 0.123025182, 0.119840911, 0.117166868, 0.11489384, 0.112949644, 0.11128469, 0.109863709, 0.10866078, 0.10765621, 0.106834517, 0.106183085, 0.105691242, 0.105349631, 0.105149754, 0.105083666, 0.105143752, 0.105322575, 0.10561278, 0.106007025, 0.106497957, 0.107078197, 0.107740345, 0.10847701, 0.109280828, 0.110144488, 0.111060815, 0.112022759, 0.113023467, 0.114056328, 0.115114953, 0.116193327, 0.11728575, 0.118386848, 0.119491669, 0.120595658, 0.121694676, 0.12278503, 0.1238634, 0.124926943, 0.125973221, 0.127000212, 0.128006292, 0.128990225, 0.129951143, 0.130888527, 0.131802186, 0.132692269, 0.133559108, 0.134403386, 0.13522599, 0.136028014, 0.136810739, 0.137575606, 0.138324193, 0.139058192, 0.139779387, 0.140489635, 0.141190842, 0.141884974, 0.142573939, 0.143259709, 0.143944216, 0.144629359, 0.14531699, 0.146008903, 0.146706813, 0.147412363, 0.148127109, 0.148852482, 0.149589838, 0.1503404, 0.151105277, 0.151885464, 0.152681819, 0.15349505, 0.154325756, 0.155174414, 0.15604132, 0.156926667, 0.157830504, 0.158752743, 0.159693163, 0.16065141, 0.161626956, 0.162619308, 0.1636276, 0.1646511, 0.165688808, 0.166739662, 0.167802495, 0.168876037, 0.169958922, 0.171049756, 0.172147043, 0.173249185, 0.174354569, 0.175461512, 0.176568284, 0.177673124, 0.178774242, 0.179869829, 0.180958063, 0.182037118, 0.183105172, 0.18416041, 0.185201039, 0.186225287, 0.187231416, 0.188217723, 0.18918255, 0.190124286, 0.191041375, 0.191932319, 0.192795682, 0.193630095, 0.19443426, 0.195206948, 0.195947008, 0.196653365, 0.197325023, 0.197961065, 0.198560655, 0.199123037, 0.199647538, 0.200133598, 0.200580618, 0.200988216, 0.201356017, 0.201683791, 0.201971282, 0.202218375, 0.202425006, 0.202591183, 0.20271698, 0.202802535, 0.202848049, 0.202853758, 0.202820053, 0.202747236, 0.202635758, 0.202486098, 0.202298783, 0.202074385, 0.201813521, 0.201516851, 0.201185082, 0.200818928, 0.200419208, 0.199986681, 0.199522233, 0.199026736, 0.198501096, 0.197946255, 0.197363191, 0.196752931, 0.196116472, 0.19545489, 0.194769279, 0.194060758, 0.193330477, 0.192579614, 0.191809374, 0.191020995, 0.190215739, 0.189394901, 0.188559804, 0.187711798, 0.186852266, 0.185982617, 0.185104331, 0.184218803, 0.183327556, 0.182432113, 0.181534018, 0.180634839, 0.179736168, 0.178839614, 0.177946804, 0.177059379, 0.17617899, 0.175307296, 0.174445958, 0.173596636, 0.172760982, 0.17194064, 0.171137232, 0.170352363, 0.169587605, 0.168844497, 0.168124538, 0.167429179, 0.166759816, 0.166117788, 0.165504365, 0.164920747, 0.164368054, 0.16384732, 0.163359491, 0.162905415, 0.162485839, 0.162101402, 0.161752634, 0.161439944, 0.161163623, 0.160923833, 0.160720609, 0.16055385, 0.160423319, 0.160328578, 0.160269232, 0.160244549, 0.160253714, 0.160295765, 0.16036959, 0.16047393, 0.160607377, 0.16076838, 0.160955249, 0.161166157, 0.161399151, 0.161652158, 0.161922998, 0.162209399, 0.162509006, 0.162819353, 0.163138124, 0.163462715, 0.163790683, 0.164119574, 0.164446997, 0.164770638, 0.165088289, 0.165397881, 0.165697507, 0.165985386, 0.166260109, 0.16652037, 0.166644749 };
            dataLMS_Nu = _dataLMS_Nu;
        }
        public int kqCDC_cannang()
        {
            initCDC_cannang();

            int stt = 0;
            //Gioi tinh
            if (gioitinh == "nữ")
                stt = 726;
            //Thang tuoi
            stt = stt + Convert.ToInt32(tuoi);

            double phanphoi5 = datacannang[stt];
            double phanphoi50 = datacannang[stt + 242];
            double phanphoi95 = datacannang[stt + 242 * 2];

            if (cannang < phanphoi5)
            {
                return 1;
            }
            else if (cannang > phanphoi95)
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
        public string kqCDC_cannang_diengiai()
        {
            if (kqCDC_cannang() == 1)
            {
                return "Trẻ thuộc nhóm nhẹ cân (p < 5%)";
            }
            else if (kqCDC_cannang() == 3)
            {
                return "Trẻ thuộc nhóm nặng cân (p > 95%)";
            }
            else
            {
                return "Trẻ thuộc nhóm cân nặng bình thường (5% < p < 95%)";
            }
        }
        public double kqCDC_cannang_zscore()
        {
            initCDC_cannang();

            int stt = 0;
            //Thang tuoi
            stt = stt + Convert.ToInt32(tuoi);

            double L, M, S;
            if (gioitinh == "nam")
            {
                L = dataLMS_Nam[stt];
                M = dataLMS_Nam[stt + 242];
                S = dataLMS_Nam[stt + 242 * 2];
            }
            else
            {
                L = dataLMS_Nu[stt];
                M = dataLMS_Nu[stt + 242];
                S = dataLMS_Nu[stt + 242 * 2];
            }
            return z_score(cannang, L, M, S);
        }
    }
    public class CDC_chuvi : Congthuc
    {
        public string gioitinh { get; set; }
        public double tuoi { get; set; }
        public double chuvivongdau { get; set; }
        public double[] datachuvi { get; set; }
        public double[] dataLMS { get; set; }
        public CDC_chuvi()
        {
            //init("C_B12");
        }
        public CDC_chuvi(string _gioitinh, double _tuoi, double _chuvivongdau)
        {
            gioitinh = _gioitinh.ToLower();
            tuoi = _tuoi;
            chuvivongdau = _chuvivongdau;
            //init("C_B12");
        }
        public CDC_chuvi(Nguoibenh nb)
        {
            gioitinh = nb.gioitinh;
            tuoi = nb.tinhtuoi_thang();
            //init("C_B12");
        }
        private void initCDC_chuvi()
        {
            double[] _datachuvi = { 32.15, 33.83, 36.26, 37.98, 39.28, 40.31, 41.15, 41.85, 42.44, 42.95, 43.39, 43.78, 44.12, 44.43, 44.70, 44.94, 45.15, 45.35, 45.52, 45.68, 45.82, 45.95, 46.07, 46.17, 46.27, 46.35, 46.43, 46.50, 46.56, 46.62, 46.67, 46.71, 46.75, 46.78, 46.81, 46.84, 46.86, 46.87, 35.81, 37.19, 39.21, 40.65, 41.77, 42.66, 43.40, 44.04, 44.58, 45.06, 45.48, 45.86, 46.19, 46.50, 46.78, 47.03, 47.26, 47.48, 47.68, 47.86, 48.03, 48.19, 48.33, 48.47, 48.60, 48.72, 48.83, 48.94, 49.04, 49.13, 49.22, 49.30, 49.38, 49.46, 49.53, 49.59, 49.65, 49.68, 38.52, 39.77, 41.63, 42.97, 44.02, 44.87, 45.59, 46.20, 46.73, 47.20, 47.62, 48.00, 48.34, 48.65, 48.94, 49.20, 49.44, 49.67, 49.88, 50.08, 50.26, 50.44, 50.60, 50.76, 50.90, 51.04, 51.17, 51.30, 51.41, 51.53, 51.63, 51.74, 51.84, 51.93, 52.02, 52.10, 52.19, 52.23, 32.25, 33.69, 35.78, 37.27, 38.41, 39.32, 40.07, 40.71, 41.25, 41.72, 42.14, 42.51, 42.84, 43.13, 43.40, 43.64, 43.86, 44.06, 44.25, 44.42, 44.57, 44.72, 44.85, 44.97, 45.08, 45.19, 45.29, 45.38, 45.46, 45.54, 45.62, 45.68, 45.75, 45.81, 45.86, 45.91, 45.96, 45.98, 34.71, 36.03, 37.98, 39.38, 40.47, 41.35, 42.08, 42.71, 43.25, 43.73, 44.16, 44.54, 44.88, 45.20, 45.48, 45.74, 45.98, 46.21, 46.42, 46.61, 46.79, 46.96, 47.12, 47.27, 47.41, 47.54, 47.66, 47.78, 47.89, 48.00, 48.09, 48.19, 48.28, 48.37, 48.45, 48.52, 48.60, 48.63, 37.65, 38.83, 40.57, 41.84, 42.83, 43.65, 44.34, 44.93, 45.45, 45.91, 46.32, 46.69, 47.03, 47.34, 47.63, 47.90, 48.14, 48.38, 48.59, 48.80, 48.99, 49.17, 49.34, 49.50, 49.66, 49.80, 49.94, 50.08, 50.21, 50.33, 50.45, 50.56, 50.67, 50.77, 50.87, 50.97, 51.06, 51.10 };
            datachuvi = _datachuvi;
            double[] _dataLMS = { 4.427825037, 4.310927464, 3.869576802, 3.305593039, 2.720590297, 2.16804824, 1.675465689, 1.255160322, 0.91054114, 0.639510474, 0.436978864, 0.296275856, 0.210107251, 0.171147024, 0.172393886, 0.207371541, 0.270226126, 0.355757274, 0.459407627, 0.577227615, 0.705826778, 0.842319055, 0.984266833, 1.129626698, 1.276691223, 1.424084853, 1.570621291, 1.715393998, 1.857652984, 1.996810563, 2.132411346, 2.264111009, 2.391658052, 2.514878222, 2.633661226, 2.747949445, 2.857728375, 2.910932095, -1.298749689, -1.440271514, -1.581016348, -1.593136386, -1.521492427, -1.394565915, -1.231713389, -1.046582628, -0.848932692, -0.645779124, -0.442165412, -0.24163206, -0.046673786, 0.141031094, 0.320403169, 0.490807133, 0.65193505, 0.803718086, 0.946259679, 1.079784984, 1.204602687, 1.321076285, 1.429602576, 1.530595677, 1.624475262, 1.71165803, 1.792551616, 1.867550375, 1.93703258, 2.001358669, 2.060870301, 2.115889982, 2.16672113, 2.21364844, 2.256943216, 2.296844024, 2.333589434, 2.350847202, 35.81366835, 37.19361054, 39.20742929, 40.65233195, 41.76516959, 42.66116148, 43.40488731, 44.03609923, 44.58096912, 45.05761215, 45.4790756, 45.85505706, 46.19295427, 46.49853438, 46.77637684, 47.03017599, 47.2629533, 47.47720989, 47.67503833, 47.85820606, 48.02821867, 48.18636864, 48.3337732, 48.47140432, 48.60011223, 48.72064621, 48.83366629, 48.93976089, 49.03945383, 49.13321432, 49.22146409, 49.30458348, 49.38291658, 49.45677569, 49.526445, 49.59218385, 49.65422952, 49.68393611, 34.7115617, 36.03453876, 37.97671987, 39.3801263, 40.46773733, 41.34841008, 42.0833507, 42.71033603, 43.25428882, 43.73249646, 44.15742837, 44.53836794, 44.88240562, 45.19507651, 45.48078147, 45.74307527, 45.98486901, 46.20857558, 46.41621635, 46.60950084, 46.78988722, 46.95862881, 47.11681039, 47.26537682, 47.40515585, 47.53687649, 47.66118396, 47.77865186, 47.8897923, 47.99506422, 48.09488048, 48.18961365, 48.2796011, 48.36514917, 48.44653703, 48.52401894, 48.59782828, 48.63342328, 0.052172542, 0.047259148, 0.040947903, 0.037027722, 0.034364245, 0.032462175, 0.031064702, 0.03002267, 0.029242173, 0.028660454, 0.0282336, 0.027929764, 0.027725179, 0.027601686, 0.027545148, 0.027544382, 0.027590417, 0.02767598, 0.027795115, 0.0279429, 0.028115241, 0.028308707, 0.028520407, 0.028747896, 0.028989089, 0.029242207, 0.029505723, 0.029778323, 0.030058871, 0.030346384, 0.030640006, 0.030938992, 0.031242693, 0.031550537, 0.031862026, 0.03217672, 0.032494231, 0.032653934, 0.046905108, 0.042999604, 0.038067862, 0.035079612, 0.033096443, 0.03170963, 0.030709039, 0.029974303, 0.029430992, 0.029030379, 0.028739112, 0.028533537, 0.028396382, 0.028314722, 0.028278682, 0.028280585, 0.028314363, 0.028375159, 0.028459033, 0.028562759, 0.028683666, 0.028819525, 0.028968459, 0.029128879, 0.029299426, 0.029478937, 0.029666406, 0.02986096, 0.030061839, 0.030268375, 0.030479985, 0.03069615, 0.030916413, 0.031140368, 0.031367651, 0.031597939, 0.031830942, 0.031948378 };
            dataLMS = _dataLMS;
        }
        public int kqCDC_chuvi()
        {
            initCDC_chuvi();

            int stt = 0;
            //Gioi tinh
            if (gioitinh == "nữ")
                stt = 38;
            //Thang tuoi
            stt = stt + Convert.ToInt32(tuoi);

            double phanphoi5 = datachuvi[stt];
            double phanphoi50 = datachuvi[stt + 76];
            double phanphoi95 = datachuvi[stt + 76 * 2];

            if (chuvivongdau < phanphoi5)
            {
                return 1;
            }
            else if (chuvivongdau > phanphoi95)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        public string kqCDC_chuvi_diengiai()
        {
            if (kqCDC_chuvi() == 1)
            {
                return "Trẻ thuộc nhóm chu vi vòng đầu nhỏ (p < 5%)";
            }
            else if (kqCDC_chuvi() == 3)
            {
                return "Trẻ thuộc nhóm chu vi vòng đầu to (p > 95%)";
            }
            else
            {
                return "Trẻ thuộc nhóm chu vi vòng đầu bình thường (5% < p < 95%)";
            }
        }

        public double kqCDC_chuvi_zscore()
        {
            initCDC_chuvi();

            int stt = 0;
            //Gioi tinh
            if (gioitinh == "nữ")
                stt = 38;
            //Thang tuoi
            stt = stt + Convert.ToInt32(tuoi);

            double L = dataLMS[stt];
            double M = dataLMS[stt + 76];
            double S = dataLMS[stt + 76 * 2];

            double Z = z_score(chuvivongdau, L, M, S);

            return Z;
        }
    }
    public class Vbudich : Congthuc
    {
        public double cannang { get; set; }
        public Vbudich()
        {
            //init("C_B13");
        }
        public Vbudich(Nguoibenh nb)
        {
            cannang = nb.cannang;
            //init("C_B13");
        }
        public Vbudich(double _cannang)
        {
            cannang = _cannang;
            //init("C_B13");
        }
        public double kqVdich24h()
        {
            double result = (cannang < 11) ? (100 * cannang) : ((cannang <= 20) ? (1000 + 50 * (cannang - 10)) : (1500 + 20 * (cannang - 20)));
            return Math.Min(result, 2400);
        }
        public double kqtocdotruyen24h()
        {
            return kqVdich24h() / 24;
        }
        public double kqVdich_theogio()
        {
            double result = (cannang < 11) ? (4 * cannang) : ((cannang <= 20) ? (40 + 2 * (cannang - 10)) : (60 + (cannang - 20)));

            //Max 100ml/h
            return Math.Min(result, 100);
        }
        public string kqVdich24h_diengiai()
        {
            return "";
        }
    }
    public class PELD_Old : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public double tuoi { get; set; }
        public double BilirubinSerum { get; set; }
        public double INR { get; set; }
        public double AlbuminSerum { get; set; }
        public bool macbenhduoi1t { get; set; }

        public PELD_Old()
        {
            //init("C_B14");
        }
        public PELD_Old(Nguoibenh nb, Xetnghiem xn)
        {
            gioitinh = nb.gioitinh;
            tuoi = nb.tinhtuoi_nam();
            cannang = nb.cannang;
            chieucao = nb.chieucao;
            BilirubinSerum = xn.bilirubin;
            //init("C_B14");
        }
        public PELD_Old(string _gioitinh, double _chieucao, double _cannang, double _tuoi, double _BilirubinSerum, double _INR, double _AlbuminSerum, bool _macbenhduoi1t)
        {
            gioitinh = _gioitinh.ToLower();
            chieucao = _chieucao;
            cannang = _cannang;
            tuoi = _tuoi;
            BilirubinSerum = _BilirubinSerum;
            INR = _INR;
            AlbuminSerum = _AlbuminSerum;
            macbenhduoi1t = _macbenhduoi1t;
            //init("C_B14");
        }
        public double kqPELD_Old()
        {
            double hesotuoi_PELD = (macbenhduoi1t && tuoi < 2) ? 0.436 : 0;

            CDC_cannang phanbocannang = new CDC_cannang(gioitinh, tuoi, cannang);
            CDC_chieucao phanbochieucao = new CDC_chieucao(gioitinh, tuoi, chieucao);

            double hesotangtruong_PELD = (phanbocannang.kqCDC_cannang() == 1 || phanbochieucao.kqCDC_chieucao() == 1) ? 0.667 : 0;

            return 10 * (0.480 * Math.Log(BilirubinSerum) + 1.857 * Math.Log(INR) - (0.687 * Math.Log(AlbuminSerum))
                + hesotuoi_PELD + hesotangtruong_PELD);
        }
        public string kqPELD_Old_diengiai()
        {
            return "";
        }
    }
    public class PELD_New : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public double creatininSerum { get; set; }
        public double bilirubinSerum { get; set; }
        public double INR { get; set; }
        public double albuminSerum { get; set; }
        public DateTime ngayxetnghiem { get; set; }
        public DateTime ngaysinh { get; set; }
        public bool checklocmau { get; set; }


        public PELD_New()
        {
            //init("C_B23");            
        }
        public PELD_New(Nguoibenh nb, Xetnghiem xn)
        {
            ngaysinh = nb.ngaysinh;
            chieucao = nb.chieucao;
            cannang = nb.cannang;
            albuminSerum = xn.albumin;
            bilirubinSerum = xn.bilirubin;
            INR = xn.INR;
            creatininSerum = xn.creatininSerum;
            //init("C_B23");
        }
        public PELD_New(string _gioitinh, double _chieucao, double _cannang, double _Creatinin_HC,
            double _Bilirubin_HC, double _INR_HC, double _Albumin_HC, DateTime _NgayXetNghiem,
            DateTime _NgaySinh, bool _LocMau)
        {
            ngayxetnghiem = _NgayXetNghiem;
            ngaysinh = _NgaySinh;
            chieucao = _chieucao;
            cannang = _cannang;
            albuminSerum = _Albumin_HC;
            bilirubinSerum = _Bilirubin_HC;
            INR = _INR_HC;
            checklocmau = _LocMau;
            creatininSerum = _Creatinin_HC;
            //init("C_B23");
        }
        public double kqPELD_New()
        {
            //Tuoi hieu chinh, giới hạn Tuổi_HC: [1; 5,5]
            double tuoi_HC = (ngayxetnghiem - ngaysinh).TotalDays / 365;
            tuoi_HC = Math.Min(5.5, Math.Max(1, tuoi_HC));

            //Giới hạn Albumin_HC: [1; 1,9]
            double albumin_YT = Math.Max(1, Math.Min(1.9, albuminSerum));

            //Giới hạn Bilirubin_HC: [1; 40]
            double bilirubin_YT = (bilirubinSerum <= 4) ? 0.7854 * Math.Log(bilirubinSerum) + 0.3434 * Math.Log(4) :
                0.7854 * Math.Log(4) + 0.3434 * Math.Log(bilirubinSerum);
            bilirubin_YT = Math.Min(40, Math.Max(1, bilirubin_YT));

            //Giới hạn INR_HC: [1; 10]
            double INR_YT = (INR <= 2) ? 1.981 * Math.Log(INR) + 0.7298 * Math.Log(2) :
                1.981 * Math.Log(2) + 0.7298 * Math.Log(INR);
            INR_YT = Math.Min(10, Math.Max(1, INR_YT));

            //Giới hạn Creatinin_HC: [0,2; 1,3]
            double creatinin_YT = (checklocmau) ? 1.3 : Math.Max(0.2, Math.Min(1.3, creatininSerum));

            //Dùng method tính tuổi theo tháng
            Nguoibenh nb = new Nguoibenh();
            nb.ngaysinh = ngaysinh;
            //Giới hạn MZS_HC: [-5; -2,1]
            CDC_cannang phanbocannang = new CDC_cannang(gioitinh, nb.tinhtuoi_thang(), cannang);
            CDC_chieucao phanbochieucao = new CDC_chieucao(gioitinh, nb.tinhtuoi_thang(), chieucao);
            double MZS_HC = Math.Min(phanbocannang.kqCDC_cannang_zscore(), phanbochieucao.kqCDC_chieucao_zscore());
            MZS_HC = Math.Min(-2.1, Math.Max(-5, INR_YT));

            double PELD_New = 10 * ((-0.1967 * tuoi_HC) - (1.842 * Math.Log(albumin_YT)) + bilirubin_YT +
                INR_YT - (0.1807 * MZS_HC) + (1.453 * Math.Log(creatinin_YT)) + 1.5287) + 2.82;

            return Math.Max(6, Math.Round(PELD_New));
        }
        public string kqPELD_New_diengiai()
        {
            return "";
        }
    }
    public class WHO_suyDD : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public double tuoi { get; set; }
        public double[] dataLMS_H { get; set; }
        public double[] dataLMS_W { get; set; }
        public WHO_suyDD()
        {
            //init("C_B15");
        }
        public WHO_suyDD(string _gioitinh, double _chieucao, double _cannang, double _tuoi)
        {
            gioitinh = _gioitinh.ToLower();
            tuoi = _tuoi;
            chieucao = _chieucao;
            cannang = _cannang;
            //init("C_B15");
        }
        public WHO_suyDD(Nguoibenh nb)
        {
            gioitinh = nb.gioitinh;
            tuoi = nb.tinhtuoi_thang();
            chieucao = nb.chieucao;
            cannang = nb.cannang;
            //init("C_B15");
        }
        private void initWHO_suyDD()
        {
            double[] _dataLMS_H = { 49.8842, 54.7244, 58.4249, 61.4292, 63.886, 65.9026, 67.6236, 69.1645, 70.5994, 71.9687, 73.2812, 74.5388, 75.7488, 76.9186, 78.0497, 79.1458, 80.2113, 81.2487, 82.2587, 83.2418, 84.1996, 85.1348, 86.0477, 86.941, 87.8161, 87.972, 88.8065, 89.6197, 90.412, 91.1828, 91.9327, 92.6631, 93.3753, 94.0711, 94.7532, 95.4236, 96.0835, 96.7337, 97.3749, 98.0073, 98.631, 99.2459, 99.8515, 100.4485, 101.0374, 101.6186, 102.1933, 102.7625, 103.3273, 103.8886, 104.4473, 105.0041, 105.5596, 106.1138, 106.6668, 107.2188, 107.7697, 108.3198, 108.8689, 109.417, 109.9638, 0.03795, 0.03557, 0.03424, 0.03328, 0.03257, 0.03204, 0.03165, 0.03139, 0.03124, 0.03117, 0.03118, 0.03125, 0.03137, 0.03154, 0.03174, 0.03197, 0.03222, 0.0325, 0.03279, 0.0331, 0.03342, 0.03376, 0.0341, 0.03445, 0.03479, 0.03542, 0.03576, 0.0361, 0.03642, 0.03674, 0.03704, 0.03733, 0.03761, 0.03787, 0.03812, 0.03836, 0.03858, 0.03879, 0.039, 0.03919, 0.03937, 0.03954, 0.03971, 0.03986, 0.04002, 0.04016, 0.04031, 0.04045, 0.04059, 0.04073, 0.04086, 0.041, 0.04113, 0.04126, 0.04139, 0.04152, 0.04165, 0.04177, 0.0419, 0.04202, 0.04214, 1.8931, 1.9465, 2.0005, 2.0444, 2.0808, 2.1115, 2.1403, 2.1711, 2.2055, 2.2433, 2.2849, 2.3293, 2.3762, 2.426, 2.4773, 2.5303, 2.5844, 2.6406, 2.6973, 2.7553, 2.814, 2.8742, 2.9342, 2.9951, 3.0551, 3.116, 3.1757, 3.2353, 3.2928, 3.3501, 3.4052, 3.4591, 3.5118, 3.5625, 3.612, 3.6604, 3.7069, 3.7523, 3.7976, 3.8409, 3.8831, 3.9242, 3.9651, 4.0039, 4.0435, 4.081, 4.1194, 4.1567, 4.1941, 4.2314, 4.2677, 4.3052, 4.3417, 4.3783, 4.4149, 4.4517, 4.4886, 4.5245, 4.5616, 4.5977, 4.6339, 49.1477, 53.6872, 57.0673, 59.8029, 62.0899, 64.0301, 65.7311, 67.2873, 68.7498, 70.1435, 71.4818, 72.771, 74.015, 75.2176, 76.3817, 77.5099, 78.6055, 79.671, 80.7079, 81.7182, 82.7036, 83.6654, 84.604, 85.5202, 86.4153, 86.5904, 87.4462, 88.283, 89.1004, 89.8991, 90.6797, 91.443, 92.1906, 92.9239, 93.6444, 94.3533, 95.0515, 95.7399, 96.4187, 97.0885, 97.7493, 98.4015, 99.0448, 99.6795, 100.3058, 100.9238, 101.5337, 102.136, 102.7312, 103.3197, 103.9021, 104.4786, 105.0494, 105.6148, 106.1748, 106.7295, 107.2788, 107.8227, 108.3613, 108.8948, 109.4233, 0.0379, 0.0364, 0.03568, 0.0352, 0.03486, 0.03463, 0.03448, 0.03441, 0.0344, 0.03444, 0.03452, 0.03464, 0.03479, 0.03496, 0.03514, 0.03534, 0.03555, 0.03576, 0.03598, 0.0362, 0.03643, 0.03666, 0.03688, 0.03711, 0.03734, 0.03786, 0.03808, 0.0383, 0.03851, 0.03872, 0.03893, 0.03913, 0.03933, 0.03952, 0.03971, 0.03989, 0.04006, 0.04024, 0.04041, 0.04057, 0.04073, 0.04089, 0.04105, 0.0412, 0.04135, 0.0415, 0.04164, 0.04179, 0.04193, 0.04206, 0.0422, 0.04233, 0.04246, 0.04259, 0.04272, 0.04285, 0.04298, 0.0431, 0.04322, 0.04334, 0.04347, 1.8627, 1.9542, 2.0362, 2.1051, 2.1645, 2.2174, 2.2664, 2.3154, 2.365, 2.4157, 2.4676, 2.5208, 2.575, 2.6296, 2.6841, 2.7392, 2.7944, 2.849, 2.9039, 2.9582, 3.0129, 3.0672, 3.1202, 3.1737, 3.2267, 3.2783, 3.33, 3.3812, 3.4313, 3.4809, 3.5302, 3.5782, 3.6259, 3.6724, 3.7186, 3.7638, 3.8078, 3.8526, 3.8963, 3.9389, 3.9813, 4.0236, 4.0658, 4.1068, 4.1476, 4.1883, 4.2279, 4.2683, 4.3075, 4.3456, 4.3847, 4.4226, 4.4604, 4.4981, 4.5358, 4.5734, 4.6108, 4.6472, 4.6834, 4.7195, 4.7566 };
            dataLMS_H = _dataLMS_H;
            double[] _dataLMS_W = { 0.3487, 0.2297, 0.197, 0.1738, 0.1553, 0.1395, 0.1257, 0.1134, 0.1021, 0.0917, 0.082, 0.073, 0.0644, 0.0563, 0.0487, 0.0413, 0.0343, 0.0275, 0.0211, 0.0148, 0.0087, 0.0029, -0.0028, -0.0083, -0.0137, -0.0189, -0.024, -0.0289, -0.0337, -0.0385, -0.0431, -0.0476, -0.052, -0.0564, -0.0606, -0.0648, -0.0689, -0.0729, -0.0769, -0.0808, -0.0846, -0.0883, -0.092, -0.0957, -0.0993, -0.1028, -0.1063, -0.1097, -0.1131, -0.1165, -0.1198, -0.123, -0.1262, -0.1294, -0.1325, -0.1356, -0.1387, -0.1417, -0.1447, -0.1477, -0.1506, 3.3464, 4.4709, 5.5675, 6.3762, 7.0023, 7.5105, 7.934, 8.297, 8.6151, 8.9014, 9.1649, 9.4122, 9.6479, 9.8749, 10.0953, 10.3108, 10.5228, 10.7319, 10.9385, 11.143, 11.3462, 11.5486, 11.7504, 11.9514, 12.1515, 12.3502, 12.5466, 12.7401, 12.9303, 13.1169, 13.3, 13.4798, 13.6567, 13.8309, 14.0031, 14.1736, 14.3429, 14.5113, 14.6791, 14.8466, 15.014, 15.1813, 15.3486, 15.5158, 15.6828, 15.8497, 16.0163, 16.1827, 16.3489, 16.515, 16.6811, 16.8471, 17.0132, 17.1792, 17.3452, 17.5111, 17.6768, 17.8422, 18.0073, 18.1722, 18.3366, 0.14602, 0.13395, 0.12385, 0.11727, 0.11316, 0.1108, 0.10958, 0.10902, 0.10882, 0.10881, 0.10891, 0.10906, 0.10925, 0.10949, 0.10976, 0.11007, 0.11041, 0.11079, 0.11119, 0.11164, 0.11211, 0.11261, 0.11314, 0.11369, 0.11426, 0.11485, 0.11544, 0.11604, 0.11664, 0.11723, 0.11781, 0.11839, 0.11896, 0.11953, 0.12008, 0.12062, 0.12116, 0.12168, 0.1222, 0.12271, 0.12322, 0.12373, 0.12425, 0.12478, 0.12531, 0.12586, 0.12643, 0.127, 0.12759, 0.12819, 0.1288, 0.12943, 0.13005, 0.13069, 0.13133, 0.13197, 0.13261, 0.13325, 0.13389, 0.13453, 0.13517, 0.3809, 0.1714, 0.0962, 0.0402, -0.005, -0.043, -0.0756, -0.1039, -0.1288, -0.1507, -0.17, -0.1872, -0.2024, -0.2158, -0.2278, -0.2384, -0.2478, -0.2562, -0.2637, -0.2703, -0.2762, -0.2815, -0.2862, -0.2903, -0.2941, -0.2975, -0.3005, -0.3032, -0.3057, -0.308, -0.3101, -0.312, -0.3138, -0.3155, -0.3171, -0.3186, -0.3201, -0.3216, -0.323, -0.3243, -0.3257, -0.327, -0.3283, -0.3296, -0.3309, -0.3322, -0.3335, -0.3348, -0.3361, -0.3374, -0.3387, -0.34, -0.3414, -0.3427, -0.344, -0.3453, -0.3466, -0.3479, -0.3492, -0.3505, -0.3518, 3.2322, 4.1873, 5.1282, 5.8458, 6.4237, 6.8985, 7.297, 7.6422, 7.9487, 8.2254, 8.48, 8.7192, 8.9481, 9.1699, 9.387, 9.6008, 9.8124, 10.0226, 10.2315, 10.4393, 10.6464, 10.8534, 11.0608, 11.2688, 11.4775, 11.6864, 11.8947, 12.1015, 12.3059, 12.5073, 12.7055, 12.9006, 13.093, 13.2837, 13.4731, 13.6618, 13.8503, 14.0385, 14.2265, 14.414, 14.601, 14.7873, 14.9727, 15.1573, 15.341, 15.524, 15.7064, 15.8882, 16.0697, 16.2511, 16.4322, 16.6133, 16.7942, 16.9748, 17.1551, 17.3347, 17.5136, 17.6916, 17.8686, 18.0445, 18.2193, 0.14171, 0.13724, 0.13, 0.12619, 0.12402, 0.12274, 0.12204, 0.12178, 0.12181, 0.12199, 0.12223, 0.12247, 0.12268, 0.12283, 0.12294, 0.12299, 0.12303, 0.12306, 0.12309, 0.12315, 0.12323, 0.12335, 0.1235, 0.12369, 0.1239, 0.12414, 0.12441, 0.12472, 0.12506, 0.12545, 0.12587, 0.12633, 0.12683, 0.12737, 0.12794, 0.12855, 0.12919, 0.12988, 0.13059, 0.13135, 0.13213, 0.13293, 0.13376, 0.1346, 0.13545, 0.1363, 0.13716, 0.138, 0.13884, 0.13968, 0.14051, 0.14132, 0.14213, 0.14293, 0.14371, 0.14448, 0.14525, 0.146, 0.14675, 0.14748, 0.14821 };
            dataLMS_W = _dataLMS_W;
        }
        public string kqWHO_suyDD_diengiai()
        {
            string kq = "Trẻ thuộc nhóm ";
            double phanphoichieucao = kqWHO_chieucao_zscore();
            double phanphoicannang = kqWHO_cannang_zscore();

            if (phanphoichieucao > -2)
                kq = kq + "có chiều cao bình thường; ";
            else if (phanphoichieucao > -2)
                kq = kq + "thấp còi trung bình (p < 5%); ";
            else
                kq = kq + "thấp còi nghiêm trọng (p < 1%); ";

            if (phanphoicannang > -2)
                kq = kq + "có cân nặng bình thường; ";
            else if (phanphoicannang > -2)
                kq = kq + "gầy trung bình (p < 5%); ";
            else
                kq = kq + "gầy nghiêm trọng (p < 1%); ";
            return kq;
        }
        public double kqWHO_chieucao_zscore()
        {
            initWHO_suyDD();

            int stt = 0;
            //Thang tuoi
            stt = stt + Convert.ToInt32(tuoi);

            double L, M, S;
            if (gioitinh == "nữ")
                stt = stt + 183;

            L = dataLMS_H[stt];
            M = dataLMS_H[stt + 61];
            S = dataLMS_H[stt + 61 * 2];

            return z_score(chieucao, L, M, S);
        }
        public double kqWHO_cannang_zscore()
        {
            initWHO_suyDD();

            int stt = 0;
            //Thang tuoi
            stt = stt + Convert.ToInt32(tuoi);

            double L, M, S;
            if (gioitinh == "nữ")
                stt = stt + 183;

            L = dataLMS_W[stt];
            M = dataLMS_W[stt + 61];
            S = dataLMS_W[stt + 61 * 2];

            return z_score(cannang, L, M, S);
        }
    }
    public class ePER_PNCT : Congthuc
    {
        public double creatininUrine { get; set; }
        public double proteinUrine { get; set; }

        public ePER_PNCT()
        {
            //init("C_B16");
        }
        public ePER_PNCT(Xetnghiem xn)
        {
            creatininUrine = xn.creatininUrine;
            //init("C_B16");
        }
        public ePER_PNCT(double _CreatininUrine, double _ProteinUrine)
        {
            proteinUrine = _ProteinUrine;
            creatininUrine = _CreatininUrine;
            //init("C_B16");
        }
        public double kqePER_PNCT()
        {
            return (proteinUrine / creatininUrine) * 1373.5 - 60.508;
        }
        public string kqePER_PNCT_diengiai()
        {
            return "";
        }
    }
    public class OxyIndex : Congthuc
    {
        public double FIO2 { get; set; }
        public double PaO2 { get; set; }
        public double tg_hitvao { get; set; }
        public double nhiptho { get; set; }
        public double PIP { get; set; }
        public double PEEP { get; set; }

        public OxyIndex()
        {
            //init("C_B17");
        }
        public OxyIndex(Nguoibenh nb, Xetnghiem xn)
        {
            nhiptho = nb.nhiptho;
            FIO2 = xn.FiO2;
            PaO2 = xn.PaO2;
            //init("C_B17");
        }
        public OxyIndex(double _FIO2, double _PaO2, double _tg_hitvao, double _nhiptho, double _PIP, double _PEEP)
        {
            tg_hitvao = _tg_hitvao;
            nhiptho = _nhiptho;
            PIP = _PIP;
            PEEP = _PEEP;
            FIO2 = _FIO2;
            PaO2 = _PaO2;
            //init("C_B17");
        }

        public double kqOxyIndex()
        {
            double aplucduongthoTB = (tg_hitvao * nhiptho / 60) * (PIP - PEEP) + PEEP;
            return FIO2 * aplucduongthoTB / PaO2;
        }
        public string kqOxyIndex_diengiai()
        {
            double OI = kqOxyIndex();

            if (OI < 8)
            {
                return "Suy hô hấp nhẹ";
            }
            else if (8 <= OI && OI < 16)
            {
                return "Suy hô hấp trung bình";
            }
            else if (16 <= OI && OI < 40)
            {
                return "Suy hô hấp nặng";
            }
            else
            {
                return "Suy hô hấp rất nặng";
            }
        }
    }
    public class EED : Congthuc
    {
        public DateTime ngayKNcuoi { get; set; }
        public DateTime ngaysieuam { get; set; }
        public DateTime EED_calculated { get; set; }
        public int tuoithaisieuam { get; set; }
        public bool sieuam { get; set; }

        public EED()
        {
            //init("C_B18");
        }
        public EED(DateTime _ngayKNcuoi, DateTime _ngaysieuam, int _tuoithaisieuam, bool _sieuam)
        {
            sieuam = _sieuam;
            ngayKNcuoi = _ngayKNcuoi;
            ngaysieuam = _ngaysieuam;
            tuoithaisieuam = _tuoithaisieuam;
            //init("C_B18");
        }

        public DateTime EED_ngayKNcuoi()
        {
            DateTime kq = ngayKNcuoi.AddDays(280);
            return kq;
        }

        public DateTime EED_ngaysieuam()
        {
            DateTime kq = ngaysieuam.AddDays(280 - tuoithaisieuam);
            return kq;
        }
        public DateTime kqEED()
        {
            EED_calculated = sieuam ? EED_ngaysieuam() : EED_ngayKNcuoi();
            return EED_calculated;
        }
        public int kqTuoithai()
        {
            TimeSpan tuoithai = DateTime.Now - EED_calculated;
            int kq = 280 - Math.Abs(tuoithai.Days);
            return kq;
        }
        public string kqEED_diengiai()
        {
            return "Ngày dự sinh: " + EED_calculated.Day + "//" + EED_calculated.Month + "//" + EED_calculated.Year;
        }
    }
    public class EER : Congthuc
    {
        public string gioitinh { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public double tuoi { get; set; }
        public string hesohoatdong { get; set; }
        public double ECG { get; set; }

        public EER()
        {
            //init("C_B19");
        }
        public EER(Nguoibenh nb)
        {
            tuoi = nb.tinhtuoi_nam();
            gioitinh = nb.gioitinh;
            chieucao = nb.chieucao;
            cannang = nb.cannang;
            //init("C_B19");
        }
        public EER(string _gioitinh, double _chieucao, double _cannang, double _tuoi, string _hesohoatdong)
        {
            tuoi = _tuoi;
            gioitinh = _gioitinh.ToLower();
            chieucao = _chieucao;
            cannang = _cannang;
            hesohoatdong = _hesohoatdong.ToLower();
            //init("C_B19");
        }

        public double kqEER()
        {
            double ECG = (tuoi < 0.25) ? 200 : (tuoi < 0.5) ? 50 : (tuoi < 4) ? 20 : (tuoi < 9) ? 15 : (tuoi < 14) ? 25 : 20;
            double EER;

            if (gioitinh == "nam")
            {
                if (tuoi < 3)
                {
                    EER = -716.45 - tuoi + (17.82 * chieucao) + (15.06 * cannang) + ECG;
                }
                else
                {
                    if (hesohoatdong == "không vận động")
                    {
                        EER = -447.51 + (3.68 * tuoi) + (13.01 * chieucao) + (13.15 * cannang) + ECG;
                    }
                    else if (hesohoatdong == "ít vận động")
                    {
                        EER = 19.12 + (3.68 * tuoi) + (8.62 * chieucao) + (20.28 * cannang) + ECG;
                    }
                    else if (hesohoatdong == "vận động trung bình")
                    {
                        EER = -388.19 + (3.68 * tuoi) + (12.66 * chieucao) + (20.46 * cannang) + ECG;
                    }
                    else
                    {
                        EER = -671.75 + (3.68 * tuoi) + (15.38 * chieucao) + (23.25 * cannang) + ECG;
                    }
                }
            }
            else
            {
                if (tuoi < 3)
                {
                    EER = -69.15 + (80.0 * tuoi) + (2.65 * chieucao) + (54.15 * cannang) + ECG;
                }
                else
                {
                    if (hesohoatdong == "không vận động")
                    {
                        EER = 55.59 - (22.25 * tuoi) + (8.43 * chieucao) + (17.07 * cannang) + ECG;
                    }
                    else if (hesohoatdong == "ít vận động")
                    {
                        EER = -297.54 - (22.25 * tuoi) + (12.77 * chieucao) + (14.73 * cannang) + ECG;
                    }
                    else if (hesohoatdong == "vận động trung bình")
                    {
                        EER = -189.55 - (22.25 * tuoi) + (11.74 * chieucao) + (18.34 * cannang) + ECG;
                    }
                    else
                    {
                        EER = -709.59 - (22.25 * tuoi) + (18.22 * chieucao) + (14.25 * cannang) + ECG;
                    }
                }
            }
            return EER;
        }
        public string kqEER_diengiai()
        {
            return "";
        }
    }
    public class CDC_BMI : Congthuc
    {
        public string gioitinh { get; set; }
        public double thangtuoi { get; set; }
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public double BMI { get; set; }
        public double[] data_Nam { get; set; }
        public double[] data_Nu { get; set; }
        public CDC_BMI()
        {
            //init("C_B20");
        }
        public CDC_BMI(string _gioitinh, double _thangtuoi, double _BMI)
        {
            gioitinh = _gioitinh.ToLower();
            thangtuoi = _thangtuoi;
            BMI = _BMI;
            //init("C_B20");
        }
        public CDC_BMI(string _gioitinh, double _chieucao, double _cannang, double _thangtuoi)
        {
            gioitinh = _gioitinh.ToLower();
            thangtuoi = _thangtuoi;
            BMI = _chieucao * _chieucao / _cannang;
            //init("C_B20");
        }
        public CDC_BMI(Nguoibenh nb)
        {
            gioitinh = nb.gioitinh;
            thangtuoi = nb.tinhtuoi_thang();
            BMI = nb.chieucao * nb.chieucao / nb.cannang;
            //init("C_B20");
        }
        private void initCDC_BMI()
        {
            //P5, P85, P95
            double[] _data_Nam = { 14.737319472, 14.719292573, 14.683608414, 14.648433294, 14.613786255, 14.579685777, 14.546149661, 14.513194923, 14.480837948, 14.449093293, 14.417975614, 14.387498109, 14.357672932, 14.328511189, 14.300022559, 14.27221761, 14.245102016, 14.218684325, 14.192970112, 14.167964724, 14.143670894, 14.120092688, 14.097232459, 14.075089663, 14.053664432, 14.032955327, 14.012959844, 13.993674447, 13.975094635, 13.957215715, 13.940029469, 13.923531161, 13.907712809, 13.892566777, 13.878085253, 13.864259589, 13.851083083, 13.838548037, 13.82664746, 13.815374889, 13.804723699, 13.794690036, 13.785268816, 13.776456272, 13.76824936, 13.760645736, 13.75364373, 13.747242303, 13.741440993, 13.736239868, 13.731639461, 13.727640719, 13.724244334, 13.721452886, 13.719267752, 13.717690937, 13.716724582, 13.716371244, 13.716632334, 13.717510776, 13.719008868, 13.72112886, 13.723872927, 13.727243152, 13.731241504, 13.73586983, 13.741129903, 13.747023092, 13.753550999, 13.760714804, 13.76851558, 13.776954225, 13.786031452, 13.795747785, 13.806103553, 13.817098777, 13.828733753, 13.841007841, 13.853920676, 13.867471738, 13.881660126, 13.896484539, 13.911944391, 13.928037855, 13.944763576, 13.962118903, 13.98010297, 13.998713275, 14.017947378, 14.037802816, 14.058276901, 14.079366766, 14.101069371, 14.123381501, 14.146299772, 14.169820633, 14.193940365, 14.218655083, 14.243960805, 14.269853131, 14.296327886, 14.323380481, 14.351006234, 14.379200308, 14.407957708, 14.43727329, 14.467141754, 14.497557648, 14.528515371, 14.560009167, 14.592033134, 14.62458122, 14.657647224, 14.691223806, 14.725306007, 14.759886606, 14.794958832, 14.830515777, 14.86655039, 14.903055485, 14.940023736, 14.977447683, 15.01531973, 15.053632148, 15.092377074, 15.131546514, 15.171132342, 15.211126303, 15.251520013, 15.292304961, 15.333472509, 15.375013894, 15.416920229, 15.459182504, 15.501791587, 15.544738227, 15.588013052, 15.631606575, 15.67550919, 15.719711177, 15.764202704, 15.808973823, 15.854014479, 15.899314507, 15.944863634, 15.990651482, 16.036667567, 16.082901303, 16.129342004, 16.175978884, 16.222801059, 16.26979755, 16.316957284, 16.364269095, 16.411721727, 16.459303836, 16.507003989, 16.55481067, 16.60271228, 16.650697137, 16.698753478, 16.746869466, 16.795033183, 16.843232639, 16.891455767, 16.939690432, 16.987924425, 17.036145468, 17.084341215, 17.132499251, 17.180607093, 17.228652193, 17.276622589, 17.324504231, 17.372285093, 17.41995236, 17.46749315, 17.514894512, 17.562143423, 17.609226793, 17.656131455, 17.702844168, 17.749351612, 17.795640388, 17.841697008, 17.8875079, 17.933059395, 17.978337728, 18.023329032, 18.068019329, 18.112394528, 18.156440415, 18.200142556, 18.243486617, 18.286457268, 18.329041829, 18.371223685, 18.412986957, 18.454317938, 18.495200616, 18.535619323, 18.575558186, 18.615001112, 18.653931783, 18.692334366, 18.730190245, 18.767483802, 18.804197322, 18.840313184, 18.875813476, 18.910679987, 18.944894195, 18.978437264, 19.011288695, 19.043431335, 19.074844245, 19.105507218, 19.120551107, 19.13539969, 18.162194733, 18.119549228, 18.036680126, 17.957002275, 17.880471005, 17.807042591, 17.736674142, 17.669323458, 17.604948602, 17.543508981, 17.484962947, 17.429269329, 17.376386767, 17.326273561, 17.278887949, 17.234185537, 17.192125454, 17.152662007, 17.115750983, 17.081346773, 17.049405037, 17.019878597, 16.992720871, 16.967887393, 16.945332151, 16.925010281, 16.90687779, 16.890891793, 16.877010731, 16.865193742, 16.85540483, 16.847604904, 16.841759872, 16.837836599, 16.835803652, 16.835632225, 16.837292715, 16.840758382, 16.84600319, 16.853001975, 16.861731244, 16.872165194, 16.884280854, 16.898054589, 16.913462745, 16.930481531, 16.949086925, 16.969254612, 16.990959933, 17.01417786, 17.038882985, 17.065049527, 17.092652232, 17.121663187, 17.152056291, 17.18380454, 17.216880721, 17.251256966, 17.286907078, 17.323802541, 17.361915701, 17.40121891, 17.441684564, 17.483285128, 17.525993169, 17.56978138, 17.614622659, 17.660489848, 17.707356316, 17.755195413, 17.803980772, 17.853686264, 17.90428602, 17.955754442, 18.008066222, 18.061196533, 18.1151201, 18.169813026, 18.225251187, 18.281410552, 18.338267739, 18.395799994, 18.453983546, 18.512796471, 18.572216211, 18.632222229, 18.692791276, 18.753902661, 18.815535648, 18.877669577, 18.940284187, 19.003359543, 19.066876031, 19.130814356, 19.195155544, 19.259880939, 19.324972199, 19.390411298, 19.456180423, 19.522262472, 19.588640056, 19.655296491, 19.722215306, 19.789380335, 19.85677572, 19.924385908, 19.992195654, 20.060190019, 20.128354368, 20.196674371, 20.265136002, 20.333725539, 20.402429562, 20.471236496, 20.540131144, 20.609101892, 20.678136503, 20.747223033, 20.816349832, 20.885505539, 20.954679086, 21.023859692, 21.093036865, 21.1622004, 21.231340378, 21.300447165, 21.369511412, 21.438524051, 21.507476296, 21.576359641, 21.645165861, 21.713887007, 21.782515408, 21.851043669, 21.919464668, 21.987771559, 22.055957764, 22.12401698, 22.191943172, 22.259730572, 22.327373681, 22.394867265, 22.462206356, 22.529386247, 22.596402495, 22.663250919, 22.729927595, 22.79642886, 22.862751308, 22.928891788, 22.994847407, 23.060615524, 23.126193753, 23.191579959, 23.256772259, 23.321769021, 23.386568863, 23.451170652, 23.515573503, 23.579776782, 23.6437801, 23.707583317, 23.771186541, 23.834590127, 23.897794676, 23.960801041, 24.023610321, 24.086223863, 24.148643267, 24.21087038, 24.272907303, 24.334756391, 24.396419305, 24.457900896, 24.519203235, 24.580329706, 24.641283953, 24.702069886, 24.76269168, 24.823153782, 24.883460908, 24.943618051, 25.003630482, 25.063503754, 25.123243703, 25.182856454, 25.242348423, 25.301726323, 25.360997164, 25.42016826, 25.479247229, 25.538242002, 25.597160953, 25.656012444, 25.714806292, 25.773548883, 25.832252068, 25.890926872, 25.949581158, 26.008226305, 26.066873207, 26.125533104, 26.184217589, 26.242938608, 26.301707228, 26.360539323, 26.419445233, 26.478439122, 26.537534783, 26.596746393, 26.656088513, 26.715576094, 26.775224484, 26.835051321, 26.895069442, 26.955296972, 27.015750924, 27.046068183, 27.076448741, 19.338010618, 19.278898128, 19.164659645, 19.055674233, 18.951867497, 18.853165291, 18.759493593, 18.670778406, 18.586945886, 18.507921309, 18.433630719, 18.363999601, 18.298953229, 18.238416662, 18.182314401, 18.130572441, 18.083113112, 18.039861981, 18.00074262, 17.965679031, 17.934593498, 17.907410074, 17.884051832, 17.864439827, 17.848496221, 17.836142298, 17.827298905, 17.821886501, 17.819825229, 17.821035591, 17.825435776, 17.832947441, 17.843490284, 17.85698505, 17.873353184, 17.892516422, 17.914398944, 17.938925239, 17.966021441, 17.995615231, 18.027635434, 18.062013961, 18.098683166, 18.137577538, 18.178633273, 18.221788238, 18.266981906, 18.314155288, 18.363250846, 18.414212415, 18.46698511, 18.52151524, 18.577749955, 18.635638156, 18.695129036, 18.756172877, 18.818720783, 18.882724739, 18.948137004, 19.014911065, 19.08300081, 19.152360731, 19.22294589, 19.294711907, 19.367614933, 19.441611647, 19.516658939, 19.592715391, 19.669738304, 19.747686659, 19.826519632, 19.906196889, 19.986678588, 20.067925378, 20.149898403, 20.232559281, 20.315870259, 20.399793794, 20.484293234, 20.569332163, 20.654874798, 20.740885842, 20.827330585, 20.914174799, 21.001384829, 21.08892746, 21.176770225, 21.264881065, 21.353228504, 21.441781645, 21.530510151, 21.619384256, 21.708374769, 21.797453077, 21.886591146, 21.975761529, 22.064937365, 22.154092383, 22.243200913, 22.332237858, 22.42117875, 22.509999702, 22.598677436, 22.687189277, 22.775513158, 22.86362762, 22.951511816, 23.039145509, 23.126509077, 23.213583513, 23.300350425, 23.38679204, 23.472891203, 23.558631287, 23.643996517, 23.728971547, 23.813541709, 23.897692958, 23.981411874, 24.064685664, 24.147502159, 24.22984982, 24.311717735, 24.39309562, 24.473973823, 24.554343318, 24.634195716, 24.713523254, 24.792318805, 24.870575873, 24.948288599, 25.025451754, 25.10206075, 25.17811163, 25.253601077, 25.328526412, 25.402885595, 25.476677223, 25.549900538, 25.62255542, 25.694642395, 25.766162632, 25.837117943, 25.90751079, 25.977344281, 26.046622172, 26.115348872, 26.183529439, 26.251169587, 26.318275685, 26.384854755, 26.450914482, 26.516463208, 26.581509939, 26.646064342, 26.71013675, 26.773738166, 26.83688026, 26.899575372, 26.961836518, 27.023677386, 27.085112342, 27.146156433, 27.206825382, 27.267135598, 27.327104173, 27.386748884, 27.446088197, 27.505141264, 27.563927929, 27.622468728, 27.680784887, 27.738898541, 27.796831854, 27.854608369, 27.912252088, 27.969787706, 28.027240611, 28.084636885, 28.1420033, 28.199367319, 28.256757094, 28.314201462, 28.371729945, 28.429372744, 28.487160738, 28.545125479, 28.60329919, 28.661714756, 28.720405723, 28.779406289, 28.838751302, 28.898476211, 28.958617197, 29.019210796, 29.080295026, 29.141907344, 29.204085991, 29.266870788, 29.330301268, 29.394417774, 29.459261215, 29.524873058, 29.591295323, 29.658570912, 29.726742037, 29.79585303, 29.865947819, 29.93707103, 30.009267783, 30.082583688, 30.157064835, 30.232757788, 30.309709009, 30.387967, 30.467579243, 30.548594139, 30.589642847, 30.631060541 };
            data_Nam = _data_Nam;
            double[] _data_Nu = { 14.397870893, 14.380186596, 14.345272624, 14.310968062, 14.277276856, 14.244203026, 14.211749089, 14.179917754, 14.148711114, 14.118130621, 14.088177926, 14.058853902, 14.030159316, 14.002094761, 13.974660728, 13.947857672, 13.921686084, 13.89614655, 13.871239805, 13.846966782, 13.823328646, 13.800326818, 13.777962994, 13.756238848, 13.735157169, 13.714720207, 13.694930704, 13.675791616, 13.657306081, 13.639477387, 13.622308935, 13.605804206, 13.589966725, 13.574800023, 13.560307612, 13.546492949, 13.53335941, 13.520910267, 13.509148527, 13.498077398, 13.487699767, 13.47801782, 13.469034401, 13.460751754, 13.453171909, 13.44629667, 13.440127612, 13.434666084, 13.429913207, 13.425870418, 13.422537877, 13.419916293, 13.41800595, 13.416806916, 13.416319045, 13.416541975, 13.417475136, 13.419117751, 13.421468839, 13.424527221, 13.428291519, 13.432760162, 13.43793139, 13.443803253, 13.450373621, 13.45764018, 13.46560044, 13.47425174, 13.483591251, 13.493615981, 13.504322782, 13.515708349, 13.527769222, 13.540501762, 13.553901719, 13.567965559, 13.582689175, 13.598067882, 13.614097684, 13.630773884, 13.648091204, 13.666045501, 13.684631476, 13.703844013, 13.723677453, 13.744126561, 13.765186202, 13.78684956, 13.809111546, 13.83196589, 13.855406666, 13.879426466, 13.904020526, 13.92918106, 13.954901632, 13.981175358, 14.007995218, 14.035354051, 14.063244558, 14.091659301, 14.120590705, 14.150031057, 14.179972506, 14.210407062, 14.2413266, 14.272722855, 14.304587427, 14.336911777, 14.369687229, 14.402904971, 14.436556052, 14.470631805, 14.505122367, 14.540018622, 14.575311069, 14.610990067, 14.647045838, 14.683468464, 14.720247888, 14.757373914, 14.794836204, 14.832624281, 14.870727527, 14.909135184, 14.947836351, 14.986819986, 15.026074904, 15.065589778, 15.105353137, 15.145353366, 15.185578709, 15.22601726, 15.266656971, 15.307485648, 15.34849095, 15.389660389, 15.430981329, 15.472440986, 15.514026427, 15.555724571, 15.597522183, 15.639405882, 15.681362131, 15.723377243, 15.765437378, 15.807528542, 15.849636587, 15.891747208, 15.933845947, 15.975918187, 16.017949155, 16.059923921, 16.101827393, 16.143644323, 16.185359301, 16.226956758, 16.268420961, 16.309736017, 16.350885869, 16.391854299, 16.432624923, 16.473181193, 16.513506398, 16.55358366, 16.593395935, 16.632926016, 16.672156529, 16.711069932, 16.749648518, 16.787874416, 16.825729586, 16.863195825, 16.900254764, 16.936887868, 16.97307644, 17.008801618, 17.044044915, 17.078785534, 17.113005736, 17.146685474, 17.179805079, 17.212344716, 17.244285168, 17.275604072, 17.306282966, 17.336301585, 17.365638249, 17.394272864, 17.422184393, 17.449351838, 17.475755094, 17.501369744, 17.526176669, 17.550154683, 17.573280181, 17.595534679, 17.616892752, 17.637333562, 17.656834899, 17.675374442, 17.692929755, 17.709478294, 17.724997408, 17.739464345, 17.752856251, 17.765150176, 17.776323076, 17.786351816, 17.795213175, 17.802883845, 17.809340439, 17.814557553, 17.818516749, 17.821191341, 17.822557366, 17.822590601, 17.821266527, 17.820090457, 17.818563561, 18.018205792, 17.973714126, 17.887488124, 17.804890513, 17.725863959, 17.650351371, 17.578297744, 17.509648394, 17.44434994, 17.38235043, 17.323598458, 17.268044363, 17.215639512, 17.166336426, 17.120088721, 17.076851036, 17.036578944, 16.99922885, 16.964757881, 16.933123775, 16.904284765, 16.878199474, 16.85482681, 16.834126282, 16.816056377, 16.800576707, 16.787646543, 16.777225115, 16.769271588, 16.763745045, 16.760604487, 16.759808835, 16.761316945, 16.765087627, 16.771079664, 16.779251844, 16.789562983, 16.801971953, 16.816437922, 16.832919646, 16.85137625, 16.871767854, 16.894053448, 16.918192851, 16.94414613, 16.971873618, 17.001335934, 17.032493991, 17.065309004, 17.099741723, 17.135754682, 17.173309679, 17.212369113, 17.252895716, 17.294852556, 17.338203042, 17.382910923, 17.428940294, 17.476255597, 17.524821619, 17.574603494, 17.625566704, 17.67767708, 17.730900798, 17.785204382, 17.840554704, 17.896918975, 17.95426475, 18.01255992, 18.071772705, 18.131871655, 18.192825645, 18.254603884, 18.317175943, 18.380512412, 18.444582911, 18.509357979, 18.574809309, 18.640907397, 18.70762408, 18.774932139, 18.842802758, 18.911208974, 18.980123661, 19.049520628, 19.119373164, 19.189654528, 19.260340621, 19.331404869, 19.402822598, 19.474568694, 19.546620507, 19.618951499, 19.691539649, 19.764361199, 19.837393135, 19.91061278, 19.983997788, 20.05752615, 20.131176192, 20.204926574, 20.278756292, 20.352644676, 20.426571391, 20.500516439, 20.574460154, 20.648383208, 20.722266606, 20.79609169, 20.869840136, 20.943493956, 21.017034888, 21.090446529, 21.163711548, 21.2368133, 21.309735477, 21.38246211, 21.454977564, 21.527266545, 21.599314096, 21.671105598, 21.742626772, 21.813863676, 21.884802708, 21.955430607, 22.02573445, 22.095701657, 22.165319987, 22.23457754, 22.303462758, 22.371964427, 22.440071674, 22.507773969, 22.575061126, 22.641923303, 22.708351005, 22.774335079, 22.839866721, 22.904937473, 22.969539223, 23.033664209, 23.097305018, 23.160454584, 23.223106195, 23.285253487, 23.346890452, 23.408011431, 23.468611122, 23.528684575, 23.588227198, 23.647234755, 23.705703367, 23.763629515, 23.821010038, 23.877842138, 23.934123376, 23.989851678, 24.045025332, 24.099642992, 24.153703678, 24.207206776, 24.260152041, 24.312539595, 24.364369931, 24.415643913, 24.466362775, 24.516528123, 24.566141937, 24.61520657, 24.663724748, 24.711699572, 24.759134514, 24.806033424, 24.852400525, 24.898240411, 24.943558054, 24.988357847, 25.032648356, 25.076432823, 25.11971866, 25.162512708, 25.204822177, 25.246653477, 25.28801794, 25.328921084, 25.369371303, 25.409379292, 25.448953455, 25.488103769, 25.526840293, 25.56517184, 25.603114, 25.640674345, 25.67786376, 25.714697004, 25.751181066, 25.787333798, 25.823166106, 25.858691151, 25.893922413, 25.928873687, 25.963559081, 25.997993009, 26.032190187, 26.06616563, 26.099934651, 26.133512849, 26.166916112, 26.20016061, 26.23326279, 26.266239373, 26.299110386, 26.331885089, 26.364585813, 26.397230769, 26.429838734, 26.462429098, 26.478719662, 26.495016788, 19.10623522, 19.058238445, 18.965949897, 18.878533884, 18.795909992, 18.717998385, 18.644718454, 18.575990295, 18.511734023, 18.451869784, 18.396318479, 18.345000987, 18.29783876, 18.254753806, 18.215668786, 18.180507113, 18.149193044, 18.121651762, 18.097809453, 18.077593361, 18.06093184, 18.047754376, 18.037991607, 18.03157513, 18.028438215, 18.028514727, 18.031739765, 18.038049459, 18.047380913, 18.059672159, 18.074862097, 18.092890445, 18.11369768, 18.137224989, 18.163414224, 18.192207855, 18.223548932, 18.257381051, 18.293648259, 18.332295253, 18.373267126, 18.41650918, 18.461967542, 18.509588576, 18.559319065, 18.611106202, 18.664897596, 18.72064127, 18.778285671, 18.837779538, 18.899072288, 18.962113544, 19.026853412, 19.093242432, 19.161231581, 19.230772276, 19.301816386, 19.374316236, 19.448224611, 19.523494768, 19.600080436, 19.677935827, 19.757015637, 19.837275057, 19.918669774, 20.001155979, 20.08469037, 20.169230159, 20.254733074, 20.341157368, 20.428461818, 20.516605734, 20.605548956, 20.695251857, 20.785675251, 20.876780679, 20.968530139, 21.060886065, 21.153811648, 21.247270492, 21.341226663, 21.435645054, 21.530490897, 21.625730052, 21.721328849, 21.817254297, 21.913474016, 22.00995586, 22.106668641, 22.203581509, 22.300664287, 22.397887094, 22.495221076, 22.592637546, 22.690108545, 22.787606652, 22.88510501, 22.982577328, 23.079997876, 23.177341493, 23.274583578, 23.371700099, 23.468667584, 23.56546313, 23.662064395, 23.758449602, 23.854597539, 23.950487558, 24.046099573, 24.141414065, 24.236412075, 24.331075256, 24.4253857, 24.519326162, 24.612879933, 24.706030862, 24.798763363, 24.891062407, 24.982913526, 25.074302811, 25.165216914, 25.255643043, 25.345568966, 25.434983007, 25.523874049, 25.612231528, 25.700045441, 25.787306336, 25.874005319, 25.960134049, 26.045684738, 26.130650155, 26.215023617, 26.298798997, 26.381970718, 26.464533755, 26.546483632, 26.627816426, 26.70852876, 26.788617809, 26.868081296, 26.94691749, 27.025125211, 27.102703824, 27.17965324, 27.255973919, 27.331666865, 27.406733627, 27.481176301, 27.554997527, 27.62820049, 27.700788918, 27.772767087, 27.844139812, 27.914912457, 27.985090927, 28.054681672, 28.123691687, 28.192128511, 28.260000228, 28.327315465, 28.394083399, 28.460313748, 28.52601678, 28.591203309, 28.655884697, 28.720072857, 28.783780248, 28.847019885, 28.909805333, 28.972150709, 29.034070689, 29.095580504, 29.156695943, 29.217433356, 29.277809657, 29.33784232, 29.397549393, 29.456949484, 29.51606178, 29.574906037, 29.633502591, 29.691872359, 29.750036824, 29.808018082, 29.865838796, 29.923522234, 29.981092247, 30.038573291, 30.095990421, 30.153369256, 30.210736178, 30.268117989, 30.325542155, 30.38303689, 30.440630728, 30.498353237, 30.556234331, 30.614304629, 30.672595385, 30.731138492, 30.789966483, 30.849112534, 30.908610469, 30.968494761, 31.028800535, 31.089563569, 31.150820299, 31.212607819, 31.274963885, 31.337926913, 31.401536312, 31.465830973, 31.530851807, 31.596639946, 31.663237236, 31.730686253, 31.764743113, 31.799029637 };
            data_Nu = _data_Nu;
        }
        public double[] phanphoiCDC_BMI()
        {
            initCDC_BMI();
            double phanphoi5, phanphoi85, phanphoi95;
            //Thang tuoi
            int stt = Convert.ToInt32(thangtuoi);
            if (gioitinh == "nữ")
            {
                phanphoi5 = data_Nu[stt];
                phanphoi85 = data_Nu[stt + 219];
                phanphoi95 = data_Nu[stt + 219 * 2];
            }
            else
            {
                phanphoi5 = data_Nam[stt];
                phanphoi85 = data_Nam[stt + 219];
                phanphoi95 = data_Nam[stt + 219 * 2];
            }
            return new double[] { phanphoi5, phanphoi85, phanphoi95 };
        }
        public string kqCDC_BMI_diengiai()
        {
            //P5, P85, P95
            double[] kqphanphoi = phanphoiCDC_BMI();

            if (0 <= BMI && BMI < kqphanphoi[0])
            {
                return "Thiếu cân";
            }
            else if (kqphanphoi[0] <= BMI && BMI < kqphanphoi[1])
            {
                return "Bình thường";
            }
            else if (kqphanphoi[1] <= BMI && BMI < kqphanphoi[2])
            {
                return "Thừa cân";
            }
            else if (BMI >= 40 || BMI > kqphanphoi[2] * 1.4)
            {
                return "Béo phì độ 3 (nghiêm trọng)";
            }
            else if (BMI >= 35 || BMI > kqphanphoi[2] * 1.2)
            {
                return "Béo phì độ 2 (nghiêm trọng)";
            }
            else
            {
                return "Béo phì độ 1 (nhẹ)";
            }
        }
    }
    public class Noikhiquan : Congthuc
    {
        public bool bongchen { get; set; }
        public double tuoi { get; set; }
        public double ongnoikhiquan { get; set; }

        public Noikhiquan()
        {
            //init("C_B21");
        }

        public Noikhiquan(double _tuoi, bool _bongchen)
        {
            bongchen = _bongchen;
            tuoi = _tuoi;
            //init("C_B21");
        }

        public double kqNoikhiquan()
        {
            ongnoikhiquan = (bongchen) ? (3.5 + (tuoi / 4)) : (4 + (tuoi / 4));
            return ongnoikhiquan;
        }
        public string kqNoikhiquan_diengiai()
        {
            return "";
        }
    }
    public class NatriSerum_Adj : Congthuc //C_C01
    {
        public double NatriSerum { get; set; }
        public double GlucoseSerum { get; set; }

        public NatriSerum_Adj()
        {
            //init("C_C01");
        }
        public NatriSerum_Adj(Xetnghiem XN)
        {
            NatriSerum = XN.natriSerum;
            GlucoseSerum = 0;
            //init("C_C01");
        }

        public NatriSerum_Adj(double _NatriSerum, double _GlucoseSerum)
        {
            NatriSerum = _NatriSerum;
            GlucoseSerum = _GlucoseSerum;
            //init("C_C01");
        }

        public double kqNatriSerum_Adj()
        {
            double kq = NatriSerum + (2 * (GlucoseSerum - 100) / 100);
            return kq;
        }

        public string kqNatriSerum_Adj_diengiai()
        {
            double ketqua = kqNatriSerum_Adj();
            if (ketqua > 135 && ketqua < 145)
                return "Natri huyết nằm trong khoảng bình thường (135-145 mEq/L)";
            else if (ketqua < 135)
                return "Hạ hatri huyết (<135 mEq/L)";
            else
                return "Tăng hatri huyết (>145 mEq/L)";
        }
    }
    public class CardiacOutput : Congthuc
    {
        public double Hb { get; set; }
        public double PaO2 { get; set; }
        public double PvO2 { get; set; }
        public double oxytieuthu { get; set; }
        public double O2vSat { get; set; }
        public double O2Sat { get; set; }

        public CardiacOutput()
        {
            //init("C_C02");
        }
        public CardiacOutput(Xetnghiem XN)
        {
            oxytieuthu = 0;
            Hb = XN.Hb;
            O2Sat = 0;
            PaO2 = 0;
            O2vSat = 0;
            PvO2 = 0;
            //init("C_C02");
        }

        public CardiacOutput(double _Hb, double _PaO2, double _PvO2, double _oxytieuthu, double _O2Sat, double _O2vSat)
        {
            oxytieuthu = _oxytieuthu;
            Hb = _Hb;
            O2Sat = _O2Sat;
            PaO2 = _PaO2;
            O2vSat = _O2vSat;
            PvO2 = _PvO2;
            //init("C_C02");
        }
        public double kqCardiacOutput()
        {
            double kq = oxytieuthu / (((Hb * 13.4 * O2Sat / 100) + (PaO2 * 0.031)) - (Hb * 13.4 * O2vSat / 100) + (PvO2 * 0.031));
            return kq;
        }
        public string kqCardiacOutput_diengiai()
        {
            double ketqua = kqCardiacOutput();
            if (ketqua > 5 && ketqua < 6)
                return "Cung lượng tim mức bình thường: 5-6 L/phút (khi nghỉ ngơi)";
            else if (ketqua < 5)
                return "Cung lượng tim mức thấp: < 5 L/phút";
            else
                return "Cung lượng tim mức cao: > 6 L/phút (hoặc có vận động)";
        }
    }
    public class FEPO4 : Congthuc
    {
        public double creatininSerum { get; set; }
        public double creatininUrine { get; set; }
        public double phosphatUrine { get; set; }
        public double phosphatSerum { get; set; }

        public FEPO4()
        {
            //init("C_C03");
        }
        public FEPO4(Xetnghiem XN)
        {
            phosphatUrine = 0;
            phosphatSerum = 0;
            creatininUrine = XN.creatininUrine;
            creatininSerum = XN.creatininSerum;
            //init("C_C03");
        }
        public FEPO4(double _creatininUrine, double _creatininSerum, double _phosphatUrine, double _phosphatSerum)
        {
            phosphatUrine = _phosphatUrine;
            phosphatSerum = _phosphatSerum;
            creatininUrine = _creatininUrine;
            creatininSerum = _creatininSerum;
            //init("C_C03");
        }
        public double kqFEPO4()
        {
            double kq = (phosphatUrine / phosphatSerum) / (creatininUrine / creatininSerum) * 100;
            return kq;
        }
        public string kqFEPO4_diengiai()
        {
            double ketqua = kqFEPO4();
            if (ketqua > 5 && ketqua < 20)
                return "FEPO4 trong khoảng giá trị bình thường: 5 - 20%";
            else if (ketqua < 5)
                return "FEPO4 thấp hơn khoảng giá trị bình thường: < 5%";
            else
                return "FEPO4 cao hơn khoảng giá trị bình thường: > 20%";
        }
    }
    public class LDL : Congthuc
    {
        public double TotalCholesterol { get; set; }
        public double HDL { get; set; }
        public double Triglycerid { get; set; }

        public LDL()
        {
            //init("C_C04");
        }
        public LDL(Xetnghiem XN)
        {
            TotalCholesterol = XN.totalCholesterol;
            Triglycerid = XN.triglyceride;
            HDL = XN.HDL;
            //init("C_C04");
        }

        public LDL(double _TotalCholesterol, double _HDL, double _Triglycerid)
        {
            TotalCholesterol = _TotalCholesterol;
            Triglycerid = _Triglycerid;
            HDL = _HDL;
            //init("C_C04");
        }
        public double kqLDL()
        {
            double kq = TotalCholesterol - (Triglycerid / 5) - HDL;
            return kq;
        }
        public string kqLDL_diengiai()
        {
            double ketqua = kqLDL();
            if (ketqua < 100)
                return "LDL trong khoảng giá trị bình thường: <100 mg/dL";
            else
                return "LDL cao hơn khoảng giá trị bình thường: <100 mg/dL";
        }
    }
    public class FIB4 : Congthuc
    {
        public double tuoi { get; set; }
        public double AST { get; set; }
        public double ALT { get; set; }
        public double tieucau { get; set; }

        public FIB4()
        {
            //init("C_C05");
        }
        public FIB4(Nguoibenh NB, Xetnghiem XN)
        {
            tuoi = NB.tinhtuoi_nam();
            AST = XN.AST;
            tieucau = XN.platelet;
            ALT = XN.ALT;
            //init("C_C05");
        }
        public FIB4(double _tuoi, double _AST, double _tieucau, double _ALT)
        {
            tuoi = _tuoi;
            AST = _AST;
            tieucau = _tieucau;
            ALT = _ALT;
            //init("C_C05");
        }
        public double kqFIB4()
        {
            double kq = tuoi * AST / (0.001 * tieucau * Math.Sqrt(ALT));
            return kq;
        }
        public string kqFIB4_diengiai()
        {
            double ketqua = kqFIB4();
            if (ketqua < 1.45)
                return "Điểm FIB-4 < 1,45: ít có khả năng xơ gan";
            else if (ketqua < 3.25)
                return "Điểm FIB-4 ≥ 1,45 và ≤ 3,25: khả năng xơ gan trung bình";
            else
                return "Điểm FIB-4 > 3,25: khả năng cao bị xơ gan";
        }
    }
    public class TSAT : Congthuc
    {
        public double FeSerum { get; set; }
        public double TIBC { get; set; }

        public TSAT()
        {
            //init("C_C06");
        }

        public TSAT(double _FeSerum, double _TIBC)
        {
            FeSerum = _FeSerum;
            TIBC = _TIBC;
            //init("C_C06");
        }
        public double kqTSAT()
        {
            double kq = (FeSerum / TIBC) * 100;
            return kq;
        }
        public string kqTSAT_diengiai()
        {
            double ketqua = kqTSAT();
            if (ketqua < 50 && ketqua >20)
                return "TSAT trong khoảng bình thường: 20 - 50%";
            else if (ketqua < 20)
                return "TSAT thấp, khả năng có tình trạng thiếu sắt";
            else
                return "TSAT cao, khả năng quá tải sắt";
        }
    }
    public class APRI : Congthuc // C_C07
    {
        public double AST { get; set; }
        public double tieucau { get; set; }
        public double ASTNormUL { get; set; }

        public APRI()
        {
            //init("C_C07");
        }
        public APRI(Xetnghiem XN)
        {
            AST = XN.AST;
            ASTNormUL = 0;
            tieucau = XN.platelet;
            //init("C_C07");
        }
        public APRI(double _AST, double _ASTNormUL, double _tieucau)
        {
            AST = _AST;
            ASTNormUL = _ASTNormUL;
            tieucau = _tieucau;
            //init("C_C07");
        }

        public double kqAPRI()
        {
            return 100 * ((AST / ASTNormUL) / (tieucau / 1000)); ;
        }
        public string kqAPRI_diengiai()
        {
            double ketqua = kqAPRI();
            if (ketqua <= 0.3)
                return "APRI ≤ 0,3:	Ít có khả năng xơ gan hoặc có xơ hóa đáng kể";
            else if (ketqua <= 0.5)
                return "APRI > 0,3 và ≤ 0,5: Ít có khả năng xơ gan, có khả năng xơ hóa đáng kể";
            else if (ketqua <= 1.5)
                return "APRI > 0,5 và ≤ 1,5: Có khả năng xơ gan hoặc xơ hóa đáng kể";
            else if (ketqua <= 2)
                return "APRI > 1,5 và ≤ 2: Khả năng cao xơ gan hoặc xơ hóa đáng kể";
            else
                return "APRI > 2: Khả năng cao bị xơ gan";
        }
    }
    public class MELD : Congthuc
    {
        public double CreatininSerum { get; set; }
        public double BilirubinSerum { get; set; }
        public double INR { get; set; }
        public double tansuatlocmau1tuan { get; set; }
        public double thoigianlocmau1tuan { get; set; }

        public MELD()
        {
            //init("C_C08");
        }
        public MELD(Xetnghiem XN)
        {
            tansuatlocmau1tuan = 0;
            thoigianlocmau1tuan = 0;
            CreatininSerum = XN.creatininSerum;
            BilirubinSerum = XN.bilirubin;
            INR = XN.INR;
            //init("C_C08");
        }
        public MELD(double _CreatininSerum, double _BilirubinSerum, double _INR)
        {
            tansuatlocmau1tuan = 0;
            thoigianlocmau1tuan = 0;
            CreatininSerum = _CreatininSerum;
            BilirubinSerum = _BilirubinSerum;
            INR = _INR;
            //init("C_C08");
        }
        public MELD(double _CreatininSerum, double _BilirubinSerum, double _INR, double _tansuatlocmau1tuan, double _thoigianlocmau1tuan)
        {
            tansuatlocmau1tuan = _tansuatlocmau1tuan;
            thoigianlocmau1tuan = _thoigianlocmau1tuan;
            CreatininSerum = _CreatininSerum;
            BilirubinSerum = _BilirubinSerum;
            INR = _INR;
            //init("C_C08");
        }

        public double kqMELD()
        {
            double creatinineTerm = (tansuatlocmau1tuan >= 2 || thoigianlocmau1tuan >= 24) ? 4 : CreatininSerum;

            double MELDResult = 9.57 * Math.Log(creatinineTerm) + 3.78 * Math.Log(BilirubinSerum) + 11.2 * Math.Log(INR) + 6.43;
            return MELDResult;
        }
        public string kqMELD_diengiai()
        {
            double ketqua = kqMELD();
            if (ketqua >= 10)
                return "Điểm MELD > 9, cân nhắc giới thiệu đến bác sĩ chuyên khoa gan hoặc trung tâm ghép gan";
            else
                return "Điểm MELD < 9, theo dõi tình trạng & chức năng gan định kỳ";
        }
    }
    public class MELDNa : Congthuc
    {
        public double NatriSerum { get; set; }
        public double CreatininSerum { get; set; }
        public double BilirubinSerum { get; set; }
        public double INR { get; set; }
        public double tansuatlocmau1tuan { get; set; }
        public double thoigianlocmau1tuan { get; set; }

        public MELDNa()
        {
            //init("C_C09");
        }
        public MELDNa(Xetnghiem XN)
        {
            tansuatlocmau1tuan = 0;
            thoigianlocmau1tuan = 0;
            CreatininSerum = XN.creatininSerum;
            BilirubinSerum = XN.bilirubin;
            INR = XN.INR;
            NatriSerum = XN.natriSerum;
            //init("C_C09");
        }
        public MELDNa(double _NatriSerum, double _CreatininSerum, double _BilirubinSerum, double _INR, double _tansuatlocmau1tuan, double _thoigianlocmau1tuan)
        {
            tansuatlocmau1tuan = _tansuatlocmau1tuan;
            thoigianlocmau1tuan = _thoigianlocmau1tuan;
            CreatininSerum = _CreatininSerum;
            BilirubinSerum = _BilirubinSerum;
            INR = _INR;
            NatriSerum = _NatriSerum;
            //init("C_C09");
        }

        public double kqMELDNa()
        {
            MELD meld = new MELD(tansuatlocmau1tuan, thoigianlocmau1tuan, CreatininSerum, BilirubinSerum, INR);
            double meldValue = meld.kqMELD();

            double MELDNaResult = (meldValue <= 11) ? meldValue : meldValue + ((1.32 * (137 - NatriSerum)) - (0.033 * meldValue * (137 - NatriSerum)));
            return MELDNaResult;
        }
        public string kqMELDNa_diengiai()
        {
            return "";
        }
    }
    public class PVR : Congthuc //C_C10
    {
        public double HATThu { get; set; } // Systolic Blood Pressure (SBP)
        public double HATTruong { get; set; } // Diastolic Blood Pressure (DBP)
        public double aplucnhitrai { get; set; }
        public double luuluongmau { get; set; }
        public double PVR_calculated { get; private set; }

        public PVR()
        {
            //init("C_C10");
        }
        public PVR(Nguoibenh nb)
        {
            HATThu = nb.HATThu;
            HATTruong = nb.HATTruong;
            aplucnhitrai = 0;
            luuluongmau = 0;
            //init("C_C10");
        }
        public PVR(double _HATThu, double _HATTruong, double _aplucnhitrai, double _luuluongmau)
        {
            HATThu = _HATThu;
            HATTruong = _HATTruong;
            aplucnhitrai = _aplucnhitrai;
            luuluongmau = _luuluongmau;
            //init("C_C10");
        }

        public double kqPVR()
        {
            MAP MAP = new MAP(HATThu, HATTruong);
            PVR_calculated = 80 * (MAP.kqMAP() - aplucnhitrai) / luuluongmau;
            return PVR_calculated;
        }
        public string kqPVR_diengiai()
        {
            return "";
        }
    }
    public class PVRI : Congthuc// C_C11
    {
        public double chieucao { get; set; }
        public double cannang { get; set; }
        public double HATThu { get; set; }
        public double HATTruong { get; set; }
        public double aplucnhitrai { get; set; }
        public double luuluongmau { get; set; }
        public double PVR_calculated { get; set; }
        public double PVRI_calculated { get; set; }

        public PVRI()
        {
            //init("C_C11");
        }
        public PVRI(Nguoibenh nb)
        {
            HATThu = nb.HATThu;
            HATTruong = nb.HATTruong;
            chieucao = nb.chieucao;
            cannang = nb.cannang;
            //init("C_C11");
        }
        public PVRI(double _chieucao, double _cannang, double _HATThu, double _HATTruong, double _aplucnhitrai, double _luuluongmau)
        {
            HATThu = _HATThu;
            HATTruong = _HATTruong;
            aplucnhitrai = _aplucnhitrai;
            luuluongmau = _luuluongmau;
            chieucao = _chieucao;
            cannang = _cannang;
            //init("C_C11");
        }
        public double kqPVRI()
        {
            PVR thamso = new PVR(HATThu, HATTruong, aplucnhitrai, luuluongmau);
            double BSA = Math.Sqrt(chieucao * cannang / 3600);
            PVRI_calculated = BSA * thamso.kqPVR();
            return PVRI_calculated;
        }
        public string kqPVRI_diengiai()
        {
            return "";
        }
    }
    public class AdjECG : Congthuc //C_C12
    {
        public double nhiptim { get; set; }
        public double QT_ECG { get; set; }
        public double RR_ECG { get; set; }

        public AdjECG()
        {
            //init("C_C12");
        }
        public AdjECG(Nguoibenh NB)
        {
            QT_ECG = 0;
            RR_ECG = 0;
            nhiptim = NB.nhiptim;
            //init("C_C12");
        }

        public AdjECG(double _nhiptim, double _QT_ECG, double _RR_ECG)
        {
            QT_ECG = _QT_ECG;
            RR_ECG = _RR_ECG;
            nhiptim = _nhiptim;
            //init("C_C12");
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
        public string kqAdjQT_diengiai()
        {
            return "";
        }
    }
    public class SVR : Congthuc
    {
        public double HATThu { get; set; } // Systolic Blood Pressure
        public double HATTruong { get; set; } // Diastolic Blood Pressure
        public double aplucnhiphai { get; set; }
        public double luuluongmau { get; set; }

        public SVR()
        {
            //init("C_C13");
        }
        public SVR(Nguoibenh nb)
        {
            HATThu = nb.HATThu;
            HATTruong = nb.HATTruong;
            //init("C_C13");
        }
        public SVR(double _HATThu, double _HATTruong, double _luuluongmau, double _aplucnhiphai)
        {
            HATThu = _HATThu;
            HATTruong = _HATTruong;
            luuluongmau = _luuluongmau;
            aplucnhiphai = _aplucnhiphai;
            //init("C_C13");
        }

        public double kqSVR()
        {
            MAP MAP = new MAP(HATThu, HATTruong);
            double SVR_calculated = 80 * (MAP.kqMAP() - aplucnhiphai) / luuluongmau;
            return SVR_calculated;
        }
        public string kqSVR_diengiai()
        {
            double ketqua = kqSVR();
            if (ketqua > 1170 - 270 && ketqua < 1170 + 270)
                return "SVR nằm trong khoảng bình thường: 1170 ± 270 dynes-sec-cm-5";
            else
                return "SVR nằm ngoài khoảng bình thường: 1170 ± 270 dynes-sec-cm-5";
        }
    }
    public class WBCCFS_Adj : Congthuc
    {
        public double WBC { get; set; }
        public double RBC { get; set; }
        public double RBC_CFS { get; set; }
        public double WBC_CFS { get; set; }

        public WBCCFS_Adj()
        {
            //init("C_C14");
        }
        public WBCCFS_Adj(Xetnghiem XN)
        {
            WBC_CFS = 0;
            RBC_CFS = 0;
            WBC = XN.WBC;
            RBC = XN.RBC;
            //init("C_C14");
        }
        public WBCCFS_Adj(double _WBC, double _RBC, double _RBC_CFS, double _WBC_CFS)
        {
            WBC_CFS = _WBC_CFS;
            RBC_CFS = _RBC_CFS;
            WBC = _WBC;
            RBC = _RBC;
            //init("C_C14");
        }
        public double kqWBCCFS_Adj()
        {
            double kqWBCCFS_Adj = WBC_CFS - ((WBC * RBC_CFS) / (RBC * 1000000));
            return kqWBCCFS_Adj;
        }
        public string kqWBCCFS_Adj_diengiai()
        {
            return "";
        }
    }
    public class Hauphauxogan : Congthuc
    {
        public double tuoi { get; set; }
        public double CreatininSerum { get; set; }
        public double BilirubinSerum { get; set; }
        public double INR { get; set; }
        public double ASA { get; set; }

        public Hauphauxogan()
        {
            //init("C_C15");
        }
        public Hauphauxogan(Nguoibenh nb, Xetnghiem xn)
        {
            tuoi = nb.tinhtuoi_nam();
            ASA = 0;
            CreatininSerum = xn.creatininSerum;
            BilirubinSerum = xn.bilirubin;
            INR = xn.INR;
            //init("C_C15");
        }
        public Hauphauxogan(double _tuoi, double _CreatininSerum, double _BilirubinSerum, double _INR, double _ASA)
        {
            tuoi = _tuoi;
            ASA = _ASA;
            CreatininSerum = _CreatininSerum;
            BilirubinSerum = _BilirubinSerum;
            INR = _INR;
            //init("C_C15");
        }

        public double kqHauphauxogan()
        {
            MELD MELD = new MELD(CreatininSerum, BilirubinSerum, INR);
            double MELD_value = MELD.kqMELD();
            double hesohauphauxogan = Math.Exp((0.02382 * (tuoi - 60)) + (0.88884 * ASA) + (0.11798 * (MELD_value - 8)));

            return hesohauphauxogan;
        }

        public double kqhauphau7n()
        {
            double hesohauphauxogan = kqHauphauxogan();
            double mortality = 100 * (1 - Math.Pow(0.98370, hesohauphauxogan));

            return mortality;
        }

        public double kqhauphau30n()
        {
            double hesohauphauxogan = kqHauphauxogan();
            double mortality = 100 * (1 - Math.Pow(0.93479, hesohauphauxogan));

            return mortality;
        }

        public double kqhauphau90n()
        {
            double hesohauphauxogan = kqHauphauxogan();
            double mortality = 100 * (1 - Math.Pow(0.89681, hesohauphauxogan));

            return mortality;
        }
        public string kqhauphau_diengiai()
        {
            return "";
        }
    }
    public class MESA_SCORE : Congthuc
    {
        public string gioitinh { get; set; }
        public double tuoi { get; set; }
        public double SBP { get; set; }
        public double TotalCholesterol { get; set; }
        public double HDL { get; set; }
        public bool dieutriTHA { get; set; }
        public bool DTD { get; set; }
        public bool hutthuoc { get; set; }
        public string chungtoc { get; set; }
        public bool dieutriRLLH { get; set; }
        public bool lichsuNMCTGD { get; set; }
        public double CAC { get; set; }
        public int hesogioitinhCAC { get; set; }
        public double hesochungtocCAC { get; set; }

        public MESA_SCORE()
        {
            //init("C_C16");
        }
        public MESA_SCORE(Nguoibenh nb, Xetnghiem xn)
        {
            tuoi = nb.tinhtuoi_nam();
            gioitinh = nb.gioitinh;
            DTD = nb.DTD;
            hutthuoc = nb.hutthuoc;
            TotalCholesterol = xn.totalCholesterol;
            HDL = xn.HDL;
            SBP = nb.HATThu;
            dieutriTHA = nb.THA;
            lichsuNMCTGD = nb.dotquytim || nb.thieumaunao || nb.NMCT;
            //init("C_C16");
        }
        public MESA_SCORE(string _gioitinh, double _tuoi, double _SBP, double _TotalCholesterol, double _HDL,
                      bool _dieutriTHA, bool _DTD, bool _hutthuoc, string _chungtoc,
                      bool _dieutriRLLH, bool _lichsuNMCTGD)
        {
            tuoi = _tuoi;
            gioitinh = _gioitinh.ToLower();
            chungtoc = _chungtoc.ToLower();
            DTD = _DTD;
            hutthuoc = _hutthuoc;
            TotalCholesterol = _TotalCholesterol;
            HDL = _HDL;
            dieutriRLLH = _dieutriRLLH;
            SBP = _SBP;
            dieutriTHA = _dieutriTHA;
            lichsuNMCTGD = _lichsuNMCTGD;
            //init("C_C16");
        }
        public MESA_SCORE(string _gioitinh, double _tuoi, double _SBP, double _TotalCholesterol, double _HDL,
                      bool _dieutriTHA, bool _DTD, bool _hutthuoc, string _chungtoc,
                      bool _dieutriRLLH, bool _lichsuNMCTGD, double _CAC)
        {
            tuoi = _tuoi;
            gioitinh = _gioitinh.ToLower();
            chungtoc = _chungtoc.ToLower();
            DTD = _DTD;
            hutthuoc = _hutthuoc;
            TotalCholesterol = _TotalCholesterol;
            HDL = _HDL;
            dieutriRLLH = _dieutriRLLH;
            SBP = _SBP;
            dieutriTHA = _dieutriTHA;
            lichsuNMCTGD = _lichsuNMCTGD;
            CAC = _CAC;
            //init("C_C16");
        }
        public int CheckHesogioitinh(string input)
        {
            return (input == "nam") ? 1 : 0;
        }
        public double CheckHesochungtocCAC(string input)
        {
            switch (input)
            {
                case "người da trắng":
                    return 0;
                case "người da đen":
                    return -0.2111;
                case "người châu á":
                    return -0.5055;
                case "người gốc latinh & tây ban nha":
                    return -0.19;
                case "khác":
                    return 0;
                default:
                    return 0;
            }
        }
        public void initMESA()
        {
            hesogioitinhCAC = CheckHesogioitinh(gioitinh);
            hesochungtocCAC = CheckHesochungtocCAC(chungtoc);
        }
        public double kqMESA_SCORE_nonCAC()
        {
            initMESA();
            double hesoNonCAC = (tuoi * 0.0455) + (hesogioitinhCAC * 0.7496) + hesochungtocCAC +
                                (DTD ? 0.5168 : 0) + (hutthuoc ? 0.4732 : 0) + (TotalCholesterol * 0.0053) -
                                (HDL * 0.0140) + (dieutriRLLH ? 0.2473 : 0) + (SBP * 0.0085) +
                                (dieutriTHA ? 0.3381 : 0) + (lichsuNMCTGD ? 0.4522 : 0);

            double nguyco10namNonCAC = 100 * (1 - Math.Pow(0.99963, Math.Exp(hesoNonCAC)));

            return nguyco10namNonCAC;
        }

        public double kqMESA_SCORE_CAC()
        {
            initMESA();
            double hesoCAC = (tuoi * 0.0172) + (hesogioitinhCAC * 0.4079) + hesochungtocCAC +
                             (DTD ? 0.3892 : 0) + (hutthuoc ? 0.3717 : 0) + (TotalCholesterol * 0.0043) -
                             (HDL * 0.0114) + (dieutriRLLH ? 0.1206 : 0) + (SBP * 0.0066) +
                             (dieutriTHA ? 0.2278 : 0) + (lichsuNMCTGD ? 0.3239 : 0) + (Math.Log(CAC + 1) * 0.2743);

            double nguyco10namCAC = 100 * (1 - Math.Pow(0.99833, Math.Exp(hesoCAC)));

            return nguyco10namCAC;
        }
        public string kqMESA_diengiai()
        {
            return "";
        }
    }
    #endregion
    #region Chỉ số y học chi tiết - Thang điểm
    #region T_A
    public class GRACE : Thangdiem //T_A01
    {
        public double GRACE_SCORE { get; set; }

        public GRACE()
        {

        }

        public GRACE(string _input)
        {
            initchiso("T_A01");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqGRACE()
        {
            GRACE_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                GRACE_SCORE += i.diemketqua;
            }

            return GRACE_SCORE;
        }

        public List<string> kqGRACE_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, GRACE_SCORE);
            return kq;
        }
    }
    public class COWS : Thangdiem //T_A02
    {
        public double COWS_SCORE { get; set; }
        public COWS()
        {

        }

        public COWS(string _input)
        {
            initchiso("T_A02");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqCOWS()
        {
            COWS_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                COWS_SCORE += i.diemketqua;
            }

            return COWS_SCORE;
        }

        public List<string> kqCOWS_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, COWS_SCORE);
            return kq;
        }
    }
    public class qSOFA : Thangdiem //T_A03
    {
        public double qSOFA_SCORE { get; set; }

        public qSOFA()
        {

        }

        public qSOFA(string _input)
        {
            initchiso("T_A03");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqqSOFA()
        {
            qSOFA_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                qSOFA_SCORE += i.diemketqua;
            }

            return qSOFA_SCORE;
        }

        public List<string> kqqSOFA_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, qSOFA_SCORE);
            return kq;
        }
    }
    public class VNTM : Thangdiem //T_A04
    {
        public double tieuchichinh { get; set; }
        public double tieuchiphu { get; set; }
        public double VNTM_SCORE { get; set; }

        public VNTM()
        {

        }

        public VNTM(string _input)
        {
            initchiso("T_A04");
            initTongdiem(_input);
            tinhTongdiem();
            //Có danh sách nhập & thứ tự & điểm
            tieuchichinh = DStinhdiem[0].diemketqua + DStinhdiem[1].diemketqua + DStinhdiem[2].diemketqua;
            tieuchiphu = DStinhdiem[3].diemketqua + DStinhdiem[4].diemketqua +
                DStinhdiem[5].diemketqua + DStinhdiem[6].diemketqua;
        }

        public double kqVNTM_Chinh()
        {
            return tieuchichinh;
        }
        public double kqVNTM_Phu()
        {
            return tieuchiphu;
        }

        public List<string> kqVNTM_diengiai()
        {
            List<string> kq = new List<string>();
            kq.Add("Kết quả đánh giá khả năng viêm nội tâm mạc theo tiêu chuẩn DUKE");
            string cdxd = "Chẩn đoán xác định viêm nội tâm mạc";
            string ncc = "Nguy cơ cao viêm nội tâm mạc nhiễm khuẩn";
            string loaitru = "Chẩn đoán loại trừ (ít khả năng viêm nội tâm mạc)";

            if (tieuchichinh >= 2)
                kq.Add(cdxd);
            else if (tieuchichinh == 1)
            {
                if (tieuchiphu >= 3)
                    kq.Add(cdxd);
                else if (tieuchiphu >= 1)
                    kq.Add(ncc);
                else
                    kq.Add(loaitru);
            }
            else
            {
                if (tieuchiphu >= 5)
                    kq.Add(cdxd);
                else if (tieuchiphu >= 3)
                    kq.Add(ncc);
                else
                    kq.Add(loaitru);
            }
            return kq;
        }
    }
    public class MalHyperthermia : Thangdiem //T_A05
    {
        public double MalHyperthermia_SCORE { get; set; }

        public MalHyperthermia()
        {

        }

        public MalHyperthermia(string _input)
        {
            initchiso("T_A05");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqMalHyperthermia()
        {
            MalHyperthermia_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                MalHyperthermia_SCORE += i.diemketqua;
            }

            return MalHyperthermia_SCORE;
        }

        public List<string> kqMalHyperthermia_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, MalHyperthermia_SCORE);
            return kq;
        }
    }
    public class PSI : Thangdiem //T_A06
    {
        public double PSI_SCORE { get; set; }

        public PSI()
        {

        }

        public PSI(string _input)
        {
            initchiso("T_A06");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqPSI()
        {
            PSI_SCORE = 0;
            foreach (BiendiemCSYH i in DStinhdiem)
            {
                if (i.idloaibien == 1)
                    PSI_SCORE += i.giatri; //Bien DL tuoi
                else
                    PSI_SCORE += i.diemketqua; //Bien DT
            }

            return PSI_SCORE;
        }

        public List<string> kqPSI_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, PSI_SCORE);
            return kq;
        }
    }
    public class VCSS : Thangdiem //T_A07
    {
        public double VCSS_SCORE { get; set; }

        public VCSS()
        {

        }

        public VCSS(string _input)
        {
            initchiso("T_A07");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqVCSS()
        {
            VCSS_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                VCSS_SCORE += i.diemketqua;
            }

            return VCSS_SCORE;
        }

        public List<string> kqVCSS_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, VCSS_SCORE);
            return kq;
        }
    }
    public class BISAP : Thangdiem //T_A08
    {
        public double BISAP_SCORE { get; set; }

        public BISAP()
        {

        }

        public BISAP(string _input)
        {
            initchiso("T_A08");
            initTongdiem(_input);
        }
        public void xulydiem()
        {
            tinhTongdiem();
            double diemTC3 = DStinhdiem[2].diemketqua + DStinhdiem[3].diemketqua +
                DStinhdiem[4].diemketqua + DStinhdiem[5].diemketqua +
                DStinhdiem[6].diemketqua + DStinhdiem[7].diemketqua;

            if (diemTC3 >= 2)
            {
                DStinhdiem[2].diemketqua = 1;
                DStinhdiem[3].diemketqua = 0;
                DStinhdiem[4].diemketqua = 0;
                DStinhdiem[5].diemketqua = 0;
                DStinhdiem[6].diemketqua = 0;
                DStinhdiem[7].diemketqua = 0;
            }
            else
            {

                DStinhdiem[2].diemketqua = 0;
                DStinhdiem[3].diemketqua = 0;
                DStinhdiem[4].diemketqua = 0;
                DStinhdiem[5].diemketqua = 0;
                DStinhdiem[6].diemketqua = 0;
                DStinhdiem[7].diemketqua = 0;
            }
        }
        public double kqBISAP()
        {
            BISAP_SCORE = 0;
            xulydiem();

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                BISAP_SCORE += i.diemketqua;
            }

            return BISAP_SCORE;
        }

        public List<string> kqBISAP_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, BISAP_SCORE);
            return kq;
        }
    }
    public class Blatchford : Thangdiem //T_A09
    {
        public double Blatchford_SCORE { get; set; }

        public Blatchford()
        {

        }

        public Blatchford(string _input)
        {
            initchiso("T_A09");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqBlatchford()
        {
            Blatchford_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Blatchford_SCORE += i.diemketqua;
            }

            return Blatchford_SCORE;
        }

        public List<string> kqBlatchford_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Blatchford_SCORE);
            return kq;
        }
    }
    public class Rockall : Thangdiem //T_A10
    {
        public double Rockall_SCORE { get; set; }

        public Rockall()
        {

        }

        public Rockall(string _input)
        {
            initchiso("T_A10");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqRockall()
        {
            Rockall_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Rockall_SCORE += i.diemketqua;
            }

            return Rockall_SCORE;
        }

        public List<string> kqRockall_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Rockall_SCORE);
            return kq;
        }
    }
    public class ChildPugh : Thangdiem //T_A11
    {
        public double ChildPugh_SCORE { get; set; }

        public ChildPugh()
        {

        }

        public ChildPugh(string _input)
        {
            initchiso("T_A11");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqChildPugh()
        {
            ChildPugh_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                ChildPugh_SCORE += i.diemketqua;
            }

            return ChildPugh_SCORE;
        }

        public List<string> kqChildPugh_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, ChildPugh_SCORE);
            return kq;
        }
    }
    public class CLIFSOFA : Thangdiem //T_A12
    {
        public double CLIFSOFA_SCORE { get; set; }

        public CLIFSOFA()
        {

        }

        public CLIFSOFA(string _input)
        {
            initchiso("T_A12");
            initTongdiem(_input);
        }
        protected void xulybien()
        {

            //Xu ly bien
            if (DStinhdiem[1].giatri != 0)
            {
                double PaO2 = DStinhdiem[0].giatri; //PaO2
                double FiO2 = DStinhdiem[1].giatri; //FiO2
                double SpO2 = DStinhdiem[2].giatri; //SpO2
                bool dungthuocvanmach = DStinhdiem[7].thutunhap != 4;
                double phoi = 0;

                if (PaO2 != 0)
                {
                    phoi = PaO2 / FiO2;
                    if (phoi > 400)
                        DStinhdiem[3].thutunhap = 1;
                    else if (phoi > 300)
                        DStinhdiem[3].thutunhap = 2;
                    else if (phoi > 200)
                        DStinhdiem[3].thutunhap = 3;
                    else if (phoi > 100)
                        DStinhdiem[3].thutunhap = 4;
                    else
                        DStinhdiem[3].thutunhap = 5;
                }
                else if (SpO2 != 0)
                {
                    phoi = SpO2 / FiO2;
                    if (phoi > 512)
                        DStinhdiem[3].thutunhap = 1;
                    else if (phoi >= 358)
                        DStinhdiem[3].thutunhap = 2;
                    else if (phoi >= 215)
                        DStinhdiem[3].thutunhap = 3;
                    else if (phoi >= 90)
                        DStinhdiem[3].thutunhap = 4;
                    else
                        DStinhdiem[3].thutunhap = 5;
                }

                if (dungthuocvanmach)
                    DStinhdiem[6].thutunhap = 0;
            }
        }
        public double kqCLIFSOFA()
        {
            xulybien();
            tinhTongdiem();
            CLIFSOFA_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                CLIFSOFA_SCORE += i.diemketqua;
            }

            return CLIFSOFA_SCORE;
        }

        public List<string> kqCLIFSOFA_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, CLIFSOFA_SCORE);
            return kq;
        }
    }
    public class HBCrohn : Thangdiem //T_A13
    {
        public double HBCrohn_SCORE { get; set; }

        public HBCrohn()
        {

        }

        public HBCrohn(string _input)
        {
            initchiso("T_A13");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public double kqHBCrohn()
        {
            HBCrohn_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                if (i.idloaibien == 1)
                    HBCrohn_SCORE += i.giatri; //Bien DL so lan di phan long
                else
                    HBCrohn_SCORE += i.diemketqua; //Bien DT
            }

            return HBCrohn_SCORE;
        }

        public List<string> kqHBCrohn_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, HBCrohn_SCORE);
            return kq;
        }
    }
    public class Ranson : Thangdiem //T_A15
    {
        public double Ranson_SCORE { get; set; }

        public Ranson()
        {

        }

        public Ranson(string _input)
        {
            initchiso("T_A15");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqRanson()
        {
            Ranson_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Ranson_SCORE += i.diemketqua;
            }

            return Ranson_SCORE;
        }

        public List<string> kqRanson_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Ranson_SCORE);
            return kq;
        }
    }
    public class IVPO : Thangdiem //T_A16
    {
        public double IVPO_SCORE { get; set; }

        public IVPO()
        {

        }

        public IVPO(string _input)
        {
            initchiso("T_A16");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqIVPO()
        {
            IVPO_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                IVPO_SCORE += i.diemketqua;
            }

            return IVPO_SCORE;
        }

        public List<string> kqIVPO_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, IVPO_SCORE);
            return kq;
        }
    }
    public class PUMayoClinic : Thangdiem //T_A17
    {
        public double PUMayoClinic_SCORE { get; set; }

        public PUMayoClinic()
        {

        }

        public PUMayoClinic(string _input)
        {
            initchiso("T_A17");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqPUMayoClinic()
        {
            PUMayoClinic_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                PUMayoClinic_SCORE += i.diemketqua;
            }

            return PUMayoClinic_SCORE;
        }

        public List<string> kqPUMayoClinic_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, PUMayoClinic_SCORE);
            return kq;
        }
    }
    public class CDAICrohn : Thangdiem //T_A18
    {
        public double CDAICrohn_SCORE { get; set; }
        public double diemHct { get; set; }
        public double diemCannang { get; set; }
        public CDAICrohn()
        {

        }

        public CDAICrohn(string _input)
        {
            initchiso("T_A18");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public void xulybien()
        {
            string gioitinh = (DStinhdiem[0].thutunhap == 1) ? "nam" : "nữ";
            double chieucao = DStinhdiem[1].giatri;
            double cannang = DStinhdiem[2].giatri;
            double Hct = DStinhdiem[3].giatri;
            double diphanlong = DStinhdiem[4].giatri;

            if (gioitinh == "nam")
            {
                diemHct = 6 * (47 - Hct);
                diemCannang = 100 * (1 - cannang / (chieucao * chieucao * 22.1));
            }
            else
            {
                diemHct = 6 * (42 - Hct);
                diemCannang = 100 * (1 - cannang / (chieucao * chieucao * 20.8));
            }
            DStinhdiem[4].diemketqua = diphanlong * 14;
        }
        public double kqCDAICrohn()
        {
            xulybien();

            CDAICrohn_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                CDAICrohn_SCORE += i.diemketqua;
            }
            CDAICrohn_SCORE += diemCannang;
            CDAICrohn_SCORE += diemHct;

            return CDAICrohn_SCORE;
        }

        public List<string> kqCDAICrohn_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, CDAICrohn_SCORE);
            return kq;
        }
    }
    public class GlasgowComa : Thangdiem //T_A14
    {
        public double GlasgowComa_SCORE { get; set; }
        //idbien và thứ tự lựa chọn tìm từ DStinhdiem
        public GlasgowComa()
        {

        }
        public GlasgowComa(string _input)
        {
            initchiso("T_A14");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public double kqGlasgowComa()
        {
            GlasgowComa_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                GlasgowComa_SCORE = GlasgowComa_SCORE + i.diemketqua;
            }

            return GlasgowComa_SCORE;
        }
        public List<string> kqGlasgowComa_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, GlasgowComa_SCORE);
            return kq;
        }
    }
    #endregion
    #region T_B
    public class APACHE2 : Thangdiem //T_B01
    {
        public double APACHE2_SCORE { get; set; }
        public double GSC { get; set; }
        public double diembenhPT { get; set; }
        public double trongsobenhPT { get; set; }
        public double trongsochandoan { get; set; }
        public double Log_OR { get; set; }
        public double OR { get; set; }
        public double nguycotuvong { get; set; }

        public APACHE2()
        {

        }

        public APACHE2(string _input)
        {
            initchiso("T_B01");
            initTongdiem(_input);
        }
        public void xulybien()
        {

            bool FiO2_AaG = DStinhdiem[4].thutunhap == 1;
            if (FiO2_AaG)
                DStinhdiem[6].thutunhap = 0;
            else
                DStinhdiem[5].thutunhap = 0;

            bool pHSerum = DStinhdiem[7].thutunhap == 1;
            if (pHSerum)
                DStinhdiem[9].thutunhap = 0;
            else
                DStinhdiem[8].thutunhap = 0;

            GSC = Math.Min(15 - DStinhdiem[15].giatri, 3);

            bool benhmantinh = DStinhdiem[17].thutunhap == 1;
            int loaiphauthuat = DStinhdiem[18].thutunhap;

            if (benhmantinh)
            {
                diembenhPT = 0;
                if (loaiphauthuat != 3)
                {
                    trongsobenhPT = 0;
                }
                else
                {
                    trongsobenhPT = 0.603;
                }
            }
            else
            {
                if (loaiphauthuat == 1)
                {
                    diembenhPT = 5;
                    trongsobenhPT = 0;
                }
                else if (loaiphauthuat == 2)
                {
                    diembenhPT = 2;
                    trongsobenhPT = 0;
                }
                else
                {
                    diembenhPT = 5;
                    trongsobenhPT = 0.603;
                }
            }
        }
        public double kqAPACHE2()
        {
            xulybien();
            tinhTongdiem();

            bool phauthuat = DStinhdiem[19].thutunhap == 2;
            if (!phauthuat)
                DStinhdiem[21].diemketqua = 0;
            else
                DStinhdiem[20].diemketqua = 0;

            APACHE2_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                APACHE2_SCORE += i.diemketqua;
            }

            APACHE2_SCORE = APACHE2_SCORE - DStinhdiem[20].diemketqua - DStinhdiem[21].diemketqua;

            return APACHE2_SCORE;
        }

        public List<string> kqAPACHE2_diengiai()
        {
            Log_OR = -3.517 + (APACHE2_SCORE * 0.146) + trongsobenhPT + trongsochandoan;

            OR = Math.Exp(Log_OR);

            nguycotuvong = 100 * OR / (1 + OR);

            List<string> kq = new List<string>() {"LogOR", Math.Round(Log_OR,2).ToString(),
                "OR", Math.Round(OR,2).ToString(),
                "Nguy cơ tử vong", Math.Round(nguycotuvong,2).ToString() };
            return kq;
        }
    }
    public class BODECOPD : Thangdiem //T_B02
    {
        public double BODECOPD_SCORE { get; set; }

        public BODECOPD()
        {

        }

        public BODECOPD(string _input)
        {
            initchiso("T_B02");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqBODECOPD()
        {
            BODECOPD_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                BODECOPD_SCORE += i.diemketqua;
            }

            return BODECOPD_SCORE;
        }

        public List<string> kqBODECOPD_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, BODECOPD_SCORE);
            return kq;
        }
    }
    public class CURB65 : Thangdiem //T_B03
    {
        public double CURB65_SCORE { get; set; }

        public CURB65()
        {

        }

        public CURB65(string _input)
        {
            initchiso("T_B03");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public void xulybien()
        {
            if (DStinhdiem[1].diemketqua == 1 && DStinhdiem[2].diemketqua == 1)
                DStinhdiem[2].diemketqua = 0;
            if (DStinhdiem[4].diemketqua == 1 && DStinhdiem[5].diemketqua == 1)
                DStinhdiem[5].diemketqua = 0;
        }
        public double kqCURB65()
        {
            xulybien();
            CURB65_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                CURB65_SCORE += i.diemketqua;
            }

            return CURB65_SCORE;
        }

        public List<string> kqCURB65_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, CURB65_SCORE);
            return kq;
        }
    }
    public class Light : Thangdiem //T_B04
    {
        public double Light_SCORE { get; set; }

        public Light()
        {

        }

        public Light(string _input)
        {
            initchiso("T_B04");
            initTongdiem(_input);
        }
        public void xulydiem()
        {
            double protein_dichmangphoi = DStinhdiem[0].giatri;
            double ProteinSerum = DStinhdiem[1].giatri;
            double LDH_dichmangphoi = DStinhdiem[2].giatri;
            double LDHSerum = DStinhdiem[3].giatri;
            double LDHSerum_UL = DStinhdiem[4].giatri;

            DStinhdiem[5].diemketqua = (protein_dichmangphoi / ProteinSerum > 0.5) ? 1 : 0;
            DStinhdiem[6].diemketqua = (LDH_dichmangphoi / LDHSerum > 0.6) ? 1 : 0;
            DStinhdiem[7].diemketqua = (LDH_dichmangphoi / LDHSerum_UL > 0.66) ? 1 : 0;
        }
        public double kqLight()
        {
            xulydiem();
            Light_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Light_SCORE += i.diemketqua;
            }

            return Light_SCORE;
        }

        public List<string> kqLight_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Light_SCORE);
            return kq;
        }
    }
    public class GenevaDVT : Thangdiem //T_B05
    {
        public double GenevaDVT_SCORE { get; set; }

        public GenevaDVT()
        {

        }

        public GenevaDVT(string _input)
        {
            initchiso("T_B05");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqGenevaDVT()
        {
            GenevaDVT_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                GenevaDVT_SCORE += i.diemketqua;
            }

            return GenevaDVT_SCORE;
        }

        public List<string> kqGenevaDVT_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, GenevaDVT_SCORE);
            return kq;
        }
    }
    public class GenevaPE : Thangdiem //T_B06
    {
        public double GenevaPE_SCORE { get; set; }

        public GenevaPE()
        {

        }

        public GenevaPE(string _input)
        {
            initchiso("T_B06");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public double kqGenevaPE()
        {
            GenevaPE_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                GenevaPE_SCORE += i.diemketqua;
            }

            return GenevaPE_SCORE;
        }

        public List<string> kqGenevaPE_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, GenevaPE_SCORE);
            return kq;
        }
    }
    public class WellsDVT : Thangdiem //T_B07
    {
        public double WellsDVT_SCORE { get; set; }

        public WellsDVT()
        {

        }

        public WellsDVT(string _input)
        {
            initchiso("T_B07");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqWellsDVT()
        {
            WellsDVT_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                WellsDVT_SCORE += i.diemketqua;
            }

            return WellsDVT_SCORE;
        }

        public List<string> kqWellsDVT_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, WellsDVT_SCORE);
            return kq;
        }
    }
    public class NEWS2 : Thangdiem //T_B08
    {
        public double NEWS2_SCORE { get; set; }
        public bool check3diem { get; set; }
        public NEWS2()
        {

        }

        public NEWS2(string _input)
        {
            initchiso("T_B08");
            initTongdiem(_input);
        }

        public void xulybien()
        {
            double SpO2 = DStinhdiem[1].giatri;
            bool suyhohap = DStinhdiem[2].thutunhap == 1;
            bool thokhiphong = DStinhdiem[3].thutunhap == 1;

            if (suyhohap)
            {
                DStinhdiem[8].thutunhap = 0;

                if (SpO2 <= 83 || (SpO2 >= 97 && !thokhiphong))
                    DStinhdiem[9].thutunhap = 4;
                else if (SpO2 <= 85 || (SpO2 >= 95 && !thokhiphong))
                    DStinhdiem[9].thutunhap = 3;
                else if (SpO2 <= 88 || (SpO2 >= 93 && !thokhiphong))
                    DStinhdiem[9].thutunhap = 2;
                else
                    DStinhdiem[9].thutunhap = 1;
            }
            else
            {
                DStinhdiem[9].thutunhap = 0;
                if (SpO2 >= 96)
                    DStinhdiem[8].thutunhap = 1;
                else if (SpO2 >= 94)
                    DStinhdiem[8].thutunhap = 2;
                else if (SpO2 >= 92)
                    DStinhdiem[8].thutunhap = 3;
                else
                    DStinhdiem[8].thutunhap = 4;
            }
        }
        public double kqNEWS2()
        {
            xulybien();
            tinhTongdiem();

            NEWS2_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                NEWS2_SCORE += i.diemketqua;
                if (i.diemketqua == 3)
                    check3diem = true;
            }

            return NEWS2_SCORE;
        }

        public List<string> kqNEWS2_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, NEWS2_SCORE);
            if (check3diem && NEWS2_SCORE <= 4)
            {
                kq.Clear();
                kq.Add("Nguy cơ trung bình thấp, đánh giá lại, theo dõi mỗi 1h");
            }
            else if (NEWS2_SCORE <= 4)
            {
                kq.Clear();
                kq.Add("Nguy cơ thấp, tiếp tục theo dõi NEWS2");
            }
            return kq;
        }
    }
    public class PaduaVTE : Thangdiem //T_B09
    {
        public double PaduaVTE_SCORE { get; set; }

        public PaduaVTE()
        {

        }

        public PaduaVTE(string _input)
        {
            initchiso("T_B09");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public void xulydiem()
        {
            if (DStinhdiem[6].diemketqua == 1 && DStinhdiem[7].diemketqua == 1)
                DStinhdiem[7].diemketqua = 0;
            if (DStinhdiem[8].diemketqua == 1 && DStinhdiem[9].diemketqua == 1)
                DStinhdiem[9].diemketqua = 0;
            if (DStinhdiem[10].diemketqua == 1 && DStinhdiem[11].diemketqua == 1)
                DStinhdiem[11].diemketqua = 0;
        }
        public double kqPaduaVTE()
        {
            PaduaVTE_SCORE = 0;
            xulydiem();
            foreach (BiendiemCSYH i in DStinhdiem)
            {
                PaduaVTE_SCORE += i.diemketqua;
            }

            return PaduaVTE_SCORE;
        }
        public List<string> kqPaduaVTE_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, PaduaVTE_SCORE);
            return kq;
        }
    }
    public class WellsPE : Thangdiem //T_B10
    {
        public double WellsPE_SCORE { get; set; }

        public WellsPE()
        {

        }

        public WellsPE(string _input)
        {
            initchiso("T_B10");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqWellsPE()
        {
            WellsPE_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                WellsPE_SCORE += i.diemketqua;
            }

            return WellsPE_SCORE;
        }

        public List<string> kqWellsPE_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, WellsPE_SCORE);
            return kq;
        }
    }
    public class SOFA : Thangdiem //T_B11
    {
        public double SOFA_SCORE { get; set; }

        public SOFA()
        {

        }

        public SOFA(string _input)
        {
            initchiso("T_B11");
            initTongdiem(_input);
        }
        public void xulybien()
        {
            double PaO2 = DStinhdiem[0].giatri;
            double FiO2 = DStinhdiem[1].giatri;
            bool hotrohohap = DStinhdiem[2].thutunhap == 1;
            bool dungthuocvanmach = DStinhdiem[7].thutunhap != 4;

            if (PaO2 / FiO2 > 400)
                DStinhdiem[3].thutunhap = 1;
            else if (PaO2 / FiO2 > 300)
                DStinhdiem[3].thutunhap = 2;
            else if (PaO2 / FiO2 > 200)
                DStinhdiem[3].thutunhap = 3;
            else if (hotrohohap == false)
                DStinhdiem[3].thutunhap = 3;
            else if (PaO2 / FiO2 > 100)
                DStinhdiem[3].thutunhap = 4;
            else
                DStinhdiem[3].thutunhap = 5;

            if (dungthuocvanmach)
                DStinhdiem[6].thutunhap = 0;
        }
        public double kqSOFA()
        {
            xulybien();
            tinhTongdiem();
            SOFA_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                SOFA_SCORE += i.diemketqua;
            }

            return SOFA_SCORE;
        }

        public List<string> kqSOFA_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, SOFA_SCORE);
            return kq;
        }
    }
    public class VTEBLEED : Thangdiem //T_B12
    {
        public double VTEBLEED_SCORE { get; set; }

        public VTEBLEED()
        {

        }

        public VTEBLEED(string _input)
        {
            initchiso("T_B12");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqVTEBLEED()
        {
            VTEBLEED_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                VTEBLEED_SCORE += i.diemketqua;
            }

            return VTEBLEED_SCORE;
        }

        public List<string> kqVTEBLEED_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, VTEBLEED_SCORE);
            return kq;
        }
    }
    public class HeparinIT : Thangdiem //T_B13
    {
        public double HeparinIT_SCORE { get; set; }

        public HeparinIT()
        {

        }

        public HeparinIT(string _input)
        {
            initchiso("T_B13");
            initTongdiem(_input);
        }
        public void xulybien()
        {
            int tylegiamPLT = DStinhdiem[0].thutunhap; // 1.Trên 50%\2. 30-50%\3. Dưới 30%
            int muctieucau = DStinhdiem[1].thutunhap; // 1.≥20.000/microL\2. 10.000 đến 19.000/microL\3. <10.000/microL
            double khoiphatgiamPLT = DStinhdiem[2].giatri;
            double ngaydungheparin = (DateTime.Today - KetnoiDB.numbertodatetime(DStinhdiem[3].giatri.ToString())).TotalDays;

            //Giam tieu cau: bien [7]
            if (tylegiamPLT == 1 && muctieucau == 1)
                DStinhdiem[7].thutunhap = 1;
            else if (tylegiamPLT == 2 || muctieucau == 2)
                DStinhdiem[7].thutunhap = 2;
            else
                DStinhdiem[7].thutunhap = 3;

            //Danh gia giam tieu cau va tg xai heparin: bien [6]
            if (khoiphatgiamPLT < 4 && ngaydungheparin > 100)
                DStinhdiem[6].thutunhap = 3;
            else if (khoiphatgiamPLT <= 1 && ngaydungheparin > 30)
                DStinhdiem[6].thutunhap = 2;
            else if (khoiphatgiamPLT <= 1 && ngaydungheparin <= 30)
                DStinhdiem[6].thutunhap = 1;
            else if (khoiphatgiamPLT > 10)
                DStinhdiem[6].thutunhap = 2;
            else
                DStinhdiem[6].thutunhap = 1;
        }
        public double kqHeparinIT()
        {
            xulybien();
            tinhTongdiem();
            HeparinIT_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                HeparinIT_SCORE += i.diemketqua;
            }

            return HeparinIT_SCORE;
        }

        public List<string> kqHeparinIT_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, HeparinIT_SCORE);
            return kq;
        }
    }
    public class HASBLED : Thangdiem //T_B14
    {
        public double HASBLED_SCORE { get; set; }

        public HASBLED()
        {

        }

        public HASBLED(string _input)
        {
            initchiso("T_B14");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqHASBLED()
        {
            HASBLED_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                HASBLED_SCORE += i.diemketqua;
            }

            return HASBLED_SCORE;
        }

        public List<string> kqHASBLED_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, HASBLED_SCORE);
            return kq;
        }
    }
    public class DIPSSPlusPMS : Thangdiem //T_B15
    {
        public double DIPSS_SCORE { get; set; }
        public double DIPSSPlus_SCORE { get; set; }

        public DIPSSPlusPMS()
        {

        }

        public DIPSSPlusPMS(string _input)
        {
            initchiso("T_B15");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public double kqDIPSS()
        {
            DIPSS_SCORE = DStinhdiem[0].diemketqua + DStinhdiem[1].diemketqua +
                2 * DStinhdiem[2].diemketqua + DStinhdiem[3].diemketqua +
                Math.Min(DStinhdiem[7].diemketqua + DStinhdiem[8].diemketqua + DStinhdiem[9].diemketqua, 1);

            return DIPSS_SCORE;
        }

        public List<string> kqDIPSS_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ_2(IDChiso, DIPSS_SCORE, 81); // id của DIPSS
            return kq;
        }
        public double kqDIPSSPlus()
        {
            double diembosung;
            if (DIPSS_SCORE == 0)
                diembosung = 0;
            else if (DIPSS_SCORE <= 2)
                diembosung = 1;
            else if (DIPSS_SCORE <= 4)
                diembosung = 2;
            else
                diembosung = 3;

            DIPSSPlus_SCORE = DStinhdiem[4].diemketqua + DStinhdiem[5].diemketqua +
                DStinhdiem[6].diemketqua + diembosung;

            return DIPSSPlus_SCORE;
        }

        public List<string> kqDIPSSPlus_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ_2(IDChiso, DIPSSPlus_SCORE, 37);
            return kq;
        }
    }
    public class IPSHodgkin : Thangdiem //T_B16
    {
        public double IPSHodgkin_SCORE { get; set; }

        public IPSHodgkin()
        {

        }

        public IPSHodgkin(string _input)
        {
            initchiso("T_B16");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqIPSHodgkin()
        {
            IPSHodgkin_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                IPSHodgkin_SCORE += i.diemketqua;
            }

            return IPSHodgkin_SCORE;
        }

        public List<string> kqIPSHodgkin_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, IPSHodgkin_SCORE);
            return kq;
        }
    }
    public class GIPSSXotuy : Thangdiem //T_B17
    {
        public double GIPSSXotuy_SCORE { get; set; }

        public GIPSSXotuy()
        {

        }

        public GIPSSXotuy(string _input)
        {
            initchiso("T_B17");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqGIPSSXotuy()
        {
            GIPSSXotuy_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                GIPSSXotuy_SCORE += i.diemketqua;
            }

            return GIPSSXotuy_SCORE;
        }

        public List<string> kqGIPSSXotuy_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, GIPSSXotuy_SCORE);
            return kq;
        }
    }
    public class IPSNonHodgkin : Thangdiem //T_B18
    {
        public double IPSNonHodgkin_SCORE { get; set; }

        public IPSNonHodgkin()
        {

        }

        public IPSNonHodgkin(string _input)
        {
            initchiso("T_B18");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqIPSNonHodgkin()
        {
            IPSNonHodgkin_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                IPSNonHodgkin_SCORE += i.diemketqua;
            }

            return IPSNonHodgkin_SCORE;
        }

        public List<string> kqIPSNonHodgkin_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, IPSNonHodgkin_SCORE);
            return kq;
        }
    }
    public class Khorana : Thangdiem //T_B19
    {
        public double Khorana_SCORE { get; set; }

        public Khorana()
        {

        }

        public Khorana(string _input)
        {
            initchiso("T_B19");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public void xulydiem()
        {
            if (DStinhdiem[2].diemketqua == 1 && DStinhdiem[3].diemketqua == 1)
                DStinhdiem[3].diemketqua = 0;
        }
        public double kqKhorana()
        {
            Khorana_SCORE = 0;
            xulydiem();

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Khorana_SCORE += i.diemketqua;
            }

            return Khorana_SCORE;
        }

        public List<string> kqKhorana_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Khorana_SCORE);
            return kq;
        }
    }
    public class MDACC : Thangdiem //T_B20
    {
        public double MDACC_SCORE { get; set; }

        public MDACC()
        {

        }

        public MDACC(string _input)
        {
            initchiso("T_B20");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqMDACC()
        {
            MDACC_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                MDACC_SCORE += i.diemketqua;
            }

            return MDACC_SCORE;
        }

        public List<string> kqMDACC_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, MDACC_SCORE);
            return kq;
        }
    }
    public class MDSRLsinhtuy : Thangdiem //T_B21
    {
        public double MDSRLsinhtuy_SCORE { get; set; }

        public MDSRLsinhtuy()
        {

        }

        public MDSRLsinhtuy(string _input)
        {
            initchiso("T_B21");
            initTongdiem(_input);
        }
        public void xulydiem()
        {
            int giamHb = (DStinhdiem[2].thutunhap == 1) ? 1 : 0;
            int giamnNeu = (DStinhdiem[3].thutunhap == 1) ? 1 : 0;
            int giamPLT = (DStinhdiem[4].thutunhap == 1) ? 1 : 0;

            if (giamHb + giamnNeu + giamPLT >= 2)
                DStinhdiem[5].thutunhap = 2;
            else
                DStinhdiem[5].thutunhap = 1;
        }
        public double kqMDSRLsinhtuy()
        {
            xulydiem();

            tinhTongdiem();
            MDSRLsinhtuy_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                MDSRLsinhtuy_SCORE += i.diemketqua;
            }

            return MDSRLsinhtuy_SCORE;
        }

        public List<string> kqMDSRLsinhtuy_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, MDSRLsinhtuy_SCORE);
            return kq;
        }
    }
    public class Sokal : Thangdiem //T_B22
    {
        public double Sokal_SCORE { get; set; }

        public Sokal()
        {

        }

        public Sokal(string _input)
        {
            initchiso("T_B22");
            initTongdiem(_input);
        }

        public double kqSokal()
        {
            double tuoi = DStinhdiem[0].giatri;
            double kichthuoclach = DStinhdiem[1].giatri;
            double PLT = DStinhdiem[2].giatri;
            double blastSerum_tyle = DStinhdiem[3].giatri;

            Sokal_SCORE = Math.Exp(0.0116 * (tuoi - 43.4) + 0.0345 * (kichthuoclach - 7.51) +
                0.188 * (PLT / 700 - 0.563) + 0.0887 * (blastSerum_tyle - 2.1)); ;

            return Sokal_SCORE;
        }

        public List<string> kqSokal_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Sokal_SCORE);
            return kq;
        }
    }
    public class APGAR : Thangdiem //T_B23
    {
        public double APGAR_SCORE { get; set; }

        public APGAR()
        {

        }

        public APGAR(string _input)
        {
            initchiso("T_B23");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqAPGAR()
        {
            APGAR_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                APGAR_SCORE += i.diemketqua;
            }

            return APGAR_SCORE;
        }

        public List<string> kqAPGAR_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, APGAR_SCORE);
            return kq;
        }
    }
    public class PUCAI : Thangdiem //T_B24
    {
        public double PUCAI_SCORE { get; set; }

        public PUCAI()
        {

        }

        public PUCAI(string _input)
        {
            initchiso("T_B24");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqPUCAI()
        {
            PUCAI_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                PUCAI_SCORE += i.diemketqua;
            }

            return PUCAI_SCORE;
        }

        public List<string> kqPUCAI_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, PUCAI_SCORE);
            return kq;
        }
    }
    public class WestleyCroup : Thangdiem //T_B25
    {
        public double WestleyCroup_SCORE { get; set; }

        public WestleyCroup()
        {

        }

        public WestleyCroup(string _input)
        {
            initchiso("T_B25");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqWestleyCroup()
        {
            WestleyCroup_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                WestleyCroup_SCORE += i.diemketqua;
            }

            return WestleyCroup_SCORE;
        }

        public List<string> kqWestleyCroup_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, WestleyCroup_SCORE);
            return kq;
        }
    }
    public class CMMLMayoClinic : Thangdiem //T_B26
    {
        public double CMMLMayoClinic_SCORE { get; set; }

        public CMMLMayoClinic()
        {

        }

        public CMMLMayoClinic(string _input)
        {
            initchiso("T_B26");
            initTongdiem(_input);
        }
        public void xulibien()
        {
            double WBC = DStinhdiem[0].giatri;
            double tyleMONO = DStinhdiem[1].giatri;

            double slMONO = WBC * tyleMONO / 100;
            if (slMONO > 10000)
                DStinhdiem[5].thutunhap = 1;
            else
                DStinhdiem[5].thutunhap = 0;
        }

        public double kqCMMLMayoClinic()
        {
            xulibien();
            tinhTongdiem();

            CMMLMayoClinic_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                CMMLMayoClinic_SCORE += i.diemketqua;
            }

            return CMMLMayoClinic_SCORE;
        }

        public List<string> kqCMMLMayoClinic_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, CMMLMayoClinic_SCORE);
            return kq;
        }
    }
    public class EUTOS : Thangdiem //T_B27
    {
        public double EUTOS_SCORE { get; set; }

        public EUTOS()
        {

        }

        public EUTOS(string _input)
        {
            initchiso("T_B27");
            initTongdiem(_input);
        }

        public double kqEUTOS()
        {
            double tuoi = DStinhdiem[0].giatri;
            double kichthuoclach = DStinhdiem[1].giatri;
            double tyleblast = DStinhdiem[2].giatri;
            double plt = DStinhdiem[3].giatri;

            EUTOS_SCORE = 0.0025 * Math.Pow((tuoi / 10), 3) + (0.0615 * kichthuoclach) +
                (0.1052 * tyleblast) + (0.4104 * Math.Pow((plt / 1000), -0.5));

            return EUTOS_SCORE;
        }

        public List<string> kqEUTOS_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, EUTOS_SCORE);
            return kq;
        }
    }
    public class PASRuotthua : Thangdiem //T_B28
    {
        public double PASRuotthua_SCORE { get; set; }

        public PASRuotthua()
        {

        }

        public PASRuotthua(string _input)
        {
            initchiso("T_B28");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqPASRuotthua()
        {
            PASRuotthua_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                PASRuotthua_SCORE += i.diemketqua;
            }

            return PASRuotthua_SCORE;
        }

        public List<string> kqPASRuotthua_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, PASRuotthua_SCORE);
            return kq;
        }
    }
    public class GlasgowNhiB2 : Thangdiem //T_B29
    {
        public double GlasgowNhiB2_SCORE { get; set; }

        public GlasgowNhiB2()
        {

        }

        public GlasgowNhiB2(string _input)
        {
            initchiso("T_B29");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqGlasgowNhiB2()
        {
            GlasgowNhiB2_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                GlasgowNhiB2_SCORE += i.diemketqua;
            }

            return GlasgowNhiB2_SCORE;
        }

        public List<string> kqGlasgowNhiB2_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, GlasgowNhiB2_SCORE);
            return kq;
        }
    }
    public class GlasgowNhiO2 : Thangdiem //T_B32 (tach tu T_B29)
    {
        public double GlasgowNhiO2_SCORE { get; set; }

        public GlasgowNhiO2()
        {

        }

        public GlasgowNhiO2(string _input)
        {
            initchiso("T_B32");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqGlasgowNhiO2()
        {
            GlasgowNhiO2_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                GlasgowNhiO2_SCORE += i.diemketqua;
            }

            return GlasgowNhiO2_SCORE;
        }

        public List<string> kqGlasgowNhiO2_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, GlasgowNhiO2_SCORE);
            return kq;
        }
    }
    public class STOPBangS : Thangdiem //T_B30
    {
        public double STOP_SCORE { get; set; }
        public double BANG_SCORE { get; set; }
        public double Total_SCORE { get; set; }
        public STOPBangS()
        {

        }

        public STOPBangS(string _input)
        {
            initchiso("T_B30");
            initTongdiem(_input);
            tinhTongdiem();

            STOP_SCORE = DStinhdiem[0].diemketqua + DStinhdiem[1].diemketqua +
                DStinhdiem[2].diemketqua + DStinhdiem[3].diemketqua;

            BANG_SCORE = DStinhdiem[4].diemketqua + DStinhdiem[5].diemketqua +
                DStinhdiem[6].diemketqua + DStinhdiem[7].diemketqua;
            Total_SCORE = STOP_SCORE + BANG_SCORE;
        }

        public double kqSTOP()
        {
            return STOP_SCORE;
        }
        public double kqBang()
        {
            return BANG_SCORE;
        }
        public List<string> kqSTOPBangS_diengiai()
        {
            List<string> kq = new List<string>();
            if (STOP_SCORE >= 2)
            {
                if (BANG_SCORE - DStinhdiem[6].diemketqua > 0)
                    kq = db.GetDiengiaiKQ(IDChiso, 8);
                else
                    kq = db.GetDiengiaiKQ(IDChiso, Total_SCORE);
            }
            else
                kq = db.GetDiengiaiKQ(IDChiso, Total_SCORE);

            return kq;
        }
    }
    public class IPSSRLoansantuy : Thangdiem //T_B31
    {
        public double IPSSRLoansantuy_SCORE { get; set; }

        public IPSSRLoansantuy()
        {

        }

        public IPSSRLoansantuy(string _input)
        {
            initchiso("T_B31");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqIPSSRLoansantuy()
        {
            IPSSRLoansantuy_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                IPSSRLoansantuy_SCORE += i.diemketqua;
            }

            return IPSSRLoansantuy_SCORE;
        }

        public List<string> kqIPSSRLoansantuy_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, IPSSRLoansantuy_SCORE);
            return kq;
        }
    }
    #endregion
    #region T_C
    public class FraminghamE : Thangdiem //T_C01
    {
        public double FraminghamE_SCORE { get; set; }

        public FraminghamE()
        {

        }

        public FraminghamE(string _input)
        {
            initchiso("T_C01");
            initTongdiem(_input);
        }
        public void xulybien()
        {
            string gioitinh = (DStinhdiem[0].thutunhap == 1) ? "nam" : "nữ";
            double tuoi = DStinhdiem[1].giatri;
            double totalCholesterol = DStinhdiem[2].giatri;
            double HDL = DStinhdiem[3].giatri;
            double HATThu = DStinhdiem[4].giatri;
            bool THA_dieutri = DStinhdiem[5].thutunhap == 1;
            bool DTD_dieutri = DStinhdiem[6].thutunhap == 1;
            bool hutthuoc = DStinhdiem[7].thutunhap == 1;

            double hesoTHA;

            if (gioitinh == "nam")
            {
                hesoTHA = THA_dieutri ? 1.99881 : 1.93303;
                FraminghamE_SCORE = Math.Log(tuoi) * 3.06117 +
                               Math.Log(totalCholesterol) * 1.12370 -
                               Math.Log(HDL) * 0.93263 +
                               Math.Log(HATThu) * hesoTHA +
                               (hutthuoc ? 0.65451 : 0) +
                               (DTD_dieutri ? 0.57367 : 0) -
                               23.9802;
            }
            else
            {
                hesoTHA = THA_dieutri ? 2.82263 : 2.76157;
                FraminghamE_SCORE = Math.Log(tuoi) * 2.32888 +
                    Math.Log(totalCholesterol) * 1.20904 -
                    Math.Log(HDL) * 0.70833 +
                    Math.Log(HATThu) * hesoTHA +
                    (hutthuoc ? 0.52873 : 0) +
                    (DTD_dieutri ? 0.69154 : 0) -
                    26.1931;
            }
        }
        public double kqFraminghamE()
        {
            xulybien();

            return FraminghamE_SCORE;
        }

        public List<string> kqFraminghamE_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, FraminghamE_SCORE);
            kq.Add(FraminghamE_SCORE.ToString() + "%");
            return kq;
        }
    }
    public class ACCAHA : Thangdiem //T_C02
    {
        public double hesonguyco { get; set; }
        public double ACCAHA_SCORE { get; set; }
        public List<double> data;

        public ACCAHA()
        {

        }

        public ACCAHA(string _input)
        {
            initchiso("T_C02");
            initTongdiem(_input);

            data = new List<double>() { 12.344, 0, 11.853, -2.664, -7.99, 1.769, 1.797, 0, 1.764, 0, 7.837, -1.795, 0.658, 0.9144, 61.18, 2.469, 0, 0.302, 0, -0.307, 0, 1.916, 0, 1.809, 0, 0.549, 0, 0.645, 0.8954, 19.54, -29.799, 4.884, 13.54, -3.114, -13.578, 3.149, 2.019, 0, 1.957, 0, 7.574, -1.665, 0.661, 0.9665, -29.18, 17.114, 0, 0.94, 0, -18.92, 4.475, 29.291, -6.432, 27.82, -6.087, 0.691, 0, 0.874, 0.9533, 86.61 };

        }
        public void xulybien()
        {
            bool chungtocdaden = DStinhdiem[0].thutunhap == 2;
            string gioitinh = (DStinhdiem[1].thutunhap == 1) ? "nam" : "nữ";
            double tuoi = DStinhdiem[2].giatri;
            double totalCholesterol = DStinhdiem[3].giatri;
            double HDL = DStinhdiem[4].giatri;
            double HATThu = DStinhdiem[5].giatri;
            bool THA_dieutri = DStinhdiem[6].thutunhap == 1;
            bool DTD_dieutri = DStinhdiem[7].thutunhap == 1;
            bool hutthuoc = DStinhdiem[8].thutunhap == 1;

            int startindex = 0;
            if (gioitinh != "nam")
                startindex += 30;
            if (chungtocdaden)
                startindex += 15;

            List<double> datasudung = data.GetRange(startindex, 15);

            double hesoTHA = THA_dieutri ? datasudung[6] * Math.Log(HATThu) + datasudung[7] * Math.Log(HATThu) * Math.Log(tuoi) :
                datasudung[8] * Math.Log(HATThu) + datasudung[9] * Math.Log(HATThu) * Math.Log(tuoi);
            double hesohutthuoc = hutthuoc ? (datasudung[10] + datasudung[11] * Math.Log(tuoi)) : 0;
            double hesoDTD = DTD_dieutri ? (datasudung[12]) : 0;

            hesonguyco = datasudung[0] * Math.Log(tuoi) + datasudung[1] * Math.Log(tuoi) * Math.Log(tuoi) +
                datasudung[2] * Math.Log(totalCholesterol) + datasudung[3] * Math.Log(totalCholesterol) * Math.Log(tuoi) +
                datasudung[4] * Math.Log(HDL) + datasudung[5] * Math.Log(HDL) * Math.Log(tuoi) +
                hesoTHA + hesohutthuoc + hesoDTD;

            ACCAHA_SCORE = 100 * (1 - Math.Pow(datasudung[13], Math.Exp(hesonguyco - datasudung[14])));
        }
        public double kqACCAHA()
        {
            xulybien();
            return ACCAHA_SCORE;
        }

        public List<string> kqACCAHA_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, ACCAHA_SCORE);
            return kq;
        }
    }
    public class CHA2DS2VASc : Thangdiem //T_C03
    {
        public double CHA2DS2VASc_SCORE { get; set; }

        public CHA2DS2VASc()
        {

        }

        public CHA2DS2VASc(string _input)
        {
            initchiso("T_C03");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqCHA2DS2VASc()
        {
            CHA2DS2VASc_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                CHA2DS2VASc_SCORE += i.diemketqua;
            }

            return CHA2DS2VASc_SCORE;
        }

        public List<string> kqCHA2DS2VASc_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, CHA2DS2VASc_SCORE);
            return kq;
        }
    }
    public class TIMINonST : Thangdiem //T_C04
    {
        public double TIMINonST_SCORE { get; set; }

        public TIMINonST()
        {

        }

        public TIMINonST(string _input)
        {
            initchiso("T_C04");
            initTongdiem(_input);
        }
        public void xulybien()
        {
            int benhmachvanh = 0;
            for (int i = 1; i < 6; i++)
            {
                benhmachvanh += (DStinhdiem[i].thutunhap == 1) ? 1 : 0;
            }
            if (benhmachvanh >= 3)
                DStinhdiem[6].thutunhap = 1;
            else
                DStinhdiem[6].thutunhap = 2;
        }
        public double kqTIMINonST()
        {
            xulybien();
            tinhTongdiem();
            TIMINonST_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                TIMINonST_SCORE += i.diemketqua;
            }

            return TIMINonST_SCORE;
        }

        public List<string> kqTIMINonST_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, TIMINonST_SCORE);
            return kq;
        }
    }
    public class TIMIST : Thangdiem //T_C29 (tach tu T_C04)
    {
        public double TIMIST_SCORE { get; set; }

        public TIMIST()
        {

        }

        public TIMIST(string _input)
        {
            initchiso("T_C29");
            initTongdiem(_input);
        }
        public void xulybien()
        {
            bool benhkem = (DStinhdiem[1].thutunhap == 1) ||
                (DStinhdiem[2].thutunhap == 1) ||
                (DStinhdiem[3].thutunhap == 1);
            if (benhkem)
                DStinhdiem[8].thutunhap = 1;
            else
                DStinhdiem[8].thutunhap = 2;
        }
        public double kqTIMIST()
        {
            xulybien();
            tinhTongdiem();
            TIMIST_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                TIMIST_SCORE += i.diemketqua;
            }

            return TIMIST_SCORE;
        }

        public List<string> kqTIMIST_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, TIMIST_SCORE);
            return kq;
        }
    }
    public class ARISCAT : Thangdiem //T_C05
    {
        public double ARISCAT_SCORE { get; set; }

        public ARISCAT()
        {

        }

        public ARISCAT(string _input)
        {
            initchiso("T_C05");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqARISCAT()
        {
            ARISCAT_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                ARISCAT_SCORE += i.diemketqua;
            }

            return ARISCAT_SCORE;
        }

        public List<string> kqARISCAT_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, ARISCAT_SCORE);
            return kq;
        }
    }
    public class IPSSTienliet : Thangdiem //T_C06
    {
        public double IPSSTienliet_SCORE { get; set; }

        public IPSSTienliet()
        {

        }

        public IPSSTienliet(string _input)
        {
            initchiso("T_C06");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqIPSSTienliet()
        {
            IPSSTienliet_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                IPSSTienliet_SCORE += i.diemketqua;
            }

            return IPSSTienliet_SCORE;
        }

        public List<string> kqIPSSTienliet_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, IPSSTienliet_SCORE);
            return kq;
        }
    }
    public class ABCD2 : Thangdiem //T_C07
    {
        public double ABCD2_SCORE { get; set; }

        public ABCD2()
        {

        }

        public ABCD2(string _input)
        {
            initchiso("T_C07");
            initTongdiem(_input);
            tinhTongdiem();
        }
        public void xulydiem()
        {
            if (DStinhdiem[1].diemketqua + DStinhdiem[2].diemketqua > 0)
                DStinhdiem[2].diemketqua = 0;
        }
        public double kqABCD2()
        {
            ABCD2_SCORE = 0;
            xulydiem();

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                ABCD2_SCORE += i.diemketqua;
            }

            return ABCD2_SCORE;
        }

        public List<string> kqABCD2_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, ABCD2_SCORE);
            return kq;
        }
    }
    public class ESS : Thangdiem //T_C08
    {
        public double ESS_SCORE { get; set; }

        public ESS()
        {

        }

        public ESS(string _input)
        {
            initchiso("T_C08");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqESS()
        {
            ESS_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                ESS_SCORE += i.diemketqua;
            }

            return ESS_SCORE;
        }

        public List<string> kqESS_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, ESS_SCORE);
            return kq;
        }
    }
    public class NIH : Thangdiem //T_C09
    {
        public double NIH_SCORE { get; set; }

        public NIH()
        {

        }

        public NIH(string _input)
        {
            initchiso("T_C09");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqNIH()
        {
            NIH_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                NIH_SCORE += i.diemketqua;
            }

            return NIH_SCORE;
        }

        public List<string> kqNIH_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, NIH_SCORE);
            return kq;
        }
    }
    public class RoPE : Thangdiem //T_C10
    {
        public double RoPE_SCORE { get; set; }

        public RoPE()
        {

        }

        public RoPE(string _input)
        {
            initchiso("T_C10");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqRoPE()
        {
            RoPE_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                RoPE_SCORE += i.diemketqua;
            }

            return RoPE_SCORE;
        }

        public List<string> kqRoPE_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, RoPE_SCORE);
            return kq;
        }
    }
    public class FraminghamS : Thangdiem //T_C11
    {
        public double FraminghamS_SCORE { get; set; }

        public FraminghamS()
        {

        }

        public FraminghamS(string _input)
        {
            initchiso("T_C11");
            initTongdiem(_input);
        }
        public void xulybien()
        {
            double tuoi = DStinhdiem[0].giatri;
            double HATThu = DStinhdiem[1].giatri;
            bool THA_dieutri = DStinhdiem[2].thutunhap == 1;
            bool DTD_dieutri = DStinhdiem[3].thutunhap == 1;
            bool hutthuoc = DStinhdiem[4].thutunhap == 1;
            bool benhtimmach = DStinhdiem[5].thutunhap == 1;
            bool rungnhi = DStinhdiem[6].thutunhap == 1;
            bool phidaithattrai = DStinhdiem[7].thutunhap == 1;
            string gioitinh = (DStinhdiem[8].thutunhap == 1) ? "nam" : "nữ";
            int thoigiandanhgia = (DStinhdiem[9].thutunhap == 1) ? 1 : (DStinhdiem[9].thutunhap == 5) ? 5 : 10;

            double tuoiF, HATThuF, THA_F, DTD_F, hutthuocF, benhtimmachF, rungnhiF, phidaithattraiF, thoigiandanhgiaF;

            if (gioitinh == "nam")
            {
                tuoiF = tuoi * 0.0505;
                HATThuF = HATThu * 0.014;
                THA_F = 0.3263;
                DTD_F = 0.3384;
                hutthuocF = 0.5147;
                benhtimmachF = 0.5195;
                rungnhiF = 0.6061;
                phidaithattraiF = 0.8415;
                if (thoigiandanhgia == 1)
                    thoigiandanhgiaF = 0.9948;
                else if (thoigiandanhgia == 5)
                    thoigiandanhgiaF = 0.9642;
                else
                    thoigiandanhgiaF = 0.9044;
            }
            else
            {
                tuoiF = tuoi * 0.0657;
                HATThuF = HATThu * 0.0197;
                THA_F = 2.5432 - HATThu * 0.0134;
                DTD_F = 0.5442;
                hutthuocF = 0.5294;
                benhtimmachF = 0.4326;
                rungnhiF = 1.1497;
                phidaithattraiF = 0.8488;
                if (thoigiandanhgia == 1)
                    thoigiandanhgiaF = 0.9977;
                else if (thoigiandanhgia == 5)
                    thoigiandanhgiaF = 0.9741;
                else
                    thoigiandanhgiaF = 0.9353;
            }

            double total = tuoiF + HATThuF + THA_F + DTD_F + hutthuocF + benhtimmachF + rungnhiF + phidaithattraiF;

            FraminghamS_SCORE = (gioitinh == "nam") ? 100 * (1 - Math.Pow(thoigiandanhgiaF, Math.Exp(total - 5.677))) :
                 (1 - Math.Pow(thoigiandanhgiaF, Math.Exp(total - 7.5766)));
        }
        public double kqFraminghamS()
        {
            xulybien();
            return FraminghamS_SCORE;
        }

        public List<string> kqFraminghamS_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, FraminghamS_SCORE);
            return kq;
        }
    }
    public class GAD7 : Thangdiem //T_C12
    {
        public double GAD7_SCORE { get; set; }

        public GAD7()
        {

        }

        public GAD7(string _input)
        {
            initchiso("T_C12");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqGAD7()
        {
            GAD7_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                GAD7_SCORE += i.diemketqua;
            }

            return GAD7_SCORE;
        }

        public List<string> kqGAD7_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, GAD7_SCORE);
            return kq;
        }
    }
    public class PHQ9 : Thangdiem //T_C13
    {
        public double PHQ9_SCORE { get; set; }

        public PHQ9()
        {

        }

        public PHQ9(string _input)
        {
            initchiso("T_C13");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqPHQ9()
        {
            PHQ9_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                PHQ9_SCORE += i.diemketqua;
            }

            return PHQ9_SCORE;
        }

        public List<string> kqPHQ9_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, PHQ9_SCORE);
            return kq;
        }
    }
    public class Caprini : Thangdiem //T_C14
    {
        public double Caprini_SCORE { get; set; }

        public Caprini()
        {

        }

        public Caprini(string _input)
        {
            initchiso("T_C14");
            initTongdiem(_input);
        }
        public void xulybien()
        {
            if (DStinhdiem[2].thutunhap == 1)
            {
                DStinhdiem[17].thutunhap = 2;
                DStinhdiem[18].thutunhap = 2;
                DStinhdiem[19].thutunhap = 2;
            }

        }
        public double kqCaprini()
        {
            xulybien();
            tinhTongdiem();
            Caprini_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Caprini_SCORE += i.diemketqua;
            }

            double sobenhphoiF = DStinhdiem[29].giatri * 1;
            double sobenhhiemngheoF = DStinhdiem[30].giatri * 2;
            double sodotdongmauF = DStinhdiem[31].giatri * 3;
            double xetnghiemdongmaugdF = DStinhdiem[32].giatri * 3;
            double sokhopthaytheF = DStinhdiem[33].giatri * 5;

            Caprini_SCORE = Caprini_SCORE + sobenhphoiF + sobenhhiemngheoF + sodotdongmauF + xetnghiemdongmaugdF + sokhopthaytheF;

            return Caprini_SCORE;
        }

        public List<string> kqCaprini_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Caprini_SCORE);
            return kq;
        }
    }
    public class Eckardt : Thangdiem //T_C15
    {
        public double Eckardt_SCORE { get; set; }

        public Eckardt()
        {

        }

        public Eckardt(string _input)
        {
            initchiso("T_C15");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqEckardt()
        {
            Eckardt_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Eckardt_SCORE += i.diemketqua;
            }

            return Eckardt_SCORE;
        }

        public List<string> kqEckardt_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Eckardt_SCORE);
            return kq;
        }
    }
    public class LAR : Thangdiem //T_C16
    {
        public double LAR_SCORE { get; set; }

        public LAR()
        {

        }

        public LAR(string _input)
        {
            initchiso("T_C16");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqLAR()
        {
            LAR_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                LAR_SCORE += i.diemketqua;
            }

            return LAR_SCORE;
        }

        public List<string> kqLAR_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, LAR_SCORE);
            return kq;
        }
    }
    public class MESS : Thangdiem //T_C17
    {
        public double MESS_SCORE { get; set; }

        public MESS()
        {

        }

        public MESS(string _input)
        {
            initchiso("T_C17");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqMESS()
        {
            MESS_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                MESS_SCORE += i.diemketqua;
            }

            return MESS_SCORE;
        }

        public List<string> kqMESS_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, MESS_SCORE);
            return kq;
        }
    }
    public class Braden : Thangdiem //T_C18
    {
        public double Braden_SCORE { get; set; }

        public Braden()
        {

        }

        public Braden(string _input)
        {
            initchiso("T_C18");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqBraden()
        {
            Braden_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Braden_SCORE += i.diemketqua;
            }

            return Braden_SCORE;
        }

        public List<string> kqBraden_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Braden_SCORE);
            return kq;
        }
    }
    public class VSD_Obs : Thangdiem //T_C19
    {
        public double VSD_Obs_SCORE { get; set; }

        public VSD_Obs()
        {

        }

        public VSD_Obs(string _input)
        {
            initchiso("T_C19");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqVSD_Obs()
        {
            VSD_Obs_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                VSD_Obs_SCORE += i.diemketqua;
            }

            return VSD_Obs_SCORE;
        }

        public List<string> kqVSD_Obs_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, VSD_Obs_SCORE);
            return kq;
        }
    }
    public class VSD_Ref : Thangdiem //T_C30 tach tu T_C19
    {
        public double VSD_Ref_SCORE { get; set; }

        public VSD_Ref()
        {

        }

        public VSD_Ref(string _input)
        {
            initchiso("T_C30");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqVSD_Ref()
        {
            VSD_Ref_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                VSD_Ref_SCORE += i.diemketqua;
            }

            return VSD_Ref_SCORE;
        }

        public List<string> kqVSD_Ref_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, VSD_Ref_SCORE);
            return kq;
        }
    }
    public class Villalta : Thangdiem //T_C20
    {
        public double Villalta_SCORE { get; set; }

        public Villalta()
        {

        }

        public Villalta(string _input)
        {
            initchiso("T_C20");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqVillalta()
        {
            Villalta_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                Villalta_SCORE += i.diemketqua;
            }

            return Villalta_SCORE;
        }

        public List<string> kqVillalta_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, Villalta_SCORE);
            return kq;
        }
    }
    public class RA_CDAI : Thangdiem //T_C21
    {
        public double RA_CDAI_SCORE { get; set; }

        public RA_CDAI()
        {

        }

        public RA_CDAI(string _input)
        {
            initchiso("T_C21");
            initTongdiem(_input);
        }
        public double kqRA_CDAI()
        {
            RA_CDAI_SCORE = DStinhdiem[0].giatri + DStinhdiem[1].giatri +
                DStinhdiem[58].giatri + DStinhdiem[59].giatri;

            return RA_CDAI_SCORE;
        }

        public List<string> kqRA_CDAI_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, RA_CDAI_SCORE);
            return kq;
        }
    }
    public class RA_SDAI : Thangdiem //T_C22
    {
        public double RA_SDAI_SCORE { get; set; }

        public RA_SDAI()
        {

        }

        public RA_SDAI(string _input)
        {
            initchiso("T_C22");
            initTongdiem(_input);
        }

        public double kqRA_SDAI()
        {
            RA_SDAI_SCORE = DStinhdiem[0].giatri + DStinhdiem[1].giatri +
                Math.Min(DStinhdiem[58].giatri, 10) +
                DStinhdiem[59].giatri + DStinhdiem[60].giatri;

            return RA_SDAI_SCORE;
        }

        public List<string> kqRA_SDAI_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, RA_SDAI_SCORE);
            return kq;
        }
    }
    public class DAS28CRP : Thangdiem //T_C23
    {
        public double DAS28CRP_SCORE { get; set; }

        public DAS28CRP()
        {

        }

        public DAS28CRP(string _input)
        {
            initchiso("T_C23");
            initTongdiem(_input);
        }

        public double kqDAS28CRP()
        {
            DAS28CRP_SCORE = DStinhdiem[0].giatri +
                Math.Min(DStinhdiem[57].giatri, 10) +
                DStinhdiem[58].giatri + DStinhdiem[59].giatri;

            return DAS28CRP_SCORE;
        }

        public List<string> kqDAS28CRP_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, DAS28CRP_SCORE);
            return kq;
        }
    }
    public class DAS28ESR : Thangdiem //T_C24
    {
        public double DAS28ESR_SCORE { get; set; }

        public DAS28ESR()
        {

        }

        public DAS28ESR(string _input)
        {
            initchiso("T_C24");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqDAS28ESR()
        {
            DAS28ESR_SCORE = DStinhdiem[0].giatri +
                Math.Min(DStinhdiem[57].giatri, 10) +
                DStinhdiem[58].giatri + DStinhdiem[59].giatri;

            return DAS28ESR_SCORE;
        }

        public List<string> kqDAS28ESR_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, DAS28ESR_SCORE);
            return kq;
        }
    }
    public class ISI : Thangdiem //T_C25
    {
        public double ISI_SCORE { get; set; }

        public ISI()
        {

        }

        public ISI(string _input)
        {
            initchiso("T_C25");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqISI()
        {
            ISI_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                ISI_SCORE += i.diemketqua;
            }

            return ISI_SCORE;
        }

        public List<string> kqISI_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, ISI_SCORE);
            return kq;
        }
    }
    public class SCORE2 : Thangdiem //T_C26
    {
        public string gioitinh { get; set; }
        public int nhomgioitinh { get; set; }
        public double tuoi { get; set; }
        public int nhomtuoi { get; set; }
        public bool smoking { get; set; }
        public int nhomSmoking { get; set; }
        public double HATT { get; set; }
        public int nhomHATT { get; set; }
        public double TotalCholesterol { get; set; }
        public double HDL { get; set; }
        public int nhomNonHDL { get; set; }
        public string vungnguyco { get; set; }
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
            tuoi = NB.tinhtuoi_nam();
            gioitinh = NB.gioitinh;
            smoking = NB.hutthuoc;
            HATT = NB.HATThu;
            HDL = XN.HDL;
            TotalCholesterol = XN.totalCholesterol;

            checkgioitinh(NB.gioitinh);
            checktuoi(NB.tinhtuoi_nam());
            checkSmoking(NB.hutthuoc);
            checkHATT(NB.HATThu);
            checkNonHDL(XN.HDL, XN.totalCholesterol);
        }
        public SCORE2(double _tuoi, string _gioitinh, bool _smoking, double _TotalCholesterol,
            double _HDL, double _HATT, string _vungnguyco)
        {
            init_SCORE2();
            gioitinh = _gioitinh.ToLower();
            checkgioitinh(_gioitinh.ToLower());
            tuoi = _tuoi;
            checktuoi(_tuoi);
            smoking = _smoking;
            checkSmoking(_smoking);
            HATT = _HATT;
            checkHATT(_HATT);
            TotalCholesterol = _TotalCholesterol;
            HDL = _HDL;
            checkNonHDL(_HDL, _TotalCholesterol);
            vungnguyco = _vungnguyco;
            checkvungnguyco(_vungnguyco);
        }
        private void init_SCORE2()
        {
            //Vùng nguy cơ: 640; Giới tính: 320; Hút thuốc: 160; Tuổi: 16; NonHDL: 4; HATT: 1
            int[] _diem = { 1, 1, 1, 2, 1, 1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 3, 1, 1, 2, 2, 1, 2, 2, 3, 1, 2, 2, 3, 1, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 4, 2, 2, 3, 4, 2, 3, 3, 4, 2, 3, 3, 4, 2, 3, 4, 5, 3, 3, 4, 5, 3, 3, 4, 5, 3, 4, 5, 6, 3, 4, 5, 6, 4, 4, 5, 7, 4, 5, 6, 7, 5, 5, 7, 8, 5, 6, 7, 8, 5, 6, 7, 9, 5, 6, 7, 9, 6, 7, 9, 10, 6, 7, 9, 11, 6, 8, 10, 12, 7, 8, 10, 12, 9, 11, 13, 15, 10, 11, 13, 15, 10, 12, 14, 16, 11, 13, 15, 17, 15, 16, 18, 20, 15, 17, 19, 21, 16, 18, 20, 22, 17, 19, 21, 23, 23, 24, 26, 28, 24, 25, 27, 29, 25, 26, 28, 30, 26, 27, 29, 31, 2, 2, 3, 4, 2, 3, 3, 4, 2, 3, 4, 5, 2, 3, 4, 6, 2, 3, 4, 5, 2, 3, 4, 5, 3, 4, 5, 6, 3, 4, 5, 7, 3, 4, 5, 6, 3, 4, 5, 7, 4, 5, 6, 7, 4, 5, 6, 8, 4, 5, 6, 8, 4, 5, 7, 8, 5, 6, 7, 9, 5, 6, 8, 10, 5, 6, 8, 10, 6, 7, 8, 10, 6, 7, 9, 11, 6, 8, 9, 11, 7, 8, 10, 12, 7, 9, 10, 12, 7, 9, 11, 13, 8, 9, 11, 13, 9, 11, 14, 17, 10, 12, 15, 18, 10, 13, 15, 19, 11, 14, 16, 20, 13, 15, 18, 21, 14, 16, 19, 22, 15, 17, 20, 23, 15, 18, 21, 24, 18, 20, 23, 25, 19, 21, 24, 26, 20, 22, 25, 28, 21, 23, 26, 29, 25, 27, 29, 31, 26, 28, 30, 32, 27, 29, 31, 33, 28, 30, 32, 34, 1, 2, 2, 3, 2, 2, 3, 4, 2, 3, 3, 5, 2, 3, 4, 5, 2, 2, 3, 4, 2, 3, 4, 5, 3, 3, 4, 6, 3, 4, 5, 6, 3, 3, 4, 5, 3, 4, 5, 6, 3, 4, 5, 7, 4, 5, 6, 8, 4, 4, 5, 7, 4, 5, 6, 7, 4, 5, 7, 8, 5, 6, 8, 9, 5, 6, 7, 8, 5, 6, 8, 9, 6, 7, 8, 10, 6, 8, 9, 11, 6, 8, 9, 11, 7, 8, 10, 12, 7, 9, 11, 12, 8, 10, 11, 13, 8, 10, 12, 15, 8, 11, 13, 16, 9, 12, 14, 18, 10, 13, 16, 19, 12, 14, 16, 19, 13, 15, 18, 21, 15, 18, 21, 24, 17, 20, 23, 27, 17, 19, 21, 23, 20, 22, 25, 27, 24, 26, 29, 32, 28, 31, 34, 37, 25, 26, 28, 29, 30, 32, 33, 35, 36, 38, 40, 42, 43, 45, 47, 49, 3, 3, 5, 6, 3, 4, 5, 7, 4, 5, 6, 8, 5, 6, 8, 10, 3, 4, 6, 7, 4, 5, 7, 8, 5, 6, 8, 10, 5, 7, 9, 11, 4, 6, 7, 9, 5, 6, 8, 10, 6, 7, 9, 11, 7, 8, 10, 13, 6, 7, 9, 10, 6, 8, 10, 12, 7, 9, 11, 13, 8, 10, 12, 15, 7, 9, 10, 13, 8, 10, 11, 14, 9, 10, 13, 15, 10, 11, 14, 17, 9, 11, 13, 15, 10, 12, 14, 16, 11, 13, 15, 17, 11, 13, 16, 19, 12, 14, 18, 22, 13, 16, 19, 24, 14, 17, 21, 26, 15, 19, 23, 28, 15, 18, 21, 24, 17, 20, 23, 27, 19, 23, 26, 31, 22, 26, 30, 34, 19, 22, 24, 26, 23, 25, 28, 31, 27, 30, 33, 36, 31, 34, 38, 41, 25, 26, 27, 29, 30, 32, 33, 35, 36, 38, 40, 42, 43, 45, 47, 49, 1, 1, 1, 2, 1, 1, 2, 2, 1, 1, 2, 3, 1, 2, 2, 3, 1, 2, 2, 3, 1, 2, 2, 3, 1, 2, 3, 3, 2, 2, 3, 4, 2, 2, 3, 4, 2, 2, 3, 4, 2, 3, 4, 5, 2, 3, 4, 5, 3, 3, 4, 5, 3, 3, 4, 6, 3, 4, 5, 6, 3, 4, 5, 7, 4, 5, 6, 7, 4, 5, 6, 8, 4, 5, 7, 8, 5, 6, 7, 9, 5, 7, 8, 10, 6, 7, 9, 10, 6, 7, 9, 11, 6, 8, 9, 12, 7, 9, 11, 13, 7, 9, 11, 14, 8, 10, 12, 15, 8, 11, 13, 16, 12, 14, 16, 19, 12, 15, 17, 20, 13, 15, 18, 21, 14, 16, 19, 23, 19, 21, 24, 27, 20, 22, 25, 28, 21, 24, 27, 30, 22, 25, 28, 31, 30, 32, 35, 37, 32, 34, 36, 39, 33, 35, 38, 40, 34, 37, 39, 42, 2, 3, 3, 5, 2, 3, 4, 5, 2, 3, 5, 6, 3, 4, 5, 7, 3, 3, 5, 6, 3, 4, 5, 7, 3, 4, 6, 8, 4, 5, 6, 9, 3, 5, 6, 8, 4, 5, 6, 8, 4, 6, 7, 9, 5, 6, 8, 10, 5, 6, 8, 10, 5, 7, 8, 11, 6, 7, 9, 11, 6, 8, 10, 12, 6, 8, 10, 12, 7, 8, 11, 13, 7, 9, 11, 14, 8, 10, 12, 15, 9, 10, 13, 15, 9, 11, 13, 16, 9, 12, 14, 17, 10, 12, 15, 18, 12, 15, 18, 22, 13, 16, 19, 23, 13, 17, 20, 25, 14, 18, 22, 26, 17, 20, 24, 27, 18, 21, 25, 29, 19, 22, 26, 30, 20, 24, 28, 32, 24, 27, 30, 34, 25, 28, 32, 35, 27, 30, 33, 37, 28, 31, 35, 39, 34, 36, 39, 41, 35, 38, 40, 43, 37, 39, 42, 44, 38, 41, 43, 46, 2, 2, 3, 4, 2, 3, 4, 5, 2, 3, 4, 6, 3, 4, 5, 7, 2, 3, 4, 5, 3, 4, 5, 6, 3, 4, 5, 7, 4, 5, 6, 8, 3, 4, 5, 7, 4, 5, 6, 8, 4, 5, 7, 9, 5, 6, 8, 10, 4, 5, 7, 9, 5, 6, 8, 10, 6, 7, 9, 11, 6, 8, 10, 12, 6, 7, 9, 11, 7, 8, 10, 12, 7, 9, 11, 13, 8, 10, 12, 15, 8, 10, 12, 14, 9, 11, 13, 15, 10, 12, 14, 17, 10, 13, 15, 18, 10, 12, 15, 19, 11, 13, 17, 21, 12, 15, 18, 23, 13, 16, 20, 25, 15, 17, 21, 24, 17, 20, 23, 27, 19, 23, 27, 31, 22, 26, 30, 35, 22, 25, 27, 30, 26, 29, 32, 35, 31, 34, 37, 41, 36, 40, 43, 47, 32, 34, 36, 37, 39, 41, 43, 45, 47, 49, 51, 53, 55, 57, 59, 62, 3, 4, 6, 8, 4, 5, 7, 9, 5, 6, 8, 11, 6, 8, 10, 13, 4, 5, 7, 9, 5, 7, 8, 11, 6, 8, 10, 13, 7, 9, 12, 15, 5, 7, 9, 11, 6, 8, 10, 13, 7, 9, 12, 15, 8, 11, 14, 17, 7, 9, 11, 14, 8, 10, 13, 16, 9, 11, 14, 17, 10, 13, 16, 20, 9, 11, 14, 17, 10, 13, 15, 18, 11, 14, 17, 20, 12, 15, 18, 22, 12, 14, 17, 20, 13, 15, 18, 22, 14, 17, 20, 23, 15, 18, 21, 25, 15, 19, 23, 28, 16, 20, 25, 31, 18, 22, 28, 34, 20, 24, 30, 36, 19, 23, 27, 31, 22, 26, 30, 35, 25, 29, 34, 39, 29, 33, 38, 44, 25, 28, 31, 34, 30, 33, 36, 40, 35, 38, 42, 46, 40, 44, 48, 53, 32, 34, 35, 37, 39, 41, 43, 45, 46, 48, 51, 53, 55, 57, 59, 61, 1, 1, 1, 2, 1, 1, 2, 3, 1, 1, 2, 3, 1, 2, 2, 4, 1, 2, 2, 3, 1, 2, 3, 4, 2, 2, 3, 4, 2, 2, 4, 5, 2, 3, 3, 5, 2, 3, 4, 5, 2, 3, 4, 6, 3, 4, 5, 7, 3, 4, 5, 7, 3, 4, 6, 8, 4, 5, 7, 9, 4, 5, 7, 10, 5, 6, 8, 11, 5, 7, 9, 11, 6, 7, 9, 12, 6, 8, 10, 13, 8, 10, 12, 15, 8, 10, 13, 16, 8, 11, 14, 17, 9, 11, 14, 18, 11, 14, 17, 21, 12, 15, 18, 22, 13, 16, 19, 24, 14, 17, 20, 25, 18, 22, 25, 29, 19, 23, 27, 31, 20, 24, 28, 32, 22, 25, 29, 34, 29, 32, 36, 40, 31, 34, 38, 42, 32, 36, 39, 44, 34, 37, 41, 45, 44, 47, 50, 53, 46, 49, 52, 55, 48, 51, 54, 57, 50, 52, 55, 58, 2, 3, 4, 6, 2, 4, 5, 7, 3, 4, 6, 9, 3, 5, 7, 10, 3, 4, 6, 8, 3, 5, 7, 10, 4, 6, 8, 11, 5, 6, 9, 13, 4, 6, 8, 11, 5, 7, 9, 13, 6, 8, 10, 14, 6, 9, 12, 16, 6, 8, 11, 15, 7, 9, 12, 16, 8, 10, 14, 18, 8, 11, 15, 20, 9, 12, 15, 20, 10, 13, 16, 21, 11, 14, 18, 23, 11, 15, 19, 25, 13, 16, 21, 26, 14, 17, 22, 27, 14, 18, 23, 29, 15, 19, 24, 30, 19, 23, 28, 33, 20, 24, 29, 35, 21, 26, 31, 37, 22, 27, 33, 39, 26, 31, 35, 41, 28, 32, 37, 43, 29, 34, 39, 45, 31, 36, 41, 47, 36, 40, 44, 49, 38, 42, 46, 51, 40, 44, 48, 53, 41, 46, 50, 55, 49, 52, 55, 58, 51, 53, 56, 59, 52, 55, 58, 61, 54, 57, 60, 63, 1, 2, 3, 4, 2, 2, 3, 5, 2, 3, 4, 6, 3, 4, 5, 7, 2, 3, 4, 5, 2, 3, 5, 6, 3, 4, 6, 8, 4, 5, 7, 9, 3, 4, 5, 7, 3, 5, 6, 8, 4, 5, 7, 10, 5, 6, 9, 11, 4, 6, 7, 9, 5, 6, 8, 11, 6, 7, 10, 12, 7, 9, 11, 14, 6, 8, 10, 13, 7, 9, 11, 14, 8, 10, 13, 16, 9, 11, 14, 18, 9, 11, 14, 17, 10, 12, 15, 18, 11, 13, 16, 20, 12, 15, 18, 22, 12, 15, 19, 23, 14, 17, 20, 25, 15, 18, 22, 27, 16, 20, 24, 29, 18, 21, 24, 28, 20, 24, 27, 32, 23, 27, 31, 35, 26, 30, 34, 39, 26, 29, 31, 34, 30, 33, 36, 40, 35, 38, 42, 45, 40, 44, 47, 51, 36, 38, 40, 42, 43, 45, 47, 49, 51, 53, 55, 57, 58, 61, 63, 65, 3, 4, 6, 8, 4, 5, 7, 10, 5, 7, 9, 13, 6, 8, 11, 16, 4, 6, 8, 10, 5, 7, 9, 13, 6, 8, 11, 15, 7, 10, 14, 18, 6, 7, 10, 13, 7, 9, 12, 15, 8, 10, 14, 18, 9, 12, 16, 21, 8, 10, 13, 16, 9, 11, 15, 19, 10, 13, 17, 21, 12, 15, 19, 24, 10, 13, 16, 20, 12, 15, 18, 23, 13, 16, 20, 25, 15, 18, 23, 28, 14, 17, 21, 25, 15, 19, 23, 28, 17, 20, 25, 30, 18, 22, 27, 32, 18, 22, 27, 33, 20, 24, 29, 35, 22, 26, 32, 38, 23, 28, 34, 41, 23, 27, 31, 35, 26, 30, 34, 39, 29, 34, 38, 44, 33, 37, 43, 48, 29, 32, 35, 38, 34, 37, 40, 44, 39, 42, 46, 50, 44, 48, 52, 56, 36, 38, 40, 41, 43, 45, 47, 49, 50, 52, 54, 56, 58, 60, 62, 65, 2, 3, 4, 5, 2, 3, 4, 6, 2, 3, 5, 7, 3, 4, 6, 8, 3, 4, 5, 7, 3, 4, 6, 8, 4, 5, 7, 9, 4, 6, 8, 10, 4, 6, 8, 10, 5, 6, 9, 11, 5, 7, 9, 12, 6, 8, 11, 14, 7, 8, 11, 14, 7, 9, 12, 15, 8, 10, 13, 17, 9, 11, 14, 18, 10, 12, 16, 20, 11, 13, 17, 21, 11, 14, 18, 22, 12, 15, 19, 24, 15, 18, 22, 27, 16, 19, 23, 28, 16, 20, 24, 30, 17, 21, 26, 31, 26, 29, 33, 37, 27, 30, 34, 38, 28, 31, 35, 39, 29, 32, 36, 41, 34, 37, 41, 44, 35, 39, 42, 46, 36, 40, 43, 47, 37, 41, 45, 48, 44, 47, 50, 53, 45, 48, 51, 54, 47, 49, 52, 55, 48, 51, 54, 57, 56, 58, 60, 62, 57, 59, 61, 63, 58, 60, 62, 64, 60, 61, 63, 65, 5, 7, 9, 13, 6, 8, 11, 15, 6, 9, 12, 17, 7, 10, 14, 19, 7, 9, 12, 16, 8, 10, 14, 18, 9, 12, 15, 21, 10, 13, 17, 23, 9, 12, 16, 21, 10, 13, 18, 23, 11, 15, 19, 25, 13, 17, 22, 28, 13, 16, 21, 26, 14, 18, 23, 28, 15, 19, 24, 31, 16, 21, 26, 33, 17, 22, 27, 33, 18, 23, 29, 35, 20, 25, 30, 37, 21, 26, 32, 39, 23, 28, 34, 41, 24, 30, 36, 42, 26, 31, 37, 44, 27, 33, 39, 46, 34, 39, 43, 48, 36, 40, 44, 49, 37, 41, 46, 51, 38, 43, 47, 52, 42, 46, 49, 53, 43, 47, 51, 55, 44, 48, 52, 56, 46, 49, 53, 58, 50, 53, 56, 59, 51, 54, 57, 60, 53, 56, 59, 62, 54, 57, 60, 63, 59, 61, 63, 65, 60, 62, 64, 66, 61, 63, 65, 67, 63, 65, 66, 68, 3, 4, 5, 7, 4, 5, 6, 9, 4, 6, 8, 11, 5, 7, 10, 13, 4, 5, 7, 9, 5, 6, 8, 11, 6, 8, 10, 13, 7, 9, 12, 16, 6, 7, 10, 12, 7, 9, 11, 14, 8, 10, 13, 16, 9, 12, 15, 19, 8, 10, 13, 16, 9, 11, 14, 18, 10, 13, 16, 20, 12, 15, 18, 23, 11, 14, 17, 20, 12, 15, 19, 23, 14, 17, 20, 25, 15, 18, 22, 27, 15, 18, 22, 26, 17, 20, 24, 28, 18, 21, 26, 30, 19, 23, 27, 32, 25, 28, 32, 35, 26, 30, 33, 37, 28, 31, 35, 39, 29, 33, 36, 40, 31, 34, 37, 40, 33, 36, 39, 42, 36, 39, 42, 45, 38, 41, 44, 48, 38, 40, 42, 44, 41, 43, 46, 48, 45, 47, 49, 52, 48, 51, 53, 56, 46, 47, 48, 49, 50, 52, 53, 54, 55, 56, 58, 59, 60, 61, 63, 64, 6, 8, 11, 14, 7, 10, 13, 17, 9, 12, 16, 20, 11, 14, 19, 24, 8, 10, 13, 17, 9, 12, 16, 20, 11, 14, 18, 24, 13, 17, 22, 28, 10, 13, 17, 21, 12, 15, 19, 24, 14, 17, 22, 28, 16, 20, 25, 31, 13, 17, 21, 25, 15, 19, 23, 28, 17, 21, 26, 32, 19, 24, 29, 35, 17, 21, 25, 31, 19, 23, 28, 33, 21, 25, 31, 36, 23, 28, 33, 40, 22, 26, 31, 36, 24, 28, 33, 39, 26, 30, 36, 42, 28, 33, 38, 44, 31, 35, 39, 43, 33, 36, 41, 45, 34, 38, 42, 47, 36, 40, 44, 49, 36, 39, 42, 45, 38, 41, 44, 48, 41, 44, 47, 51, 43, 47, 50, 54, 40, 43, 45, 47, 44, 46, 49, 51, 48, 50, 52, 55, 51, 54, 56, 59, 46, 47, 48, 49, 50, 52, 53, 54, 55, 56, 58, 59, 60, 61, 63, 64 };
            diem = _diem;
            string[] _PLnguyco = { "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "T", "T", "T", "T", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "TB", "TB", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "T", "T", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "T", "T", "T", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "T", "T", "T", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "T", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "T", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "T", "TB", "TB", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "T", "T", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "C", "TB", "TB", "TB", "C", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "T", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "T", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "TB", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "TB", "C", "C", "TB", "TB", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "TB", "C", "C", "C", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C" };
            PLnguyco = _PLnguyco;
        }
        private void checkgioitinh(string _gioitinh)
        {
            if (_gioitinh.ToLower() == "nam")
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

            if (_vungnguyco.ToLower() == "thấp")
                nhomvungnguyco = 0;
            else if (_vungnguyco.ToLower() == "trung bình")
                nhomvungnguyco = 1;
            else if (_vungnguyco.ToLower() == "cao")
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
        public string kqSCORE2_diengiai()
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
    public class SCORE2_DM : Thangdiem //T_C27
    {
        public string gioitinh { get; set; }
        public double tuoi { get; set; }
        public int nhomtuoi { get; set; }
        public double DM_age { get; set; }
        public int nhomDM_Age { get; set; }
        public bool smoking { get; set; }
        public int nhomSmoking { get; set; }
        public double HATT { get; set; }
        public int nhomHATT { get; set; }
        public double TotalCholesterol { get; set; }
        public int nhomTotalCholesterol { get; set; }
        public double HDL { get; set; }
        public int nhomHDL { get; set; }
        public double HbA1C { get; set; }
        public int nhomHbA1C { get; set; }
        public double creatininSerum { get; set; }
        public int nhomEGFR { get; set; }
        public string vungnguyco { get; set; }
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
            tuoi = NB.tinhtuoi_nam();
            gioitinh = NB.gioitinh;
            smoking = NB.hutthuoc;
            HATT = NB.HATThu;
            HDL = XN.HDL;
            TotalCholesterol = XN.totalCholesterol;
            creatininSerum = XN.creatininSerum;

            checktuoi(NB.tinhtuoi_nam());
            checkSmoking(NB.hutthuoc);
            checkHATT(NB.HATThu);
            checkTotalCholesterol(XN.totalCholesterol);
            checkHDL(XN.HDL);
            checkEGFR(NB.gioitinh, XN.creatininSerum, NB.tinhtuoi_nam());
        }
        public SCORE2_DM(double _tuoi, string _gioitinh, double _DM_Age, bool _smoking, double _TotalCholesterol,
            double _HDL, double _HATT, double _HbA1C, double _creatininSerum, string _vungnguyco)
        {
            init_SCORE2_DM();
            gioitinh = _gioitinh.ToLower();
            tuoi = _tuoi;
            DM_age = _DM_Age;
            smoking = _smoking;
            HATT = _HATT;
            TotalCholesterol = _TotalCholesterol;
            HDL = _HDL;
            HbA1C = _HbA1C;
            creatininSerum = _creatininSerum;
            vungnguyco = _vungnguyco;
            checktuoi(_tuoi);
            double DM_Age = _tuoi - (DateTime.Now.Year - _DM_Age);
            checkDM_Age(DM_Age);
            checkSmoking(_smoking);
            checkHATT(_HATT);
            checkTotalCholesterol(_TotalCholesterol);
            checkHDL(_HDL);
            checkHbA1C(_HbA1C);
            checkEGFR(_gioitinh.ToLower(), _creatininSerum, _tuoi);
            checkvungnguyco(_vungnguyco);
        }
        private void init_SCORE2_DM()
        {
            int[] _diemNam = { 3, 2, 1, 0, 0, 0, 0, 0, -9, -2, -1, 1, 3, 6, -4, -3, -1, 1, 3, 2, 0, -1, 1, 2, 4, 5, 7, 8, 4, 1, -1, 3, 2, 1, 0, 0, 0, 0, 0, -5, 2, -1, 1, 3, 5, -4, -2, -1, 1, 3, 1, 0, -1, 1, 2, 3, 5, 6, 7, 4, 1, -1, 3, 2, 1, 0, 0, 0, 0, 0, 0, 6, -1, 1, 3, 4, -3, -2, -1, 1, 2, 1, 0, -1, 0, 2, 3, 4, 5, 6, 3, 1, -1, 3, 2, 1, 0, 0, -1, 0, 0, 4, 9, -1, 1, 2, 4, -3, -2, -1, 1, 2, 1, 0, -1, 0, 2, 3, 4, 5, 6, 3, 1, 0, 3, 2, 1, 0, 0, -1, -2, 0, 9, 13, -1, 1, 2, 3, -3, -2, -1, 1, 2, 1, 0, -1, 0, 1, 2, 3, 4, 5, 3, 1, 0, 3, 2, 1, 0, 0, -1, -2, -3, 13, 17, 0, 0, 1, 2, -2, -1, 0, 0, 1, 1, 0, -1, 0, 1, 2, 3, 4, 4, 2, 1, 0 };
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
            string[] _PLnguycoNam = { "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC" };
            PLnguycoNam = _PLnguycoNam;
            //PL Nguy co nu: -14++ (tổng 53)
            string[] _PLnguycoNu = { "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "T", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "TB", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC", "RC" };
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
            //eGFR CKD khong can can nang & chung toc
            eGFR_CKD eGFR_CKD_temp = new eGFR_CKD(_gioitinh.ToLower(), _tuoi, _creatininSerum);
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
            if (_vungnguyco.ToLower() == "thấp")
                nhomvungnguyco = 0;
            else if (_vungnguyco.ToLower() == "trung bình")
                nhomvungnguyco = 1;
            else if (_vungnguyco.ToLower() == "cao")
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
            if (gioitinh == "nam")
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

            if (gioitinh == "nam")
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

            if (gioitinh == "nam")
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
    public class SCORED : Thangdiem //T_C28
    {
        public double SCORED_SCORE { get; set; }

        public SCORED()
        {

        }
        public SCORED(string _input)
        {
            initchiso("T_C28");
            initTongdiem(_input);
            tinhTongdiem();
        }

        public double kqSCORED()
        {
            SCORED_SCORE = 0;

            foreach (BiendiemCSYH i in DStinhdiem)
            {
                SCORED_SCORE += i.diemketqua;
            }

            return SCORED_SCORE;
        }

        public List<string> kqSCORED_diengiai()
        {
            List<string> kq = db.GetDiengiaiKQ(IDChiso, SCORED_SCORE);
            return kq;
        }
    }
    #endregion
    #endregion
}
