using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHome
{
    class Driver : IDriver
    {
        private const string URL = "http://193.6.19.58:8182/smarthome/";
        public async Task<int> sendCommand(Command c)
        {
            string boilerStop = "";
            string boilerStart = "";
            string acStop = "";
            string acStart = "";

            initCommands(c.subscriber, ref boilerStop, ref boilerStart, ref acStop, ref acStart);

            var commands = createCommands(c.subscriber, c.boilderCommand, c.airConditionerCommand, boilerStop, boilerStart, acStop, acStart);

            string responseString = await getResponseFromServerAsync(c.subscriber, commands);

            int resp = -1;
            Int32.TryParse(responseString, out resp);

            return resp;
        }

        private Dictionary<string, string> createCommands(Subscriber sub, bool boiler, bool ac, string boilerStop, string boilerStart, string acStop, string acStart)
        {
            return new Dictionary<string, string>
            {
                { "homeId", sub.homeId },
                { "boilerCommand", boiler ? boilerStart : boilerStop },
                { "airConditionerCommand", ac ? acStart : acStop}
            };
        }

        private async Task<string> getResponseFromServerAsync(Subscriber sub, Dictionary<string, string> values)
        {
            var client = new HttpClient();
            string commands = JsonConvert.SerializeObject(values);
            var data = new StringContent(commands);
            var response =await client.PostAsync(URL + sub.homeId, data);
            var responseContent = response.Content;
            string responseString = responseContent.ReadAsStringAsync().Result;
            return responseString;
        }

        private void initCommands(Subscriber sub, ref string boilerStop, ref string boilerStart, ref string acStop, ref string acStart)
        {
            switch (sub.boilerType)
            {
                case "Boiler 1200W":
                    boilerStop = "bX1232";
                    boilerStart = "bX3434";
                    break;
                case "Boiler p5600":
                    boilerStop = "cX3452";
                    boilerStart = "cX7898";
                    break;
                case "Boiler tw560":
                    boilerStop = "dX111";
                    boilerStart = "dX3422";
                    break;
                case "Boiler 1400L":
                    boilerStop = "kx8417";
                    boilerStart = "kx4823";
                    break;
                default: 
                    Console.WriteLine("No boiler command found for this type of boiler OR subscriber has no boiler!");
                    break;
            }

            switch (sub.airConditionerType)
            {
                case "Air p5600":
                    acStop = "bX3421";
                    acStart = "bX5676";
                    break;
                case "Air c3200":
                    acStop = "cX5423";
                    acStart = "cX3452";
                    break;
                case "Air rk110":
                    acStop = "eX2222";
                    acStart = "eX1111";
                    break;
                default:
                    Console.WriteLine("No airconditioner command found for this type of ac OR subscriber has no ac!");
                    break;
            }
        }
    }
}
