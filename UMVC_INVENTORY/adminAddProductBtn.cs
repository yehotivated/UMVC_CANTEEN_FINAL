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
#if DEBUG
    public partial class adminAddProductBtn : Form

#else
    public partial class adminAddProductBtn : BaseActionForm
#endif
    {
        #region Constructor

        public adminAddProductBtn()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                INSERT INTO products
                (product_code, product_name, category, supplier, quantity, critical_level, unit_price, cost, status)
                VALUES
                (@code, @name, @category, @supplier, @qty, @critical, @price, @cost, 'ACTIVE')";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@code", txtProductCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", txtProductName.Text.Trim());
                    cmd.Parameters.AddWithValue("@category", txtCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@supplier", txtSupplier.Text.Trim());
                    cmd.Parameters.AddWithValue("@qty", int.Parse(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@critical", int.Parse(txtCritical.Text));
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(txtUnitPrice.Text));
                    cmd.Parameters.AddWithValue("@cost", decimal.Parse(txtCost.Text));

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the Add Product form and refresh the inventory
                this.Close();
                    AdminNavigation.Open(this, new adminInventory());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private Form _parentForm;

        public adminAddProductBtn(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }



        #endregion

        #region Event Handlers

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }





        #endregion

        #region Protected Override Methods




        #endregion

        private void adminAddProductBtn_Load(object sender, EventArgs e)
        {

        }

        
    }
}
