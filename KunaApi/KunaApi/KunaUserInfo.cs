using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunaApi
{
    public class KunaUserInfo
    {
        public class KunaAccount
        {
            public string Currency { get; set; }
            public double Balance { get; set; }
            public double Locked { get; set; }
        }

        public string Email { get; set; }
        public bool Activated { get; set; }
        public List<KunaAccount> Accounts;
        public Dictionary<string, KunaAccount> AccountsByCurr;

        public KunaUserInfo()
        {
            Accounts = new List<KunaAccount>();
            AccountsByCurr = new Dictionary<string, KunaAccount>();
        }

        public static KunaUserInfo FromJson(JObject json)
        {
            var res = new KunaUserInfo()
            {
                Email = (json["email"] as JValue).Value.ToString(),
                Activated = (bool)(json["activated"] as JValue).Value
            };

            foreach (JObject acc in (json["accounts"] as JArray))
            {
                var kunaAcc = new KunaAccount()
                {
                    Currency = acc["currency"].ToString(),
                    Balance = Convert.ToDouble(acc["balance"].ToString(), CultureInfo.InvariantCulture),
                    Locked = Convert.ToDouble(acc["locked"].ToString(), CultureInfo.InvariantCulture)
                };
                res.Accounts.Add(kunaAcc);
                res.AccountsByCurr[kunaAcc.Currency] = kunaAcc;
            }

            return res;
        }
    }
}
