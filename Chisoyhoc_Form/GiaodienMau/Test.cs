using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassChung;

namespace Chisoyhoc_Form.GiaodienMau
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string gioitinh = "";
            double tuoi = double.Parse(textBox1.Text);
            bool hutthuoc = checkBox1.Checked;
            int DM_Nam = int.Parse(textBox2.Text);
            double HATT = double.Parse(textBox3.Text);
            double TCho = double.Parse(textBox4.Text);
            double HDL = double.Parse(textBox5.Text);
            double HbA1C = double.Parse(textBox6.Text);
            double creatininSer = double.Parse(textBox7.Text);
            string vungnguyco = comboBox1.Text;

            if (radioButton1.Checked)
                gioitinh = "Nam";
            else
                gioitinh = "Nữ";

            eGFR_CKD eGFR_CKD_Cal = new eGFR_CKD(gioitinh, tuoi, creatininSer);
            label15.Text = "eGFR: " + Math.Round(eGFR_CKD_Cal.kqeGFR_CKD(),2).ToString() +" mL/phút/1,73m^2";
            SCORE2_DM SCORE2_DM_Cal = new SCORE2_DM(tuoi, gioitinh, DM_Nam, hutthuoc, HATT, TCho, HDL, HbA1C, creatininSer, vungnguyco);
/*            label8.Text = SCORE2_DM_Cal.nhomDM_Age.ToString();
            label9.Text = SCORE2_DM_Cal.nhomHATT.ToString();
            label10.Text = SCORE2_DM_Cal.nhomTotalCholesterol.ToString();
            label11.Text = SCORE2_DM_Cal.nhomHDL.ToString();
            label12.Text = SCORE2_DM_Cal.nhomHbA1C.ToString();
            label13.Text = SCORE2_DM_Cal.nhomEGFR.ToString();
            label14.Text = SCORE2_DM_Cal.nhomSmoking.ToString();
            label16.Text = "Điểm: " + SCORE2_DM_Cal.kqSCORE2_DM().ToString();*/
            label16.Text = "Điểm SCORE2-Diabetes: " + SCORE2_DM_Cal.kqSCORE2_DM().ToString();
            label8.Text = SCORE2_DM_Cal.diemNam[SCORE2_DM_Cal.diem_start_index + SCORE2_DM_Cal.nhomDM_Age].ToString();
            label9.Text = SCORE2_DM_Cal.diemNam[SCORE2_DM_Cal.diem_start_index + 8 + 2 + SCORE2_DM_Cal.nhomHATT].ToString();
            label10.Text = SCORE2_DM_Cal.diemNam[SCORE2_DM_Cal.diem_start_index + 8 + 2 + 4 + SCORE2_DM_Cal.nhomTotalCholesterol].ToString();
            label11.Text = SCORE2_DM_Cal.diemNam[SCORE2_DM_Cal.diem_start_index + 8 + 2 + 4 + 5 + SCORE2_DM_Cal.nhomHDL].ToString();
            label12.Text = SCORE2_DM_Cal.diemNam[SCORE2_DM_Cal.diem_start_index + 8 + 2 + 4 + 5 + 3 + SCORE2_DM_Cal.nhomHbA1C].ToString();
            label13.Text = SCORE2_DM_Cal.diemNam[SCORE2_DM_Cal.diem_start_index + 8 + 2 + 4 + 5 + 3 + 5 + SCORE2_DM_Cal.nhomEGFR].ToString();
            label14.Text = SCORE2_DM_Cal.diemNam[SCORE2_DM_Cal.diem_start_index + 8 + SCORE2_DM_Cal.nhomSmoking].ToString();
            //MessageBox.Show(SCORE2_DM_Cal.diemNam[SCORE2_DM_Cal.diem_start_index + 8].ToString());
            labelnguyco.Text = "Nguy cơ ASCVD 10 năm: " + SCORE2_DM_Cal.kqNguycoSCORE2_DM().ToString() + " %";
            labelPLnguyco.Text = "Phân nhóm nguy cơ: " + SCORE2_DM_Cal.kqPLNguycoSCORE2_DM();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            List<string> datacb = new List<string>() { "Thấp", "Trung bình", "Cao", "Rất cao" };
            comboBox1.DataSource = datacb;

            textBoxGT.Text = "Mục đích\r\nĐánh giá nguy cơ mắc biến cố tim mạch gây tử vong (fatal) hoặc không (non-fatal) trong 10 năm theo khuyến cáo về phòng ngừa tim mạch của Hội Tim mạch châu Âu (ESC)." +
                "\r\nỨng dụng\r\nTheo dõi và đánh giá nguy cơ mắc biến cố tim mạch, đánh giá các yếu tố nguy cơ để lựa chọn phương án điều trị và dự phòng phù hợp.";
            textBoxPP.Text = "Căn cứ vào tuổi (năm), giới tính, hút thuốc lá, non-HDL cholesterol (mmol/L), huyết áp tâm thu (mmHg) và nguy cơ tim mạch trong dân số (4 nhóm)." +
                "\r\nnon-HDL cholesterol = Cholesterol toàn phần - HDL";

            textBox1.Text = "55";
            textBox2.Text = "1990";
            textBox3.Text = "140";
            textBox4.Text = "4.5";
            textBox5.Text = "1";
            textBox6.Text = "45";
            textBox7.Text = "0.8";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            label15.Text = "eGFR: ";
            checkBox1.Checked = false;
            radioButton1.Checked = true;
            label16.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            label12.Text = "";
            label13.Text = "";
            label14.Text = "";
            labelnguyco.Text = "";
            labelPLnguyco.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
    (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
    (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
    (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string gioitinh = "";
            double tuoi = double.Parse(textBox1.Text);
            bool hutthuoc = checkBox1.Checked;
            int DM_Nam = int.Parse(textBox2.Text);
            double HATT = double.Parse(textBox3.Text);
            double TCho = double.Parse(textBox4.Text);
            double HDL = double.Parse(textBox5.Text);
            double HbA1C = double.Parse(textBox6.Text);
            double creatininSer = double.Parse(textBox7.Text);
            string vungnguyco = comboBox1.Text;

            if (radioButton1.Checked)
                gioitinh = "Nam";
            else
                gioitinh = "Nữ";

            SCORE2 SCORE2_Cal = new SCORE2(tuoi, gioitinh, hutthuoc, HATT, TCho, HDL, vungnguyco);
            label16.Text = "Nguy cơ biến cố tim mạch 10 năm: " + SCORE2_Cal.kqSCORE2().ToString() + "%";
            labelnguyco.Text = "Phân nhóm nguy cơ: " + SCORE2_Cal.kqSCORE2_diengiai();
        }
    }
}
