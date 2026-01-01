using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY.Database
{
    public class DashboardDAL
    {
        // ============================
        // WEEKLY SALES (LAST 7 DAYS)
        // ============================
        public static DataTable GetWeeklySales()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
                    SELECT 
                        DATE(transaction_date) AS sale_date,
                        SUM(total) AS total_sales
                    FROM transactions
                    WHERE transaction_date >= DATE_SUB(CURDATE(), INTERVAL 7 DAY)
                    GROUP BY DATE(transaction_date)
                    ORDER BY sale_date ASC
                ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                da.Fill(dt);
            }

            return dt;
        }

        // ============================
        // TOTAL TRANSACTIONS (ALL)
        // ============================
        public static int GetTotalTransactions()
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = "SELECT COUNT(*) FROM transactions";

                MySqlCommand cmd = new MySqlCommand(query, con);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ============================
        // TODAY SALES AMOUNT
        // ============================
        public static decimal GetTodaySales()
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
                    SELECT IFNULL(SUM(total), 0)
                    FROM transactions
                    WHERE DATE(transaction_date) = CURDATE()
                ";

                MySqlCommand cmd = new MySqlCommand(query, con);
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        // ============================
        // TODAY TRANSACTION COUNT
        // ============================
        public static int GetTotalTransactionsToday()
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
                    SELECT COUNT(*)
                    FROM transactions
                    WHERE DATE(transaction_date) = CURDATE()
                ";

                MySqlCommand cmd = new MySqlCommand(query, con);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ============================
        // LOW STOCK COUNT
        // ============================
        public static int GetLowStockCount()
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
            SELECT COUNT(*)
            FROM products
            WHERE quantity <= critical_level
        ";

                MySqlCommand cmd = new MySqlCommand(query, con);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


    }
}