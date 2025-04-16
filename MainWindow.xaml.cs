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
                      SELECT 
    MAX(BatchName) AS BatchName,
    MAX(BatchStartTime) AS BatchStartTime,
    MAX(BatchEndTime) AS BatchEndTime,
    MAX(RecipeName) AS RecipeName,
	MAX(RecipeStartTime) AS RecipeStartTime,
    MAX(RecipeEndTime) AS RecipeEndTime,
    MAX(OperationName) AS OperationName,
    MAX(OperationId) AS OperationId,
    MAX(Iteration) AS Iteration,
    MAX(OperationStartTime) AS OperationStartTime,
    MAX(OperationEndTime) AS OperationEndTime,
    DateTime,
    MAX(Temp_SetMin) AS Temp_SetMin,
    MAX(Temp_SetMax) AS Temp_SetMax,
    MAX(Hum_SetMin) AS Hum_SetMin,
    MAX(Hum_SetMax) AS Hum_SetMax,
    MAX(Pres_SetMin) AS Pres_SetMin,
    MAX(Pres_SetMax) AS Pres_SetMax,
    MAX(Speed_SetMin) AS Speed_SetMin,
    MAX(Speed_SetMax) AS Speed_SetMax,
    MAX(Temperature) AS Temperature,
    MAX(Humidity) AS Humidity,
    MAX(Pressure) AS Pressure,
    MAX(MachineSpeed) AS MachineSpeed
FROM ParameterLogTable
GROUP BY DateTime  ORDER BY OperationId, DateTime ASC;";

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
