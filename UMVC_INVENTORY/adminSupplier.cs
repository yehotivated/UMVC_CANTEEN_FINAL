using System;
using System.Data;
using System.Windows.Forms;
using UMVC_INVENTORY.Database;


namespace UMVC_INVENTORY
{
    public partial class adminSupplier : Form
    {
        public adminSupplier()
        {
            InitializeComponent();

            dgvSupplier.AutoGenerateColumns = true;

            timerSupplier.Interval = 1000;  // 1 second
            timerSupplier.Tick += TimerSupplier_Tick;
            timerSupplier.Enabled = true;


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



        public void LoadSuppliers()
        {
            dgvSupplier.DataSource = SupplierDAL.GetSuppliers();

            if (dgvSupplier.Columns.Contains("id"))
                dgvSupplier.Columns["id"].Visible = false;

            if (dgvSupplier.Columns.Contains("supplier_id"))
                dgvSupplier.Columns["supplier_id"].Visible = false;

            if (dgvSupplier.Columns.Contains("created_at"))
                dgvSupplier.Columns["created_at"].Visible = false;
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            adminTransactions frm = new adminTransactions();
            frm.Show();
            this.Hide(); // optional
        }

        private void TimerSupplier_Tick(object sender, EventArgs e)
        {
            LoadSuppliers();
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


        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            AddSupplier form = new AddSupplier(this);
            form.ShowDialog();
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



        private void adminSupplier_Load(object sender, EventArgs e)
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
