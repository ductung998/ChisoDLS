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
    public partial class FormIBW : Form
    {
        public FormIBW()
        {
            InitializeComponent();
            BindingSource donvisource = new BindingSource();
            List<string> donvilist = new List<string> { "m", "cm" };
            donvisource.DataSource = donvilist;
            cmbdonviIBW.DataSource = donvisource;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            double a = double.Parse(txtChieuCao.Text);
            if (radNam.Checked)
            {
                if (cmbdonviIBW.SelectedItem.ToString() == "m")
                {
                    IBW testibw = new IBW("Nam", a * 100);
                    double b = testibw.kqIBW();
                    lblKetQua.Text = "Cân nặng lý tưởng (IBW) của bạn là: " + b.ToString() + " (kg)";
                }
                else
                {
                    IBW testibw = new IBW("Nam", a);
                    double b = testibw.kqIBW();
                    lblKetQua.Text = "Cân nặng lý tưởng (IBW) của bạn là: " + b.ToString() + " (kg)";
                }
            }
            else
            {
                if (cmbdonviIBW.SelectedItem.ToString() == "m")
                {
                    IBW testibw = new IBW("Nữ", a * 100);
                    double b = testibw.kqIBW();
                    lblKetQua.Text = "Cân nặng lý tưởng (IBW) của bạn là: " + b.ToString() + " (kg)";
                }
                else
                {
                    IBW testibw = new IBW("Nữ", a);
                    double b = testibw.kqIBW();
                    lblKetQua.Text = "Cân nặng lý tưởng (IBW) của bạn là: " + b.ToString() + " (kg)"; 
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            lblTieude.Text = "KHI NÀO BẠN CẦN SỬ DỤNG CHỈ SỐ CÂN NẶNG LÝ TƯỞNG (IBW)?";
            lblNDXuat.Text = "Khi bạn muốn tính toán cân nặng lý tưởng của một người. \nỨng dụng: Tính toán liều một số thuốc sử dụng.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lblTieude.Text = "CÁCH TÍNH TOÁN CHỈ SỐ CÂN NẶNG LÝ TƯỞNG (IBW)";
            lblNDXuat.Text = "- Ở nam: IBW = 50 + [0,91 × (chiều cao − 152,4)]. \n- Ở nữ: IBW = 45.5 + [0,91 × (chiều cao − 152,4)].\nĐơn vị tính: kg.";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblTieude.Text = "GIỚI THIỆU CHUNG VỀ CÂN NẶNG LÝ TƯỞNG (IBW)";
            lblNDXuat.Text = "Cân nặng lý tưởng (IBW) tính toán cân nặng lý tưởng dựa vào:\n- Các thông số đầu vào: Giới tính, chiều cao (m, cm).\n- Thông số đầu ra: Cân nặng lý tưởng (kg).\nNguồn tham khảo: Uptodate.";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lblKetQua.Text = "";
            txtChieuCao.Text = "";
        }
    }
}
