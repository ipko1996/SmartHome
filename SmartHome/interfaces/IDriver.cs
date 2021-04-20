using System.Threading.Tasks;

namespace SmartHome
{
    interface IDriver
    {
        Task<int> sendCommand(Command c);
    }
}
