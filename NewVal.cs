using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class NewVal : Form
    {
        public NewVal()
        {
            InitializeComponent();
        }
        MySqlConnectionStringBuilder mysqlCSB;
        private bool dob = true;
        private List<string> s;
        public List<string> S
        {
            set { s = value; }
        }
        public bool D
        {
            set { dob = value; }
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
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string n = textBox1.Text;
                string pok = textBox2.Text;
                string prod = textBox3.Text;
                string con = "insert into valuta (nazvanie) values(\""+n+"\"); ";
                GetComments(con);
                dataGridView1.DataSource = GetComments("select id_valuti from valuta where nazvanie=\"" + n + "\";");
                string id_v = dataGridView1.Rows[0].Cells[0].Value.ToString();
                con = "insert into pokupka_prodacha (id_valuti,pokupak, prodacha) values(" + id_v + "," + pok + "," + prod + ");";
                GetComments(con);
            }
            catch
            { MessageBox.Show("Проверьте введённые данные!"); }
            dataGridView1.DataSource = GetComments("SELECT * from valuta");
        }

        private void NewVal_Load(object sender, EventArgs e)
        {
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "obmen";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "12345";
            if (dob)
            {
                button1.Visible = true;
            }
            else
            {
                button2.Visible = true;
                textBox1.Text = s[1];
                textBox2.Text = s[2];
                textBox3.Text = s[3];
                dataGridView1.DataSource = GetComments("SELECT id_valuti from pokupka_prodacha where id_valuti=(select id_valuti from valuta where nazvanie=\"" + s[1] + "\");");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string n = textBox1.Text;
                string pok = textBox2.Text;
                string prod = textBox3.Text;
                string id_v = dataGridView1.Rows[0].Cells[0].Value.ToString();
                string com = "update valuta set nazvanie=\"" + n + "\" where id_valuti=" + id_v + ";";
                GetComments(com);
                com = "update pokupka_prodacha set id_valuti=" + id_v + ", pokupak=" + pok + ", prodacha=" + prod + " where id_pokupki =" + s[0] + ";";
                GetComments(com);
                MessageBox.Show("Успешно!");
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
    }
}
