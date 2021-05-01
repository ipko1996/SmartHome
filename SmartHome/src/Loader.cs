using Newtonsoft.Json;
using System.IO;

namespace SmartHome
{
    public class Loader : ILoader
    {
        public Subscribers loadSubscribers()
        {
            return JsonConvert.DeserializeObject<Subscribers>(File.ReadAllText("data.json"));
        }

        public Subscribers loadSubscribersBadFileName()
        {
            return JsonConvert.DeserializeObject<Subscribers>(File.ReadAllText("dat.json"));
        }

    }
}
