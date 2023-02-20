using AutoMapper;
using KoenZomers.UniFi.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Context;
using WifiCentenelApiPortal.Helper;
using WifiCentenelApiPortal.Interfaces;
using WifiCentenelApiPortal.Models;
using static WifiCentenelApiPortal.Hubs.ClientHubs;

namespace WifiCentenelApiPortal.Repo
{
    public class ClientDevice : IClientDevice
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ClientDevice(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
        }
        //public async Task<bool> IsDeviceExist(string MacAddress)
        //{
        //    return await _applicationDbContext.Devices.Where(d => d.MacAddress == MacAddress).SingleOrDefaultAsync() != null;
        //}
        //public async Task<Device> FindDeviceByMac(string MacAddress)
        //{
        //    return await _applicationDbContext.Devices.Where(d => d.MacAddress == MacAddress).SingleOrDefaultAsync();
        //}
        //public async Task<Device> FindDeviceById(string Id)
        //{
        //    return await _applicationDbContext.Devices.Where(d => d.UserRef == Id).SingleOrDefaultAsync();
        //}

        //public async Task<ApplicationUser> FindUserByMac(string MacAddress)
        //{
        //    return await (from d in _applicationDbContext.Devices
        //                  join a in _applicationDbContext.Accounts
        //                    on d.UserRef equals a.IdentityId
        //                  join u in _applicationDbContext.Users
        //                    on a.IdentityId equals u.Id
        //                  where d.MacAddress == MacAddress
        //                  select u).FirstOrDefaultAsync();
        //}
        //public async Task<Account> FindClientAccountById(string Id)
        //{
        //    return await (from ca in _applicationDbContext.Accounts
        //                  where ca.IdentityId == Id
        //                  select ca).SingleOrDefaultAsync();
        //}
        //public async Task<Account> FindClientAccountByMac(string Mac)
        //{
        //    return await (from d in _applicationDbContext.Devices
        //                  join cl in _applicationDbContext.Accounts
        //                      on d.UserRef equals cl.IdentityId
        //                  where d.MacAddress == Mac
        //                  select cl).SingleOrDefaultAsync();
        //}
        //public async Task<ICollection< Price>> GetAllPrices()
        //{
        //    return await _applicationDbContext.Prices.ToListAsync();
        //}
        //public async Task<Price> FindPriceCode(string Code)
        //{
        //    return await _applicationDbContext.Prices.Where(pr => pr.Code == Code).SingleOrDefaultAsync();
        //}
        //public async Task<Ap> FindApByMac(string mac)
        //{
        //    return await _applicationDbContext.Aps.Where(ap => ap.MacAddress == mac).SingleOrDefaultAsync();
        //}

        //public async Task AddTransaction(Price price, string ApMac, string UserId)
        //{
        //    DateTime currentTime = DateTime.Now;
        //    DateTime expiredTime = currentTime.AddMinutes(price.Minutes);
        //    var ap = await FindApByMac(ApMac);
        //    var client = await FindClientAccountById(UserId);
        //    var trans = new Transaction
        //    {
        //        ApRef = ap.Id,
        //        DateLoaded = currentTime,
        //        StartedTime = currentTime,
        //        ExpiredTime = expiredTime,
        //        PriceRef = price.Id
        //    };
        //    await _applicationDbContext.Transactions.AddAsync(trans);
        //    client.Balance -= price.Amount;
        //    client.TransactionRefId = trans.Id;
        //    var transLogs = new TransactionLog
        //    {
        //        TransactionRef = trans.Id,
        //        Identity = client.IdentityId
        //    };
        //    await _applicationDbContext.TransactionLogs.AddAsync(transLogs);
        //    await _applicationDbContext.SaveChangesAsync();
           
        //}

