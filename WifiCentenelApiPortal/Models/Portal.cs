using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Models
{
    public class Account
    {
        public long Id { get; set; }
        public string IdentityId { get; set; }
        public ApplicationUser Identity { get; set; }
    }

    public class Station
    {
        public long Id { get; set; }
        public string IdentityId { get; set; }
        public ApplicationUser Identity { get; set; }
        public string Name { get; set; }
        public long LocationStationRefId { get; set; }
    }

    public class Ap
    {
        public long Id { get; set; }
        [StringLength(20)]
        [Required]
        public string MacAddress { get; set; }
        public string Name { get; set; }
        public long LocationApRefId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class Location
    {
        public long Id { get; set; }
        [StringLength(10)]
        [Required]
        public string Code { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
    }

    public class LocationStation
    {
        public long Id { get; set; }
        public long LocationRefId { get; set; }
        [ForeignKey("LocationRefId")]
        public virtual Location Location { get; set; }
        [ForeignKey("LocationStationRefId")]
        public ICollection<Station> Stations { get; set; }
    }

    public class LocationAp
    {
        public long Id { get; set; }
        public long LocationRefId { get; set; }
        [ForeignKey("LocationRefId")]
        public virtual Location Location { get; set; }
        [ForeignKey("LocationApRefId")]
        public ICollection<Ap> Aps { get; set; }
    }

    public class Device
    {
        public long Id { get; set; }
        public string IdentityId { get; set; }
        public ApplicationUser Identity { get; set; }
        [StringLength(20)]
        [Required]
        public string MacAddress { get; set; }
        [StringLength(250)]
        public string Hostname { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class InternetAuthorization  
    {
        public long Id { get; set; }
        public long DeviceRef { get; set; }
        [ForeignKey("DeviceRef")]
        public virtual Device Device { get; set; }
        public DateTime EndDateAuthorized { get; set; }
    }

    public class CoinLog
    {
        public long Id { get; set; }
        public long DeviceRef { get; set; }
        [ForeignKey("DeviceRef")]
        public virtual Device Device { get; set; }
        public long CoinIdentityRef { get; set; }
        [ForeignKey("CoinIdentityRef")] 
        public virtual CoinIdentity CoinIdentity { get; set; }
        public DateTime InsertedDate { get; set; }
    }

    public class CoinIdentity
    {
        public long Id { get; set; }
        public string AmountCode { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        [DefaultValue(0.00)]
        public double AmountIdentity { get; set; }
    }

    public class Price
    {
        public long Id { get; set; }
        public long StationRefId { get; set; }
        [ForeignKey("StationRefId")]
        public virtual Station Station { get; set; }
        public long CoinIdentityRefId { get; set; }
        [ForeignKey("CoinIdentityRefId")]
        public virtual CoinIdentity CoinIdentity { get; set; }
        public int BandwidthUp { get; set; }
        public int BandwidthDown { get; set; }
        public int TotalMinutes { get; set; }
    }

    public class LegalTerms
    {
        public long Id { get; set; }
        public bool AcceptTerms { get; set; }
        public string TermsLink { get; set; }
        public DateTime AcceptDate { get; set; }
        public string Version { get; set; }
        public string IdentityId { get; set; }
        public ApplicationUser Identity { get; set; }
    }

}
