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
    public partial class UpdateSupplier : Form
    {
        private Form _parentForm;
        private Form _parent;

        public int SupplierId { get; set; }

        public UpdateSupplier(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }


        public void LoadSupplierData(
            int id,
            string code,
            string name,
            string contact,
            string phone,
            string email,
            string address,
            string status)
        {
            SupplierId = id;
            txtSupplierCode.Text = code;
            txtSupplierName.Text = name;
            txtContactPerson.Text = contact;
            txtPhone.Text = phone;
            txtEmail.Text = email;
            txtAddress.Text = address;
            txtStatus.Text = status;
        }

        private void btnSave_Click(object sender, EventArgs e)
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

            if (_parentForm is adminSupplier admin)
            {
                admin.LoadSuppliers();
            }
            else if (_parentForm is clerkSupplier clerk)
            {
                clerk.LoadSuppliers();
            }


            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateSupplier_Load(object sender, EventArgs e)
        {

        }
    }

}
