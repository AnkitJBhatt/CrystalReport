using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrystalReportDemo.Forms
{
    /// <summary>
    /// Interaction logic for Report1.xaml
    /// </summary>
    public partial class Report1 : Window
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IA46C6P\\SQLEXPRESS;Initial Catalog=ArkaPRP83_Alarm;User ID=sa;Password=Test#1234");
        public Report1()
        {
            InitializeComponent();
            con.Open();
        }

        private void btnViewer_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP (100) * FROM AlarmHistory", con);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds, "Alarm");

            if (ds.Tables["Alarm"].Rows.Count == 0)
            {
                MessageBox.Show("No data found.");
                return;
            }

            //CrystalReport2 crystalReport1 = new CrystalReport2();
            //crystalReport1.SetDataSource(ds.Tables["Alarm"]);

            //// Use ViewerCore for WPF viewer
            //crystalReportViewer1.ViewerCore.ReportSource = crystalReport1;
        }
    }
}
