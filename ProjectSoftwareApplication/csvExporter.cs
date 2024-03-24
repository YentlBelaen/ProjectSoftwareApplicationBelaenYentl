using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectSoftwareApplication
{
    internal class csvExporter
    {
        public static void ExportToCsv(string symbol, string price, string fiftyTwoWeekRange, string averageAnalystRating, string dividendYield, string lastDividendDate, string marketCap, string earningsDate)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string fileName = $"StockData_{symbol}_{currentDate}.csv";

            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, true))
                {
                    if (new FileInfo(fileName).Length == 0)
                    {
                        sw.WriteLine("Symbol,Price,52 Week Range,Average Analyst Rating,Dividend Yield,Last Dividend Date,Market Cap,Earnings Date");
                    }
                    sw.WriteLine($"{symbol},{price},{fiftyTwoWeekRange},{averageAnalystRating},{dividendYield},{lastDividendDate},{marketCap},{earningsDate}");
                }

                MessageBox.Show($"Data exported to {fileName}", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
