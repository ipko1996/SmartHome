using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace SmartHome
{
    class Monitor : IMonitor
    {
        private const string URL = "http://193.6.19.58:8182/smarthome/";
        public async Task<Session> getSessionAsync(string homeId)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(URL + homeId);
            var responseContent = response.Content;
            string responseString = responseContent.ReadAsStringAsync().Result;

            //Console.WriteLine(response.StatusCode);

            return JsonConvert.DeserializeObject<Session>(responseString);          
        }
    }
}
