using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class Rez : Form
    {
        public Rez()
        {
            InitializeComponent();
        }
        private DataTable dt;
        public DataTable DT
        {
            set { dt = value; }
        }
        private DataGridView Razmer(DataGridView dg)
        {
            dg.Width = dg.ColumnCount * 100 + 45;
            dg.RowHeadersVisible = false;
            if (dg.RowCount > 5)
            {
                dg.Height = 150;
            }
            else
                dg.Height = dg.RowCount * 50;
            return dg;
        }
        private void Rez_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dt;
            Razmer(dataGridView1);
            if (this.Width < dataGridView1.Width)
            {
                this.Width = dataGridView1.Width + 100;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BaseFont bs = BaseFont.CreateFont(Application.StartupPath + @"\Times New Roman.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font f = new iTextSharp.text.Font(bs);
            PdfPTable pdfTable = new PdfPTable(dataGridView1.ColumnCount);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 30;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, f));
                cell.BackgroundColor = new iTextSharp.text.Color(240, 240, 240);
                pdfTable.AddCell(cell);
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    string c = cell.Value.ToString();
                    PdfPCell col = new PdfPCell(new Phrase(c, f));
                    pdfTable.AddCell(col);
                }
            }
            saveFileDialog1.Filter = "PDF файлы(.pdf)|*.pdf|Все файлы|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string s = saveFileDialog1.FileName;
                using (FileStream stream = new FileStream(s, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
            }
        }
    }
}

