using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using KoenZomers.UniFi.Api;
using KoenZomers.UniFi.Api.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WifiCentenelApiPortal.Helper;
using WifiCentenelApiPortal.Interfaces;
using WifiCentenelApiPortal.Models;

namespace WifiCentenelApiPortal.Controllers
{
    [Authorize("ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ClaimsPrincipal _caller;
        private readonly IClientDevice _clientDevice;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpContextAccessor httpContextAccessor, IClientDevice clientDevice, IConfiguration configuration)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _clientDevice = clientDevice;
            _configuration = configuration;
        }

        [HttpPost("homeinfo")]
        public async Task<IActionResult> HomeInfo([FromBody] Credentials credentials)
        {
            try
            {
                var userId = _caller.Claims.SingleOrDefault(c => c.Type == "id");
                ResponseEnvelope<Clients> device;
                //return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "NoActiveDevice" });
                //return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "NoActiveClient" });
                using (var UnifiApi = new Api(new Uri(_configuration.GetSection("ControllerSettings")["Url"])))
                {
                    UnifiApi.DisableSslValidation();
                    await UnifiApi.Authenticate(_configuration.GetSection("ControllerSettings")["Id"], _configuration.GetSection("ControllerSettings")["Password"]);
                    try
                    {
                        device = await UnifiApi.GetActiveClient(credentials.device);

                    }

                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpWebResponse = (HttpWebResponse)response;
                            return new BadRequestObjectResult(new UnifiRequest { StatusCode = Convert.ToInt32(httpWebResponse.StatusCode).ToString(), StatusDescription = "NoActiveClient" });
                        }

                    }
                }

                //var clientInfo = await _clientDevice.GetAccountById(userId.Value);
                //if (clientInfo != null)
                //{

                var internetAuth = await _clientDevice.GetInternetAuthorizationByDeviceMacAddress(credentials.device);
                double timeSpan = 0;
                string expiredTrans = "Expired";
                if ( internetAuth != null)
                {
                    timeSpan = Generate.GetTotalMinutes(DateTime.Now, internetAuth.EndDateAuthorized).TotalSeconds;
                }
                if (timeSpan > 0)
                {
                    expiredTrans = Constants.Strings.DateFormat.DateWithTime(internetAuth.EndDateAuthorized);
                }

                var AP_INFO = await _clientDevice.GetApInfoByMacAddress(device.data[0].AccessPointMacAddress);
                var STATUS = device.data[0].IsAuthorized ?? false;

                var HomeInfo = new HomeInfo
                {
                    AccessPoint = AP_INFO.Name,
                    AccessPointMac = AP_INFO.MacAddress,
                    Expire = expiredTrans,
                    AvailableMinutes = timeSpan,
                    IsAuthorized = STATUS,
                    Location = AP_INFO.LocationCode,
                    Signal = STATUS ? "Authorized" : "Not Authorized"
                };

                return new OkObjectResult(HomeInfo);
                //}
                //else
                //{
                //    return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
                //}
                
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult(e);
                //return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "ServerError" });
            }
        }

        [HttpGet("info")]
        public async Task<IActionResult> Info()
        {
            try
            {
                var userId = _caller.Claims.SingleOrDefault(c => c.Type == "id");
                var roles = _caller.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role);
                var userInfo = await _clientDevice.GetUserRoleByUserId(userId.Value, roles.Value);
                return new OkObjectResult(userInfo);
            }
            catch
            {
                return new BadRequestObjectResult(new UnifiRequest {  StatusCode = "400", StatusDescription = "ServerError" });
            }
        }
    }
}
