using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using UMVC_INVENTORY.Database;


namespace UMVC_INVENTORY
{
    public partial class clerkInventory : Form
    {
        public clerkInventory()
        {
            InitializeComponent();
        }

        private void clerkInventory_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadInventory();

        }
        private void LoadCategories()
        {
            // Load categories from the database
            DataTable categories = InventoryDAL.GetCategories();

            // Add an "All" option for showing all products
            DataRow allRow = categories.NewRow();
            allRow["category"] = "All";
            categories.Rows.InsertAt(allRow, 0);  // Add "All" at the top

            // Bind categories to the ComboBox
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "category";
            cmbCategory.ValueMember = "category";
        }
        // ===============================
        // INVENTORY METHODS
        // ===============================

        public void LoadInventory()
        {
            // Get the latest data from the database
            dgvInventory.DataSource = InventoryDAL.GetInventory();

            // Hide unnecessary columns
            if (dgvInventory.Columns.Contains("id"))
                dgvInventory.Columns["id"].Visible = false;

            if (dgvInventory.Columns.Contains("created_at"))
                dgvInventory.Columns["created_at"].Visible = false;

            // Style the inventory rows based on the stock level
            foreach (DataGridViewRow row in dgvInventory.Rows)
            {
                if (row.Cells["status"].Value != null && row.Cells["status"].Value.ToString() == "Low Stock")
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral; // Highlight low stock items
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }




        // ===============================
        // MENU NAVIGATION (CLERK ONLY)
        // ===============================

        // DASHBOARD
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            new clerkDashboard().Show();
            this.Close();
        }

        // INVENTORY (CURRENT)
        private void btnInventory_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }

        // SUPPLIER
        private void btnSupplier_Click(object sender, EventArgs e)
        {
            new clerkSupplier().Show();
            this.Close();
        }

        // TRANSACTIONS
        private void btnTransaction_Click(object sender, EventArgs e)
        {
            new clerkTransactions().Show(); // MUST EXIST
            this.Close();
        }

        // LOGOUT
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login.ShowLoginForm();
            this.Close();
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            new clerkDashboard().Show();
            this.Close();
        }

        // ===============================
        // OPTIONAL: ADD PRODUCT (IF ALLOWED)
        // ===============================
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            // Open the Add Product form for the clerk to add products
            new adminAddProductBtn().ShowDialog(); // Use ShowDialog to keep the form open until the user finishes

            // Reload inventory to reflect the new product
            LoadInventory();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Open the 'adminAddProductBtn' form when the button is clicked
            new adminAddProductBtn().ShowDialog();  // Show the form modally (the user must interact with this form before returning)

            // Optionally refresh the inventory after adding a product
            LoadInventory();  // Reload the inventory to reflect any changes
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = cmbCategory.SelectedValue.ToString();

            // If "All" is selected, show all products
            if (selectedCategory == "All")
            {
                LoadInventory();
            }
            else
            {
                // Filter products based on the selected category
                LoadInventoryByCategory(selectedCategory);
            }
        }
        private void LoadInventoryByCategory(string category)
        {
            dgvInventory.DataSource = InventoryDAL.GetInventoryByCategory(category);

            if (dgvInventory.Columns.Contains("id"))
                dgvInventory.Columns["id"].Visible = false;

            if (dgvInventory.Columns.Contains("created_at"))
                dgvInventory.Columns["created_at"].Visible = false;

            // Apply color for low stock
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

                    // Remove the selected row from the DataGridView
                    dgvInventory.Rows.Remove(selectedRow);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dgvInventory.SelectedRows.Count > 0)
            {
                // Get the first selected row
                DataGridViewRow selectedRow = dgvInventory.SelectedRows[0];

                // Retrieve values from the selected row
                int productId = Convert.ToInt32(selectedRow.Cells["product_id"].Value);
                string productCode = selectedRow.Cells["product_code"].Value.ToString();
                string productName = selectedRow.Cells["product_name"].Value.ToString();
                int quantity = Convert.ToInt32(selectedRow.Cells["quantity"].Value);
                decimal unitPrice = Convert.ToDecimal(selectedRow.Cells["unit_price"].Value);
                string status = selectedRow.Cells["status"].Value.ToString();

                // Create and show the UpdateProduct form, passing the selected data
                // In clerkInventory.cs
                UpdateProduct updateForm = new UpdateProduct(productId, productCode, productName, quantity, unitPrice, status, this);
                updateForm.ShowDialog();  // Open the form modally

            }
            else
            {
                MessageBox.Show("Please select a product to update.");
            }
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
