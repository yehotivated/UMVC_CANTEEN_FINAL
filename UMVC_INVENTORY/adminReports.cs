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
    /// Admin Reports form for UMVC Canteen Inventory & POS System
    /// </summary>
    public partial class adminReports : Form
    {
        public adminReports()
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
        /// Refreshes the current Reports form
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            // Refresh the Reports form - reload data if needed
            this.Refresh();
        }

        /// <summary>
        /// User button click event handler
        /// Opens the Admin Users form
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {
            OpenFormAndHideCurrent(new adminUsers());
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

        private void adminReports_Load(object sender, EventArgs e)
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.Add("Inventory");
            cmbReportType.Items.Add("Transactions");
            cmbReportType.Items.Add("Sales");
            cmbReportType.Items.Add("Low Stock");
            // ✅ SET DATE PICKERS TO TODAY
            dateFrom.Value = DateTime.Today;
            dateTo.Value = DateTime.Today;
        }

        
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a report type.");
                return;
            }

            dgvReport.AutoGenerateColumns = true;

            DateTime from = dateFrom.Value.Date;
            DateTime to = dateTo.Value.Date.AddDays(1); // ✅ include entire end date

            string reportType = cmbReportType.Text;

            if (reportType == "Inventory")
            {
                dgvReport.DataSource = ReportsDAL.GetInventoryReport();
            }
            else if (reportType == "Low Stock") // ✅ ADD THIS
            {
                dgvReport.DataSource = ReportsDAL.GetLowStockReport();
            }
            else if (reportType == "Transactions")
            {
                dgvReport.DataSource =
                    ReportsDAL.GetTransactionReportByDate(from, to);
            }
            else if (reportType == "Sales")
            {
                dgvReport.DataSource =
                    ReportsDAL.GetSalesReportByDate(from, to);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // 1. Clear DataGridView
            dgvReport.DataSource = null;
            dgvReport.Rows.Clear();
            dgvReport.Columns.Clear();

            // 2. Reset ComboBox
            cmbReportType.SelectedIndex = -1;

            // 3. Reset DatePickers to today
            dateFrom.Value = DateTime.Today;
            dateTo.Value = DateTime.Today;

            // 4. Optional: Focus back to report type
            cmbReportType.Focus();
        }
    }
}