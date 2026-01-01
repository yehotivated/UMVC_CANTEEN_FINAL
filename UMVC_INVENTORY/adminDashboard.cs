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
    public partial class adminDashboard : BaseDashboardForm
    {
        #region Constructor

        public adminDashboard()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void adminDashboard_Load(object sender, EventArgs e)
        {
            lblTodaySales.Text = "₱ " + DashboardDAL.GetTodaySales().ToString("N2");
            lblTotalTransactions.Text = DashboardDAL.GetTotalTransactions().ToString();
            lblLowStock.Text = DashboardDAL.GetLowStockCount().ToString();

            LoadWeeklySalesChart();
        }

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminTransactions());
        }


        private void btnInventory_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminInventory());
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminDashboard());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminPOS());
        }


        private void btnReports_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminReports());
        }


        private void btnUsers_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminUsers());
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            AdminNavigation.Open(this, new adminSupplier());
        }


        #endregion

        #region Navigation Event Handlers

        private void button1_Click(object sender, EventArgs e)
        {
            OnDashboardButtonClick(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigateToAdminPOS();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigateToAdminInventory();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NavigateToAdminSupplier();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NavigateToAdminTransactions();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NavigateToAdminReports();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            NavigateToAdminUsers();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OnLogoutButtonClick(sender, e);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            OnBackButtonClick(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            NavigateToAddProduct();
        }

        #endregion

        #region Navigation Methods

        private void NavigateToAdminPOS()
        {
            OpenFormAndHideCurrent(new adminPOS());
        }

        private void NavigateToAdminInventory()
        {
            OpenFormAndHideCurrent(new adminInventory());
        }

        private void NavigateToAdminSupplier()
        {
            OpenFormAndHideCurrent(new adminSupplier());
        }

        private void NavigateToAdminTransactions()
        {
            OpenFormAndHideCurrent(new adminTransactions());
        }

        private void NavigateToAdminReports()
        {
            OpenFormAndHideCurrent(new adminReports());
        }

        private void NavigateToAdminUsers()
        {
            OpenFormAndHideCurrent(new adminUsers());
        }

        private void NavigateToAddProduct()
        {
            OpenFormAndHideCurrent(new adminAddProductBtn());
        }

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

}
