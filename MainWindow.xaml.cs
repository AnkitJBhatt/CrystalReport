using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System;

namespace CrystalReportDemo
{
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void btnViewer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-IA46C6P\\SQLEXPRESS;Initial Catalog=ArkaPRP84_Alarm;User ID=sa;Password=Test#1234");
                con.Open();
                string query = @"SELECT UA.[Actions], UA.[Comment], UA.[OldValue], UA.[NewValue], UA.[ActionType], UA.[ActionDateAndTime], U.FullName FROM [ArkaPRP84_UserAudit].[dbo].[UserAction] UA 
                                INNER JOIN [ArkaPRP84_UserAudit].[dbo].[User] U ON U.Id = UA.UserId WHERE U.Id in(141,10140,10141) ORDER BY U.FullName, UA.ActionType;";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                sqlDataAdapter.Fill(ds, "Alarm");

                if (ds.Tables["Alarm"].Rows.Count == 0)
                {
                    MessageBox.Show("No data found.");
                    return;
                }

                CrystalReport crystalReport = new CrystalReport(); 
                crystalReport.SetDataSource(ds.Tables["Alarm"]);
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

                        BatchParameterReport report = new BatchParameterReport();
                        report.SetDataSource(ds.Tables["ParameterLogTable"]);
                        crystalReportViewer2.ViewerCore.ReportSource = report;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
