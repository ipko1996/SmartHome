using System.Collections.Generic;

namespace SmartHome
{
    public class Subscriber
    {
        public string subscriber { get; set; }
        public string homeId { get; set; }
        public string boilerType { get; set; }
        public string airConditionerType { get; set; }
        public List<Temperature> temperatures { get; set; }
    }
}
