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
    public partial class Test2 : Form
    {
        public DataTable kq2;
        public Test2()
        {
            InitializeComponent();
        }

        private void Test2_Load(object sender, EventArgs e)
        {
            List<ComboBoxItem> itemList = new List<ComboBoxItem>{
                new ComboBoxItem { ID = 1, DisplayText = "Calculator 1" },
                new ComboBoxItem { ID = 2, DisplayText = "Calculator 2" }};

            comboBox1.DataSource = itemList;
            comboBox1.DisplayMember = "DisplayText";
            comboBox1.ValueMember = "ID";

            KetnoiDB db = new KetnoiDB();

            dataGridView1.DataSource = db.GetDSchisoyhoc();
            textBox5.Text = "C_A01-C_A02";
            //Chisoyhoc a = new Chisoyhoc()
        }
        public class ComboBoxItem
        {
            public string DisplayText { get; set; }
            public int ID { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((ComboBoxItem)comboBox1.SelectedItem).ID.ToString());
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            Tuongtac.ReadCSV a = new Tuongtac.ReadCSV(filePath);
            DataTable kq = a.readCSV;
            textBox3.Text = filePath;

            Tuongtac b = new Tuongtac("");

            kq2 = b.tinhketqua(kq);
            dataGridView3.DataSource = kq2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string filePath = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }

            Tuongtac a = new Tuongtac(filePath);
            a.exceltoCSV(filePath);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string filePath = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }

            Tuongtac a = new Tuongtac(filePath);
            a.CSVtoexcel(filePath);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.InitialDirectory = "c:\\";

            string filePath = "";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                filePath = savefile.FileName;
            }

            Tuongtac.DatatableToCSV(kq2, filePath);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string input = textBox5.Text;
            KetnoiDB db = new KetnoiDB();
            List<string> show = db.GetdiengiaiNCKH(input);

            StringBuilder a = new StringBuilder();
            foreach (string i in show)
            {
                a.Append(i);
            }
            textBox4.Text = a.ToString();
        }

    }
}
