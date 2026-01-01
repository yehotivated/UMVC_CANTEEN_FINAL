using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using UMVC_INVENTORY.Database;
using System.Drawing.Printing;

namespace UMVC_INVENTORY
{
    public partial class cashierPOS : Form
    {
        public cashierPOS()
        {
            InitializeComponent();

            printDocument1.PrintPage += printDocument1_PrintPage;

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.StartPosition = FormStartPosition.CenterScreen;
            printPreviewDialog1.Width = 600;
            printPreviewDialog1.Height = 800;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.5; // 🔍 ZOOM IN
            printDocument1.DefaultPageSettings.PaperSize =
            new PaperSize("Receipt", 280, 800);
        }

        // ===============================
        // FORM LOAD EVENT HANDLER
        // ===============================
        private void cashierPOS_Load(object sender, EventArgs e)
        {
            DataTable dt = POSDAL.GetProducts();
            MessageBox.Show("Products found: " + dt.Rows.Count);
            LoadProducts(dt);
        }

        // ===============================
        // ADD PRODUCT TO CART (DGV ONLY)
        // ===============================
        private void Product_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            DataRow row = (DataRow)btn.Tag;

            string name = row["name"].ToString();
            decimal price = Convert.ToDecimal(row["unit_price"]);

            // CHECK IF PRODUCT EXISTS IN CART
            foreach (DataGridViewRow cartRow in dgvCart.Rows)
            {
                if (cartRow.Cells["colProduct"].Value.ToString() == name)
                {
                    int qty = Convert.ToInt32(cartRow.Cells["colQty"].Value) + 1;
                    cartRow.Cells["colQty"].Value = qty;
                    cartRow.Cells["colSubtotal"].Value = qty * price;

                    CalculateTotal();
                    return;
                }
            }

            // ADD NEW ROW
            dgvCart.Rows.Add(
                name,   // colProduct
                price,  // colPrice
                1,      // colQty
                price   // colSubtotal
            );

            CalculateTotal();
        }

