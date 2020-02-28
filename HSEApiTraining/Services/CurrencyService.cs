using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HSEApiTraining
{
    public interface ICurrencyService
    {
        IEnumerable<double> GetRates(string currency, DateTime? start, DateTime? end);
    }

    public class CurrencyService : ICurrencyService
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly Regex CurrencyRegex = new Regex(@"^[A-Z]{3}$");

        public IEnumerable<double> GetRates(string currency, DateTime? start, DateTime? end)
        {
            // HttpResponseMessage msg = await _client.GetAsync();
            return GetRatesAsync(currency, start, end).Result;
        }

        private async Task<IEnumerable<double>> GetRatesAsync(string currency, DateTime? start, DateTime? end)
        {
            if (!CurrencyRegex.IsMatch(currency))
                throw new FormatException($"`{currency}` is not a currency shortcut");

            HttpResponseMessage msg = new HttpResponseMessage();
            if (start == null)
            {
                msg = await Client.GetAsync($"https://api.ratesapi.io/api/latest?base=RUB&symbols={currency}");
            }
            else if (end == null)
            {
                msg = await Client.GetAsync($"https://api.ratesapi.io/api/{start.Value.Year}-{start.Value.Month}-{start.Value.Day}?base=RUB&symbols={currency}");
            }
            else
            {
                List<double> res = new List<double>();
                for (DateTime t = start.Value; t <= end.Value; t = t.AddDays(1))
                {
                    msg = await Client.GetAsync($"https://api.ratesapi.io/api/{t.Year}-{t.Month}-{t.Day}?base=RUB&symbols={currency}");
                    msg.EnsureSuccessStatusCode();
                    res.Add(double.Parse(JObject.Parse(msg.Content.ReadAsStringAsync().Result)["rates"][currency].ToString()));
                }
                return res;
            }
            msg.EnsureSuccessStatusCode();
            JObject obj = JObject.Parse(await msg.Content.ReadAsStringAsync());
            if (!obj.ContainsKey("rates")) return null;
            return new List<double> { double.Parse(obj["rates"][currency].ToString()) };
        }
    }
}