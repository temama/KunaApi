using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace KunaApi
{
    public class KunaOrder
    {
        public string Id { get; set; }
        public string Side { get; set; }
        public string OrderType { get; set; }
        public double Price { get; set; }
        public double AvgPrice { get; set; }
        public string State { get; set; }
        public string Market { get; set; }
        public DateTime CreatedAt { get; set; }
        public double Volume { get; set; }
        public double RemainingVolume { get; set; }
        public double ExecutedVolume { get; set; }
        public int TradesCount { get; set; }

        public override string ToString()
        {
            return string.Format("#{0}:{1}:{2}:{3}", Id, Side, Price, Volume);
        }

        public static KunaOrder FromJson(JObject json)
        {
            return new KunaOrder()
            {
                Id = (json["id"] as JValue).Value.ToString(),
                Side = (json["side"] as JValue).Value.ToString(),
                OrderType = (json["ord_type"] as JValue).Value.ToString(),
                Price = Convert.ToDouble((json["price"] as JValue).Value.ToString(), CultureInfo.InvariantCulture),
                AvgPrice = Convert.ToDouble((json["avg_price"] as JValue).Value.ToString(), CultureInfo.InvariantCulture),
                State = (json["state"] as JValue).Value.ToString(),
                Market = (json["market"] as JValue).Value.ToString(),
                CreatedAt = DateTime.Parse((json["created_at"] as JValue).Value.ToString()),
                Volume = Convert.ToDouble((json["volume"] as JValue).Value.ToString(), CultureInfo.InvariantCulture),
                RemainingVolume = Convert.ToDouble((json["remaining_volume"] as JValue).Value.ToString(), CultureInfo.InvariantCulture),
                ExecutedVolume = Convert.ToDouble((json["executed_volume"] as JValue).Value.ToString(), CultureInfo.InvariantCulture),
                TradesCount = Convert.ToInt32((json["trades_count"] as JValue).Value)
            };
        }
    }
}