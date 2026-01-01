using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY.Database
{
    public class POSDAL
    {
        public static DataTable GetProducts()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
            SELECT
                product_id,   -- Use product_id instead of id
                product_code AS code,
                product_name AS name,
                quantity AS stock_qty,
                unit_price
            FROM products
            WHERE quantity > 0
        ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                da.Fill(dt);
            }

            return dt;
        }

        public static int SaveTransaction(string cashier, decimal total)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO transactions
                    (transaction_no, transaction_date, cashier, total, status)
                    VALUES
                    (@no, NOW(), @cashier, @total, 'Completed');
                    SELECT LAST_INSERT_ID();
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@no", "TRX-" + DateTime.Now.Ticks);
                cmd.Parameters.AddWithValue("@cashier", cashier);
                cmd.Parameters.AddWithValue("@total", total);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // SEARCH PRODUCT (POS)
        public static DataTable SearchProduct(string keyword)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT 
                product_id,   -- Use product_id instead of id
                product_code AS code,
                product_name AS name,
                quantity AS stock_qty,
                unit_price
            FROM products
            WHERE product_code LIKE @key
               OR product_name LIKE @key
        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@key", "%" + keyword + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public static void SaveTransactionItem(
            int transactionId,
            string name,
            decimal price,
            int qty,
            decimal subtotal)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO transaction_items
                    (transaction_id, product_name, price, quantity, subtotal)
                    VALUES
                    (@tid, @name, @price, @qty, @sub)
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tid", transactionId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@sub", subtotal);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeductStock(string productName, int qty)
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
