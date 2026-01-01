using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UMVC_INVENTORY
{
    public static class AdminNavigation
    {
        public static void Open(Form current, Form target)
        {
            target.Show();
            current.Hide();
        }
    }
}
