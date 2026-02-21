using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using System.Windows.Forms;

namespace Nilaya_Project_Form
{
    public partial class Form2: Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtName.Clear();
            txtPassword.Clear();
            txtMobileNo.Clear();
            txtConfirmPassword.Clear();
        }

        private void bthRegister_Click(object sender, EventArgs e)
        {
            //Validation
            if (
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtMobileNo.Text) ||
        string.IsNullOrWhiteSpace(txtPassword.Text) ||
        string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show(
                    "Please Fill all the fields as all fields are mandory",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );
                return;
            }


            //Mobile Number validation
            if (!long.TryParse(txtMobileNo.Text, out _) ||
                txtMobileNo.Text.Length != 10)
            {
                MessageBox.Show("" +
                    "Enter a valid 10 digit mobile number.",
                    "Validation Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                    );
                txtMobileNo.Focus();
                return;
            }

            //Password Match Check
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(
                    "Password nd Confirm Password do no match!!. Please try again ",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                txtConfirmPassword.Focus();
                return;
            }
                
            //Minimum password length logic

            if(txtPassword.Text.Length < 8)
            {
                MessageBox.Show(
                    "Password must be atleast 8 character long",
                    "Password Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );
                txtPassword.Focus();
                return;
            }

            try
            {
                using (
                   SqlConnection con = new SqlConnection(
                    @"Server=DELL3400-5VPD10;
              Database=Asian_Paints_Nilaya;
              Trusted_Connection=True;
              TrustServerCertificate=True;"))
                {
                    con.Open();

                    //Check if username exists
                    string checkUser = "SELECT COUNT (*) FROM dbo.tblRegister WHERE UserName = @UserName";

                    using(SqlCommand checkcmd = new SqlCommand(checkUser, con))
                    {
                        checkcmd.Parameters.AddWithValue("@UserName", txtUsername.Text.Trim());


                        if ((int)checkcmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("UserName already exists", "Exists Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    //Insert user

                    string insertQuery = @"
INSERT INTO dbo.tblRegister
(Name, UserName, MobileNo, Password, IsActive, CreatedDate)
VALUES
(@Name, @UserName, @MobileNo, @Password, 1, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@UserName", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim()); 

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
               "Registration Successfull",
               "Success",
               MessageBoxButtons.OK,
               MessageBoxIcon.Information
               );

                Form4 frm = new Form4();
                frm.Show();
                this.Hide();

                //Clear Fields after successful registration

                txtName.Clear();
                txtUsername.Clear();
                txtMobileNo.Clear();
                txtPassword.Clear();
                txtConfirmPassword.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message, 
                    "Registration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
            //Registration Successfull but not saved to db
           

           
                
        }

        private void btnGotoLogin_Click(object sender, EventArgs e)
        {
                Form4 frm = new Form4();
                frm.Show();
                this.Hide();
        }

        private void btnSaveDB_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == ""||
                txtUsername.Text.Trim() == ""  ||
                txtMobileNo.Text.Trim() == "" ||
                txtPassword.Text.Trim() == ""  ||
                txtConfirmPassword.Text.Trim() == ""
                )
                {
                MessageBox.Show(
                    "All the fields are required, Please fill all the fields to save to database", 
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );
            }

            try
            {
                using (
                    SqlConnection con = new SqlConnection(
                    @"Server=DELL3400-5VPD10;
Database=Asian_Paints_Nilaya;
Trusted_Connection=True;
TrustServerCertificate=True;"
))
                {
                    con.Open();
                    string query = @"INSERT INTO do.tblRegister
(Name, UserName, Mobile No, Password, ISActive,CreatedDate)
VALUES
(@Name, @UserName, @MobileNo, @ISActive@GETDATE())";

                    using(SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@UserName", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show(
                    "Registration details saved successfully", 
                    "Success", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                    );

                //Clearing fields after success
                txtName.Clear();
                txtUsername.Clear();
                txtPassword.Clear();
                txtMobileNo.Clear();
                txtConfirmPassword.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message, 
                    "Database Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
