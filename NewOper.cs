using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class NewOper : Form
    {
        public NewOper()
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

        private void NewOper_Load(object sender, EventArgs e)
        {
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "obmen";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "12345";
            dataGridView1.DataSource = GetComments(@"SELECT familia,id_sotrudika from sotrudnik where status='работает';");
            for (int i=0;i<dataGridView1.RowCount;i++)
            {
                comboBox1.Items.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            comboBox1.SelectedIndex = 0;
            dataGridView2.DataSource = GetComments(@"SELECT operacia,id_tipa from tip_operacii");
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
            }
            comboBox2.SelectedIndex = 0;
            dataGridView3.DataSource = GetComments(@"SELECT nazvanie,id_valuti from valuta");
            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
            }
            comboBox3.SelectedIndex = 0;
        }
        private string ID(DataGridView dg, string s)
        {
            string id="";
            for(int i=0;i<dg.RowCount;i++)
            {
                if (dg.Rows[i].Cells[0].Value.ToString()==s)
                {
                    id = dg.Rows[i].Cells[1].Value.ToString();
                    break;
                }
            }
            return id;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string id_s = ID(dataGridView1, comboBox1.SelectedItem.ToString());
                string id_t = ID(dataGridView2, comboBox2.SelectedItem.ToString());
                string id_v = ID(dataGridView3, comboBox3.SelectedItem.ToString());
                string f = textBox1.Text;
                string i = textBox2.Text;
                string o = textBox3.Text;
                string n = textBox4.Text;
                string s = textBox5.Text;
                string sum = textBox6.Text;
                string itog = textBox7.Text;
                string com = "insert into kassa (summa_vvoda, itog, id_pokupki) values (" + sum + ", " + itog + "," + id_v + "); ";
                GetComments(com);
                dataGridView4.DataSource = GetComments("SELECT id_kassi from kassa;");
                string id_k = dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[0].Value.ToString();
                com = "insert into pasport (nomer, seria) values("+n+", \""+s+"\"); ";
                GetComments(com);
                dataGridView4.DataSource = GetComments("SELECT id_pasporta from pasport;");
                string id_pas = dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[0].Value.ToString();
                com = "insert into pokupatel (familia, ima, otchestvo,id_pasporta) values (\""+f+"\", \""+i+"\", \""+o+"\","+id_pas+");";
                GetComments(com);
                dataGridView4.DataSource = GetComments("SELECT id_pokupatela from pokupatel;");
                string id_p = dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[0].Value.ToString();
                com = "insert into operacia (id_sotrudika, id_tipa,id_kassi, id_pokupatela, data) values (" + id_s + "," + id_t + "," + id_k + "," + id_p + ", NOW())";
                GetComments(com);
                MessageBox.Show("Операция успешно добавдена!");
            }
            catch
            { MessageBox.Show("Проверьте введённые данные!","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Warning); }
        }
    }
}
