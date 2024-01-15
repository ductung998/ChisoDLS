using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chisoyhoc_API;

namespace Chisoyhoc_Form
{
    public partial class TestChiso1_10 : Form
    {
        public TestChiso1_10()
        {
            InitializeComponent();
        }

        #region IBW
        private void button1_Click(object sender, EventArgs e)
        {

        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            int chieucaoIBW = int.Parse(txtChieuCao.Text);
            if (radNam.Checked)
            {
                IBW dtIBW = new IBW("nam", chieucaoIBW);
                lblKetQua.Text = dtIBW.kqIBW().ToString() + "kg";  
            }
            else
            {
                IBW dtIBW = new IBW("nữ", chieucaoIBW);
                lblKetQua.Text = dtIBW.kqIBW().ToString() + "kg"; 
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
        
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
        
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            lblIBW.Text = "";
            lblKetQua.Text = "";
            txtChieuCao.Text = "";
            radNam.Checked = false;
            radNu.Checked = false;
        }
        

        
        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            lblIBW.Text = "Tính toán cân nặng lý tưởng của người bệnh";
        }

        private void button8_Click(object sender, EventArgs e)
        {
      
        }

        private void button9_Click(object sender, EventArgs e)
        {
      
        }

        private void button10_Click(object sender, EventArgs e)
        {
        
        }
        

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void IBW_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            lblIBW.Text = "1. Devine BJ. Gentamicin therapy. Drug Intell Clin Pharm 1974; 8:650."
+ "\n2. Hanley MJ, Abernethy DR, Greenblatt DJ. Effects of obesity on the pharmacokinetics of drugs in humans. Clin Pharmacokinet 2010; 49:71."
+ "\n3. Erstad BL. Dosing of medications in morbidly obese patients in the intensive care unit setting. Intensive Care Med 2004; 30:18."
+ "\n4. Shank BR, Zimmerman DE. Demystifying Drug Dosing in Obese Patients. American Society of Health-System Pharmacists 2016.";
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            lblIBW.Text = "Tính toán dựa trên giới tính, chiều cao (cm) theo công thức:"  
                + "\nỞ nam: IBW = 50 + [0,91 × (chiều cao − 152,4)] \nỞ nữ: IBW = 45,5 + [0,91 × (chiều cao − 152,4)] \nĐơn vị tính: kg";
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            lblIBW.Text = "Hỗ trợ xác định liều lượng một số thuốc sử dụng liên quan cân nặng, người béo phì và một số ước tính về chức năng thận.";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void radNu_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radNam_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtChieuCao_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbdonviIBW_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblKetQua_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void lblNoidungABW_Click(object sender, EventArgs e)
        {

        }

        private void lblTieudeABW_Click(object sender, EventArgs e)
        {

        }

        private void lblKqABW_Click(object sender, EventArgs e)
        {

        }

        private void txtCanNangABW_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void cmbABW_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtChieuCaoABW_TextChanged(object sender, EventArgs e)
        {

        }

        private void radNuABW_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radNamABW_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage8_Click(object sender, EventArgs e)
        {

        }

