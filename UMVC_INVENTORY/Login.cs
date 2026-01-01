using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY
{
    public partial class Login : Form
    {
        private static Login loginInstance = null;

        public Login()
        {
            InitializeComponent();
            loginInstance = this;
            this.Load += Login_Load;
        }

        public static void ShowLoginForm()
        {
            if (loginInstance != null && !loginInstance.IsDisposed)
            {
                loginInstance.txtUsername.Clear();
                loginInstance.txtPassword.Clear();
                loginInstance.Show();
                loginInstance.txtUsername.Focus();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        // ===============================
        // LOGIN BUTTON
        // ===============================
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (MySqlConnection conn = new MySqlConnection(
                "server=localhost;database=canteen_db;uid=root;pwd=joshua;"))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT PASSWORD, ROLE 
                                        FROM Usserr 
                                        WHERE username = @username";


                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string role = reader["role"].ToString().Trim().ToLower();

                        // ? STEP 2 — STORE USERNAME
                        UserSession.Username = username;

                        switch (role)
                        {
                            case "admin":
                                UserSession.Role = UserRole.Admin;
                                new adminDashboard().Show();
                                break;

                            case "clerk":
                                UserSession.Role = UserRole.Clerk;
                                new clerkDashboard().Show();
                                break;

                            case "cashier":
                                UserSession.Role = UserRole.Cashier;
                                new cashierDashboard().Show();
                                break;

                            case "user":
                                UserSession.Role = UserRole.User;
                                new UserPOS().Show();
                                break;

                            default:
                                MessageBox.Show("Unknown role: " + role);
                                return;
                        }

                        MessageBox.Show("Login Successful");
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            CreateAccount form = new CreateAccount(this);
            form.ShowDialog(); // modal (recommended)
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
