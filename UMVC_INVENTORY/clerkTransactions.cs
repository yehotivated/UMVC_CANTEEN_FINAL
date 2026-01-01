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
    public partial class clerkTransactions : Form
    {
        public clerkTransactions()
        {
            InitializeComponent();
            dgvTransactions.AutoGenerateColumns = true;
        }

        private void clerkTransactions_Load(object sender, EventArgs e)
        {
            LoadTransactions();
        }

        // ===============================
        // LOAD TRANSACTIONS (SAME AS ADMIN)
        // ===============================
        private void LoadTransactions()
        {
            dgvTransactions.DataSource = TransactionDAL.GetTransactions();

            if (dgvTransactions.Columns.Contains("id"))
                dgvTransactions.Columns["id"].Visible = false;
        }

        // ===============================
        // FILTER BY DATE
        // ===============================
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


        // ===============================
        // MENU NAVIGATION (CLERK)
        // ===============================
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            new clerkDashboard().Show();
            this.Close();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            new clerkInventory().Show();
            this.Close();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            new clerkSupplier().Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login.ShowLoginForm();
            this.Close();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {

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