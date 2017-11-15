using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace KunaApi
{
    public class KunaTrade
    {
        public string Id { get; set; }
        public double Price { get; set; }
        public double Volume { get; set; }
        public double Funds { get; set; }
        public string Market { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Side { get; set; }

        public override string ToString()
        {
            return string.Format("#{0}:{1}:{2}:{3}", Id, Side, Volume, Price);
        }

        public static KunaTrade FromJson(JObject json)
        {
            return new KunaTrade()
            {
                Id = (json["id"] as JValue).Value.ToString(),
                Price = Convert.ToDouble((json["price"] as JValue).Value.ToString(), CultureInfo.InvariantCulture),
                Volume = Convert.ToDouble((json["volume"] as JValue).Value.ToString(), CultureInfo.InvariantCulture),
                Funds = json["funds"] != null ? Convert.ToDouble((json["funds"] as JValue).Value.ToString(), CultureInfo.InvariantCulture) : 0,
                Market = (json["id"] as JValue).Value.ToString(),
                CreatedAt = DateTime.Parse((json["created_at"] as JValue).Value.ToString()),
                Side = (json["side"] as JValue).Value != null ? (json["side"] as JValue).Value.ToString() : "null"
            };
        }
    }
}
