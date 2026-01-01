using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;

namespace UMVC_INVENTORY.Database
{
    class SupplierDAL
    {
        // ===============================
        // GET ALL SUPPLIERS
        // ===============================
        public static DataTable GetSuppliers()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM supplieer";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        // ===============================
        // ADD SUPPLIER
        // ===============================
        public static void AddSupplier(
            string supplierCode,
            string supplierName,
            string contactPerson,
            string phone,
            string email,
            string address,
            string status
)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
            INSERT INTO supplieer
            (supplier_code, supplier_name, contact_person, phone, email, address, status)
            VALUES
            (@code, @name, @contact, @phone, @email, @address, @status)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@code", supplierCode);
                cmd.Parameters.AddWithValue("@name", supplierName);
                cmd.Parameters.AddWithValue("@contact", contactPerson);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@status", status);

                cmd.ExecuteNonQuery();
            }
        }


        // ===============================
        // UPDATE SUPPLIER
        // ===============================
        public static void UpdateSupplier(
            int supplierId,
            string supplierCode,
            string supplierName,
            string contactPerson,
            string phone,
            string email,
            string address,
            string status
)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
            UPDATE supplieer SET
                supplier_code = @code,
                supplier_name = @name,
                contact_person = @contact,
                phone = @phone,
                email = @email,
                address = @address,
                status = @status
            WHERE id = @id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", supplierId);
                cmd.Parameters.AddWithValue("@code", supplierCode);
                cmd.Parameters.AddWithValue("@name", supplierName);
                cmd.Parameters.AddWithValue("@contact", contactPerson);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@status", status);

                cmd.ExecuteNonQuery();
            }
        }


        // ===============================
        // DELETE SUPPLIER
        // ===============================
        public static void DeleteSupplier(int id)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM supplieer WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
