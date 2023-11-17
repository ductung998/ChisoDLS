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
            #region IBW
            BindingSource donvisource = new BindingSource();
            List<string> donvilist = new List<string> { "m", "cm" };
            donvisource.DataSource = donvilist;
            cmbdonviIBW.DataSource = donvisource;
            #endregion

            #region ABW
            BindingSource donvisourceABW = new BindingSource();
            List<string> donvilistABW = new List<string> { "m", "cm" };
            donvisource.DataSource = donvilist;
            cmbABW.DataSource = donvisource;
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        #region IBW
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

        private void button5_Click_1(object sender, EventArgs e)
        {
            lblKetQua.Text = "";
            txtChieuCao.Text = "";
            radNam.Checked = false;
            radNu.Checked = false;
            cmbdonviIBW.SelectedIndex = 0;
        }
        #endregion

        #region ABW
        private void button6_Click(object sender, EventArgs e)
        {
            double a = double.Parse(txtChieuCaoABW.Text);
            double cannangABW = double.Parse(txtCanNangABW.Text);
            if (radNamABW.Checked)
            {
                if (cmbABW.SelectedItem.ToString() == "m")
                {
                    ABW testabw = new ABW("Nam", a * 100, cannangABW);
                    double kqABW = testabw.kqABW();
                    lblKqABW.Text = "Cân nặng hiệu chỉnh (ABW) của bạn là: " + kqABW.ToString() + " (kg)";
                }
                else
                {
                    ABW testabw = new ABW("Nam", a, cannangABW);
                    double kqABW = testabw.kqABW();
                    lblKqABW.Text = "Cân nặng hiệu chỉnh (ABW) của bạn là: " + kqABW.ToString() + " (kg)";
                }
            }
            else
            {
                if (cmbABW.SelectedItem.ToString() == "m")
                {
                    ABW testabw = new ABW("Nữ", a * 100, cannangABW);
                    double kqABW = testabw.kqABW();
                    lblKqABW.Text = "Cân nặng hiệu chỉnh (ABW) của bạn là: " + kqABW.ToString() + " (kg)";
                }
                else
                {
                    ABW testabw = new ABW("Nữ", a, cannangABW);
                    double kqABW = testabw.kqABW();
                    lblKqABW.Text = "Cân nặng hiệu chỉnh (ABW) của bạn là: " + kqABW.ToString() + " (kg)";
                }
            }  
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lblKqABW.Text = "";
            txtChieuCaoABW.Text = "";
            txtCanNangABW.Text = "";
            radNamABW.Checked = false;
            radNuABW.Checked = false;
            cmbABW.SelectedIndex = 0;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            lblTieudeABW.Text = "GIỚI THIỆU CHUNG VỀ CÂN NẶNG HIỆU CHỈNH (ABW)";
            lblNoidungABW.Text = "Cân nặng hiệu chỉnh (ABW) tính toán cân nặng hiệu chỉnh dựa vào:\n- Các thông số đầu vào: Giới tính, chiều cao, cân nặng thực tế (m, cm, kg).\n- Thông số đầu ra: Cân nặng hiệu chỉnh (kg).\nNguồn tham khảo: Uptodate.";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            lblTieudeABW.Text = "KHI NÀO BẠN CẦN SỬ DỤNG CHỈ SỐ CÂN NẶNG HIỆU CHỈNH (ABW)?";
            lblNoidungABW.Text = "Khi bạn muốn tính toán cân nặng hiệu chỉnh của một người. \nỨng dụng: Tính toàn liều thuốc sử dụng.";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            lblTieudeABW.Text = "CÁCH TÍNH TOÁN CHỈ SỐ CÂN NẶNG HIỆU CHỈNH (ABW)";
            lblNoidungABW.Text = "Cân nặng lý tưởng (IBW) + 0,4 x (câng nặng thực tế - IBW).\nĐơn vị tính: kg.";
        }
        #endregion
    }
}
