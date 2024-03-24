using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectSoftwareApplication
{
    internal class MySQLExporter
    {
        private static readonly string connectionString = "Server=localhost;Database=tickerdata;User ID=root;Password=¨PASSWORD;";

        public static void ExportToMySql(string symbol, decimal? price, string fiftyTwoWeekRange, string averageAnalystRating, string dividendYield, string marketCap, string earningsDate)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO StockData (Symbol, Price, FiftyTwoWeekRange, AverageAnalystRating, DividendYield, MarketCap, EarningsDate) 
                                 VALUES (@Symbol, @Price, @FiftyTwoWeekRange, @AverageAnalystRating, @DividendYield, @MarketCap, @EarningsDate)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Symbol", symbol);
                    cmd.Parameters.AddWithValue("@Price", price.HasValue ? .ToDecimal(price) : "N/A");
                    cmd.Parameters.AddWithValue("@FiftyTwoWeekRange", fiftyTwoWeekRange ?? "N/A");
                    cmd.Parameters.AddWithValue("@AverageAnalystRating", averageAnalystRating ?? "N/A");
                    cmd.Parameters.AddWithValue("@DividendYield", dividendYield ?? "N/A");
                    cmd.Parameters.AddWithValue("@MarketCap", marketCap ?? "N/A");
                    cmd.Parameters.AddWithValue("@EarningsDate", earningsDate ?? "N/A");

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data exported to MySQL database successfully.", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting data to MySQL: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
