using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppGreat.Services
{
    public class ExchangeRateService
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<decimal> GetExchangeRate(string currencyCode)
        {
            HttpResponseMessage response = await client.GetAsync("https://api.exchangeratesapi.io/latest?base=BGN");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Rate>(responseBody);

            var rateExhange = decimal.Parse(result.Fields["rates"][currencyCode].ToString());

            return rateExhange;
        }
    }

    public class Rate
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> Fields { get; set; }
    }
}
