using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY.Database
{
    public class ReportsDAL
    {
        public static DataTable GetInventoryReport()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                        SELECT
                            product_code,
                            product_name,
                            quantity,
                            unit_price
                        FROM products
                    ";


                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            return dt;
        }
        // ===============================
        // SALES REPORT (BY DATE RANGE)
        // ===============================
        public static DataTable GetSalesReportByDate(DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                        SELECT
                            product_code,
                            product_name,
                            quantity,
                            unit_price
                        FROM products
                    ";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        // ===============================
        // TRANSACTION REPORT (BY DATE RANGE)
        // ===============================
        public static DataTable GetTransactionReportByDate(DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                        SELECT
                            product_code,
                            product_name,
                            quantity,
                            unit_price
                        FROM products
                    ";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public static DataTable GetLowStockReport()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT 
                product_code,
                product_name,
                category,
                quantity,
                critical_level,
                unit_price,
                status
            FROM products
            WHERE quantity <= critical_level
            ORDER BY quantity ASC";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            return dt;
        }

    }
}
