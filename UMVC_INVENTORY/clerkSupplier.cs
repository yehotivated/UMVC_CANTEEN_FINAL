using System;
using System.Windows.Forms;
using UMVC_INVENTORY.Database;

namespace UMVC_INVENTORY
{
    public partial class clerkSupplier : Form
    {
        public clerkSupplier()
        {
            InitializeComponent();
            dgvSupplier.AutoGenerateColumns = true;
        }

        private void clerkSupplier_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        // ===============================
        // LOAD SUPPLIERS (SAME AS ADMIN)
        // ===============================
        public void LoadSuppliers()
        {
            dgvSupplier.DataSource = SupplierDAL.GetSuppliers();

            if (dgvSupplier.Columns.Contains("id"))
                dgvSupplier.Columns["id"].Visible = false;
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

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            new clerkTransactions().Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login.ShowLoginForm();
            this.Close();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
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

        private void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSupplier.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier first.");
                return;
            }

            DataGridViewRow r = dgvSupplier.SelectedRows[0];

            string idColumnName = dgvSupplier.Columns.Contains("supplier_id") ? "supplier_id" : "id";
            int supplierId = Convert.ToInt32(r.Cells[idColumnName].Value);
            string supplierCode = r.Cells["supplier_code"].Value.ToString();
            string supplierName = r.Cells["supplier_name"].Value.ToString();
            string contactPerson = r.Cells["contact_person"].Value.ToString();
            string phone = r.Cells["phone"].Value.ToString();
            string email = r.Cells["email"].Value.ToString();
            string address = r.Cells["address"].Value.ToString();
            string status = r.Cells["status"].Value.ToString();

            // Open UpdateSupplier form
            UpdateSupplier form = new UpdateSupplier(this);

            // Load selected supplier data into the form
            form.LoadSupplierData(
                supplierId,
                supplierCode,
                supplierName,
                contactPerson,
                phone,
                email,
                address,
                status
            );

            form.ShowDialog();
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSupplier.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a supplier first.");
                return;
            }

            string idColumnName = dgvSupplier.Columns.Contains("supplier_id") ? "supplier_id" : "id";
            int id = Convert.ToInt32(
                dgvSupplier.SelectedRows[0].Cells[idColumnName].Value);

            if (MessageBox.Show("Delete this supplier?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SupplierDAL.DeleteSupplier(id);
                LoadSuppliers(); // REAL-TIME refresh
            }
        }
    }
}
