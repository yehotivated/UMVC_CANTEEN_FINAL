using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UMVC_INVENTORY.BaseForms;
using UMVC_INVENTORY;
using MySql.Data.MySqlClient;
using UMVC_INVENTORY.Database;
using System.Windows.Forms.DataVisualization.Charting;

namespace UMVC_INVENTORY
{
    public partial class cashierDashboard : BaseDashboardForm
    {
        #region Constructor

        public cashierDashboard()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void cashierDashboard_Load(object sender, EventArgs e)
        {
            lblTodaySales.Text = "₱ " + DashboardDAL.GetTodaySales().ToString("N2");
            lblTotalTransactions.Text = DashboardDAL.GetTotalTransactions().ToString();
            lblLowStock.Text = DashboardDAL.GetLowStockCount().ToString();

            LoadWeeklySalesChart();
        }

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new cashierTransactions());
        }



        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new cashierDashboard());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new cashierPOS());
        }



        #endregion

        #region Navigation Event Handlers


        #endregion
        private void LoadWeeklySalesChart()
        {
            chartWeeklySales.Series.Clear();
            chartWeeklySales.ChartAreas.Clear();

            ChartArea area = new ChartArea();
            area.AxisX.Title = "Date";
            area.AxisY.Title = "Sales Amount";
            chartWeeklySales.ChartAreas.Add(area);

            Series series = new Series("Weekly Sales");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;

            DataTable dt = DashboardDAL.GetWeeklySales();

            foreach (DataRow row in dt.Rows)
            {
                string dateLabel = Convert
                    .ToDateTime(row["sale_date"])
                    .ToString("MMM dd");

                decimal totalSales = Convert.ToDecimal(row["total_sales"]);

                series.Points.AddXY(dateLabel, totalSales);
            }

            chartWeeklySales.Series.Add(series);
        }


        private void btnTestDB_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    MessageBox.Show("Database Connected Successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PerformLogout();
        }

        private void PerformLogout()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                // Clear navigation history on logout
                NavigationManager.ClearHistory();

                // Mark that we're logging out (this ensures Login form will show)
                NavigationManager.SetLoggingOut(this);

                this.Close();
            }
        }
    }

}
