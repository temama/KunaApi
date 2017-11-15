using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace KunaApi
{
    public class KunaOrderBook
    {
        public List<KunaOrder> Asks { get; private set; }

        public List<KunaOrder> Bids { get; private set; }

        public KunaOrderBook()
        {
            Asks = new List<KunaOrder>();
            Bids = new List<KunaOrder>();
        }

        public static KunaOrderBook FromJson(JObject json)
        {
            var res = new KunaOrderBook();
            foreach (JObject orderJson in (json["asks"] as JArray))
            {
                res.Asks.Add(KunaOrder.FromJson(orderJson));
            }

            foreach (JObject orderJson in (json["bids"] as JArray))
            {
                res.Bids.Add(KunaOrder.FromJson(orderJson));
            }
            return res;
        }
    }
}
