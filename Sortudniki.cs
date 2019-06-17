using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class Sortudniki : Form
    {
        public Sortudniki()
        {
            InitializeComponent();
        }
        private bool admin;
        public bool Adm
        {
            set { admin = value; }
        }
        MySqlConnectionStringBuilder mysqlCSB;
        private DataTable GetComments(string queryString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mysqlCSB.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }
        private DataGridView Razmer(DataGridView dg)
        {
            dg.RowHeadersVisible = false;
            if (dg.RowCount > 5)
            {
                dg.Height = 150;
            }
            else
                dg.Height = dg.RowCount * 40;
            return dg;
        }
        private void Sortudniki_Load(object sender, EventArgs e)
        {
            if (admin)
                button1.Visible = true;
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "obmen";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "12345";
            dataGridView1.DataSource = GetComments(@"SELECT id_sotrudika, familia Фамилия,ima Имя,otchestvo Отчество,telefon Телефон, dolchosts Должность, rabochaa_smena 
FROM   sotrudnik, smena,dolchnost 
where sotrudnik.id_dolchnosti=dolchnost.id_dolchnosti AND sotrudnik.id_smeni=smena.id_smeni AND status='работает';");
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[6].HeaderText = "Рабочая смена";
            Razmer(dataGridView1);
        }
        public void TB(string s)
        {
            dataGridView1.ClearSelection();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value.ToString() == s)
                {
                    dataGridView1.Rows[i].Selected = true;
                    return;
                }
            }
            MessageBox.Show("Сотрудник уволен");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            NewSotr f = new NewSotr();
            f.ShowDialog();
            dataGridView1.DataSource = GetComments(@"SELECT id_sotrudika, familia Фамилия,ima Имя,otchestvo Отчество,telefon Телефон, dolchosts Должность, rabochaa_smena 
FROM   sotrudnik, smena,dolchnost 
where sotrudnik.id_dolchnosti=dolchnost.id_dolchnosti AND sotrudnik.id_smeni=smena.id_smeni;");
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[6].HeaderText = "Рабочая смена";
            Razmer(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (admin)
            {
                int n = dataGridView1.CurrentRow.Index;
                List<string> l = new List<string>();
                l.Add(dataGridView1.Rows[n].Cells[0].Value.ToString());
                l.Add(dataGridView1.Rows[n].Cells[1].Value.ToString());
                l.Add(dataGridView1.Rows[n].Cells[2].Value.ToString());
                l.Add(dataGridView1.Rows[n].Cells[3].Value.ToString());
                l.Add(dataGridView1.Rows[n].Cells[4].Value.ToString());
                l.Add(dataGridView1.Rows[n].Cells[5].Value.ToString());
                l.Add(dataGridView1.Rows[n].Cells[6].Value.ToString());
                NewSotr f = new NewSotr();
                f.Li = l;
                f.ShowDialog();
                dataGridView1.DataSource = GetComments(@"SELECT id_sotrudika, familia Фамилия,ima Имя,otchestvo Отчество,telefon Телефон, dolchosts Должность, rabochaa_smena 
FROM   sotrudnik, smena,dolchnost 
where sotrudnik.id_dolchnosti=dolchnost.id_dolchnosti AND sotrudnik.id_smeni=smena.id_smeni AND status='работает';");
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[6].HeaderText = "Рабочая смена";
                Razmer(dataGridView1);
            }
        }
    }
}
