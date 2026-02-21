using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nilaya_Project_Form
{
    public partial class Form4: Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
        }

        //This redirects to the registration form from the login page
        private void btnRegister_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtUsername.Text) 
                || string.IsNullOrWhiteSpace (txtPassword.Text) 
                || string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show(
                    "Please fill all the fileds as all the fields are required", 
                    "Validation Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
            }


                if (txtUsername.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                MessageBox.Show(
                    "Please enter username and password to proceed further", 
                    "Login Message", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                    );
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(
                    @"Server=DELL3400-5VPD10;
              Database=Asian_Paints_Nilaya;
              Trusted_Connection=True;
              TrustServerCertificate=True;"))
                {
                    con.Open();

                    //Validate user from tblRegister
                    string checkquery = @"
SELECT COUNT(*) 
FROM dbo.tblRegister
WHERE UserName = @UserName
  AND Password = @Password
  AND IsActive = 1";

                    using (SqlCommand cmd = new SqlCommand(checkquery, con))
                    {
                        cmd.Parameters.AddWithValue("@UserName", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                        int userExists = Convert.ToInt32(cmd.ExecuteScalar());

                        if (userExists == 0)
                        {
                            MessageBox.Show(
                                "Invalid username or password",
                                "Oops !!!, Login Failed Try Again",
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error
                                );
                            return;
                        }
                    }


                    //Check user is already logged in or not

                    string checkLogin = @"SELECT COUNT (*) FROM tblLogin WHERE  UserName=@UserName AND ISACTIVE=1";

                    using (SqlCommand cmd = new SqlCommand(checkLogin, con))
                    {
                        cmd.Parameters.AddWithValue("@UserName", txtUsername.Text.Trim());

                        int alreadyLoggedIn = Convert.ToInt32(cmd.ExecuteScalar());

                        if (alreadyLoggedIn > 0)
                        {
                            MessageBox.Show(
                                "User is already logged into this application ,please try with another user", 
                                "User Blocked",
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Warning
                                );
                            return;
                        }
                    }

                    //Save Login history -> here unlimited timesallowed
                    string insertLogin = @"
INSERT INTO tblLogin (UserName, Password, LoginDateTime, ISActive)
VALUES (@UserName, @Password, GETDATE(), 1)";


                    using (SqlCommand cmdLogin = new SqlCommand(insertLogin, con))
                    {
                        cmdLogin.Parameters.AddWithValue(@"UserName", txtUsername.Text.Trim());
                        cmdLogin.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                        cmdLogin.ExecuteNonQuery();
                    }

                }

                MessageBox.Show(
                    "Login Successfull",
                    "Welcome", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                //After login it should open form1 which is of Asian Paints --- Form1
                Form1 frm = new Form1();
                frm.Show();
                this.Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Login Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }

        }
    }
}
