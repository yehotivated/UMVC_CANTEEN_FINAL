using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UMVC_INVENTORY.Database;

namespace UMVC_INVENTORY
{
        /// <summary>
        /// Admin Users form for UMVC Canteen Inventory & POS System
        /// </summary>
        public partial class adminUsers : Form
        {
        public adminUsers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dashboard button click event handler
        /// Opens the Admin Dashboard form
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFormAndHideCurrent(new adminDashboard());
        }
        private void btnTransactions_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminTransactions());
        }


        private void btnInventory_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminInventory());
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminDashboard());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminPOS());
        }


        private void btnReports_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminReports());
        }


        private void btnUsers_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminUsers());
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminSupplier());
        }


        /// <summary>
        /// POS button click event handler
        /// Opens the Admin POS form
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFormAndHideCurrent(new adminPOS());
        }

        /// <summary>
        /// Inventory button click event handler
        /// Opens the Admin Inventory form
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFormAndHideCurrent(new adminInventory());
        }

        /// <summary>
        /// Supplier button click event handler
        /// Opens the Admin Supplier form
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFormAndHideCurrent(new adminSupplier());
        }

        /// <summary>
        /// Transaction button click event handler
        /// Opens the Admin Transactions form
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFormAndHideCurrent(new adminTransactions());
        }

        /// <summary>
        /// Reports button click event handler
        /// Opens the Admin Reports form
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFormAndHideCurrent(new adminReports());
        }

        /// <summary>
        /// User button click event handler
        /// Refreshes the current Users form
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {
            // Refresh the Users form - reload data if needed
            this.Refresh();
        }

        /// <summary>
        /// Logout button click event handler
        /// </summary>
        private void button8_Click(object sender, EventArgs e)
        {
            PerformLogout();
        }

        /// <summary>
        /// Opens a new form and hides the current form using NavigationManager
        /// Maintains navigation history for back button functionality
        /// </summary>
        /// <param name="form">The form to open</param>
        private void OpenFormAndHideCurrent(Form form)
        {
            NavigationManager.NavigateTo(this, form);
        }

        /// <summary>
        /// Back button click event handler
        /// Navigates back to the previous form in the navigation history
        /// </summary>
        private void BackButton_Click(object sender, EventArgs e)
        {
            if (!NavigationManager.NavigateBack(this))
            {
                // No previous form - show message or stay on current form
                MessageBox.Show("No previous page to go back to.", "Navigation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Performs the logout process with user confirmation
        /// </summary>
        private void PerformLogout()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                // Clear navigation history on logout
                NavigationManager.ClearHistory();
                
                // Mark that we're logging out (this ensures Login form will show)
                NavigationManager.SetLoggingOut(this);
                
                this.Close();
            }
        }
        public void LoadUsers()
        {
            DataTable dt = UsersDAL.GetAllUsers();
            MessageBox.Show("Users found: " + dt.Rows.Count);

            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.DataSource = dt;
            dgvUsers.DataSource = UsersDAL.GetAllUsers();
        }


        private void adminUsers_Load(object sender, EventArgs e)
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadUsers();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            CreateAccount form = new CreateAccount(this);
            form.ShowDialog(); // modal (recommended)
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            DataGridViewRow r = dgvUsers.SelectedRows[0];

            UpdateUsers form = new UpdateUsers(this);

            form.LoadUserData(
                Convert.ToInt32(r.Cells["user_id"].Value),
                r.Cells["username"].Value.ToString(),
                r.Cells["ROLE"].Value.ToString()
            );

            form.ShowDialog();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            DataGridViewRow row = dgvUsers.SelectedRows[0];
            int userId = Convert.ToInt32(row.Cells["user_id"].Value);

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this user?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                UsersDAL.DeleteUser(userId);
                LoadUsers(); // 🔄 REAL-TIME refresh
                MessageBox.Show("User deleted successfully.");
            }
        }

    }
}
