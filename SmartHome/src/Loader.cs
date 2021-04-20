using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
