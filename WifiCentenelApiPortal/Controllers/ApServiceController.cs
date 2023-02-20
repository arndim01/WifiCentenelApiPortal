using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using KoenZomers.UniFi.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using WifiCentenelApiPortal.Helper;
using WifiCentenelApiPortal.Hubs;
using WifiCentenelApiPortal.Interfaces;
using WifiCentenelApiPortal.Models;

namespace WifiCentenelApiPortal.Controllers
{
    [Authorize("ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApServiceController : ControllerBase
    {
        private readonly ClaimsPrincipal _caller;
        private readonly IClientModel _clientModel;
        private readonly IClientDevice _clientDevice;
        private readonly IHubContext<ClientHubs> _hubContext;
        private readonly IConfiguration _configuration;
        public ApServiceController(IHttpContextAccessor httpContextAccessor, 
            IClientModel clientModel, 
            IHubContext<ClientHubs> hubContext, 
            IClientDevice clientDevice,
            IConfiguration configuration)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _clientModel = clientModel;
            _hubContext = hubContext;
            _clientDevice = clientDevice;
            _configuration = configuration;
        }

        

        [HttpGet("available/{id}")]
        public IActionResult CheckApConnection(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return new BadRequestResult();
            try
            {
                var groups = _clientModel.GetAps();
                if (groups != null && groups.Count > 0)
                {
                    var group = groups.Where(g => g.Key == id).SingleOrDefault();
                    if (group != null && !group.IsUsed)
                    {
                        return new OkObjectResult(new UnifiRequest { StatusCode = "200", StatusDescription = "ActiveAp" });
                    }
                    else
                    {
                        return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "NoActiveAp" });
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "NoActiveAp" });
                }
            }
            catch
            {
                return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
            }
        }
        [HttpGet("stationinfo")]
        public async Task<IActionResult> GetStatioInfo()
        {
            try
            {
                var userId = _caller.Claims.SingleOrDefault(c => c.Type == "id");
                var stationInfo = await _clientDevice.GetStationInfoByIdentityId(userId.Value);
                if( stationInfo != null)
                {
                    return new OkObjectResult(stationInfo);
                }
                else
                {
                    return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
                }
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult(e);
            }
        }

        [HttpPost("stationreg")]
        public async Task<IActionResult> StationRegistration()
        {




            return new OkResult();
        }
        // Reconnect
        [HttpGet("recon")]
        public async Task<IActionResult> Reconnect()
        {
            try
            {
                var userId = _caller.Claims.SingleOrDefault(c => c.Type == "id");
                var internetAuth = await _clientDevice.GetInternetAuthorizationByIdentity(userId.Value);
                double totalMinutes = Generate.GetTotalMinutes(DateTime.Now, internetAuth.EndDateAuthorized).TotalMinutes;
                using (var UnifiApi = new Api(new Uri(_configuration.GetSection("ControllerSettings")["Url"])))
                {
                    UnifiApi.DisableSslValidation();
                    await UnifiApi.Authenticate(_configuration.GetSection("ControllerSettings")["Id"], _configuration.GetSection("ControllerSettings")["Password"]);

                    try
                    {
                        var client = await UnifiApi.AuthorizeGuest(internetAuth.Device.MacAddress, Convert.ToInt32(totalMinutes));
                        await UnifiApi.Logout();

                        return new OkObjectResult(client);
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpWebResponse = (HttpWebResponse)response;
                            return new BadRequestObjectResult(new UnifiRequest { StatusCode = Convert.ToInt32(httpWebResponse.StatusCode).ToString(), StatusDescription = "FailedAuthorized" });
                        }

                    }


                }
                


            }
            catch
            {
                return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
            }
            


        }

        //[HttpGet("disconnect")]
        //public async Task<ActionResult> Disconnect()
        //{
        //    try
        //    {

        //        var userId = _caller.Claims.SingleOrDefault(c => c.Type == "id");
        //        var user = await _clientDevice.FindDeviceById(userId.Value);

        //        using (var UnifiApi = new Api(new Uri(_configuration.GetSection("ControllerSettings")["Url"])))
        //        {
        //            UnifiApi.DisableSslValidation();
        //            await UnifiApi.Authenticate(_configuration.GetSection("ControllerSettings")["Id"], _configuration.GetSection("ControllerSettings")["Password"]);
        //            try
        //            {

        //                var client = await UnifiApi.UnauthorizeGuest(user.MacAddress);
        //                await UnifiApi.Logout();

        //                return new OkObjectResult(client);
        //            }
        //            catch (WebException e)
        //            {
        //                using (WebResponse response = e.Response)
        //                {
        //                    HttpWebResponse httpWebResponse = (HttpWebResponse)response;
        //                    return new BadRequestObjectResult(new UnifiRequest { StatusCode = Convert.ToInt32(httpWebResponse.StatusCode).ToString(), StatusDescription = "FailedAuthorized" });
        //                }

        //            }
        //        }

        //    }
        //    catch
        //    {
        //        return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
        //    }
        //}

        // Auto Reload 
        [HttpPost("insertcoin")]
        public async Task<IActionResult> InsertCoin([FromBody] ReceiveCoin receiveCoin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = _caller.Claims.SingleOrDefault(c => c.Type == "id");
                var stationPrice = await _clientDevice.GetStationBenefitByStation(userId.Value, receiveCoin.Coin);
                await _clientDevice.UpdateCoinLog(receiveCoin.Device, receiveCoin.Coin);
                await _clientDevice.UpdateInternetAuthorizationLog(receiveCoin.Device, stationPrice);

                var internetAuth = await _clientDevice.GetInternetAuthorizationByDeviceMacAddress(receiveCoin.Device);


                double totalMinutes = Generate.GetTotalMinutes(DateTime.Now, internetAuth.EndDateAuthorized).TotalMinutes;
                double totalSeconds = Generate.GetTotalMinutes(DateTime.Now, internetAuth.EndDateAuthorized).TotalSeconds;


                using (var UnifiApi = new Api(new Uri(_configuration.GetSection("ControllerSettings")["Url"])))
                {
                    UnifiApi.DisableSslValidation();
                    await UnifiApi.Authenticate(_configuration.GetSection("ControllerSettings")["Id"], _configuration.GetSection("ControllerSettings")["Password"]);
                    try
                    {
                        var client = await UnifiApi.AuthorizeGuest(receiveCoin.Device,  Convert.ToInt32(totalMinutes), stationPrice.BandwidthUp, stationPrice.BandwidthDown );
                        await UnifiApi.Logout();
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpWebResponse = (HttpWebResponse)response;
                            return new BadRequestObjectResult(new UnifiRequest { StatusCode = Convert.ToInt32(httpWebResponse.StatusCode).ToString(), StatusDescription = "FailedAuthorized" });
                        }

                    }
                }

                //SEND MESSAGE TO SPECIFIC ONLINE CLIENTS FOR TIME EXTENDS
                //await _hubContext.Clients.Client(receiveCoin.ConnectionId).SendAsync("COIN_REC", new TransactionResult { IsAuthorized = true, TotalSecondsExpired = totalSeconds, DateExpired = Constants.Strings.DateFormat.DateWithTime(internetAuth.EndDateAuthorized)} );
                return new OkObjectResult(new UnifiRequest { StatusCode = "200", StatusDescription = "CoinAccepted", JsonResult = new TransactionResult { IsAuthorized = true, TotalSecondsExpired = totalSeconds, DateExpired = Constants.Strings.DateFormat.DateWithTime(internetAuth.EndDateAuthorized) } });
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError", JsonResult = e });
            }

        }
    }
}