        //public async Task<Transaction> GetTransactionAsync(long? Id)
        //{
        //    return await (from ta in _applicationDbContext.Transactions
        //                  join pr in _applicationDbContext.Prices
        //                    on ta.PriceRef equals pr.Id
        //                  join ap in _applicationDbContext.Aps
        //                    on ta.ApRef equals ap.Id
        //                  where ta.Id == Id
        //                  select new Transaction
        //                  {
        //                      ApRef = ta.ApRef,
        //                      Ap = ap,
        //                      DateLoaded = ta.DateLoaded,
        //                      StartedTime = ta.StartedTime,
        //                      ExpiredTime = ta.ExpiredTime,
        //                      Price = pr,
        //                      PriceRef = ta.PriceRef
        //                  }).SingleOrDefaultAsync();
        //}

        //public async Task<ICollection<HistoryLog>> GetTransactionLogsAsync(string UserId)
        //{
        //    return await (from tl in _applicationDbContext.TransactionLogs
        //                  join t in _applicationDbContext.Transactions
        //                   on tl.TransactionRef equals t.Id
        //                  join pr in _applicationDbContext.Prices
        //                    on t.PriceRef equals pr.Id
        //                  where tl.Identity == UserId
        //                  select new HistoryLog
        //                  {
        //                      DateLoaded = t.DateLoaded,
        //                      Amount = pr.Amount,
        //                      PriceCode = pr.Code,
        //                      StartedTime = t.StartedTime,
        //                      ExpiredTime = t.ExpiredTime
        //                  }).OrderByDescending(h => h.DateLoaded).Take(8).ToListAsync();
        //}

        //public async Task UpdateCoinByMac(string Mac, decimal Coin)
        //{

        //    var client = await FindClientAccountByMac(Mac);
        //    if( client != null)
        //    {
        //        client.Balance += Coin;
        //        await _applicationDbContext.SaveChangesAsync();
        //    }

        //}




        //VERSION 2.0 ONLINE MODULE

        //public async Task UpdateApAuthorizedDate(DateTime Start,  string MacId, double Minutes)
        //{
        //    var client = await FindClientAccountByMac(MacId);
        //    DateTime EndApAuth;
        //    double currentMinutes = Generate.GetTotalMinutes(Start, client.DateApEndAuthorized).TotalMinutes;
        //    if( currentMinutes > 0 )
        //    {
        //        EndApAuth = client.DateApEndAuthorized.AddMinutes(Minutes);
        //    }
        //    else
        //    {
        //        EndApAuth = Start.AddMinutes(Minutes);
        //    }
        //    client.DateApStartAuthorized = Start;
        //    client.DateApEndAuthorized = EndApAuth;
        //    await _applicationDbContext.SaveChangesAsync();
        //}

        //public async Task InsertCoinLogs(long Receiver, long Sender, decimal Amount)
        //{
        //    var coin = new CoinLog
        //    {
        //        DeviceRef = Receiver,
        //        ApRef = Sender,
        //        Date = DateTime.Now,
        //        Amount = Amount
        //    };

        //    await _applicationDbContext.Coins.AddAsync(coin);
        //    await _applicationDbContext.SaveChangesAsync();
        //}

        //public async Task<UserInfo> GetUserInfoByUserId(string Id, string Role)
        //{

        //    return await (from c in _applicationDbContext.ClientAccounts
        //                    join u in _applicationDbContext.Users
        //                        on c.IdentityId equals u.Id    
        //                    where c.IdentityId == Id
        //                    select new UserInfo
        //                    {
        //                        Username =  u.UserName,
        //                        Balance = c.Balance,
        //                        Role = Role
        //                    }).SingleOrDefaultAsync();
        //}

        //public async Task RegisterAp(ApRegistrationDetails apRegistrationDetails)
        //{

            


        //}

        //public Task AddTransaction(Price price, string ApMac, string UserId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Price> FindPriceCode(string Code)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ICollection<HistoryLog>> GetTransactionLogsAsync(string UserId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateCoinByMac(string Mac, decimal Coin)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateApAuthorizedDate(DateTime Start, string MacId, double Minutes)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task InsertCoinLogs(long Receiver, long Sender, decimal Amount)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<UserInfo> GetUserInfoByUserId(string Id, string Role)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ApplicationUser> FindUserByMac(string MacAddress)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Device> FindDeviceById(string Id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Account> FindClientAccountByMac(string Mac)
        //{
        //    throw new NotImplementedException();
        //}

