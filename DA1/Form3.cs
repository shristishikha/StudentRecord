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
    public partial class Form3 : Form
    {
        int id;
        string nm, fnm, mob, ads;
        MySqlConnection cn = new MySqlConnection("datasource = localhost; port = 3306; username = root; password =; database = DA");
        public Form3(int id, string nm, string fnm, string mob, string ads)
        {
            InitializeComponent();
            this.id = id;
            this.nm = nm;
            this.fnm = fnm;
            this.mob = mob;
            this.ads = ads;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            txtName.Text = nm;
            txtFName.Text = fnm;
            txtMob.Text = mob;
            txtAdd.Text = ads;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                if (check())
                {
                    MySqlCommand comm = cn.CreateCommand();
                    comm.CommandText = "UPDATE student SET name = '" + txtName.Text.Trim() + "', f_name = '" + txtFName.Text.Trim() + "', mobile = '" + txtMob.Text.Trim() + "', address = '" + txtAdd.Text.Trim() + "' where id = " + id.ToString();
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Successfully updated record for ID " + id.ToString(), "Updation Successful", MessageBoxButtons.OKCancel);
                    btnClose.Text = "Close";
                }
                else
                    throw new Exception();
                cn.Close();
            }
            catch (Exception ex)
            {
                btnClose.Text = "Cancel";
                DialogResult dr = MessageBox.Show("Improper input...", "Updation Unsuccessful", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (dr == DialogResult.OK)
                    Form3_Load(sender, e);
                cn.Close();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Dispose();
        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
        }
        public bool check()
        {
            if (txtName.Text.Trim() == "" || txtFName.Text.Trim() == "" || txtMob.Text.Trim() == "" || txtAdd.Text.Trim() == "")
                return false;
            string st = @"^[1-9][0-9]{9}$";
            Regex re = new Regex(st);
            if (!re.IsMatch(txtMob.Text.Trim()))
                return false;
            return true;
        }
    }
}
