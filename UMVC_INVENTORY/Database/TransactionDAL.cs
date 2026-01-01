using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY.Database
{
    public class TransactionDAL
    {
        public static DataTable GetTransactions()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        id,
                        transaction_no,
                        transaction_date,
                        cashier,
                        total,
                        status
                    FROM transactions
                    ORDER BY transaction_date DESC
                ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            return dt;
        }

        public static DataTable GetTransactionsByDate(DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        id,
                        transaction_no,
                        transaction_date,
                        cashier,
                        total,
                        status
                    FROM transactions
                    WHERE transaction_date BETWEEN @from AND @to
                    ORDER BY transaction_date DESC
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
    }
}
