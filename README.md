# Kuna.io C# API Client

[Kuna.io](https://kuna.io)

If this code was helpful for you and you want to say 'thanks', you are welcome. BTC: **1H1UFRLfcNkMLVWxDowrPgyWLesDyVnJuA**

## Usage

```cs
using KunaApi;

namespace KunaApiTest
{
    class Program
    {
        private static string _publicKey = ""; // Your public key
        private static string _secret = ""; // Your secret key

        static void Main(string[] args)
        {
            // Public methods:

            var time = KunaApiClient.GetServerTimestamp();

            var ticker = KunaApiClient.GetTicker(KunaMarket.BtcUah);

            var orderBook = KunaApiClient.GetOrderBook(KunaMarket.BtcUah);

            var marketTrades = KunaApiClient.GetTrades(KunaMarket.BtcUah);


            // User methods:

            var kunaApi = new KunaApiClient(_publicKey, _secret);

            var userInfo = kunaApi.GetUserInfo();

            var placedOrder = kunaApi.PlaceOrder(KunaMarket.BtcUah, "buy", 0.0001, 100);

            var cancelledOrder = kunaApi.CancelOrder(placedOrder);
            // var cancelledOrder = kunaApi.CancelOrder(placedOrder.Id); - the same

            var activeOrders = kunaApi.ActiveOrders(KunaMarket.BtcUah);

            var userTrades = kunaApi.UserTrades(KunaMarket.BtcUah);
        }
    }
}
```
