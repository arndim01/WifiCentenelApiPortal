using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KoenZomers.UniFi.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using WifiCentenelApiPortal.Context;
using WifiCentenelApiPortal.Helper;
using WifiCentenelApiPortal.Hubs;
using WifiCentenelApiPortal.Interfaces;
using WifiCentenelApiPortal.Models;

namespace WifiCentenelApiPortal.Controllers
{
    [Authorize("ApiUser")]
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClientModel _clientModel;
        private readonly IClientDevice _clientDevice;
        private readonly IHubContext<ClientHubs> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ClientController(IConfiguration configuration, IClientModel clientModel, 
            IHubContext<ClientHubs> hubContext, IClientDevice clientDevice, IMapper mapper,
            UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _clientModel = clientModel;
            _clientDevice = clientDevice;
            _hubContext = hubContext;
            _configuration = configuration;
            _mapper = mapper;
        }
        [HttpPost("sreg")]
        public async Task<IActionResult> ApStationRegistration([FromBody] ApRegistrationDetails  apRegistrationDetails)
        {

            //return new BadRequestObjectResult(apRegistrationDetails);
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(apRegistrationDetails);
            }

            try
            {
                //var ap = await _clientDevice.FindApByMac(apRegistrationDetails.Mac);
                //if( ap == null)
                //{
                    //var model = new RegistrationDetails
                    //{
                    //    Email = "example@centenel.com",
                    //    UserName = apRegistrationDetails.Mac.Replace(":", ""),
                    //    FirstName = "Test_First_Name",
                    //    LastName = "Test_Last_Name",
                    //    Password = _configuration.GetSection("ControllerSettings")["ApPassword"],
                    //    UserType = UserType.STATION,
                    //    Created = DateTime.Now
                    //};

                    //var userIdentity = _mapper.Map<ApplicationUser>(model);
                    //var result = await _userManager.CreateAsync(userIdentity, model.Password);
                    //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
                    //await _userManager.AddToRoleAsync(userIdentity, "Member");
                    //var Ap = new Ap
                    //{
                    //    MacAddress = apRegistrationDetails.Mac,
                    //    SSID = _configuration.GetSection("ControllerSettings")["SSID"],
                    //    Name = apRegistrationDetails.Name,
                    //    Address = apRegistrationDetails.Address
                    //};

                    //await _applicationDbContext.Aps.AddAsync(Ap);
                    //await _applicationDbContext.SaveChangesAsync();

                    return new OkObjectResult(new UnifiRequest { StatusCode = "200", StatusDescription = "DevOk" });

                    

                //}
                //else
                //{
                //    return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "DeviceExist" });
                //}
            }
            catch
            {
                return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
            }
        }
        [HttpGet("devices")]
        public async Task<IActionResult> GetDevices()
        {
            try
            {
                using (var UnifiApi = new Api(new Uri(_configuration.GetSection("ControllerSettings")["Url"])))
                {
                    UnifiApi.DisableSslValidation();
                    await UnifiApi.Authenticate(_configuration.GetSection("ControllerSettings")["Id"], _configuration.GetSection("ControllerSettings")["Password"]);

                    try
                    {
                        var devices = await UnifiApi.GetDevices();
                        await UnifiApi.Logout();
                        // SET DEVICE REGISTRATION
                        //for(int i =0; i < devices.Count; i++)
                        //{
                        //    var ap = await _clientDevice.FindApByMac(devices[i].MacAddress);
                        //    if( ap != null)
                        //    {
                        //        devices[i].Name = ap.Name;
                        //        devices[i].Registered = true;
                        //    }
                        //}

                        return new OkObjectResult(devices);
                    }
                    catch
                    {
                        return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
                    }
                }

            }
            catch(Exception e)
            {
                //return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
                return new BadRequestObjectResult(e);
            }
        }
        [HttpGet("clients")]
        public async Task<IActionResult> GetActiveClients()
        {
            try
            {
                using (var UnifiApi = new Api(new Uri(_configuration.GetSection("ControllerSettings")["Url"])))
                {
                    UnifiApi.DisableSslValidation();
                    await UnifiApi.Authenticate(_configuration.GetSection("ControllerSettings")["Id"], _configuration.GetSection("ControllerSettings")["Password"]);

                    try
                    {
                        var clients = await UnifiApi.GetActiveClients();
                        await UnifiApi.Logout();
                        return new OkObjectResult(clients);
                    }
                    catch
                    {
                        return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
                    }
                }
            }
            catch
            {
                return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
            }
        }
        [HttpGet("acstations")]
        public IActionResult ConnectedClients()
        {
            try
            {
                if (_clientModel.GetAps() == null)
                {
                    return new OkObjectResult(new object[] { });
                }
                else
                {

                    return new OkObjectResult(_clientModel.GetAps());
                }
            }
            catch
            {
                return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
            }
        }
        
        [HttpGet("clientsync/{client}/{ap}")]
        public IActionResult ClientSyncStatus(string client, string ap)
        {
            if(String.IsNullOrWhiteSpace(client) || String.IsNullOrWhiteSpace(ap))
            {
                return new BadRequestResult();
            }
            try
            {
                var groups = _clientModel.GetAps();
                if( groups != null && groups.Count > 0)
                {
                    var group = groups.Where(g => g.Key == ap).SingleOrDefault();
                    if( group != null)
                    {
                        if( group.Client != null)
                        {
                            var clientStatus = groups.Where(g => g.Key == ap && g.Client.Mac == client).SingleOrDefault();
                            if (clientStatus != null)
                            {
                                return new OkObjectResult(new UnifiRequest { StatusCode = "200", StatusDescription = "ApSync" });
                            }
                            else
                            {
                                return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "NoSyncAp" });
                            }
                        }
                        else
                        {
                            return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "NoSyncAp" });
                        }
                        
                    }
                    else
                    {
                        return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "NoSyncAp" });
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "NoSyncAp" });
                }

            }
            catch
            {
                return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
            }
        }

    }
}