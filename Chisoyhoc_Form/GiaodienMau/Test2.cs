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
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;


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
            textBox5.Text = "C_A01-C_A02-C_A03-C_A04-C_A05-C_A06-C_A07-C_A08-C_A09-C_A10-C_A11-C_A12-C_A13-C_A14-C_A15-C_A29-C_A16-C_A17-C_A18-C_A19-C_A20-C_A21-C_A22-C_A23-C_A24-C_A25-C_A26-C_A27-C_A28";
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
            List<string> name = input.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            List<List<string>> show = db.GetdiengiaiNCKH(input);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Save Excel Files";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = "Data"; // Default filename

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = Path.GetDirectoryName(saveFileDialog.FileName);

                for (int i = 0; i < Math.Min(name.Count, show.Count); i++)
                {
                    string fileName = name[i];
                    string filePath = Path.Combine(folderPath, fileName);

                    // Create Excel application
                    Excel.Application excelApp = new Excel.Application();
                    excelApp.Visible = false;
                    excelApp.DisplayAlerts = false;

                    // Create a new workbook
                    Excel.Workbook workbook = excelApp.Workbooks.Add();

                    // Get the first worksheet
                    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];

                    // Write data to the worksheet
                    for (int row = 0; row < show[i].Count; row++)
                    {
                        // Write each item in the current row to the corresponding cell
                        worksheet.Cells[row + 1, 1] = show[i][row];
                    }

                    // Save the workbook
                    workbook.SaveAs(filePath);
                    workbook.Close();

                    // Close the Excel application
                    excelApp.Quit();
                }

                MessageBox.Show("Excel files have been exported to the selected folder.");
            }
        }
    }
}
