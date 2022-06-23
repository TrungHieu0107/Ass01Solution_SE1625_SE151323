using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess.Repository;
namespace MyStoreWinApp
{
    public partial class frmLogin : Form
    {
        IMemberRepository memberRepository = new MemberRepository();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var Email = txtEmail.Text;
            var Password = txtPassword.Text;
            //Check valid password
            bool PasswordFlag = Password.Trim().Length == 0;
            bool EmailFlag = true;
            var trimmedEmail = Email.Trim();

            //Check valid Email
            if (trimmedEmail.EndsWith(".")) {
                EmailFlag = false;
            }
            else {
                if (!trimmedEmail.Contains(".")) {
                    EmailFlag &= false;
                } else {
                    try {
                        var addr = new System.Net.Mail.MailAddress(Email);
                        EmailFlag = (addr.Address == trimmedEmail);
                    }
                    catch {
                        EmailFlag = false;
                    }
                }
                
            }

            

            if (EmailFlag && !PasswordFlag) {
                try {
                    string role = memberRepository.CheckLogin(Email, Password);
                    if (role == "admin") {
                        this.Hide();
                        frmMemberManagement frmMemberManagement = new frmMemberManagement();
                        frmMemberManagement.ShowDialog();
                    }
                    else if (role == "user") {
                        this.Hide();
                        frmMemberDetail frm = new frmMemberDetail() {
                            Text = "Update Account",

                            MemberObject = memberRepository.GetMemberByEmail(Email),
                            MemberRepository = memberRepository,
                            UpdateFlag = true
                        };

                        frm.ShowDialog();
                    }

                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            } else {
                if (!EmailFlag) {
                    MessageBox.Show("Invalid email");
                } else if (PasswordFlag) {
                    MessageBox.Show("Please input password");
                }

            }

        }
    }
}
