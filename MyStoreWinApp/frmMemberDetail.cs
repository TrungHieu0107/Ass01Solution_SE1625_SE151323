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
using BusinessObject;

namespace MyStoreWinApp
{
    public partial class frmMemberDetail : Form
    {
        List<string> Country = new List<string>() {
            "Việt Nam", "Campuchia"
        };
        List<string> CityOfVietNam = new List<string>() {
            "Hà Nội",
            "TP HCM",
            "Đà Nẵng",
            "Bình Dương",
            "Đồng Nai",
            "Long An",
            "Cao Bằng",
            "Thừa Thiên - Huế"
        };
        List<string> CityOfCampuchia= new List<string>() {
            "Phnom Penh",
            "Siem Reap",
            "Battambang",
            "Serei Saophoan",
            "Pursat",
            "Samraong",
            "Stung Saen",
            "Chbar Mon"
        };
        public IMemberRepository MemberRepository { get; set; }
        public bool UpdateFlag { get; set; }

        public MemberObject MemberObject { get; set; }
        public frmMemberDetail()
        {
            InitializeComponent();
            cboCountry.DataSource = Country;
            cboCity.DataSource = CityOfVietNam;
        }

        private void frmMemberDetail_Load(object sender, EventArgs e)
        {
            txtMemberID.Enabled = !UpdateFlag;
            txtEmail.Enabled = !UpdateFlag;
            if (UpdateFlag) {
                txtMemberID.Text = MemberObject.MemberID.ToString();
                txtMemberName.Text = MemberObject.MemberName;
                txtEmail.Text = MemberObject.Email;
                txtPassword.Text = MemberObject.Password;
                txtDateTime.Text = MemberObject.DateOfBirth.ToShortDateString();
                cboCity.Text = MemberObject.City;
                cboCountry.Text = MemberObject.Country;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try {
                var Member = new MemberObject() {
                    MemberID = int.Parse(txtMemberID.Text),
                    MemberName = txtMemberName.Text,
                    DateOfBirth = txtDateTime.Value,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    City = cboCity.Text,
                    Country = cboCountry.Text,
                };

                (string msg, bool flag) check = CheckValidMember(Member);
                if (check.flag) { //flag == true
                    if (UpdateFlag == true) {
                        MemberRepository.UpdateMember(Member);
                        MessageBox.Show("Update successfully!!!");
                    }
                    if (UpdateFlag == false) {
                        MemberRepository.InsertMember(Member);
                    }
                }
                else {
                    MessageBox.Show(check.msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                }

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, UpdateFlag ? "Update Member Err" : "Insert member err");
                DialogResult = DialogResult.None;
            }
        }

        private (string msg, bool flag) CheckValidMember(MemberObject member)
        {
            string Msg = "";
            bool Flag = true;

            if (member.MemberName.Length == 0) {
                Flag = false;
                Msg += "Please input name \n";
            }

            //Check valid Email
            var trimmedEmail = member.Email.Trim();
            if (trimmedEmail.Length == 0) {
                Flag |= false;
                Msg += "Please input email \n";
            }
            else {
                if (trimmedEmail.EndsWith(".")) {
                    Flag = false;
                    Msg += "Invalid email \n";
                }
                else {
                    if (!trimmedEmail.Contains(".")) {
                        Flag = false;
                        Msg += "Invalid email \n";
                    }
                    else {
                        try {
                            var addr = new System.Net.Mail.MailAddress(member.Email);
                            Flag = (addr.Address == trimmedEmail);
                        }
                        catch {
                            Flag = false;
                            Msg += "Invalid email \n";
                        }
                    }

                }
            }


            //Check password 
            if (member.Password.Length == 0) {
                Flag = false;
                Msg += "Please input password \n";
            }
            else if (member.Password.Trim().Contains(' ')) {
                Flag = false;
                Msg += "Password not contain white space \n";
            }

            //check city
            if (member.City.Length == 0) {
                Flag = false;
                Msg += "Please input city \n";
            }

            //check country
            if (member.Country.Length == 0) {
                Flag = false;
                Msg += "Please input country \n";
            }
            //check date time
            DateTime today = DateTime.Now;
            TimeSpan Time = today - member.DateOfBirth;
            var NumberOfDays = Time.Days;
            if (NumberOfDays < 18 * 12 * 30) {
                Flag = false;
                Msg += "You are not old enough \n";
            }


            return (Msg, Flag);


        }

        private void cboCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboCountry.Text == "Việt Nam") {
                cboCity.DataSource = CityOfVietNam;
            } else {
                cboCity.DataSource = CityOfCampuchia;
            }
        }
    }
}
