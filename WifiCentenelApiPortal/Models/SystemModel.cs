using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Models.Validations;

namespace WifiCentenelApiPortal.Models
{

    public class UserInfo
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class HomeInfo
    {
        public string AccessPoint { get; set; }
        public string AccessPointMac { get; set; }
        public string Expire { get; set; }
        public string Location { get; set; }
        public string Signal { get; set; }
        public bool IsAuthorized { get; set; }
        public double AvailableMinutes { get; set; }
    }
    
    public class ApInfo
    {
        public string MacAddress { get; set; }
        public string Name { get; set; }
        public string LocationCode { get; set; }
        public string LocationAddress { get; set; }
    }

    public class StationInfo
    {
        public string Name { get; set; }
        public string LocationCode { get; set; }
    }
    
    public class TransactionResult
    {
        public bool IsAuthorized { get; set; }
        public double TotalSecondsExpired { get; set; }
        public string DateExpired { get; set; }
    }

    public class Preferences
    {
        public string Username { get; set; }
        public string Balance { get; set; }
        public string AvailableMinutes { get; set; }
    }

    public class ChangeUsername
    {
        public string Username { get; set; }
        public string Newusername { get; set; }
    }

    [Validator(typeof(SubscribeLoadValidator))]
    public class SubscribeLoad
    {
        public string Code { get; set; }
        public string Ap { get; set; }

    }
    
    public class SystemModel
    {
        public DateTime SystemTime { get; set; }
        public DateTime Expiration { get; set; }
        public double Balance { get; set; }
        public string Plan { get; set; }
        public string Quota { get; set; } = "Unlimited";
        public string Status { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public SystemModel() { }
    }

    public class HistoryLog
    {
        public DateTime DateLoaded { get; set; }
        public decimal Amount { get; set; }
        public string PriceCode { get; set; }
        public DateTime StartedTime { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
    
    public class ReceiveCoin
    {
        //public string ConnectionId { get; set; }
        public string Device { get; set; }
        public string Coin { get; set; }
    }
    
}
