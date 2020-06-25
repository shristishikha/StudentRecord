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
using Microsoft.VisualBasic;

namespace DA1
{
    public partial class Form1 : Form
    {
        MySqlConnection cn = new MySqlConnection("datasource = localhost; port = 3306; username = root; password =; database = DA");
        Form2 f2 = new Form2();
        Form3 f3;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            rdId.Checked = true;
            showAll("Select * from student");
            btnShowAll.Enabled = false;
            
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            btmDeleteAll.Enabled = false;
            try
            {
                if (rdId.Checked)
                {
                    int x = int.Parse(textBox1.Text);
                    showAll("select * from student where id = " + textBox1.Text);
                }
                else
                    showAll("select * from student where name = '" + textBox1.Text + "'");
                btnSearch.Enabled = true;
                textBox1.Enabled = true;
                rdId.Enabled = true;
                rdName.Enabled = true;
                btnShowAll.Enabled = true;
            }
            catch(Exception ex)
            {
                label2.Visible = true;
                dataGridView1.Visible = false;
                label2.Text = "No such ID exist...";
                btnModify.Enabled = false;
                btnDelete.Enabled = false;
                btnShowAll.Enabled = true;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            f2.Show();
            this.Visible = false;
        }
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            showAll("Select * from student");
            btnShowAll.Enabled = false;
        }
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                string s = Interaction.InputBox("Enter ID : ", "Modify", "0");
                if (s != "0" && s != "")
                {
                    int id = int.Parse(s);
                    DataTable dt = new DataTable();
                    MySqlCommand cmd = new MySqlCommand("select * from student where id = " + id, cn);
                    MySqlDataReader rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    if (dt.Rows.Count > 0)
                    {
                        f3 = new Form3(id, dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString(), dt.Rows[0][3].ToString(), dt.Rows[0][4].ToString());
                        f3.Show();
                        this.Visible = false;
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("Sorry ID " + id + " doesn't exist.", "Not Exist", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        cn.Close();
                        if (dr == DialogResult.OK)
                            btnModify_Click(sender, e);
                    }
                    cn.Close();
                }
                else
                {
                    if(s.Length != 0)
                        throw new Exception();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                DialogResult dr = MessageBox.Show("Enter a valid ID...", "Invalid Field Value", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                cn.Close();
                if (dr == DialogResult.OK)
                    btnModify_Click(sender, e);
            }
            cn.Close();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                string s = Interaction.InputBox("Enter ID : ", "Modify", "0");
                if (s != "0" && s != "")
                {
                    int id = int.Parse(s);
                    DataTable dt = new DataTable();
                    MySqlCommand cmd = new MySqlCommand("select * from student where id = " + id, cn);
                    MySqlDataReader rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    if (dt.Rows.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("Are you sure you want to delete record of ID " + id.ToString() + "? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            MySqlCommand comm = cn.CreateCommand();
                            comm.CommandText = "DELETE FROM Student where ID = " + id;
                            comm.ExecuteNonQuery();
                            MessageBox.Show("Successfully deleted record for ID " + id + ".", "Deletion Successful", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            cn.Close();
                            Form1_Load(sender, e);
                        }
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("Sorry ID " + id + " doesn't exist.", "Not Exist", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        cn.Close();
                        if (dr == DialogResult.OK)
                            btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                DialogResult dr = MessageBox.Show("Enter a valid ID...", "Invalid Field Value", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                cn.Close();
                if (dr == DialogResult.OK)
                    btnDelete_Click(sender, e);
            }
            cn.Close();
        }
        private void btmDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                MySqlCommand comm = cn.CreateCommand();
                comm.CommandText = "DELETE FROM Student";
                DialogResult dr = MessageBox.Show("Are you sure you want to delete all record? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Successfully deleted all records.", "Deletion Successful", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    cn.Close();
                    Form1_Load(sender, e);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while deleting records.", "Deletion Unuccessful", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                cn.Close();
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
        public void showAll(string qry)
        {
            try
            {
                cn.Open();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand(qry, cn);
                MySqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);

                btnSearch.Enabled = true;
                btnAdd.Enabled = true;
                btnModify.Enabled = true;
                btnDelete.Enabled = true;
                textBox1.Enabled = true;
                rdId.Enabled = true;
                btmDeleteAll.Enabled = true;
                rdName.Enabled = true;
                dataGridView1.Visible = false;
                label2.Visible = false;

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.Visible = true;
                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                }
                else
                {
                    label2.Text = "Sorry!! No Records!!!";
                    label2.Visible = true;
                    btnSearch.Enabled = false;
                    btnModify.Enabled = false;
                    btnDelete.Enabled = false;
                    textBox1.Enabled = false;
                    rdId.Enabled = false;
                    rdName.Enabled = false;
                    btmDeleteAll.Enabled = false;
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                dataGridView1.Visible = false;
                label2.Visible = true;
                label2.Text = "There seem to be some problem...";
                btnSearch.Enabled = false;
                btnAdd.Enabled = false;
                btnModify.Enabled = false;
                btnDelete.Enabled = false;
                textBox1.Enabled = false;
                btmDeleteAll.Enabled = false;
                rdId.Enabled = false;
                rdName.Enabled = false;
                cn.Close();
            }
        }
    }
}
