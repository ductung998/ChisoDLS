using System;
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
        List<Bien> listdem = new List<Bien>();
        List<string> input = _input.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (string chiso in input)
        {
            listdem.AddRange(GetDSbien(chiso));
        }

        List<Bien> kq = GetDSBiengoc(listdem);
        return kq;
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
                            break;
                        }
                    case "C_A02": //3 AdjBW
                        {
                            AdjBW AdjBWCal = new AdjBW(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(Math.Round(AdjBWCal.kqAdjBW(), 2).ToString());
                            break;
                        }
                    case "C_A03": //3 LBW
                        {
                            LBW LBWCal = new LBW(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(Math.Round(LBWCal.KqLBW(), 2).ToString());
                            break;
                        }
                    case "C_A04": //3 AlcoholSerum
                        {
                            AlcoholSerum AlcoholSerumCal = new AlcoholSerum(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(Math.Round(AlcoholSerumCal.kqAlcoholSerum(), 2).ToString());
                            break;
                        }
                    case "C_A05"://2 Budichbong
                        {
                            Budichbong BudichbongCal = new Budichbong(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(BudichbongCal.kqVdich24h(), 2).ToString());
                            kq.Add(Math.Round(BudichbongCal.kqtocdotruyen8h(), 2).ToString());
                            kq.Add(Math.Round(BudichbongCal.kqtocdotruyen16h(), 2).ToString());
                            break;
                        }
                    case "C_A06": //BMI
                        {
                            BMI BMICal = new BMI(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(BMICal.kqBMI(), 2).ToString());
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
                    case "C_A10"://4
                        {
                            SAG SAGCal = new SAG(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(SAGCal.kqSAG(), 2).ToString());
                            break;
                        }
                    case "C_A11"://4
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
                    case "C_A14"://5
                        {
                            UOG UOGCal = new UOG(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]),
                                double.Parse(inputs[4]));
                            kq.Add(Math.Round(UOGCal.kqUOG(), 2).ToString());
                            break;
                        }
                    case "C_A15"://5 CKD 5 MDRD
                        {
                            eGFR_CKD eGFR_CKDCal = new eGFR_CKD(double.Parse(inputs[0]),
                                inputs[1],
                                double.Parse(inputs[2]));
                            eGFR_MDRD eGFR_MDRDCal = new eGFR_MDRD(double.Parse(inputs[0]),
                                inputs[1],
                                double.Parse(inputs[2]),
                                inputs[3]);
                            kq.Add(Math.Round(eGFR_CKDCal.kqeGFR_CKD(), 2).ToString());
                            kq.Add(Math.Round(eGFR_MDRDCal.kqeGFR_MDRD(), 2).ToString());
                            break;
                        }
                    case "C_A16": //4
                        {
                            eCrCl eCrClCal = new eCrCl(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(eCrClCal.kqeCrCl(), 2).ToString());
                            break;
                        }
                    case "C_A17": //4
                        {
                            FEMg FEMgCal = new FEMg(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(FEMgCal.kqFEMg(), 2).ToString());
                            break;
                        }
                    case "C_A18"://4
                        {
                            FENa FENaCal = new FENa(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(FENaCal.kqFENa(), 2).ToString());
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
                switch (machiso)
                {
                    case "C_B01": //4
                        {
                            DLCO_Adj DLCO_AdjCal = new DLCO_Adj(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(DLCO_AdjCal.kqDLCO_Adj(), 2).ToString());
                            break;
                        }
                    case "C_B02": //2
                        {
                            MAP MAPCal = new MAP(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(MAPCal.kqMAP(), 2).ToString());
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
                            break;
                        }
                    case "C_B04": //2
                        {
                            AEC AECCal = new AEC(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(AECCal.kqAEC(), 2).ToString());
                            break;
                        }
                    case "C_B05": //2
                        {
                            ANC ANCCal = new ANC(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(ANCCal.kqANC(), 2).ToString());
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
                            kq.Add(MIPICal.kqMIPI_danhgia());
                            break;
                        }
                    case "C_B07": //2
                        {
                            RPI RPICal = new RPI(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(RPICal.kqRPI(), 2).ToString());
                            break;
                        }
                    case "C_B08": //2
                        {
                            sTfR sTfRCal = new sTfR(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(sTfRCal.kqsTfR(), 2).ToString());
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
                            break;
                        }
                    case "C_B10": //3
                        {
                            CDC_chieucao CDC_chieucaoCal = new CDC_chieucao(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(CDC_chieucaoCal.kqCDC_chieucao_danhgia());
                            kq.Add(Math.Round(CDC_chieucaoCal.kqCDC_chieucao_zscore(), 2).ToString());
                            break;
                        }
                    case "C_B11": //3
                        {
                            CDC_cannang CDC_cannangCal = new CDC_cannang(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(CDC_cannangCal.kqCDC_cannang_danhgia());
                            kq.Add(Math.Round(CDC_cannangCal.kqCDC_cannang_zscore(), 2).ToString());
                            break;
                        }
                    case "C_B12": //3
                        {
                            CDC_chuvi CDC_chuviCal = new CDC_chuvi(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(CDC_chuviCal.kqCDC_chuvi_danhgia());
                            kq.Add(Math.Round(CDC_chuviCal.kqCDC_chuvi_zscore(), 2).ToString());
                            break;
                        }
                    case "C_B13": //1
                        {
                            Vbudich VbudichCal = new Vbudich(double.Parse(inputs[0]));
                            kq.Add(Math.Round(VbudichCal.kqVdich24h(), 2).ToString());
                            kq.Add(Math.Round(VbudichCal.kqtocdotruyen24h(), 2).ToString());
                            kq.Add(Math.Round(VbudichCal.kqVdich_theogio(), 2).ToString());
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
                            break;
                        }
                    case "C_B15": //4
                        {
                            WHO_suyDD WHO_suyDDCal = new WHO_suyDD(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(WHO_suyDDCal.kqWHO_suyDD());
                            kq.Add(Math.Round(WHO_suyDDCal.kqWHO_chieucao_zscore(), 2).ToString());
                            kq.Add(Math.Round(WHO_suyDDCal.kqWHO_cannang_zscore(), 2).ToString());
                            break;
                        }
                    case "C_B16": //2
                        {
                            ePER_PNCT ePER_PNCTCal = new ePER_PNCT(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(ePER_PNCTCal.kqePER_PNCT(), 2).ToString());
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
                            kq.Add(OxyIndexCal.kqOxyIndex_danhgia());
                            break;
                        }
                    case "C_B18": //4
                        {
                            EED EEDCal = new EED(DateTime.Parse(inputs[0]),
                                DateTime.Parse(inputs[1]),
                                int.Parse(inputs[2]),
                                KetnoiDB.str_to_bool(inputs[3]));
                            kq.Add(EEDCal.kqEED().ToString());
                            kq.Add(EEDCal.kqTuoithai().ToString());
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
                            break;
                        }
                    case "C_B20": //3
                        {
                            CDC_BMI CDC_BMICal = new CDC_BMI(inputs[0],
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(CDC_BMICal.kqCDC_BMI());
                            break;
                        }
                    case "C_B21": //2
                        {
                            Noikhiquan NoikhiquanCal = new Noikhiquan(KetnoiDB.str_to_bool(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(NoikhiquanCal.kqNoikhiquan(), 2).ToString());
                            break;
                        }
                    case "C_B22": //3
                        {
                            PEF PEFCal = new PEF(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                inputs[2]);
                            kq.Add(Math.Round(PEFCal.kqPEF(), 2).ToString());
                            kq.Add(PEFCal.kqPEF_danhgia());
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
                                DateTime.Parse(inputs[7]),
                                DateTime.Parse(inputs[8]),
                                KetnoiDB.str_to_bool(inputs[9]));
                            kq.Add(Math.Round(PELD_NewCal.kqPELD_New(), 2).ToString());
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
                            break;
                        }
                    case "C_C03": //4
                        {
                            FEPO4 FEPO4Cal = new FEPO4(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(FEPO4Cal.kqFEPO4(), 2).ToString());
                            break;
                        }
                    case "C_C04": //3
                        {
                            LDL LDLCal = new LDL(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(Math.Round(LDLCal.kqLDL(), 2).ToString());
                            break;
                        }
                    case "C_C05": //4
                        {
                            FIB4 FIB4Cal = new FIB4(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(FIB4Cal.kqFIB4(), 2).ToString());
                            break;
                        }
                    case "C_C06": //2
                        {
                            TSAT TSATCal = new TSAT(double.Parse(inputs[0]),
                                double.Parse(inputs[1]));
                            kq.Add(Math.Round(TSATCal.kqTSAT(), 2).ToString());
                            break;
                        }
                    case "C_C07": //3
                        {
                            APRI APRICal = new APRI(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(Math.Round(APRICal.kqAPRI(), 2).ToString());
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
                            break;
                        }
                    case "C_C10": // 3 PVR, 5 PVRI
                        {
                            PVR PVRCal = new PVR(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(PVRCal.kqPVR(), 2).ToString());

                            PVRI PVRICal = new PVRI(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]),
                                double.Parse(inputs[4]),
                                double.Parse(inputs[5]));
                            kq.Add(Math.Round(PVRICal.kqPVRI(), 2).ToString());
                            break;
                        }
                    case "C_C11": //3 AdjECG
                        {
                            AdjECG AdjECGCal = new AdjECG(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]));
                            kq.Add(Math.Round(AdjECGCal.kqAdjQT_Bazett(), 2).ToString());
                            kq.Add(Math.Round(AdjECGCal.kqAdjQT_Framingham(), 2).ToString());
                            kq.Add(Math.Round(AdjECGCal.kqAdjQT_Fridericia(), 2).ToString());
                            kq.Add(Math.Round(AdjECGCal.kqAdjQT_Hodges(), 2).ToString());
                            break;
                        }
                    case "C_C12": //4 SVR
                        {
                            SVR SVRCal = new SVR(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(SVRCal.kqSVR(), 2).ToString());
                            break;
                        }
                    case "C_C13": //4 WBCCFS_Adj
                        {
                            WBCCFS_Adj WBCCFS_AdjCal = new WBCCFS_Adj(double.Parse(inputs[0]),
                                double.Parse(inputs[1]),
                                double.Parse(inputs[2]),
                                double.Parse(inputs[3]));
                            kq.Add(Math.Round(WBCCFS_AdjCal.kqWBCCFS_Adj(), 2).ToString());
                            break;
                        }
                    case "C_C14": //5 Hauphauxogan + tu vong 7n, 30n, 90n
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
                            break;
                        }
                    case "C_C15": //12 MESA Score, CAC & khong CAC
                        {
                            if (inputs.Count() == 11)
                            {
                                MESA_SCORE MESA_SCORECal = new MESA_SCORE(
                                    double.Parse(inputs[0]),
                                    inputs[1],
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
                            }
                            else
                            {
                                MESA_SCORE MESA_SCORECal = new MESA_SCORE(
                                    double.Parse(inputs[0]),
                                    inputs[1],
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

                    case "T_B32": //GlasgowNhiO2 3 var: 4,5,6
                        {
                            GlasgowNhiO2 GlasgowNhiO2Cal = new GlasgowNhiO2(input);
                            kq.Add(GlasgowNhiO2Cal.kqGlasgowNhiO2().ToString());
                            kq.AddRange(GlasgowNhiO2Cal.kqGlasgowNhiO2_diengiai());
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
                            kq.Add(SCORE2Cal.kqPLNguycoSCORE2());
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
        DateTime referenceDate = new DateTime(0001, 1, 1);

        kq = (input - referenceDate).TotalDays.ToString();
        return kq;
    }
    public static DateTime numbertodatetime(string input)
    {
        //Chuyển giá trị biến CHUỖI trở lại thành biến THỜI GIAN
        DateTime kq;
        DateTime referenceDate = new DateTime(0001, 1, 1);

        kq = referenceDate.AddDays(double.Parse(input));
        return kq;
    }
}
#endregion
