using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using KoenZomers.UniFi.Api;
using KoenZomers.UniFi.Api.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WifiCentenelApiPortal.Auth;
using WifiCentenelApiPortal.Context;
using WifiCentenelApiPortal.Helper;
using WifiCentenelApiPortal.Interfaces;
using WifiCentenelApiPortal.Models;

namespace WifiCentenelApiPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IConfiguration _configuration;
        private readonly IJwtFactory _jwtFactory;
        private readonly IClientDevice _clientDevice;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager, 
            IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions,
            IClientDevice clientDevice, IMapper mapper, ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _clientDevice = clientDevice;
            _jwtOptions = jwtOptions.Value;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DeviceRegister deviceRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var model = new RegistrationDetails
            {
                Email = "mobiledevice@centenel.com",
                UserName = deviceRegister.Mac.Replace(":",""),
                FirstName = "MobileDeviceAccont",
                LastName = "MobileDeviceAccount",
                Password = deviceRegister.Mac,
                UserType = UserType.DEVICE,
                Created = DateTime.Now
            };

            try
            {

                ResponseEnvelope<Clients> client;
                using (var UnifiApi = new Api(new Uri(_configuration.GetSection("ControllerSettings")["Url"])))
                {
                    UnifiApi.DisableSslValidation();
                    await UnifiApi.Authenticate(_configuration.GetSection("ControllerSettings")["Id"], _configuration.GetSection("ControllerSettings")["Password"]);


                    try
                    {
                        client = await UnifiApi.GetClientInfo(deviceRegister.Mac);
                        await UnifiApi.Logout();
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpWebResponse = (HttpWebResponse)response;
                            return new BadRequestObjectResult(new UnifiRequest { StatusCode = Convert.ToInt32(httpWebResponse.StatusCode).ToString(), StatusDescription = "NoActiveClient" });
                            //return new BadRequestObjectResult(e);
                        }

                    }

                }
                var userIdentity = _mapper.Map<ApplicationUser>(model);
                var result = await _userManager.CreateAsync(userIdentity, model.Password);
                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
                await _clientDevice.RegisterDevice(userIdentity, deviceRegister.Mac, client.data[0].Hostname);
                return new OkResult();

            }
            catch(Exception e)
            {
                return new BadRequestObjectResult(e);
            }
           
        }

        [HttpPost("checkcrendential")]
        public async Task<IActionResult> CheckCredential([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (var UnifiApi = new Api(new Uri(_configuration.GetSection("ControllerSettings")["Url"])))
                {
                    UnifiApi.DisableSslValidation();
                    await UnifiApi.Authenticate(_configuration.GetSection("ControllerSettings")["Id"], _configuration.GetSection("ControllerSettings")["Password"]);
                    try
                    {
                        await UnifiApi.GetActiveClient(credentials.device);

                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpWebResponse = (HttpWebResponse)response;
                            return new BadRequestObjectResult(new UnifiRequest { StatusCode = Convert.ToInt32(httpWebResponse.StatusCode).ToString(), StatusDescription = "NoActiveClient" });
                        }
                    }
                    if (!await _clientDevice.IsDeviceExist(credentials.device))
                    {
                        return new BadRequestObjectResult(new UnifiRequest { StatusCode = "400", StatusDescription = "IsNewUser" });
                    }
                    await UnifiApi.Logout();
                    return new OkResult();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse)response;
                    return new OkObjectResult(new UnifiRequest { StatusCode = Convert.ToInt32(httpWebResponse.StatusCode).ToString(), StatusDescription = "Internel Server Error" });
                }
            }
        }
        [HttpGet("admintest")]
        public async Task<IActionResult> Test()
        {
            return new OkResult();
        }
        [HttpPost("adminlogin")]
        public async Task<IActionResult> LoginAdmin([FromBody] AdminCredentials adminCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var identity = await GetClaimsIdentity(adminCredentials.Username, adminCredentials.Password);

            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, adminCredentials.Username, _jwtOptions, new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented });

            return new OkObjectResult(jwt);
        }
        [HttpPost("stationlogin")]
        public async Task<IActionResult> LoginStation([FromBody] AdminCredentials adminCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var identity = await GetClaimsIdentity(adminCredentials.Username, adminCredentials.Password);

            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, adminCredentials.Username, _jwtOptions, new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented });

            return new OkObjectResult(jwt);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthCredentials authCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var macusername = await _clientDevice.GetUserByMacAddress(authCredentials.MacAddress);
            if( macusername == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "NewDevice", ModelState));
            }
            var identity = await GetClaimsIdentity(macusername.UserName, authCredentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "InvalidAuth", ModelState));
            }
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, macusername.UserName, _jwtOptions, new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

    }
}