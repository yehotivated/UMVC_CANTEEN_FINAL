using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace UMVC_INVENTORY.Database
{
    public class TransactionPOSDAL
    {
        public static int SaveTransaction(string cashier, decimal total)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string trxNo = "TRX-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                string query = @"INSERT INTO transactions
                                (transaction_no, transaction_date, cashier, total, status)
                                VALUES (@no, NOW(), @cashier, @total, 'Completed');
                                SELECT LAST_INSERT_ID();";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@no", trxNo);
                cmd.Parameters.AddWithValue("@cashier", cashier);
                cmd.Parameters.AddWithValue("@total", total);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static void SaveTransactionItem(
            int transactionId,
            string product,
            decimal price,
            int qty,
            decimal subtotal)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO transaction_items
                                (transaction_id, product_name, price, qty, subtotal)
                                VALUES (@tid, @product, @price, @qty, @subtotal)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tid", transactionId);
                cmd.Parameters.AddWithValue("@product", product);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@subtotal", subtotal);

                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateStock(string productName, int qty)
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
            UPDATE products
            SET quantity = quantity - @qty
            WHERE product_name = @name
        ";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@name", productName);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
