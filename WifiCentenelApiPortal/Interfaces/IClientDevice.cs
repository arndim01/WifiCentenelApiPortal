using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Models;
using static WifiCentenelApiPortal.Hubs.ClientHubs;

namespace WifiCentenelApiPortal.Interfaces
{
    public interface IClientDevice
    {
        //Task<bool> IsDeviceExist(string MacAddress);
        //Task<Device> FindDeviceByMac(string MacAddress);
        //Task<ApplicationUser> FindUserByMac(string MacAddress);
        //Task<Account> FindClientAccountById(string Id);
        //Task<Device> FindDeviceById(string Id);
        //Task<Account> FindClientAccountByMac(string Mac);
        //Task<ICollection<Price>> GetAllPrices();
        //Task<Ap> FindApByMac(string mac);
        //Task AddTransaction(Price price, string ApMac, string UserId);
        //Task<Price> FindPriceCode(string Code);

        //Task<Transaction> GetTransactionAsync(long? transactionRefId);
        //Task<ICollection<HistoryLog>> GetTransactionLogsAsync(string UserId);
        //Task<DeviceInfo> GetDeviceInfoByMac(string Mac);
        //Task UpdateCoinByMac(string Mac, decimal Coin);
        //Task TransacLoad(string Code, string ApMac, string Mac);
        //Task InsertCoinLogs(long Receiver, long Sender, decimal Amount);
        //Task UpdateApAuthorizedDate(DateTime Start, string MacId, double Minutes);
        //Task<UserInfo> GetUserInfoByUserId(string Id, string Role);

        //VERSION 2.1 ONLINE MODULE VOID
        Task UpdateCoinLog(string SenderDeviceMacAddress, string CoinCode);
        Task UpdateInternetAuthorizationLog(string SenderDeviceMacAddress, Price Price);
        Task RegisterDevice(ApplicationUser UserIdentity, string DeviceMacAddress, string DeviceHostname);

        //VERSION 2.1 ONLINE MODULE RETURN
        Task<ApplicationUser> GetUserByMacAddress(string MacAddress);
        Task<Account> GetAccountById(string Id);
        Task<Ap> GetApByMacAddress(string MacAddress);
        Task<ApInfo> GetApInfoByMacAddress(string MacAddress);
        Task<UserInfo> GetUserRoleByUserId(string Id, string Role);
        Task<Price> GetStationBenefitByStation(string Id, string CoinCode);
        Task<Device> GetDeviceByMacAddress(string MacAddress);
        Task<Device> GetDeviceByIdentity(string IdentityId);
        Task<bool> IsDeviceExist(string MacAddress);
        Task<CoinIdentity> GetCoinIdentityByCode(string CoinCode);
        Task<InternetAuthorization> GetInternetAuthorizationByIdentity(string IdentityId);
        Task<InternetAuthorization> GetInternetAuthorizationByDeviceMacAddress(string MacAddress);

        Task<StationInfo> GetStationInfoByIdentityId(string IdentityId);
    }
}
