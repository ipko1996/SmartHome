using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome
{
    class Controller
    {
        static async Task Main(string[] args)
        {
            try
            {
                await MainTask();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        private static async Task MainTask()
        {
            Subscribers subs = getSubsribers();

            IMonitor mon = new Monitor();
            IDriver driver = new Driver();
            while (true)
            {
                foreach (var subscriber in subs.subscribers)
                {
                    double desiredTemperature = 0;
                    Session session = await mon.getSessionAsync(subscriber.homeId);

                    showSubscriberAndSessionDetails(subscriber, session, ref desiredTemperature);

                    await checkIfActionNeededAsync(session, desiredTemperature, subscriber, driver);
                }
                Console.WriteLine("Press Ctrl + C to exit");
                Thread.Sleep(300000);
            }
        }

        private static async Task checkIfActionNeededAsync(Session session, double desiredTemperature, Subscriber subscriber, IDriver driver)
        {

            bool ac = false;
            bool boiler = false;
            int res = 0;

            if (Math.Abs(session.temperature - desiredTemperature) < 0.2)
            {
                if (session.airConditionerState == true ||
                    session.boilerState == true)
                {
                    Console.Write("Everything is OK, but ");
                    Console.WriteLine(session.boilerState ? "- boiler needs to be turned off" : "");
                    Console.WriteLine(session.airConditionerState ? "- ac needs to be turned off" : "");

                    ac = false;
                    boiler = false;
                    res = await driver.sendCommand(new Command(subscriber, boiler, ac));
                    showResult(res);
                }
                else
                {
                    Console.WriteLine("Everything is OK, no action was needed!");
                }
            }
            else
            {
                Console.Write("Temperature is ");
                if (session.temperature > desiredTemperature)
                {
                    Console.WriteLine("higher then desired.");
                    Console.WriteLine("Trying to turn ac on and turn boiler off...");
                    ac = true;
                    boiler = false;

                }
                if (session.temperature < desiredTemperature)
                {
                    Console.WriteLine("lower then desired.");
                    Console.WriteLine("Trying to turn ac off and turn boiler on...");
                    ac = false;
                    boiler = true;
                }
                res = await driver.sendCommand(new Command(subscriber, boiler, ac));
                showResult(res);

                if ((Math.Abs(session.temperature - desiredTemperature)) >= desiredTemperature * 0.2)
                {
                    writeErrorToFile(session, desiredTemperature, subscriber);
                }
            }
            Console.WriteLine("\n");
        }

        private static void writeErrorToFile(Session session, double desiredTemperature, Subscriber subscriber)
        {
            using (StreamWriter sw = new StreamWriter("errors.log", append: true))
            {
                sw.WriteLine(
                    "SessionID: {0}\n" +
                    "HomeId: {1}\n" +
                    "DesiredTemperature: {2}\n" +
                    "RealTemperature: {3}\n" +
                    "Time: {4}:{5}\n\n",
                    session.sessionId,
                    subscriber.homeId,
                    desiredTemperature,
                    session.temperature,
                    now().Hour, now().Minute
                    );
            }
            Console.WriteLine("ERROR detected with temperature difference, entry created in logs!");
        }

        private static void showResult(int res)
        {
            Console.Write("Message from server: ({0})  ", res);
            switch (res)
            {
                case 100:
                    Console.WriteLine("Action done successfully!");
                    break;
                case 101:
                    Console.WriteLine("Wrong command!");
                    break;
                case 102:
                    Console.WriteLine("No device found with this id!");
                    break;
                case 103:
                    Console.WriteLine("Home id not found!");
                    break;
                default:
                    Console.WriteLine("Something went wrong!");
                    break;
            }
        }

        private static void showSubscriberAndSessionDetails(Subscriber subscriber, Session session, ref double desiredTemperature)
        {
            Console.WriteLine("SESSION ID: {0}", session.sessionId);
            Console.WriteLine("Subsciber: {0}", subscriber.subscriber);
            Console.WriteLine("HomeId: {0}", subscriber.homeId);
            Console.Write("Boiler type: {0}", subscriber.boilerType != "" ?
                subscriber.boilerType + ", Status: " : "Subscriber does not have a boiler system!\n");
            if (subscriber.boilerType != "")
                Console.WriteLine(session.boilerState ? "On" : "Off");

            Console.Write("Air conditioner type: {0}", subscriber.airConditionerType != "" ?
                subscriber.airConditionerType + ", Status: " : "Subsciber does not have an airconditioner system!\n");
            if (subscriber.airConditionerType != "")
                Console.WriteLine(session.airConditionerState ? "On" : "Off");

            int hour = now().Hour;
            int mins = now().Minute;
            Console.WriteLine("Temperatures:");
            foreach (var temperature in subscriber.temperatures)
            {
                Console.WriteLine("Period: {0}  Temperature: {1}",
                    temperature.period, temperature.temperature);

                //napszaknak megfelelo homerseklet beallitasa
                var temps = temperature.period.Split('-');
                int start = int.Parse(temps[0]);
                int end = int.Parse(temps[1]);
                if (hour >= start && hour <= end)
                    desiredTemperature = temperature.temperature;
            }
            Console.WriteLine("The time is: {0}:{1}", hour, mins);
            Console.WriteLine("Temperature in the house: {0} Celsius", session.temperature);
            Console.WriteLine("Desired temperature in this period is: {0}", desiredTemperature);
        }

        private static Subscribers getSubsribers()
        {
            ILoader loader = new Loader();

            Subscribers subs = loader.loadSubscribers();
            return subs;
        }

        private static DateTime now()
        {
            return DateTime.Now;
        }

    }
}
