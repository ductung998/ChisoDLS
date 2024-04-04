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

            dataGridView1.DataSource = db.GetDSchisoyhoc(); ;

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

    }
}
