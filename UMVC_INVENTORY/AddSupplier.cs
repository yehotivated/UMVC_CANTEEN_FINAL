using System;
using System.Windows.Forms;
using UMVC_INVENTORY.Database;

namespace UMVC_INVENTORY
{
    public partial class AddSupplier : Form
    {
        public int SupplierId { get; set; } = 0;
        private Form _parentForm;

        // Constructor for ADD
        public AddSupplier(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        // Load event (ONLY ONE)
        private void AddSupplier_Load(object sender, EventArgs e)
        {
            if (SupplierId > 0)
            {
                this.Text = "Update Supplier";
            }
            else
            {
                this.Text = "Add Supplier";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SupplierId == 0)
            {
                SupplierDAL.AddSupplier(
                    txtSupplierCode.Text,
                    txtSupplierName.Text,
                    txtContactPerson.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    txtAddress.Text,
                    txtStatus.Text
                );
            }
            else
            {
                SupplierDAL.UpdateSupplier(
                    SupplierId,
                    txtSupplierCode.Text,
                    txtSupplierName.Text,
                    txtContactPerson.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    txtAddress.Text,
                    txtStatus.Text
                );
            }

            // REAL-TIME refresh
            if (_parentForm is adminSupplier admin)
                admin.LoadSuppliers();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
