using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Models;

namespace WifiCentenelApiPortal.Interfaces
{
    public interface IStation
    {
        Task<ICollection<Station>> GetAllStationAsync();
    }
}
