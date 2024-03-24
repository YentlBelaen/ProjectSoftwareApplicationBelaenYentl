using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;



namespace ProjectSoftwareApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 01522287e9msh8852813cf9cbd0dp190f50jsne27225221cb0
    /// Bovenstaand = RAPID API KEY
    public partial class MainWindow : Window
    {
        private string _symbol;
        private decimal? _regularMarketPrice;
        private string _fiftyTwoWeekRangeFmt;
        private string _averageAnalystRating;
        private string _dividendYieldFmt;
        private string _marketCap;
        private string _earningsDate;
        public MainWindow()
        {
            InitializeComponent();
        }


        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string stockTicker = SearchTxtBx.Text.ToUpper();

            if (string.IsNullOrEmpty(stockTicker))
            {
                MessageBox.Show("Please enter a valid stock ticker.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://yahoo-finance127.p.rapidapi.com/key-statistics/{stockTicker}"),
                Headers =
        {
            { "X-RapidAPI-Key", "01522287e9msh8852813cf9cbd0dp190f50jsne27225221cb0" },
            { "X-RapidAPI-Host", "yahoo-finance127.p.rapidapi.com" },
        },
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SearchResponseLbl.Content = "Succes!";
                        string jsonString = await response.Content.ReadAsStringAsync();
                        JObject jsonResponse = JObject.Parse(jsonString);
                        string symbol = jsonResponse["symbol"]?.ToString();
                        decimal? regularMarketPrice = jsonResponse["regularMarketPrice"]?["raw"]?.ToObject<decimal>();
                        string fiftyTwoWeekRange = jsonResponse["fiftyTwoWeekRange"]?["fmt"]?.ToString();
                        string averageAnalystRating = jsonResponse["averageAnalystRating"]?.ToString();
                        string dividendYieldFmt = jsonResponse["dividendYield"]?["fmt"]?.ToString();
                        string marketCap = jsonResponse["marketCap"]?["fmt"]?.ToString();
                        string earningsDate = jsonResponse["earningsTimestamp"]?["fmt"]?.ToString();

                        _symbol = symbol;
                        _regularMarketPrice = regularMarketPrice;
                        _fiftyTwoWeekRangeFmt = fiftyTwoWeekRange;
                        _averageAnalystRating = averageAnalystRating;
                        _dividendYieldFmt = dividendYieldFmt;
                        _marketCap = marketCap;
                        _earningsDate = earningsDate;

                        if (symbol != null)
                        {
                            SymbolLbl.Content = $"Symbol: {symbol}";
                        }

                        if (regularMarketPrice.HasValue)
                        {
                            PriceLbl.Content = $"Price: ${regularMarketPrice.Value.ToString("F2")}";
                        }

                        if (!string.IsNullOrEmpty(fiftyTwoWeekRange) && fiftyTwoWeekRange != "{")
                        {
                            FiftyTwoWeekRangeLbl.Content = $"52 Week Range: {fiftyTwoWeekRange}";
                        }

                        if (averageAnalystRating != null)
                        {
                            AverageAnalystRatingLbl.Content = $"Average Analyst Rating: {averageAnalystRating}";
                        }

                        if (!string.IsNullOrEmpty(dividendYieldFmt))
                        {
                            DividendYieldLbl.Content = $"Dividend Yield: {dividendYieldFmt}";
                        }
                        else
                        {
                            DividendYieldLbl.Content = "Dividend Yield: N/A";
                        }

                        if (marketCap != null)
                        {
                            MarketCapLbl.Content = $"Market Cap: {marketCap}";
                        }

                        if (earningsDate != null && earningsDate != "")
                        {
                            EarningsDateLbl.Content = $"Last Earnings Date: {earningsDate}";
                        }else
                        {
                            EarningsDateLbl.Content = "";
                        }
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error: {response.StatusCode}\n{errorMessage}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            csvExporter.ExportToCsv(_symbol, _regularMarketPrice?.ToString("F2") ?? "N/A", _fiftyTwoWeekRangeFmt ?? "N/A", _averageAnalystRating ?? "N/A", _dividendYieldFmt ?? "N/A", "N/A", _marketCap ?? "N/A", _earningsDate ?? "N/A");
        }

        private void DataBankButton_Click(object sender, RoutedEventArgs e)
        {
            MySQLExporter.ExportToMySql(_symbol, _regularMarketPrice, _fiftyTwoWeekRangeFmt, _averageAnalystRating, _dividendYieldFmt, _marketCap, _earningsDate);
        }
    }
}