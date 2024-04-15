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
    public partial class Test3 : Form
    {
        public KetnoiDB db;
        public int demnb = 1;
        public Test3()
        {
            InitializeComponent();
        }

        private void Test3_Load(object sender, EventArgs e)
        {
            db = new KetnoiDB();
            refreshNB();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            refreshNB();
        }
        public void refreshNB()
        {
            List<Nguoibenh> DSNB = db.getDSNB();

            BindingSource bindingNB = new BindingSource { DataSource = DSNB };
            dataGridView1.DataSource = bindingNB;
        }
        public void refreshXN()
        {
            try
            {
                DataGridViewRow selectedRow = getSelectedRow(dataGridView1);
                int idNB = int.Parse(selectedRow.Cells["ID_NB"].Value.ToString());
                
                List<Xetnghiem> dsXN = db.getDSXN_NB(idNB);
                BindingSource bindingXN = new BindingSource { DataSource = dsXN };
                dataGridView2.DataSource = bindingXN;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public DataGridViewRow getSelectedRow(DataGridView input)
        {
            try
            {
                int selectedrowindex = input.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = input.Rows[selectedrowindex];
                return selectedRow;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            db.nhapNguoibenh(new Nguoibenh(demnb.ToString(), "Nam", "Nam",
                KetnoiDB.datetimetonumber(new DateTime(1995, 10, 10)),
                170, 55, 70, 25, 37, 0, 0, false, false, false, false, false, false, false, false));
            refreshNB();
            demnb++;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                DataGridViewRow selectedRow = getSelectedRow(dataGridView1);
                string cellValue = Convert.ToString(selectedRow.Cells["masoNB"].Value);
                textBox1.Text = cellValue;
                refreshXN();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = getSelectedRow(dataGridView1);
            int idNB = int.Parse(selectedRow.Cells["ID_NB"].Value.ToString());
            db.xoaNB(idNB);
            refreshNB();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = getSelectedRow(dataGridView1);
            int idNB = int.Parse(selectedRow.Cells["ID_NB"].Value.ToString());

            Nguoibenh chinhsuaNB = db.getNB(idNB);
            chinhsuaNB.masoNB = textBox1.Text;
            db.capnhatNB(chinhsuaNB);
            refreshNB();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = getSelectedRow(dataGridView1);
            int idNB = int.Parse(selectedRow.Cells["ID_NB"].Value.ToString());

            Xetnghiem addXN = new Xetnghiem(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            
            int idXN_vuanhap = db.nhapXetnghiem(addXN);
            db.nhapXN_NB(idNB, idXN_vuanhap);
            refreshXN();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            refreshXN();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedXN = getSelectedRow(dataGridView2);
            int idXN = int.Parse(selectedXN.Cells["IDXN"].Value.ToString());

            db.xoaXN(idXN);
            refreshXN();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                DataGridViewRow selectedRow = getSelectedRow(dataGridView2);
                string cellValue = Convert.ToString(selectedRow.Cells["albumin"].Value);
                textBox2.Text = cellValue;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedXN = getSelectedRow(dataGridView2);
            int idXN = int.Parse(selectedXN.Cells["IDXN"].Value.ToString());

            Xetnghiem suaXN = db.getXN(idXN);
            suaXN.albumin = double.Parse(textBox2.Text);

            db.capnhatXN(suaXN);
            refreshXN();
        }

    }
}
