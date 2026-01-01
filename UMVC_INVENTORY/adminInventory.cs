using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;        // For Color
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using UMVC_INVENTORY.BaseForms;
using UMVC_INVENTORY.Database;


namespace UMVC_INVENTORY
{
    public partial class adminInventory : BaseDashboardForm
    {
        #region Constructor

        public adminInventory()
        {
            InitializeComponent();

            // Initialize Timer
            timerInventory.Interval = 1000;  // 1 second
            timerInventory.Tick += TimerInventory_Tick;
            timerInventory.Enabled = true;
        }

        #endregion

        #region Form Events

        private void adminInventory_Load(object sender, EventArgs e)
        {
            LoadInventory();
            LoadCategories();
        }
        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All");

            DataTable dt = InventoryDAL.GetCategories();

            foreach (DataRow row in dt.Rows)
            {
                cmbCategory.Items.Add(row["category"].ToString());
            }

            cmbCategory.SelectedIndex = 0; // Default = All
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


        #endregion

        #region Inventory Methods

        // adminInventory.cs

        public void LoadInventory()
        {
            // Reload the data from the database to refresh the DataGridView
            dgvInventory.DataSource = InventoryDAL.GetInventory();

            // Hide unnecessary columns or adjust the DataGridView if necessary
            if (dgvInventory.Columns.Contains("id"))
                dgvInventory.Columns["id"].Visible = false;

            if (dgvInventory.Columns.Contains("created_at"))
                dgvInventory.Columns["created_at"].Visible = false;

            // You can add any additional customization for styling the rows here
            foreach (DataGridViewRow row in dgvInventory.Rows)
            {
                if (row.Cells["status"].Value != null &&
                    row.Cells["status"].Value.ToString() == "Low Stock")
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }





        private void TimerInventory_Tick(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem == null)
                return;

            if (cmbCategory.SelectedItem.ToString() == "All")
            {
                LoadInventory();
            }
            else
            {
                dgvInventory.DataSource =
                    InventoryDAL.GetInventoryByCategory(cmbCategory.SelectedItem.ToString());
            }
        }


        #endregion

        #region Navigation Event Handlers

        private void button1_Click(object sender, EventArgs e)
        {
            NavigateToAdminDashboard();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigateToAdminPOS();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OnInventoryButtonClick(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NavigateToAdminSupplier();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NavigateToAdminTransactions();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NavigateToAdminReports();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            NavigateToAdminUsers();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OnLogoutButtonClick(sender, e);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            OnBackButtonClick(sender, e);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            adminAddProductBtn form = new adminAddProductBtn(this);
            form.ShowDialog(); // ✅ keeps adminInventory open
        }


        #endregion

        #region Protected Override Methods

        public override void RefreshForm()
        {
            this.Refresh();
        }

        #endregion

        #region Protected Methods

        protected virtual void OnInventoryButtonClick(object sender, EventArgs e)
        {
            RefreshForm();
        }

        #endregion

        #region Navigation Methods

        private void NavigateToAdminDashboard()
        {
            OpenFormAndHideCurrent(new adminDashboard());
        }

        private void NavigateToAdminPOS()
        {
            OpenFormAndHideCurrent(new adminPOS());
        }

        private void NavigateToAdminSupplier()
        {
            OpenFormAndHideCurrent(new adminSupplier());
        }

        private void NavigateToAdminTransactions()
        {
            OpenFormAndHideCurrent(new adminTransactions());
        }

        private void NavigateToAdminReports()
        {
            OpenFormAndHideCurrent(new adminReports());
        }

        private void NavigateToAdminUsers()
        {
            OpenFormAndHideCurrent(new adminUsers());
        }

        private void NavigateToAddProduct()
        {
            OpenFormAndHideCurrent(new adminAddProductBtn());
        }

        #endregion

        #region Timer Control Declaration

        // Add this Timer control declaration if it doesn't exist yet.


        #endregion

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem.ToString() == "All")
            {
                LoadInventory(); // show all
            }
            else
            {
                dgvInventory.DataSource =
                    InventoryDAL.GetInventoryByCategory(cmbCategory.SelectedItem.ToString());
            }
        }


        // In adminInventory.cs
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dgvInventory.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgvInventory.SelectedRows[0];
                int productId = Convert.ToInt32(selectedRow.Cells["product_id"].Value); // Assuming 'product_id' is the unique ID column in your DataGridView

                // Ask for confirmation before deleting
                var confirmResult = MessageBox.Show("Are you sure you want to delete this product?",
                                                     "Confirm Delete",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Call the method to delete the product from the database
                    DeleteProductFromDatabase(productId);
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteProductFromDatabase(int productId)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM products WHERE product_id = @productId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@productId", productId);

                    cmd.ExecuteNonQuery();  // Execute the DELETE query
                }

                MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();  // Reload the DataGridView to reflect the deletion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure it's not the header row
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgvInventory.Rows[e.RowIndex];

                int productId = Convert.ToInt32(selectedRow.Cells["product_id"].Value);
                string productCode = selectedRow.Cells["product_code"].Value.ToString();
                string productName = selectedRow.Cells["product_name"].Value.ToString();
                int quantity = Convert.ToInt32(selectedRow.Cells["quantity"].Value);
                decimal unitPrice = Convert.ToDecimal(selectedRow.Cells["unit_price"].Value);
                string status = selectedRow.Cells["status"].Value.ToString();

                // Open UpdateProduct form and pass the data
                UpdateProduct updateForm = new UpdateProduct(productId, productCode, productName, quantity, unitPrice, status, this);
                updateForm.ShowDialog(); // Open the form modally
            }
        }


        // In adminInventory.cs
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvInventory.SelectedRows[0];

                int productId = Convert.ToInt32(selectedRow.Cells["product_id"].Value);
                string productCode = selectedRow.Cells["product_code"].Value.ToString();
                string productName = selectedRow.Cells["product_name"].Value.ToString();
                int quantity = Convert.ToInt32(selectedRow.Cells["quantity"].Value);
                decimal unitPrice = Convert.ToDecimal(selectedRow.Cells["unit_price"].Value);
                string status = selectedRow.Cells["status"].Value.ToString();

                // Pass data and current form (adminInventory) to UpdateProduct
                UpdateProduct updateForm = new UpdateProduct(productId, productCode, productName, quantity, unitPrice, status, this);
                updateForm.ShowDialog(); // Open the form modally
            }
            else
            {
                MessageBox.Show("Please select a product to update.");
            }
        }

        
    }
}
