using System;
using Binance.Net;
using Binance.Net.Objects;
using Binance.Net.Logging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using BinanceAPI.ClientConsole;

namespace BinanceApi.ClientConsole
{
    class Program
    {
        Dictionary<string, string> mainConfig;
        string APIKey = "";
        string APISecret = "";
        bool HasKeyAndSecret = false;

        static void Main(string[] args)
        {
            new Program().Start(args);
        }

        public void Start(string[] args)
        {
            LoadKeyAndSecret();

            if (HasKeyAndSecret)
            {
                BinanceDefaults.SetDefaultApiCredentials(APIKey, APISecret);
            }
            BinanceDefaults.SetDefaultLogVerbosity(LogVerbosity.Debug);
            BinanceDefaults.SetDefaultLogOutput(Console.Out);

            using (var client = new BinanceClient())
            using (var socketClient = new BinanceSocketClient())
            {
                // Public
                var ping = client.Ping();
                var serverTime = client.GetServerTime();
                var orderBook = client.GetOrderBook("BNBBTC", 10);
                var aggTrades = client.GetAggregatedTrades("BNBBTC", startTime: DateTime.UtcNow.AddMinutes(-2), endTime: DateTime.UtcNow, limit: 10);
                var klines = client.GetKlines("BNBBTC", KlineInterval.OneHour, startTime: DateTime.UtcNow.AddHours(-10), endTime: DateTime.UtcNow, limit: 10);
                var prices24h = client.Get24HPrice("BNBBTC");
                var allPrices = client.GetAllPrices();
                var allBookPrices = client.GetAllBookPrices();

                if (HasKeyAndSecret)
                {
                    // Private
                    var openOrders = client.GetOpenOrders("BNBBTC");
                    var allOrders = client.GetAllOrders("BNBBTC");
                    var testOrderResult = client.PlaceTestOrder("BNBBTC", OrderSide.Buy, OrderType.Limit, 1, price: 1, timeInForce: TimeInForce.GoodTillCancel);
                    var queryOrder = client.QueryOrder("BNBBTC", allOrders.Data[0].OrderId);
                    var orderResult = client.PlaceOrder("BNBBTC", OrderSide.Sell, OrderType.Limit, 10, price: 0.0002m, timeInForce: TimeInForce.GoodTillCancel);
                    var cancelResult = client.CancelOrder("BNBBTC", orderResult.Data.OrderId);
                    var accountInfo = client.GetAccountInfo();
                    var myTrades = client.GetMyTrades("BNBBTC");
                }

                if (HasKeyAndSecret)
                {
                    // Withdrawal/deposit
                    var withdrawalHistory = client.GetWithdrawHistory();
                    var depositHistory = client.GetDepositHistory();
                    var withdraw = client.Withdraw("ASSET", "ADDRESS", 0);
                }

                // Streams
                var successDepth = socketClient.SubscribeToDepthStream("bnbbtc", (data) =>
                {
                    // handle data
                });
                var successTrades = socketClient.SubscribeToTradesStream("bnbbtc", (data) =>
                {
                    // handle data
                });
                var successKline = socketClient.SubscribeToKlineStream("bnbbtc", KlineInterval.OneMinute, (data) =>
                {
                    // handle data
                });


                if (HasKeyAndSecret)
                {
                    var successStart = client.StartUserStream();
                    var successAccount = socketClient.SubscribeToAccountUpdateStream(successStart.Data.ListenKey, (data) =>
                    {
                        // handle data
                    });
                    var successOrder = socketClient.SubscribeToOrderUpdateStream(successStart.Data.ListenKey, (data) =>
                    {
                        // handle data
                    });
                }

                socketClient.UnsubscribeFromStream(successDepth.Data);
                socketClient.UnsubscribeFromAccountUpdateStream();
                socketClient.UnsubscribeAllStreams();
            }

            Console.WriteLine("Finished, Press and key to continue...");
            Console.ReadKey(true);
        }

        private void LoadKeyAndSecret()
        {
            // Load key and secret
            string configLocation = "config.ini";
            if (File.Exists(configLocation))
            {
                mainConfig = File.ReadAllLines(configLocation).ToDictionary(line => line.Split('=')[0], line => line.Split('=')[1].Trim());
                var apiKey = mainConfig.GetValue("APIKey");
                var apiSecret = mainConfig.GetValue("APISecret");
                if (!string.IsNullOrWhiteSpace(apiKey))
                    APIKey = apiKey;
                if (!string.IsNullOrWhiteSpace(apiSecret))
                    APISecret = apiSecret;
            }
            else
            {
                mainConfig = new Dictionary<string, string>();
                mainConfig.Add("APIKey", "");
                mainConfig.Add("APISecret", "");
                File.WriteAllLines(configLocation, mainConfig.Select(kvp => $"{kvp.Key}={kvp.Value}").ToList());
            }

            HasKeyAndSecret = !string.IsNullOrEmpty(APIKey) && !string.IsNullOrEmpty(APISecret);
        }
    }
}
