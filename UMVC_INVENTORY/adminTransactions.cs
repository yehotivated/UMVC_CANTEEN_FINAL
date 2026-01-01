using System;
using System.Data;
using System.Windows.Forms;
using UMVC_INVENTORY.Database;

namespace UMVC_INVENTORY
{
    public partial class adminTransactions : Form
    {
        public adminTransactions()
        {
            InitializeComponent();
            dgvTransactions.AutoGenerateColumns = true;
        }

        private void adminTransaction_Load(object sender, EventArgs e)
        {
            LoadTransactions();
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


        private void LoadTransactions()
        {
            dgvTransactions.DataSource = TransactionDAL.GetTransactions();

            if (dgvTransactions.Columns.Contains("id"))
                dgvTransactions.Columns["id"].Visible = false;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            DateTime from = dateFrom.Value.Date;
            DateTime to = dateTo.Value.Date.AddDays(1).AddSeconds(-1);

            dgvTransactions.DataSource =
                TransactionDAL.GetTransactionsByDate(from, to);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            LoadTransactions();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PerformLogout();
        }
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
    }
}
