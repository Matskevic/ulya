using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class Pokupateli : Form
    {
        public Pokupateli()
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
            dg.RowHeadersVisible = false;
            if (dg.RowCount > 5)
            {
                dg.Height = 150;
            }
            else
                dg.Height = dg.RowCount * 40;
            return dg;
        }
        public void TB(string s)
        {
            dataGridView1.ClearSelection();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == s)
                    dataGridView1.Rows[i].Selected = true;
            }
        }
        private void Pokupateli_Load(object sender, EventArgs e)
        {
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "obmen";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "12345";
            dataGridView1.DataSource = GetComments(@"SELECT familia Фамилия,ima Имя,otchestvo Отчество, seria,nomer FROM   pokupatel,pasport WHERE pokupatel.id_pasporta=pasport.id_pasporta;");
            dataGridView1.Columns[3].HeaderText = "Серия пасспорта";
            dataGridView1.Columns[4].HeaderText = "Номер пасспорта";
            Razmer(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rez f = new Rez();
            string c = textBox1.Text;
            f.DT = GetComments(@"SELECT familia Фамилия,ima Имя,otchestvo Отчество, seria,nomer FROM   pokupatel,pasport WHERE pokupatel.id_pasporta=pasport.id_pasporta AND familia='" + c + "';");
            f.Show();
        }
    }
}
