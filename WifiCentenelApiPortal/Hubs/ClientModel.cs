using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Hubs
{
    public class StationGroup
    {
        public bool IsUsed { get; set; }
        public string StationConnectionId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string LocationCode { get; set; }
        public Client Client { get; set; } = new Client();

    }

    public class Client
    {
        public bool StatusStation { get; set; }
        public string ConnectionId { get; set; }
        public string Mac { get; set; }
        public string LocationCode { get; set; }
        
    }
    public class ConnectionData
    {
        public Client Client { get; set; }
        public List<StationGroup> StationGroups { get; set; }
        
    }

    public class ClientModel : IClientModel
    {

        private List<StationGroup> _stationGroups;
        private List<Client> _clients;

        public void SetAps(List<StationGroup> stationGroups)
        {
            _stationGroups = stationGroups;
        }

        public void SetClients(List<Client> clients)
        {
            _clients = clients;
        }

        public List<StationGroup> GetAps()
        {
            return _stationGroups;
        }

        public List<Client> GetClients()
        {
            return _clients;
        }
    }


}
