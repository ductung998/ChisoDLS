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


namespace Chisoyhoc_Form.TestChiso
{
    public partial class TestChiso11_20 : Form
    {
        public TestChiso11_20()
        {
            InitializeComponent();
        }

        #region Chuyển AST sang APRI
        private void button1_Click_2(object sender, EventArgs e)
        {
            double astAPRI = double.Parse(txtASTAPRI.Text);
            double asttrenAPRI = double.Parse(txtASTtrenAPRI.Text);
            double tieucauAPRI = double.Parse(txtTieucauAPRI.Text);
            APRI dtAPRI = new APRI(astAPRI,asttrenAPRI,tieucauAPRI);
            lblKetQuaAPRI.Text = dtAPRI.kqAPRI().ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtASTAPRI.Text = "";
            txtASTtrenAPRI.Text = "";
            txtTieucauAPRI.Text = "";
            lblKetQuaAPRI.Text = "";
            lblAPRI.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblAPRI.Text = "Tiên lượng xơ gan và xơ hóa từ kết quả xét nghiệm huyết học.";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lblAPRI.Text = "Dự đoán tình trạng xơ hóa và xơ gan ở người bệnh HCV và APRI sử dụng các giá trị xét nghiệm thông thường có sẵn để giúp tránh nhu cầu sinh thiết gan. Có thể kết hợp điểm FIB-4 để đánh giá thêm tình trạng xơ gan.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lblAPRI.Text = "Từ xét nghiệm huyết học, tính toán dựa trên số lượng tiểu cầu (tế bào/microL), AST (U/L), AST mức bình thường trên (U/L) theo công thức:"
+ "APRI = 100  × ((AST/AST mức bình thường trên)/(tiểu cầu/1000))";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            lblAPRI.Text = "1. Loaeza-del-Castillo A, Paz-Pineda F, Oviedo-Cardenas E, et al. AST to platelet ratio index (APRI) for the noninvasive evaluation of liver fibrosis. Ann Hepatol 2008; 7:350."
+ "Lin ZH, Xin YN, Dong QJ, et al. Performance of the aspartate aminotransferase-to-platelet ratio index for the staging of hepatitis C-related fibrosis: an updated meta-analysis. Hepatology 2011; 53:726.";
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
