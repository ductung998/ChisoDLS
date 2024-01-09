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

        private void button1_Click(object sender, EventArgs e)
        {

        }
        #region IBW
        private void button1_Click_1(object sender, EventArgs e)
        {
            int chieucaoIBW = int.Parse(txtChieuCao.Text);
            if (radNam.Checked)
            {
                IBW dtIBW = new IBW("nam", chieucaoIBW);
                lblKetQua.Text = dtIBW.kqIBW().ToString(); 
            }
            else
            {
                IBW dtIBW = new IBW("nữ", chieucaoIBW);
                lblKetQua.Text = dtIBW.kqIBW().ToString(); 
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
                lblKetQuaAdjBW.Text = dtAdjBW.kqAdjBW().ToString();
            }
            else
            {
                AdjBW dtAdjBW = new AdjBW("nữ", chieucaoAdjBW, cannangAdjBW);
                lblKetQuaAdjBW.Text = dtAdjBW.kqAdjBW().ToString();
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


    }
}