        private void tabPage9_Click(object sender, EventArgs e)
        {

        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region AdjBW
        private void button15_Click(object sender, EventArgs e)
        {
            int chieucaoAdjBW = int.Parse(txtChieucaoAdjBW.Text);
            int cannangAdjBW = int.Parse(txtCannangAdjBW.Text);
            if (radNamAdj.Checked)
            {
                AdjBW dtAdjBW = new AdjBW("nam", chieucaoAdjBW, cannangAdjBW);
                lblKetQuaAdjBW.Text = dtAdjBW.kqAdjBW().ToString() + "kg";
            }
            else
            {
                AdjBW dtAdjBW = new AdjBW("nữ", chieucaoAdjBW, cannangAdjBW);
                lblKetQuaAdjBW.Text = dtAdjBW.kqAdjBW().ToString() + "kg";
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            lblAdjBW.Text = "Tính toán cân nặng hiệu chỉnh của người bệnh dựa trên cân nặng lý tưởng.";
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            lblAdjBW.Text = "Tính toán liều thuốc dựa trên phân phối thuốc trong cơ thể người béo phì.";
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            lblAdjBW.Text = "Tính toán dựa trên giới tính, chiều cao (cm), cân nặng lý tưởng (IBW) và cân nặng thực tế (kg) theo công thức:"
            + "\nAdjBW = IBW + 0,4  × (cân nặng thực tế - IBW)"
            + "\nĐơn vị tính: kg";
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            lblAdjBW.Text = "1. Devine BJ. Gentamicin therapy. Drug Intell Clin Pharm 1974; 8:650."
            + "\n2. Hanley MJ, Abernethy DR, Greenblatt DJ. Effects of obesity on the pharmacokinetics of drugs in humans. Clin Pharmacokinet 2010; 49:71."
            + "\n3. Erstad BL. Dosing of medications in morbidly obese patients in the intensive care unit setting. Intensive Care Med 2004; 30:18."
            + "\n4. Shank BR, Zimmerman DE. Demystifying Drug Dosing in Obese Patients. American Society of Health-System Pharmacists 2016.";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            radNamAdj.Checked = false;
            radNuAdj.Checked = false;
            txtChieucaoAdjBW.Text = "";
            txtCannangAdjBW.Text = "";
            lblKetQuaAdjBW.Text = "";
            lblAdjBW.Text = "";
        }
        #endregion

        #region BMI
        private void button23_Click(object sender, EventArgs e)
        {
            double chieucaoBMI = double.Parse(txtChieuCaoBMI.Text);
            double cannangBMI = double.Parse(txtCanNangBMI.Text);
            BMI dtBMI = new BMI(chieucaoBMI, cannangBMI);
            lblKetQuaBMI.Text = dtBMI.kqBMI().ToString() + "kg/m^2"; ;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            txtChieuCaoBMI.Text = "";
            txtCanNangBMI.Text = "";
            lblKetQuaBMI.Text = "";
            lblBMI.Text = "";
        }

        private void button22_Click(object sender, EventArgs e)
        {
            lblBMI.Text = "Tính toán chỉ số khối cơ thể, đánh giá mức độ thừa cân/béo phì/gầy dựa trên BMI";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            lblBMI.Text = "Xác định nguyên nhân và đánh giá tình trạng thiếu oxy ở người bệnh";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            lblBMI.Text = "Dựa trên cân nặng (kg) và chiều cao (cm) theo công thức:"
            + "\nBMI = cân nặng /(chiều cao^2)"
            + "\nĐơn vị tính: kg/m^2";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            lblBMI.Text = "1. National Institutes of Health (NIH), National Heart, Lung, and Blood Institute (NHLBI). The practical guide: Identification, evaluation, and treatment of overweight and obesity in adults, NIH publication 00-4084, National Institutes of Health, Bethesda 2000.";
        }
        #endregion

        #region eGFR (CKD)
        private void button31_Click(object sender, EventArgs e)
        {
            double crclCKD = double.Parse(txtCrClCKD.Text);
            double tuoiCKD = double.Parse(txtTuoiCKD.Text);
            if (radNamCKD.Checked)
            {
                eGFR_CKD dtCKD = new eGFR_CKD("nam", crclCKD, tuoiCKD);
                lblKetQuaCKD.Text = dtCKD.kqeGFR_CKD().ToString() + "ml/phút/1,73m^2";
            }
            else
            {
                eGFR_CKD dtCKD = new eGFR_CKD("nữ", crclCKD, tuoiCKD);
                lblKetQuaCKD.Text = dtCKD.kqeGFR_CKD().ToString() + "ml/phút/1,73m^2";
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            lblCKD.Text = "Ước lượng độ lọc cầu thận dựa trên một số thông số và xét nghiệm.";
        }

        private void button29_Click(object sender, EventArgs e)
        {
            lblCKD.Text = "Xác định chức năng thận trong chẩn đoán, điều trị và sử dụng thuốc, công thức được đánh giá phản ánh tốt hơn về GFR so với Cockcroft–Gault và phù hợp với người có BMI cao.";
        }

        private void button28_Click(object sender, EventArgs e)
        {
            lblCKD.Text = "Tính toán từ tuổi, giới tính và creatinin huyết thanh đo được (mg/dL) theo công thức:"
+ "\neGFR = 142 × min(creatinin huyết thanh/kappa,1)^alpha × max(creatinin huyết thanh/kappa, 1)^(-1,2) × 0,9938^(Tuổi) × Hệ số giới tính"
+ "\nTrong đó:"
+ "\nGiới tính và các hệ số:"
+ "\n        Hệ số giới tính               Kappa                 Apha"
+ "\nNữ:        1,012                           0,7                  -0,241"
+ "\nNam:         1                              0,9                  -0,302"
+ "\nmin/max là giá trị nhỏ nhất/lớn nhất giữa 2 trị số creatinin huyết thanh/kappa so với 1"
+ "\nĐơn vị tính: mL/phút/1.73m^2";
        }

        private void button27_Click(object sender, EventArgs e)
        {
            lblCKD.Text = "1. Inker LA, Eneanya ND, Coresh J, et al. Chronic Kidney Disease Epidemiology Collaboration. New Creatinine- and Cystatin C-Based Equations to Estimate GFR without Race. N Engl J Med 2021; 385:1737."
+ "\n2. Levey AS, Bosch JP, Lewis JB, Greene T, Rogers N, Roth D. A more accurate method to estimate glomerular filtration rate from serum creatinine: a new prediction equation. Modification of Diet in Renal Disease Study Group. Ann Intern Med. 1999;130(6):461-70."
+ "\n3. Kidney Disease: Improving Global Outcomes (KDIGO) Anemia Work Group. KDIGO clinical practice guideline for anemia in chronic kidney disease. Kidney Int Suppl. 2012;2(4):279–335.";
        }
        private void button32_Click(object sender, EventArgs e)
        {
            radNamCKD.Checked = false;
            radNuCKD.Checked = false;
            txtCrClCKD.Text = "";
            txtTuoiCKD.Text = "";
            lblKetQuaCKD.Text = "";
            lblCKD.Text = "";
        }
        #endregion

        #region eGFR (MDRD)
        private void button39_Click(object sender, EventArgs e)
        {
            double crclMDRD = double.Parse(txtCrClMDRD.Text);
            double tuoiMDRD = double.Parse(txtTuoiMDRD.Text);
            if (radNamMDRD.Checked)
            {
                if (radDaDenMDRD.Checked)
                {
                    eGFR_MDRD dtMDRD = new eGFR_MDRD(crclMDRD, tuoiMDRD, "người da đen", "nam");
                    lblKetQuaMDRD.Text = dtMDRD.kqeGFR_MDRD().ToString() + "mL/phút/1.73m2";
                }
                else
                {
                    eGFR_MDRD dtMDRD = new eGFR_MDRD(crclMDRD, tuoiMDRD, "khác", "nam");
                    lblKetQuaMDRD.Text = dtMDRD.kqeGFR_MDRD().ToString() + "mL/phút/1.73m2";
                }
            }
            else
            {
                if (radDaDenMDRD.Checked)
                {
                    eGFR_MDRD dtMDRD = new eGFR_MDRD(crclMDRD, tuoiMDRD, "người da đen", "nữ");
                    lblKetQuaMDRD.Text = dtMDRD.kqeGFR_MDRD().ToString() + "mL/phút/1.73m2";
                }
                else
                {
                    eGFR_MDRD dtMDRD = new eGFR_MDRD(crclMDRD, tuoiMDRD, "khác", "nữ");
                    lblKetQuaMDRD.Text = dtMDRD.kqeGFR_MDRD().ToString() + "mL/phút/1.73m2";
                }
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            txtCrClMDRD.Text = "";
            txtTuoiMDRD.Text = "";
            radDaDenMDRD.Checked = false;
            radKhacMDRD.Checked = false;
            radNamMDRD.Checked = false;
            radNuMDRD.Checked = false;
            lblKetQuaMDRD.Text = "";
            lblMDRD.Text = "";
        }

        private void button38_Click(object sender, EventArgs e)
        {
            lblMDRD.Text = "Ước lượng độ lọc cầu thận dựa trên một số thông số và xét nghiệm.";
        }

        private void button37_Click(object sender, EventArgs e)
        {
            lblMDRD.Text = "Xác định chức năng thận trong chẩn đoán, điều trị và sử dụng thuốc, công thức được đánh giá phản ánh tốt hơn về GFR so với Cockcroft–Gault và phù hợp với người có BMI cao.";
        }

        private void button36_Click(object sender, EventArgs e)
        {
            lblMDRD.Text = "Tính toán từ tuổi, giới tính, chủng tộc và creatinin huyết thanh đo được (mg/dL) theo công thức:"
+ "\neGFR = 175 × creatinin huyết thanh^(-1,154) × Tuổi^(-0,203) × 1,212 (đối với người da đen) × 0,742 (đối với nữ)"
+ "\nĐơn vị tính: mL/phút/1.73m^2";
        }

        private void button35_Click(object sender, EventArgs e)
        {
            lblMDRD.Text = "1. Inker LA, Eneanya ND, Coresh J, et al. Chronic Kidney Disease Epidemiology Collaboration. New Creatinine- and Cystatin C-Based Equations to Estimate GFR without Race. N Engl J Med 2021; 385:1737."
+ "\n2. Levey AS, Bosch JP, Lewis JB, Greene T, Rogers N, Roth D. A more accurate method to estimate glomerular filtration rate from serum creatinine: a new prediction equation. Modification of Diet in Renal Disease Study Group. Ann Intern Med. 1999;130(6):461-70."
+ "\n3. Kidney Disease: Improving Global Outcomes (KDIGO) Anemia Work Group. KDIGO clinical practice guideline for anemia in chronic kidney disease. Kidney Int Suppl. 2012;2(4):279–335.";
        }
        #endregion

        #region CrCl
        private void button47_Click(object sender, EventArgs e)
        {
            double tuoiCrCl = double.Parse(txtTuoiCrCl.Text);
            double cannangCrCl= double.Parse(txtCanNangCrCl.Text);
            double crclCrCl = double.Parse(txtCreCrCl.Text);
            if (radNamCrCl.Checked)
            {
                eCrCl dteCrCl = new eCrCl(tuoiCrCl, cannangCrCl, crclCrCl, "nam");
                lblKetQuaCrCl.Text = dteCrCl.kqeCrCl().ToString() + "mL/phút/1.73m^2";
            }
            else
            {
                eCrCl dteCrCl = new eCrCl(tuoiCrCl, cannangCrCl, crclCrCl, "nữ");
                lblKetQuaCrCl.Text = dteCrCl.kqeCrCl().ToString() + "mL/phút/1.73m^2";
            }

        }

        private void button48_Click(object sender, EventArgs e)
        {
            txtTuoiCrCl.Text = "";
            txtCanNangCrCl.Text = "";
            txtCreCrCl.Text = "";
            radNamCrCl.Checked = false;
            radNuCrCl.Checked = false;
            lblKetQuaCrCl.Text = "";
            lblCrCl.Text = "";
        }

        private void button46_Click(object sender, EventArgs e)
        {
            lblCrCl.Text = "Tính toán độ thanh thải creatinin để ước lượng độ lọc cầu thận, đánh giá chức năng thận";
        }

        private void button45_Click(object sender, EventArgs e)
        {
            lblCrCl.Text = "Đánh giá chức năng thận trong chẩn đoán, điều trị và sử dụng thuốc";
        }

        private void button44_Click(object sender, EventArgs e)
        {
            lblCrCl.Text = "Tính toán từ tuổi, cân nặng, giới tính và creatinin huyết thanh đo được theo công thức:"
+ "\nĐộ thanh thải Creatinin = (140 − tuổi) × cân nặng × (0,85 đối với nữ)/(72 × creatinin huyết thanh)"
+ "\nĐơn vị tính: mL/phút/1.73m2";
        }

        private void button43_Click(object sender, EventArgs e)
        {
            lblCrCl.Text = "1. Cockcroft DW, Gault MH. Prediction of creatinine clearance from serum creatinine. Nephron 1976; 16:31."
+ "\n2.National Institute of Diabetes and Digestive and Kidney Diseases. CKD & drug dosing: Information for providers - estimation of kidney function for prescription medication dosage in adults.";
        }
        #endregion

        #region Na huyết
        private void button55_Click(object sender, EventArgs e)
        {
            double natriNa = double.Parse(txtNatriNa.Text);
            double glucoseNa = double.Parse(txtGlucoseNa.Text);
            NatriSerum_Adj dtNatri = new NatriSerum_Adj(natriNa, glucoseNa);
            lblKetQuaNa.Text = dtNatri.kqNatriSerum_Adj().ToString() + "mEq/L";
        }

        private void button56_Click(object sender, EventArgs e)
        {
            txtNatriNa.Text = "";
            txtGlucoseNa.Text = "";
            lblKetQuaNa.Text = "";
            lblNa.Text = "";
        }

        private void button54_Click(object sender, EventArgs e)
        {
            lblNa.Text = "Nồng độ natri huyết tương hiệu chỉnh đối với tăng đường huyết";
        }

        private void button53_Click(object sender, EventArgs e)
        {
            lblNa.Text = "Tăng đường huyết gây giảm nồng độ natri huyết tương đo được, cần hiệu chỉnh để đánh giá chính xác tình trạng thiếu hụt/dư natri";
        }

        private void button52_Click(object sender, EventArgs e)
        {
            lblNa.Text = "Chẩn đoán, điều trị tăng/giảm natri cho người bệnh đái tháo đường.";
        }

        private void button51_Click(object sender, EventArgs e)
        {
            lblNa.Text = "1. Emmett M. Case 6: Diabetes and acidosis. In: NephSAP: Nephrology Self-Assessment Program: Fluid, Electrolyte, and Acid-Base Disturbances, Sterns RH, Emmett M (Eds), American Society of Nephrology 2013. Vol 12, p.191.";
        }
        #endregion

        #region LDL-C (Friedewald)
        private void button63_Click(object sender, EventArgs e)
        {
            double cholesterol = double.Parse(txtCholesLDL.Text);
            double hdl = double.Parse(txtHDLLDL.Text);
            double triglycerid = double.Parse(txtTriglyLDL.Text);
            LDL dtLDL = new LDL(cholesterol, hdl, triglycerid);
            lblKetQuaLDL.Text = dtLDL.kqLDL().ToString() + "mg/dL";
        }

        private void button64_Click(object sender, EventArgs e)
        {
            txtCholesLDL.Text = "";
            txtHDLLDL.Text = "";
            txtTriglyLDL.Text = "";
            lblKetQuaLDL.Text = "";
            lblLDL.Text = "";
        }

        private void button62_Click(object sender, EventArgs e)
        {
            lblLDL.Text = "Tính toán nồng độ LCL-C trong máu, không phù hợp khi Triglycerid tăng quá 400 mg/dL (4,5 mmol/L)";
        }

        private void button61_Click(object sender, EventArgs e)
        {
            lblLDL.Text = "Đánh giá tình trạng tăng lipid huyết ở người bệnh.";
        }

        private void button60_Click(object sender, EventArgs e)
        {
            lblLDL.Text = "Căn cứ xét nghiệm huyết học, tính toán dựa trên nồng độ cholesterol toàn phần (mg/dL), HDL-C (mg/dL) và Triglyceride (mg/dL) theo công thức:"
+ "\nLDL-C = cholesterol toàn phần - Triglyceride/5 - HDL-C"
+ "\nĐơn vị tính: mg/dL";
        }

        private void button59_Click(object sender, EventArgs e)
        {
            lblLDL.Text = "1. Friedewald WT, Levy RI, Fredrickson DS. Estimation of the concentration of low-density lipoprotein cholesterol in plasma, without use of the preparative ultracentrifuge. Clin Chem 1972; 18:499.";
        }
        #endregion

        #region FIB-4
        private void button65_Click(object sender, EventArgs e)
        {
            double tuoiFib = double.Parse(txtTuoiFib.Text);
            double astFib = double.Parse(txtASTFib.Text);
            double tieucauFib = double.Parse(txtTieucauFib.Text);
            double altFib = double.Parse(txtALTFib.Text);
            FIB4 dtFib = new FIB4(tuoiFib, astFib, tieucauFib, altFib);
            lblKetQuaFib.Text = dtFib.kqFIB4().ToString();
        }

        private void button66_Click(object sender, EventArgs e)
        {
            txtTuoiFib.Text = "";
            txtASTFib.Text = "";
            txtTieucauFib.Text = "";
            txtALTFib.Text = "";
            lblKetQuaFib.Text = "";
            lblFib.Text = "";
        }

        private void button72_Click(object sender, EventArgs e)
        {
            lblFib.Text = "Đánh giá mức độ xơ gan ở người bệnh từ kết quả xét nghiệm huyết học.";
        }

        private void button71_Click(object sender, EventArgs e)
        {
            lblFib.Text = "Sử dụng trong người bệnh mắc các bệnh gan, biểu hiện lâm sàng liên quan tới xơ gan, người nhiễm virus viêm gan…";
        }

        private void button70_Click(object sender, EventArgs e)
        {
            lblFib.Text = "Căn cứ xét nghiệm huyết học, tính toán dựa trên các chỉ số AST (U/L), ALT (U/L), số lượng tiểu cầu (số lượng/microL) và tuổi (năm) theo công thức:"
+ "FIB-4 = Tuổi × AST/(0,001 × số lượng tiểu cầu × √ALT";
        }

        private void button69_Click(object sender, EventArgs e)
        {
            lblFib.Text = "1. Udell JA, Wang CS, Tinmouth J, et al. Does this patient with liver disease have cirrhosis? JAMA 2012; 307:832."
+ "2. Vallet-Pichard A, Mallet V, Nalpas B, et al. FIB-4: an inexpensive and accurate marker of fibrosis in HCV infection. comparison with liver biopsy and fibrotest. Hepatology 2007; 46:32.";
        }
        #endregion

        #region TSAT (chưa code xong)
        private void button73_Click(object sender, EventArgs e)
        {
            
        }

        private void button74_Click(object sender, EventArgs e)
        {

        }

        private void button80_Click(object sender, EventArgs e)
        {
            lblTSAT.Text = "Tính toán độ bão hòa transferrin - protein liên kết với sắt trong máu";
        }

        private void button79_Click(object sender, EventArgs e)
        {
            lblTSAT.Text = "Đánh giá tình trạng thiếu/thừa sắt ở người bệnh thiếu máu liên quan tới thiếu sắt.";
        }

        private void button78_Click(object sender, EventArgs e)
        {
            lblTSAT.Text = "Từ xét nghiệm huyết học, tính toán dựa trên nồng độ sắt huyết thanh (mcg/dL) và tổng lượng sắt có khả năng gắn kết với các protein trong máu (TIBC, mcg/dL) theo công thức:"
+ "\nTSAT = (nồng độ sắt huyết thanh / TIBC) × 100"
+ "\nĐơn vị tính: %";
        }

        private void button77_Click(object sender, EventArgs e)
        {
            lblTSAT.Text = "1. Cook JD. Clinical evaluation of iron deficiency. Semin Hematol 1982; 19:6.";
        }
        #endregion

    }
}