        //VERSION 2.1 MODULE TRANSACTION
        /// <summary>
        /// 
        /// Retrieve Price Benefit of the Station
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CoinCode"></param>
        /// <returns> Return Price Of the Station Selected By Coin Code</returns>
        public async Task<Price> GetStationBenefitByStation(string Id, string CoinCode)
        {
            return await (from st in _applicationDbContext.Stations
                          join pr in _applicationDbContext.Prices
                            on st.Id equals pr.StationRefId
                          join ci in _applicationDbContext.CoinIdentities
                            on pr.CoinIdentityRefId equals ci.Id
                          where st.IdentityId == Id && ci.AmountCode == CoinCode
                          select pr).SingleOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// Add New Coin Log
        /// 
        /// </summary>
        /// <param name="SenderDeviceMacAddress"></param>
        /// <param name="CoinCode"></param>
        public async Task UpdateCoinLog(string SenderDeviceMacAddress, string CoinCode)
        {
            var coinInfo = await GetCoinIdentityByCode(CoinCode);
            var deviceInfo = await GetDeviceByMacAddress(SenderDeviceMacAddress);
            var coinLog = new CoinLog
            {
                Device = deviceInfo,
                CoinIdentity = coinInfo,
                InsertedDate = DateTime.Now
            };
            await _applicationDbContext.CoinLogs.AddAsync(coinLog);
            await _applicationDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Add/Update Device Internet Expiration
        /// </summary>
        /// <param name="SenderDeviceMacAddress"></param>
        /// <param name="Price"></param>
        public async Task UpdateInternetAuthorizationLog(string SenderDeviceMacAddress, Price Price)
        {
            var deviceInternetAuth = await GetInternetAuthorizationByDeviceMacAddress(SenderDeviceMacAddress);
            if( deviceInternetAuth == null)
            {

                var internetAuthInfo = new InternetAuthorization
                {
                    Device = await GetDeviceByMacAddress(SenderDeviceMacAddress),
                    EndDateAuthorized = DateTime.Now.AddMinutes(Price.TotalMinutes)
                };
                await _applicationDbContext.InternetAuthorizations.AddAsync(internetAuthInfo);
                await _applicationDbContext.SaveChangesAsync();
            }
            else
            {
                deviceInternetAuth.EndDateAuthorized = deviceInternetAuth.EndDateAuthorized.AddMinutes(Price.TotalMinutes);
                await _applicationDbContext.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Register Mobile Device Account
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="DeviceMacAddress"></param>
        /// <param name="DeviceHostname"></param>
        public async Task RegisterDevice(ApplicationUser UserIdentity, string DeviceMacAddress, string DeviceHostname)
        {
            DateTime DefaultTime = DateTime.Now;
            await _userManager.AddToRoleAsync(UserIdentity, "Member");
            //await _applicationDbContext.Accounts.AddAsync(new Account { IdentityId = UserIdentity.Id });
            await _applicationDbContext.Devices.AddAsync(new Device { IdentityId = UserIdentity.Id, MacAddress = DeviceMacAddress, Hostname = DeviceHostname, CreatedDate = DefaultTime });
            await _applicationDbContext.LegalTerms.AddAsync(new LegalTerms { AcceptTerms = true, TermsLink = _configuration.GetSection("ControllerSettings")["LegalTermsLink"], Version = _configuration.GetSection("ControllerSettings")["LegalTermsVersion"], AcceptDate = DateTime.Now, IdentityId = UserIdentity.Id });
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task RegisterAp(string MacAddress, string Name, long LocationId )
        {
            var ap = new Ap()
            {
                MacAddress = MacAddress,
                Name = Name,
                LocationApRefId = LocationId,
                CreatedDate = DateTime.Now
            };

            await _applicationDbContext.Aps.AddAsync(ap);
            await _applicationDbContext.SaveChangesAsync();

        }

        //public async Task RegisterStation(ApplicationUser UserIdentity, )

        //VERSION 2.1 MODULE 
        public async Task<Device> GetDeviceByMacAddress(string MacAddress)
        {
            return await _applicationDbContext.Devices.SingleOrDefaultAsync(c => c.MacAddress == MacAddress);
        }
        public async Task<Device> GetDeviceByIdentity(string IdentityId)
        {
            return await _applicationDbContext.Devices.SingleOrDefaultAsync(c => c.IdentityId == IdentityId);
        }

        public async Task<Ap> GetApByMacAddress(string MacAddress)
        {
            return await _applicationDbContext.Aps.SingleOrDefaultAsync(c => c.MacAddress == MacAddress);
        }

        public async Task<ApInfo> GetApInfoByMacAddress(string MacAddress)
        {
            return await (from ap in _applicationDbContext.Aps
                          join la in _applicationDbContext.LocationAps
                            on ap.LocationApRefId equals la.Id
                          join l in _applicationDbContext.Locations
                            on la.LocationRefId equals l.Id
                          where ap.MacAddress == MacAddress
                          select new ApInfo {
                              MacAddress = ap.MacAddress,
                              Name = ap.Name,
                              LocationCode = l.Code,
                              LocationAddress = l.Address
                          }).SingleOrDefaultAsync();
        }

        public async Task<StationInfo> GetStationInfoByIdentityId(string IdentityId)
        {
            return await (from st in _applicationDbContext.Stations
                          join ls in _applicationDbContext.LocationStations
                            on st.LocationStationRefId equals ls.Id
                          join l in _applicationDbContext.Locations
                            on ls.LocationRefId equals l.Id
                          where st.IdentityId == IdentityId
                          select new StationInfo {
                              Name = st.Name,
                              LocationCode = l.Code
                          }).SingleOrDefaultAsync();
        }


        public async Task<CoinIdentity> GetCoinIdentityByCode(string CoinCode)
        {
            return await _applicationDbContext.CoinIdentities.SingleOrDefaultAsync(c => c.AmountCode == CoinCode);
        }
        public async Task<InternetAuthorization> GetInternetAuthorizationByIdentity(string IdentityId)
        {
            return await (from dv in _applicationDbContext.Devices
                          join nt in _applicationDbContext.InternetAuthorizations
                            on dv.Id equals nt.DeviceRef
                          where dv.IdentityId == IdentityId
                          select nt).SingleOrDefaultAsync();
        }
        public async Task<InternetAuthorization> GetInternetAuthorizationByDeviceMacAddress(string MacAddress)
        {
            return await (from dv in _applicationDbContext.Devices
                          join nt in _applicationDbContext.InternetAuthorizations
                            on dv.Id equals nt.DeviceRef
                          where dv.MacAddress == MacAddress
                          select nt).SingleOrDefaultAsync();
        }
        public async Task<bool> IsDeviceExist(string MacAddress)
        {
            return await _applicationDbContext.Devices.SingleOrDefaultAsync(d => d.MacAddress == MacAddress) != null;
        }
        public async Task<ApplicationUser> GetUserByMacAddress(string MacAddress)
        {
            return await (from d in _applicationDbContext.Devices
                          join u in _applicationDbContext.Users
                            on d.IdentityId equals u.Id
                          where d.MacAddress == MacAddress
                          select u).FirstOrDefaultAsync();
        }
        public async Task<UserInfo> GetUserRoleByUserId(string Id, string Role)
        {

            return await (from c in _applicationDbContext.Accounts
                          join u in _applicationDbContext.Users
                              on c.IdentityId equals u.Id
                          where c.IdentityId == Id
                          select new UserInfo
                          {
                              Username = u.UserName,
                              Role = Role
                          }).SingleOrDefaultAsync();
        }
        public async Task<Account> GetAccountById(string Id)
        {
            return await _applicationDbContext.Accounts.SingleOrDefaultAsync(a => a.IdentityId == Id);
        }
    }
}
