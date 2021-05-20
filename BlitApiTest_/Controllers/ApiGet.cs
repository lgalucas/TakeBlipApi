using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace MyNamespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class Apicontroller : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            string url = "https://api.github.com/users/takenet/repos";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("MyApplication", "1"));

            HttpResponseMessage response = await client.GetAsync(url);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonString);
            List<Data> responseData = new List<Data>();
            foreach (var item in jsonObject)
            {
                if (Convert.ToString(item.language) == "C#")
                {
                    string date = item.created_at;
                    Console.WriteLine("{0},{1},{2}", item.created_at, item.name, item.description);
                    DateTime myDate = DateTime.ParseExact(date, "MM/dd/yyyy HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture);
                    responseData.Add(new Data(Convert.ToString(item.name), myDate, Convert.ToString(item.description)));
                }
            }
            responseData.Sort((a, b) => a.created_at.CompareTo(b.created_at));
            string isoJson = JsonConvert.SerializeObject(responseData);
            Console.WriteLine(isoJson);
            return Ok(isoJson);
        }    
    }

    class Data
    {
        public string name { get; set; }
        public DateTime created_at { get; set; }
        public string description { get; set; }
        public Data(string name, DateTime created_at, string description)
        {
            this.name = name;
            this.created_at = created_at;
            this.description = description;
        }

    }
}

