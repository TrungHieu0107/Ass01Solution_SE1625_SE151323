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

    public partial class frmMemberManagement : Form
    {
        IMemberRepository memberRepository = new MemberRepository();
        BindingSource source;
        List<string> CityList = new List<string>() {
            "Hà Nội",
            "TP HCM",
            "Đà Nẵng",
            "Bình Dương",
            "Đồng Nai",
            "Long An",
            "Cao Bằng",
            "Thừa Thiên - Huế",
            "Phnom Penh",
            "Siem Reap",
            "Battambang",
            "Serei Saophoan",
            "Pursat",
            "Samraong",
            "Stung Saen",
            "Chbar Mon"
        };
        public frmMemberManagement()
        {
            InitializeComponent();
            cboFilter.DataSource = CityList;
        }

        private void frmMemberManagement_Load(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            //dataGridViewMember.CellDoubleClick += dataGridViewMember_CellDoubleClick;
            txtDateTime.Format = DateTimePickerFormat.Custom;
            // Display the date as "Mon 27 Feb 2012".  
            txtDateTime.CustomFormat = "ddd dd MMM yyyy";
        }

        private MemberObject GetMember()
        {
            MemberObject member = null;
            try {
                member = new MemberObject() {
                    MemberID = int.Parse(txtMemberID.Text),
                    MemberName = txtMemberName.Text,
                    City = txtCity.Text,
                    Country = txtCountry.Text,
                    DateOfBirth = txtDateTime.Value,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                };
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Get Member Error!!!");
            }
            return member;
        }

        private void ClearText ()
        {
            txtMemberID.Text = string.Empty;
            txtMemberName.Text = string.Empty;
            txtDateTime.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtCountry.Text = string.Empty;

        }

        public void LoadMemberList(IEnumerable<MemberObject> ListMember)
        {
            var Members = ListMember;
            try {
                source = new BindingSource();
                source.DataSource = Members;

                txtMemberID.DataBindings.Clear();
                txtMemberName.DataBindings.Clear();
                txtEmail.DataBindings.Clear();
                txtPassword.DataBindings.Clear();
                txtDateTime.DataBindings.Clear();
                txtCity.DataBindings.Clear();
                txtCountry.DataBindings.Clear();

                txtMemberID.DataBindings.Add("Text", source, "MemberID");
                txtMemberName.DataBindings.Add("Text", source, "MemberName");
                txtEmail.DataBindings.Add("Text", source, "Email");
                txtPassword.DataBindings.Add("Text", source, "Password");
                txtDateTime.DataBindings.Add("Text", source, "DateOfBirth");
                txtCity.DataBindings.Add("Text", source, "City");
                txtCountry.DataBindings.Add("Text", source, "Country");

                dataGridViewMember.DataSource = source;

                if(Members.Count() == 0) {
                    btnDelete.Enabled = false;
                    ClearText();
                } else {
                    btnDelete.Enabled=true;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Get Member list error");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadMemberList(memberRepository.GetMemberList());
        }

        private void dataGridViewMember_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmMemberDetail frmMemberDetail = new frmMemberDetail() {
                Text = "Update member",
                UpdateFlag = true,
                MemberRepository = memberRepository,
                MemberObject = GetMember(),

            };

            if (frmMemberDetail.ShowDialog() == DialogResult.OK) {
                LoadMemberList(memberRepository.GetMemberList());
                source.Position = source.Count - 1;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try {
                var Member = GetMember();
                memberRepository.DeleteMember(Member.MemberID);
                LoadMemberList(memberRepository.GetMemberList());
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message, "Delete a member");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmMemberDetail frm = new frmMemberDetail() {
                Text = "Add member",
                UpdateFlag = false,
                MemberRepository = memberRepository,
                //MemberObject = GetMember(),
            };

            if (frm.ShowDialog() == DialogResult.OK) {
                LoadMemberList(memberRepository.GetMemberList());
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var SearchValue = txtSearchValue.Text;
            var listResult = memberRepository.GetMemberByName(SearchValue);
            LoadMemberList(listResult);
            
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var listResult = memberRepository.FilterByCity(cboFilter.Text);
            LoadMemberList(listResult);
        }

        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearchValue.Text = String.Empty;
        }
    }
}
