using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY.Database
{
    public class InventoryDAL
    {
        // ===============================
        // GET ALL PRODUCTS (INVENTORY VIEW)
        // ===============================
        public static DataTable GetInventory()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT
                        product_id,
                        product_code,
                        product_name,
                        quantity,
                        unit_price,
                        CASE 
                            WHEN quantity <= 5 THEN 'Low Stock'
                            ELSE 'OK'
                        END AS status
                    FROM products
                ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            return dt;
        }


        public static DataTable GetCategories()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT DISTINCT category FROM products ORDER BY category";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            return dt;
        }

        public static DataTable GetInventoryByCategory(string category)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT
                product_id,
                product_code,
                product_name,
                category,
                supplier,
                quantity,
                critical_level,
                unit_price,
                status
            FROM products
            WHERE category = @category
        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@category", category);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        // ===============================
        // ADD PRODUCT
        // ===============================
        public static void AddProduct(
            string code,
            string name,
            string category,
            string supplier,
            int qty,
            int critical,
            decimal price,
            decimal cost)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO products
                    (product_code, product_name, category, supplier, quantity, critical_level, unit_price, cost)
                    VALUES
                    (@code, @name, @category, @supplier, @qty, @critical, @price, @cost)
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@supplier", supplier);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@critical", critical);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@cost", cost);

                cmd.ExecuteNonQuery();
            }
        }

        // ===============================
        // UPDATE STOCK
        // ===============================
        public static void UpdateStock(int id, int qty)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    UPDATE products
                    SET quantity = @qty
                    WHERE id = @id
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // ===============================
        // DELETE PRODUCT
        // ===============================
        public static void DeleteProduct(int id)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM products WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
