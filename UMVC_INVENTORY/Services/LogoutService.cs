using System.Windows.Forms;

namespace UMVC_INVENTORY.Services
{
    public class LogoutService
    {
        public bool RequestLogout(Form form)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                ExecuteLogout(form);
                return true;
            }

            return false;
        }

        private void ExecuteLogout(Form form)
        {
            NavigationManager.ClearHistory();
            NavigationManager.SetLoggingOut(form);
            form.Close();
        }
    }
}
