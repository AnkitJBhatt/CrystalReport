using System.Windows;
using System.Data;
using System.Data.SqlClient;
using CrystalReportDemo.Forms;
using System;

namespace CrystalReportDemo
{
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        //private void btnGrid_Click(object sender, RoutedEventArgs e)
        //{
        //    SqlCommand cmd = new SqlCommand("SELECT TOP (100) * FROM AlarmHistory", con);
        //    DataTable dt = new DataTable();
        //    dt.Load(cmd.ExecuteReader());
        //    dataGridViewTest.ItemsSource = dt.DefaultView;
        //    con.Close();
        //}

        private void btnViewer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-IA46C6P\\SQLEXPRESS;Initial Catalog=ArkaPRP84_Alarm;User ID=sa;Password=Test#1234");
                con.Open();
                // SQL Query: Get user action data ordered by user name and action date
                string query = @"
            SELECT UA.[Actions], UA.[Comment], UA.[OldValue], UA.[NewValue], UA.[ActionType], UA.[ActionDateAndTime], U.FullName FROM [ArkaPRP84_UserAudit].[dbo].[UserAction] UA 
            INNER JOIN [ArkaPRP84_UserAudit].[dbo].[User] U ON U.Id = UA.UserId WHERE U.Id in(141,10140,10141) ORDER BY U.FullName, UA.ActionType;";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                sqlDataAdapter.Fill(ds, "Alarm");

                // Check if any data is returned
                if (ds.Tables["Alarm"].Rows.Count == 0)
                {
                    MessageBox.Show("No data found.");
                    return;
                }

                // Create Crystal Report instance and bind data
                CrystalReport crystalReport = new CrystalReport(); // Replace with your actual report class name
                crystalReport.SetDataSource(ds.Tables["Alarm"]);

                // Assign report to the WPF viewer
                crystalReportViewer.ViewerCore.ReportSource = crystalReport;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void btnNextReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-IA46C6P\\SQLEXPRESS;Initial Catalog=CrystalReportDemo;User ID=sa;Password=Test#1234";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT  BatchName, BatchStartTime, BatchEndTime, RecipeName, 
                            OperationName, OperationId, OperationStartTime, OperationEndTime, DateTime, Temp_SetMin, Temp_SetMax, Hum_SetMin, Hum_SetMax, 
                            Pres_SetMin, Pres_SetMax, Speed_SetMin, Speed_SetMax, Temperature, Humidity, Pressure, MachineSpeed 
                            FROM ParameterLogTable;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "ParameterLogTable");

                        if (ds.Tables["ParameterLogTable"].Rows.Count == 0)
                        {
                            MessageBox.Show("No data found.");
                            return;
                        }

                        // Load the report
                        BatchParameterReport report = new BatchParameterReport();
                        report.SetDataSource(ds.Tables["ParameterLogTable"]);

                        // Bind to the Crystal Reports viewer
                        crystalReportViewer2.ViewerCore.ReportSource = report;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }




            //DataTable dt = GetReportData();
            //BatchParameterReport batchParameterReport = new BatchParameterReport(); // your .rpt file
            //batchParameterReport.SetDataSource(dt);
            //crystalReportViewer2.ViewerCore.ReportSource = batchParameterReport;

        }

        public DataTable GetReportData()
        {
            SqlConnection con2 = new SqlConnection("Data Source=DESKTOP-IA46C6P\\SQLEXPRESS;Initial Catalog=CrystalReportDemo;User ID=sa;Password=Test#1234");
            var dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_GetBatchParameterReport", con2))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con2.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}
