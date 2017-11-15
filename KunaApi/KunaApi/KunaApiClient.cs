using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KunaApi
{
    public class KunaApiClient
    {
        private static string _baseUri = "https://kuna.io/api/v2/";
        private string _publicKey;
        private string _secretKey;

        public KunaApiClient(string publicKey, string secret)
        {
            _publicKey = publicKey;
            _secretKey = secret;
        }


        public static long GetServerTimestamp()
        {
            return Convert.ToInt64(Query(_baseUri + "timestamp"));
        }

        public static KunaTicker GetTicker(string market)
        {
            var tickerResponse = Query(_baseUri + "tickers/" + market);            
            var json = JObject.Parse(tickerResponse);
            return KunaTicker.FromJson(json);
        }

        public static KunaOrderBook GetOrderBook(string market)
        {
            var tickerResponse = Query(_baseUri + "order_book?market=" + market);
            var json = JObject.Parse(tickerResponse);
            return KunaOrderBook.FromJson(json);
        }

        public static List<KunaTrade> GetTrades(string market)
        {
            var tickerResponse = Query(_baseUri + "trades?market=" + market);
            var json = JArray.Parse(tickerResponse);

            var trades = new List<KunaTrade>();
            foreach (var jsonTrade in json)
            {
                trades.Add(KunaTrade.FromJson(jsonTrade as JObject));
            }
            return trades;
        }


        public KunaUserInfo GetUserInfo()
        {
            var response = UserQuery("members/me", "GET", new Dictionary<string, string>());
            var json = JObject.Parse(response);
            return KunaUserInfo.FromJson(json);
        }

        public KunaOrder PlaceOrder(string market, string side, double volume, double price)
        {
            var response = UserQuery("orders", "POST", new Dictionary<string, string>(){
                { "side", side },
                { "volume", volume.ToString(CultureInfo.InvariantCulture) },
                { "market", market },
                { "price", price.ToString(CultureInfo.InvariantCulture) }
            });
            
            return KunaOrder.FromJson(JObject.Parse(response));
        }

        public KunaOrder CancelOrder(KunaOrder order)
        {
            return CancelOrder(order.Id);
        }

        public KunaOrder CancelOrder(string orderId)
        {
            var response = UserQuery("order/delete", "POST", new Dictionary<string, string>()
                { { "id", orderId } });

            return KunaOrder.FromJson(JObject.Parse(response));
        }

        public List<KunaOrder> ActiveOrders(string market)
        {
            var response = UserQuery("orders", "GET", new Dictionary<string, string>() { { "market", market } });            
            var json = JArray.Parse(response);

            var orders = new List<KunaOrder>();
            foreach (var jsonOrder in json)
            {
                orders.Add(KunaOrder.FromJson(jsonOrder as JObject));
            }
            return orders;
        }

        public List<KunaTrade> UserTrades(string market)
        {
            var response = UserQuery("orders", "GET", new Dictionary<string, string>() { { "market", market } });
            var json = JArray.Parse(response);

            var trades = new List<KunaTrade>();
            foreach (var jsonTrade in json)
            {
                trades.Add(KunaTrade.FromJson(jsonTrade as JObject));
            }
            return trades;
        }
        

        private string UserQuery(string path, string method, Dictionary<string, string> args)
        {
            args["access_key"] = _publicKey;
            args["tonce"] = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            args["signature"] = GenerateSignature(path, method, args);
            var dataStr = BuildPostData(args, true);

            if (method == "POST")
            {
                var request = WebRequest.Create(new Uri(_baseUri + path + "?" + dataStr)) as HttpWebRequest;
                if (request == null)
                    throw new Exception("Non HTTP WebRequest: " + _baseUri + path);
                
                request.Method = method;
                request.Timeout = 15000;
                request.ContentType = "application/x-www-form-urlencoded";
                return new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            }
            else
            {
                return Query(_baseUri + path + "?" + dataStr);
            }
        }

        private string GenerateSignature(string path, string method, Dictionary<string, string> args)
        {
            var uri = "/api/v2/" + path;
            var sortetDict = new SortedDictionary<string, string>(args);
            var sortedArgs = BuildPostData(sortetDict, true);
            var msg = method + "|" + uri + "|" + sortedArgs;  // "HTTP-verb|URI|params"
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var msgBytes = Encoding.ASCII.GetBytes(msg);
            using (var hmac = new HMACSHA256(key))
            {
                byte[] hashmessage = hmac.ComputeHash(msgBytes);
                return BitConverter.ToString(hashmessage).Replace("-", string.Empty).ToLower();
            }
        }

        private static string BuildPostData(IDictionary<string, string> dict, bool escape = true)
        {
            return string.Join("&", dict.Select(kvp =>
                 string.Format("{0}={1}", kvp.Key, escape ? HttpUtility.UrlEncode(kvp.Value) : kvp.Value)));
        }

        private static string Query(string url)
        {
            var request = WebRequest.Create(url);
            request.Proxy = WebRequest.DefaultWebProxy;
            request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            if (request == null)
                throw new Exception("Non HTTP WebRequest");
            return new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
        }
    }
}
