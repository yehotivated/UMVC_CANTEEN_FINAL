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
using UMVC_INVENTORY.Database;

namespace UMVC_INVENTORY
{
    public partial class UpdateUsers : Form
    {
        private int _userId;
        private Form _parentForm;
        public UpdateUsers(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }
        public void LoadUserData(int userId, string username, string role)
        {
            _userId = userId;
            txtUsername.Text = username;
            cmbRole.Text = role;
        }

        public UpdateUsers()
        {
            InitializeComponent();
        }

        private void UpdateUsers_Load(object sender, EventArgs e)
        {

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string role = cmbRole.Text;

            // OPTIONAL password update
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }

                UsersDAL.UpdateUserWithPassword(
                    _userId,
                    username,
                    BCrypt.Net.BCrypt.HashPassword(txtPassword.Text),
                    role
                );
            }
            else
            {
                UsersDAL.UpdateUser(
                    _userId,
                    username,
                    role
                );
            }

            // 🔄 REAL-TIME REFRESH
            if (_parentForm is adminUsers admin)
            {
                admin.LoadUsers();
            }

            MessageBox.Show("User updated successfully.");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}