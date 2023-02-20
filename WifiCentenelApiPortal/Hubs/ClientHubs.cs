using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Interfaces;

namespace WifiCentenelApiPortal.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    //[HubName("ServerRefreshHub")]
    public class ClientHubs: Hub
    {
        private static List<StationGroup> _stationGroups = new List<StationGroup>();
        private static List<Client> _clients = new List<Client>();
        private readonly ClaimsPrincipal _caller;
        private readonly IClientModel _clientModel;

        public ClientHubs(IHttpContextAccessor httpContextAccessor, IClientModel clientModel)
        {
            _clientModel = clientModel;
            _caller = httpContextAccessor.HttpContext.User;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            
            var station_result = _stationGroups.SingleOrDefault(g => g.StationConnectionId == Context.ConnectionId);
            if (station_result != null)
            {
                if (station_result.Client != null)
                {
                    var client = _clients.SingleOrDefault(g => g == station_result.Client);
                    if (client != null)
                    {
                        client.StatusStation = false;
                        BroadcastClient(client);
                    }
                }
                _stationGroups.Remove(station_result);
                BroadcastStationsInLocationCode(station_result.LocationCode);
                

            }

            var client_result = _clients.SingleOrDefault(g => g.ConnectionId == Context.ConnectionId);
            if (client_result != null)
            {
                if (client_result.StatusStation)
                {
                    var stations = _stationGroups.Where(g => g.LocationCode == client_result.LocationCode).ToList();
                    foreach (var station in stations)
                    {
                        if (station.Client.ConnectionId == client_result.ConnectionId)
                        {
                            station.Client = null;
                            station.IsUsed = false;
                            BroadcastStation(station);
                        }
                    }
                }
                _clients.Remove(client_result);
                BroadcastStationsInLocationCode(client_result.LocationCode);
            }


            SetStations();
            SetClients();


            //else
            //{
            //    for (int i = 0; i < _stationGroups.Count; i++)
            //    {
            //        if( _stationGroups.ElementAt(i).Client != null)
            //        {
            //            if( _stationGroups.ElementAt(i).Client.ConnectionId == Context.ConnectionId)
            //            {
            //                _stationGroups.ElementAt(i).IsUsed = false;
            //                _stationGroups.ElementAt(i).Client = null;

            //                BroadcastStation(_stationGroups.ElementAt(i));
            //            }
            //        }
            //    }
            //}
            //SetStations();






            return base.OnDisconnectedAsync(exception);
        }


        public void OnCloseApp()
        {
            BroadcastErrorToClients("SORRY CLOSED");
        }

        
        /// <summary>
        /// REGISTER STATION IN THE MEMORY
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void CreateStationConnection(string key, string name, string lcode)
        {
            if ( _stationGroups.Count > 0)
            {
                var qstation = _stationGroups.Where(g => g.Key == key).SingleOrDefault();
                if(qstation != null)
                {
                    _stationGroups.Remove(qstation);
                }
            }

            var station = new StationGroup
            {
                IsUsed = false,
                Key = key,
                Name = name,
                LocationCode = lcode,
                StationConnectionId = Context.ConnectionId,
                Client = null
            };
            _stationGroups.Add(station);
            BroadcastStation(station);
            BroadcastStationsInLocationCode(lcode);
            SetStations();
        }

        /// <summary>
        /// CREATE FLOAT STATE DEVICE
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="lcode"></param>
        public void StateFloatDevice(string macAddress, string lcode)
        {
            if( _clients.Count > 0)
            {
                var clients = _clients.SingleOrDefault(g => g.Mac == macAddress);
                if( clients != null)
                {
                    _clients.Remove(clients);
                }
            }
            var client = new Client()
            {
                StatusStation = false,
                ConnectionId = Context.ConnectionId,
                Mac = macAddress,
                LocationCode = lcode
            };
            _clients.Add(client);
            BroadcastClient(client);
            SetClients();
        }

        /// <summary>
        ///  JOIN CONNECTION ON BOTH DEVICE AND STATION
        /// </summary>
        /// <param name="key"> STATION KEY </param>
        /// <param name="lcode"> LOCATION CODE </param>
        /// <param name="deviceMacAddress"> DEVICE CONNECTION ID </param>
        /// <returns></returns>
        public void JoinStationConnection(string key, string lcode, string connectionId)
        {
            if( _stationGroups.Count > 0)
            {
                BroadcastErrorToClients("STATION VALIDATE KEY: " + key + ", CODE : " + lcode + ", ID: " + connectionId);
                var station = _stationGroups.SingleOrDefault(g => g.Key == key && g.LocationCode == lcode);
                if (station != null)
                {
                    BroadcastErrorToClients("STATION FOUND");
                    if (!station.IsUsed)
                    {
                        BroadcastErrorToClients("STATION IS USED");
                        var client = _clients.SingleOrDefault(g => g.ConnectionId == connectionId);
                        if( client != null)
                        {
                            BroadcastErrorToClients("STATION CLIENT");

                            client.StatusStation = true;
                            station.IsUsed = true;
                            station.Client = client;
                            BroadcastStationsInLocationCode(lcode);
                            BroadcastStation(station);
                            BroadcastClient(client);
                            SetStations();
                            SetClients();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  DISCONNECT CONNECTION ON BOTH DEVICE AND STATION
        /// </summary>
        /// <param name="key">STATION KEY</param>
        /// <param name="lcode">LOCATION CODE</param>
        /// <param name="connectionId">DEVICE CONNECTION ID</param>
        public void DisconnetStationConnection(string key, string lcode, string connectionId)
        {
            if( _stationGroups.Count > 0)
            {
                var station = _stationGroups.SingleOrDefault(g => g.Key == key && g.LocationCode == lcode);
                if( station != null)
                {
                    var client = _clients.SingleOrDefault(g => g.ConnectionId == connectionId);
                    if( client != null)
                    {
                        client.StatusStation = false;
                        station.IsUsed = false;
                        station.Client = null;
                        BroadcastStationsInLocationCode(lcode);
                        BroadcastStation(station);
                        SetStations();
                        SetClients();
                    }
                }
            }
        }

        private void SetStations()
        {
            _clientModel.SetAps(_stationGroups);
        }

        private void SetClients()
        {
            _clientModel.SetClients(_clients);
        }


        private void BroadcastStationsInLocationCode(string lcode)
        {

            var clients = _clients.Where(g => g.LocationCode == lcode).ToList();
            if (clients.Count > 0)
            {
                var stations = _stationGroups.Where(g => g.LocationCode == lcode && g.IsUsed == false).ToList(); 
                foreach (var client in clients)
                {
                    //if (!client.StatusStation)
                        Clients.Client(client.ConnectionId).SendAsync("LS_STATION", stations);
                }
            }
        }

        private void BroadcastClient(Client client)
        {
            var stations = _stationGroups.Where(g => g.LocationCode == client.LocationCode && g.IsUsed == false).ToList();
            var connectionData = new ConnectionData
            {
                Client = client,
                StationGroups = stations
            };
            Clients.Client(client.ConnectionId).SendAsync("C_STAT", connectionData);
        }

        private void BroadcastStation(StationGroup station)
        {
            Clients.Client(station.StationConnectionId).SendAsync("STAT", station);
        }

        private void BroadcastErrorMessageByConnectionId(string ConnectionId, string ErrorMessage)
        {
            Clients.Client(ConnectionId).SendAsync("ERROR_STAT", ErrorMessage);
        }

        private void BroadcastErrorToClients(string ErrorMessage)
        {
            foreach(var client in _clients)
            {
                Clients.Client(client.ConnectionId).SendAsync("ERROR_TEST", ErrorMessage);
            }
        }
        
    }
}
