using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace DA1
{
    public partial class Form2 : Form
    {
        MySqlConnection cn = new MySqlConnection("datasource = localhost; port = 3306; username = root; password =; database = DA");

        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            btnAddAn.Visible = false;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                MySqlCommand comm = cn.CreateCommand();
                if (check())
                {
                    DataTable dt = new DataTable();
                    comm.CommandText = "INSERT INTO student VALUES (NULL, '" + textBox1.Text.Trim() + "','" + textBox2.Text.Trim() + "','" + textBox3.Text.Trim() + "','" + textBox4.Text.Trim() + "')";
                    comm.ExecuteNonQuery();
                    MySqlCommand cmd = new MySqlCommand("select max(id) from student", cn);
                    MySqlDataReader rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    MessageBox.Show("Insertion Successful...\n" + textBox1.Text + " ID is " + (int.Parse(dt.Rows[0][0].ToString())), "Insertion Successful", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else
                    throw new Exception();
                cn.Close();
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                btnAddAn.Visible = true;
                btnAdd.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Enter proper values...", "Improper Arguments", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                cn.Close();
            }
        }
        private void btnAddAn_Click(object sender, EventArgs e)
        {
            btnAdd.Visible = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            Form2_Load(sender, e);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Dispose();
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
        }
        public bool check()
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "")
                return false;
            string st = @"^[1-9][0-9]{9}$";
            Regex re = new Regex(st);
            if (!re.IsMatch(textBox3.Text.Trim()))
                return false;
            return true;
        }

    }
}
