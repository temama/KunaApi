using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace KunaApi
{
    public class KunaTicker
    {
        public long Timestamp { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public double Last { get; set; }
        public double Volume { get; set; }
        public double Amount { get; set; }

        public static KunaTicker FromJson(JObject json)
        {
            return new KunaTicker()
            {
                Timestamp = Convert.ToInt64((json["at"] as JValue).Value),
                Buy = Convert.ToDouble((json["ticker"]["buy"] as JValue).Value, CultureInfo.InvariantCulture),
                Sell = Convert.ToDouble((json["ticker"]["sell"] as JValue).Value, CultureInfo.InvariantCulture),
                Low = Convert.ToDouble((json["ticker"]["low"] as JValue).Value, CultureInfo.InvariantCulture),
                High = Convert.ToDouble((json["ticker"]["high"] as JValue).Value, CultureInfo.InvariantCulture),
                Last = Convert.ToDouble((json["ticker"]["last"] as JValue).Value, CultureInfo.InvariantCulture),
                Volume = Convert.ToDouble((json["ticker"]["vol"] as JValue).Value, CultureInfo.InvariantCulture),
                Amount = json["ticker"]["amount"] != null ? Convert.ToDouble((json["ticker"]["amount"] as JValue).Value, CultureInfo.InvariantCulture) : 0, // Kuna doesn't send this
            };
        }
    }
}