        // ===============================
        // CALCULATE TOTAL
        // ===============================
        private void CalculateTotal()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                total += Convert.ToDecimal(row.Cells["colSubtotal"].Value);
            }

            lblTotal.Text = "₱ " + total.ToString("N2");
        }

        // ===============================
        // LOAD PRODUCTS
        // ===============================
        private void LoadProducts(DataTable dt)
        {
            flpProducts.Controls.Clear(); // Clear previous buttons

            flpProducts.WrapContents = true;
            flpProducts.FlowDirection = FlowDirection.LeftToRight;
            flpProducts.AutoScroll = true;

            foreach (DataRow row in dt.Rows)
            {
                Button btn = new Button();
                btn.Width = 180;
                btn.Height = 100;
                btn.Margin = new Padding(10);

                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;

                btn.Text =
                    $"{row["name"]}\n" +
                    $"Code: {row["code"]}\n" +
                    $"₱{row["unit_price"]}\n" +
                    $"Stock: {row["stock_qty"]}";

                btn.Tag = row;
                btn.Click += Product_Click;

                flpProducts.Controls.Add(btn);
            }
        }


        // ===============================
        // PRINT RECEIPT
        // ===============================
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("Consolas", 12);
            float y = 20;

            g.DrawString("UMVC CANTEEN", new Font("Consolas", 16, FontStyle.Bold), Brushes.Black, 10, y);
            y += 25;

            g.DrawString("POS RECEIPT", font, Brushes.Black, 10, y);
            y += 20;

            g.DrawString("Date: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm"), font, Brushes.Black, 10, y);
            y += 20;

            g.DrawString("--------------------------------", font, Brushes.Black, 10, y);
            y += 20;

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                string product = row.Cells["colProduct"].Value.ToString();
                int qty = Convert.ToInt32(row.Cells["colQty"].Value);
                decimal price = Convert.ToDecimal(row.Cells["colPrice"].Value);
                decimal subtotal = Convert.ToDecimal(row.Cells["colSubtotal"].Value);

                g.DrawString(product, font, Brushes.Black, 10, y);
                y += 15;
                g.DrawString($"{qty} x ₱{price:N2} = ₱{subtotal:N2}", font, Brushes.Black, 10, y);
                y += 20;
            }

            g.DrawString("--------------------------------", font, Brushes.Black, 10, y);
            y += 15;

            g.DrawString("TOTAL: " + lblTotal.Text, new Font("Consolas", 11, FontStyle.Bold), Brushes.Black, 10, y);
            y += 20;

            g.DrawString("Cash: ₱" + txtCash.Text, font, Brushes.Black, 10, y);
            y += 15;
            g.DrawString("Change: " + lblChange.Text, font, Brushes.Black, 10, y);
            y += 25;

            g.DrawString("Thank you! Please come again.", font, Brushes.Black, 10, y);
        }

        // ===============================
        // SEARCH PRODUCT
        // ===============================
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                // Show all products if search is empty
                LoadProducts(POSDAL.GetProducts());
                return;
            }

            DataTable result = POSDAL.SearchProduct(keyword);

            if (result.Rows.Count == 0)
            {
                MessageBox.Show("No product found.");
                flpProducts.Controls.Clear();
            }
            else
            {
                LoadProducts(result);
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            // IF SEARCH BOX IS EMPTY → SHOW ALL PRODUCTS
            if (string.IsNullOrEmpty(keyword))
            {
                LoadProducts(POSDAL.GetProducts());
                return;
            }

            // SEARCH BY NAME OR BARCODE
            DataTable result = POSDAL.SearchProduct(keyword);

            LoadProducts(result);
        }




        // ===============================
        // CLEAR CART
        // ===============================
        private void btnClearCart_Click(object sender, EventArgs e)
        {
            dgvCart.Rows.Clear();
            lblTotal.Text = "₱ 0.00";
        }

        // ===============================
        // CALCULATE CHANGE
        // ===============================
        private void CalculateChange()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                total += Convert.ToDecimal(row.Cells["colSubtotal"].Value);
            }

            if (decimal.TryParse(txtCash.Text, out decimal cash))
            {
                decimal change = cash - total;
                lblChange.Text = "₱ " + change.ToString("N2");
            }
            else
            {
                lblChange.Text = "₱ 0.00";
            }
        }

        // ===============================
        // CHECKOUT
        // ===============================
        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (dgvCart.Rows.Count == 0)
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            decimal total = dgvCart.Rows
                .Cast<DataGridViewRow>()
                .Sum(r => Convert.ToDecimal(r.Cells["colSubtotal"].Value));

            // Save transaction
            int transactionId = TransactionPOSDAL.SaveTransaction(
                UserSession.Username, total);

            // Save items + update stock
            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                string product = row.Cells["colProduct"].Value.ToString();
                decimal price = Convert.ToDecimal(row.Cells["colPrice"].Value);
                int qty = Convert.ToInt32(row.Cells["colQty"].Value);
                decimal subtotal = Convert.ToDecimal(row.Cells["colSubtotal"].Value);

                TransactionPOSDAL.SaveTransactionItem(
                    transactionId, product, price, qty, subtotal);

                TransactionPOSDAL.UpdateStock(product, qty);
            }

            MessageBox.Show("Transaction Completed!");

            dgvCart.Rows.Clear();
            lblTotal.Text = "₱ 0.00";

            LoadProducts(POSDAL.GetProducts()); // refresh stock
        }

        // ===============================
        // PAY
        // ===============================
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (dgvCart.Rows.Count == 0)
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            decimal total = 0;
            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                total += Convert.ToDecimal(row.Cells["colSubtotal"].Value);
            }

            if (!decimal.TryParse(txtCash.Text, out decimal cash))
            {
                MessageBox.Show("Enter valid cash amount.");
                return;
            }

            if (cash < total)
            {
                MessageBox.Show("Insufficient cash.");
                return;
            }

            // ===== SAVE TRANSACTION =====
            int transactionId = POSDAL.SaveTransaction(
                UserSession.Username,
                total
            );

            // ===== SAVE ITEMS + DEDUCT STOCK =====
            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                string name = row.Cells["colProduct"].Value.ToString();
                decimal price = Convert.ToDecimal(row.Cells["colPrice"].Value);
                int qty = Convert.ToInt32(row.Cells["colQty"].Value);
                decimal subtotal = Convert.ToDecimal(row.Cells["colSubtotal"].Value);

                POSDAL.SaveTransactionItem(
                    transactionId, name, price, qty, subtotal
                );

                POSDAL.DeductStock(name, qty);
            }

            MessageBox.Show("Payment Successful!");

            // ===== SHOW RECEIPT PREVIEW =====
            printPreviewDialog1.ShowDialog();

            // ===== CLEAR CART =====
            dgvCart.Rows.Clear();
            lblTotal.Text = "₱ 0.00";
            txtCash.Clear();
            lblChange.Text = "₱ 0.00";

            // ===== REFRESH PRODUCTS =====
            LoadProducts(POSDAL.GetProducts());
        }

        // ===============================
        // REMOVE ITEM FROM CART
        // ===============================
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to remove.");
                return;
            }

            dgvCart.Rows.Remove(dgvCart.SelectedRows[0]);
            CalculateTotal();
        }

        // ===============================
        // MINUS QUANTITY
        // ===============================
        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item.");
                return;
            }

            DataGridViewRow row = dgvCart.SelectedRows[0];

            int qty = Convert.ToInt32(row.Cells["colQty"].Value);
            decimal price = Convert.ToDecimal(row.Cells["colPrice"].Value);

            if (qty > 1)
            {
                qty--;
                row.Cells["colQty"].Value = qty;
                row.Cells["colSubtotal"].Value = qty * price;
            }
            else
            {
                dgvCart.Rows.Remove(row);
            }

            CalculateTotal();
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            CalculateChange();
        }
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
    }

}
