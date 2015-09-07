using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grove.PaypalIntegration.Models;

namespace Grove.PaypalIntegration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult PaymentComplete(string tx)
        {
            var results = RetrieveTransactionDetails(tx);

            var model = new PaymentCompleteModel(results);

            return View(model);

        }

        /// <summary>
        /// Retrieve transacation details from Paypal Sandbox
        /// Code is not mine, but taken from Paypal API documentation
        /// </summary>
        /// <param name="tx">Transactions identifier</param>
        /// <returns>Key-Value pairs of all details about a transaction</returns>
        private static Dictionary<string, string> RetrieveTransactionDetails(string tx)
        {
            const string authToken = "";

            var query = "cmd=_notify-synch&tx=" + tx + "&at=" + authToken;
            var request = WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = query.Length;

            //Send the request to PayPal and get the response
            var streamOut = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(query);
            streamOut.Close();
            var streamIn = new StreamReader(request.GetResponse().GetResponseStream());
            var strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            var results = new Dictionary<string, string>();

            var reader = new StringReader(strResponse);
            var line = reader.ReadLine();

            if (line != "SUCCESS")
                throw new NotSupportedException("Only happy path supported");

            while ((line = reader.ReadLine()) != null)
                results.Add(line.Split('=')[0], WebUtility.UrlDecode(line.Split('=')[1]));
            return results;
        }
    }
}