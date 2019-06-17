using System;
using System.Windows.Forms;

namespace Курсовой
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool admin = false;
        public bool ADMIN
        {
            set { admin = value; }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();
            f.Owner = this;
            f.ShowDialog();
            if (admin)
            {
                button5.Visible = false;
                button6.Visible = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button5.Visible = true;
            button6.Visible = false;
            admin = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Operacii f = new Operacii();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kurs f = new Kurs();
            f.AD = admin;
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Sortudniki f = new Sortudniki();
            f.Adm = admin;
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Pokupateli f = new Pokupateli();
            f.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            About f = new About();
            f.Show();
        }
    }
}
