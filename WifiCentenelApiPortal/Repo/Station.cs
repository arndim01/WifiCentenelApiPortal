using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Context;
using WifiCentenelApiPortal.Interfaces;
using WifiCentenelApiPortal.Models;

namespace WifiCentenelApiPortal.Repo
{
    public class Station : IStation
    {
        private readonly ApplicationDbContext _applicationDbContext; 

        public Station(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<ICollection<Models.Station>> GetAllStationAsync()
        {
            return await _applicationDbContext.Stations.ToListAsync();
        }
    }
}
