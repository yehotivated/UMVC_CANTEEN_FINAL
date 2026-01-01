using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY
{
    public partial class UpdateProduct : Form
    {
        // Properties to hold product information
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }

        private Form _parentForm; // Reference to parent form (either adminInventory or clerkInventory)

        // Constructor that accepts product data and the parent form (could be either adminInventory or clerkInventory)
        // In UpdateProduct.cs
        // In UpdateProduct.cs
        // In UpdateProduct.cs
        public UpdateProduct(int productId, string productCode, string productName, int quantity, decimal unitPrice, string status, Form parentForm)
        {
            InitializeComponent();

            // Set properties
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Status = status;

            // Assign parent form
            _parentForm = parentForm;  // This will hold reference to either adminInventory or clerkInventory
        }




        // Form Load logic
        private void UpdateProduct_Load(object sender, EventArgs e)
        {
            // Set the form fields with the passed data
            txtProductId.Text = ProductId.ToString();
            txtProductCode.Text = ProductCode;
            txtProductName.Text = ProductName;
            txtQuantity.Text = Quantity.ToString();
            txtUnitPrice.Text = UnitPrice.ToString();
            txtStatus.Text = Status;
        }

        // Save Button Logic
        // UpdateProduct.cs

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
            UPDATE products
            SET product_code = @code,
                product_name = @name,
                quantity = @qty,
                unit_price = @price,
                status = @status
            WHERE product_id = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", ProductId);
                    cmd.Parameters.AddWithValue("@code", txtProductCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", txtProductName.Text.Trim());
                    cmd.Parameters.AddWithValue("@qty", int.Parse(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(txtUnitPrice.Text));
                    cmd.Parameters.AddWithValue("@status", txtStatus.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Call the method to refresh the DataGridView in the parent form (adminInventory)
                if (_parentForm is adminInventory adminForm)
                {
                    adminForm.LoadInventory(); // Refresh the inventory in adminInventory
                }
                else if (_parentForm is clerkInventory clerkForm)
                {
                    clerkForm.LoadInventory(); // Refresh the inventory in clerkInventory
                }

                this.Close();  // Close the update form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Cancel Button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); // Just close the form without saving
        }
    }
}
