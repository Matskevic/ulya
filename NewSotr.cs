using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class NewSotr : Form
    {
        public NewSotr()
        {
            InitializeComponent();
        }
        MySqlConnectionStringBuilder mysqlCSB;
        List<string> l;
        bool fl = true;
        public List<string> Li
        {
            set
            {
                l = value;
                fl = false;
                button1.Text = "Изменить";
                this.Text = "Изменить сотрудника";
            }
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
        private void Izm()
        {
            textBox1.Text = l[1];
            textBox2.Text = l[2];
            textBox3.Text = l[3];
            textBox4.Text = l[4];
            comboBox1.SelectedItem = l[5];
            comboBox2.SelectedItem = l[6];
            button2.Visible = true;
        }
        private void NewSotr_Load(object sender, EventArgs e)
        {
            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "obmen";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "12345";
            dataGridView1.DataSource = GetComments("select * from dolchnost;");
            for (int i=0;i<dataGridView1.RowCount;i++)
            {
                comboBox1.Items.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
            }
            comboBox1.SelectedItem = 0;
            dataGridView2.DataSource = GetComments("select * from smena;");
            for (int i=0;i<dataGridView2.RowCount;i++)
            {
                comboBox2.Items.Add(dataGridView2.Rows[i].Cells[1].Value.ToString());
            }
            comboBox2.SelectedItem = 0;
            if (fl==false)
                Izm();
        }
        private string ID(DataGridView dg, string s)
        {
            string id = "";
            for (int i = 0; i < dg.RowCount; i++)
            {
                if (dg.Rows[i].Cells[1].Value.ToString() == s)
                {
                    id = dg.Rows[i].Cells[0].Value.ToString();
                    break;
                }
            }
            return id;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string f = textBox1.Text;
                string i = textBox2.Text;
                string o = textBox3.Text;
                string tel = textBox4.Text;
                string id_d = ID(dataGridView1, comboBox1.SelectedItem.ToString());
                string id_s = ID(dataGridView2, comboBox2.SelectedItem.ToString());
                if (fl)
                {
                    string com = "insert into sotrudnik (familia, ima, otchestvo, telefon, id_dolchnosti, id_smeni) values(\"" + f + "\", \"" + i + "\", \"" + o + "\"," + tel + " ," + id_d + "," + id_s + "); ";
                    GetComments(com);
                    MessageBox.Show("Сотрудник успешно добавлен");
                    this.Close();
                }
                else
                {
                    string com = "update sotrudnik set familia='" + f + "',ima ='" + i + "',otchestvo ='" + o + "',telefon =" + tel + ",id_dolchnosti =" + id_d + ",id_smeni =" + id_s + " where id_sotrudika=" + l[0] + ";";
                    GetComments(com);
                    MessageBox.Show("Сотрудник успешно изменён");
                    this.Close();
                }
            }
            catch
            { MessageBox.Show("Проверьте введённые данные!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string com = "update sotrudnik set status='уволен' where id_sotrudika=" + l[0] + ";";
            GetComments(com);
            MessageBox.Show("Сотрудник уволен");
            this.Close();
        }
    }
}
