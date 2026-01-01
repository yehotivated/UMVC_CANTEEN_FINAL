using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using UMVC_INVENTORY.Database;

namespace UMVC_INVENTORY
{
    public partial class cashierTransactions : Form
    {
        public cashierTransactions()
        {
            InitializeComponent();
            dgvTransactions.AutoGenerateColumns = true;
        }

        private void cashierTransactions_Load(object sender, EventArgs e)
        {
            dgvTransactions.AutoGenerateColumns = true;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Make columns fill the space
            dgvTransactions.Dock = DockStyle.Fill; // Ensure the DataGridView fills the form's space
            LoadTransactions();
        }


        // This method will load the transactions into the DataGridView
        private void LoadTransactions(DateTime? fromDate = null, DateTime? toDate = null)
        {
            DataTable dt = new DataTable();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();

                // Build the base query to get all transactions
                string query = @"
                    SELECT 
                        transaction_no, 
                        transaction_date, 
                        cashier, 
                        total, 
                        status
                    FROM transactions";

                // Apply the date range if specified
                if (fromDate.HasValue && toDate.HasValue)
                {
                    query += " WHERE transaction_date BETWEEN @fromDate AND @toDate";
                }

                query += " ORDER BY transaction_date DESC"; // Sort by transaction date in descending order

                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Add parameters only if dates are provided
                if (fromDate.HasValue && toDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@toDate", toDate.Value.ToString("yyyy-MM-dd"));
                }

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            // Bind the fetched data to the DataGridView
            dgvTransactions.DataSource = dt;

            // Hide the 'id' column, as it's not needed in the view
            if (dgvTransactions.Columns.Contains("id"))
                dgvTransactions.Columns["id"].Visible = false;
        }

        // This method will handle the filter button click event
        private void btnFilter_Click(object sender, EventArgs e)
        {
            DateTime from = dateFrom.Value.Date; // Start of the selected day (00:00:00)
            DateTime to = dateTo.Value.Date.AddDays(1).AddSeconds(-1); // End of the selected day (23:59:59)

            // Log the date values for debugging
            Console.WriteLine($"From: {from.ToString("yyyy-MM-dd HH:mm:ss")}");
            Console.WriteLine($"To: {to.ToString("yyyy-MM-dd HH:mm:ss")}");

            dgvTransactions.DataSource = TransactionDAL.GetTransactionsByDate(from, to);
        }

        // This method will clear the date filters and reload all transactions
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear the date filters and reload all transactions
            dateFrom.Value = DateTime.Now;  // Set the date pickers back to today's date
            dateTo.Value = DateTime.Now;
            LoadTransactions();  // Reload all transactions without any filters
        }

        // Navigation buttons

        // This is for staying in the cashier transactions screen
        private void btnTransactions_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new cashierTransactions()); // Stay in cashier transactions screen
        }

        // This is for navigating to the cashier dashboard
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new cashierDashboard()); // Open cashier dashboard
        }

        // This is for navigating to the cashier POS screen
        private void btnPOS_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new cashierPOS()); // Open cashier POS screen
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



        // Optional: Add more navigation buttons here (e.g. Reports, Inventory, etc.)
    }
}
