using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Hubs
{
    public interface IClientModel
    {
        void SetAps(List<StationGroup> apGroups);
        void SetClients(List<Client> clients);
        List<StationGroup> GetAps();
        List<Client> GetClients();
    }

    public interface IClientRestrictModel
    {
        List<StationGroup> GetAps();
    }
}
