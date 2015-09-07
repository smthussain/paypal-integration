using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grove.PaypalIntegration.Models
{
    public class PaymentCompleteModel
    {
        public string Name { get; private set; }
        public string Address { get; private set;  }

        public string Item { get; private set; }

        public string Amount { get; private set; }
        public string Currency { get; private set; }

        public PaymentCompleteModel(Dictionary<string, string> paypalResponse)
        {
            Name = paypalResponse["first_name"] + " " + paypalResponse["last_name"];

            Address =
                paypalResponse["address_street"] + ", " +
                paypalResponse["address_city"] + ", " +
                paypalResponse["address_zip"];

            Item = paypalResponse["item_name"];
            Amount = paypalResponse["mc_gross"];
            Currency = paypalResponse["mc_currency"];

        }

    }
}