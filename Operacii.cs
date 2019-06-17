using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class Operacii : Form
    {
        public Operacii()
        {
            InitializeComponent();
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
            if (dg.RowCount > 5)
            {
                dg.Height = 150;
            }
            else
                dg.Height = dg.RowCount * 40;
            return dg;
        }
        private void Stolb()
        {
            dataGridView1.Columns[0].HeaderText = "Фамилия сотрудника";
            dataGridView1.Columns[1].HeaderText = "Фамилия покупателя";
            dataGridView1.Columns[2].HeaderText = "Тип операции";
            dataGridView1.Columns[3].HeaderText = "Дата";
            dataGridView1.Columns[4].HeaderText = "Сумма ввода";
            dataGridView1.Columns[5].HeaderText = "Итог";
            dataGridView1.RowHeadersVisible = false;
            Razmer(dataGridView1);
        }
        private void Operacii_Load(object sender, EventArgs e)
        {
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "obmen";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "12345";
            dataGridView1.DataSource = GetComments(@"SELECT sotrudnik.familia, pokupatel.familia,tip_operacii.operacia, data,summa_vvoda,itog 
FROM tip_operacii, kassa, operacia o,sotrudnik,pokupatel 
where o.id_sotrudika=sotrudnik.id_sotrudika AND o.id_pokupatela=pokupatel.id_pokupatela AND o.id_tipa=tip_operacii.id_tipa AND o.id_kassi=kassa.id_kassi;");
            Stolb();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetComments(@"SELECT sotrudnik.familia, pokupatel.familia,tip_operacii.operacia, data,summa_vvoda,itog 
FROM tip_operacii, kassa, operacia o,sotrudnik,pokupatel 
where o.id_sotrudika=sotrudnik.id_sotrudika AND o.id_pokupatela=pokupatel.id_pokupatela AND o.id_tipa=tip_operacii.id_tipa AND o.id_kassi=kassa.id_kassi
ORDER BY data;");
            Stolb();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetComments(@"SELECT sotrudnik.familia, pokupatel.familia,tip_operacii.operacia, data,summa_vvoda,itog 
FROM tip_operacii, kassa, operacia o,sotrudnik,pokupatel 
where o.id_sotrudika=sotrudnik.id_sotrudika AND o.id_pokupatela=pokupatel.id_pokupatela AND o.id_tipa=tip_operacii.id_tipa AND o.id_kassi=kassa.id_kassi
ORDER BY sotrudnik.familia;");
            Stolb();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NewOper f = new NewOper();
            f.ShowDialog();
            dataGridView1.DataSource = GetComments(@"SELECT sotrudnik.familia, pokupatel.familia,tip_operacii.operacia, data,summa_vvoda,itog 
FROM tip_operacii, kassa, operacia o,sotrudnik,pokupatel 
where o.id_sotrudika=sotrudnik.id_sotrudika AND o.id_pokupatela=pokupatel.id_pokupatela AND o.id_tipa=tip_operacii.id_tipa AND o.id_kassi=kassa.id_kassi;");
            Stolb();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int c = dataGridView1.CurrentCell.ColumnIndex;
            if (c==0)
            {
                Sortudniki f = new Sortudniki();
                f.Show();
                int n = dataGridView1.CurrentRow.Index;
                f.TB(dataGridView1.Rows[n].Cells[c].Value.ToString());
            }
            else
                if (c==1)
            {
                Pokupateli f = new Pokupateli();
                f.Show();
                int n = dataGridView1.CurrentRow.Index;
                f.TB(dataGridView1.Rows[n].Cells[c].Value.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
