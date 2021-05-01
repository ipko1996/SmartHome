using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    public class Command
    {
        public Command(Subscriber subscriber, bool boilderCommand, bool airConditionerCommand)
        {
            this.subscriber = subscriber;
            this.boilderCommand = boilderCommand;
            this.airConditionerCommand = airConditionerCommand;
        }

        public Subscriber subscriber { get; set; }
        public bool boilderCommand { get; set; }
        public bool airConditionerCommand { get; set; }
    }
}
