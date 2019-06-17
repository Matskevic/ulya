using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class Kurs : Form
    {
        public Kurs()
        {
            InitializeComponent();
        }
        MySqlConnectionStringBuilder mysqlCSB;
        private bool admin;
        public bool AD
        {
            set { admin = value; }
        }
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

        private void Kurs_Load(object sender, EventArgs e)
        {
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "obmen";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "12345";
            dataGridView1.DataSource = GetComments(@"SELECT id_pokupki,nazvanie Название, pokupak Покупка,prodacha Продажа FROM   pokupka_prodacha, valuta where pokupka_prodacha.id_valuti=valuta.id_valuti;");
            Razmer(dataGridView1);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            if (admin)
            {
                button1.Visible = true;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (admin)
            {
                NewVal f = new NewVal();
                f.D = false;
                List<string> s = new List<string>();
                int i = dataGridView1.CurrentRow.Index;
                s.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                s.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                s.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
                s.Add(dataGridView1.Rows[i].Cells[3].Value.ToString());
                f.S = s;
                f.Text = "Изменить валюту";
                f.ShowDialog();
                dataGridView1.DataSource = GetComments(@"SELECT id_pokupki,nazvanie Название, pokupak Покупка,prodacha Продажа FROM   pokupka_prodacha, valuta where pokupka_prodacha.id_valuti=valuta.id_valuti;");
                Razmer(dataGridView1);
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.Columns[0].Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewVal f = new NewVal();
            f.D = true;
            f.Text = "Добавить валюту";
            f.ShowDialog();
            dataGridView1.DataSource = GetComments(@"SELECT id_pokupki,nazvanie Название, pokupak Покупка,prodacha Продажа FROM   pokupka_prodacha, valuta where pokupka_prodacha.id_valuti=valuta.id_valuti;");
            Razmer(dataGridView1);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
        }
    }
}
