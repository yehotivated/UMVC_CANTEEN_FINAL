using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY.Database
{
    public class UsersDAL
    {
        // ===============================
        // LOAD USERS FOR DATAGRIDVIEW
        // ===============================
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
            SELECT 
                user_id,
                username,
                ROLE
            FROM Usserr
        ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                da.Fill(dt);
            }

            return dt;
        }

        // ===============================
        // DELETE USER
        // ===============================
        public static void DeleteUser(int userId)
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = "DELETE FROM Usserr WHERE user_id = @id";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", userId);

                cmd.ExecuteNonQuery();
            }
        }

        // ===============================
        // UPDATE USER (NO PASSWORD)
        // ===============================
        public static void UpdateUser(int userId, string username, string role)
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
                    UPDATE Usserr 
                    SET username = @username, ROLE = @role 
                    WHERE user_id = @id
                ";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@id", userId);

                cmd.ExecuteNonQuery();
            }
        }

        // ===============================
        // UPDATE USER (WITH PASSWORD)
        // ===============================
        public static void UpdateUserWithPassword(int userId, string username, string password, string role)
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
                    UPDATE Usserr 
                    SET username = @username, PASSWORD = @password, ROLE = @role 
                    WHERE user_id = @id
                ";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@id", userId);

                cmd.ExecuteNonQuery();
            }
        }

        // ===============================
        // ADD USER (CREATE ACCOUNT)
        // ===============================
        public static void AddUser(string username, string password, string role)
        {
            using (MySqlConnection con = DBConnection.GetConnection())
            {
                con.Open();

                string query = @"
                    INSERT INTO Usserr (username, PASSWORD, ROLE)
                    VALUES (@username, @password, @role)
                ";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", role);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
