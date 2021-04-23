using Newtonsoft.Json;
using System.IO;

namespace SmartHome
{
    class Loader : ILoader
    {
        public Subscribers loadSubscribers()
        {
            return JsonConvert.DeserializeObject<Subscribers>(File.ReadAllText("data.json"));
        }
    }
}
