using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Context;
using WifiCentenelApiPortal.Models;

namespace WifiCentenelApiPortal.Data
{
    public static class Seed
    {

        public static async Task INTIALIZE_DATABASE_DATA(IServiceProvider serviceProvider, IConfiguration Configuration, ApplicationDbContext context)
        {

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if ( (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
            
                

                return;
            }

            context.Database.Migrate();
            string[] roleNames = { "Admin", "Manager", "Member", "Station" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }

            }


            var poweruser = new ApplicationUser
            {
                UserName = Configuration.GetSection("AppSettings")["UserName"],
                Email = Configuration.GetSection("AppSettings")["UserEmail"],
                FirstName = "Super",
                LastName = "Admin",
                UserType = UserType.USER,
                CreatedDate = DateTime.Now
            };

            string userPassword = Configuration.GetSection("AppSettings")["UserPassword"];
            var user = await UserManager.FindByNameAsync(Configuration.GetSection("AppSettings")["UserName"]);
            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    try
                    {
                        await UserManager.AddToRoleAsync(poweruser, "Admin");
                        //await 
                        await context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }



            var coinIdentities = new List<CoinIdentity>
            {
                new CoinIdentity { AmountCode = "1C", AmountIdentity = 1.00},
                new CoinIdentity { AmountCode = "5C", AmountIdentity = 5.00},
                new CoinIdentity { AmountCode = "10C", AmountIdentity = 10.00}
            };
            await context.CoinIdentities.AddRangeAsync(coinIdentities);
            await context.SaveChangesAsync();

            var location = new Location()
            {
                 Code = "CARCEB1", 
                Address = "Carreta, Negative, Cebu City"
            };

            await context.Locations.AddAsync(location);
            await context.SaveChangesAsync();

            var locationAp = new LocationAp()
            {
                Location = location
            };

            await context.LocationAps.AddAsync(locationAp);
            await context.SaveChangesAsync();

            var locationStation = new LocationStation()
            {
                Location = location
            };

            await context.LocationStations.AddAsync(locationStation);
            await context.SaveChangesAsync();

            var aps = new List<Ap>
            {
                new Ap { MacAddress = "f0:9f:c2:56:bf:3a", Name = "Kolot Ap", LocationApRefId = locationAp.Id, CreatedDate = DateTime.Now },
                new Ap { MacAddress = "18:e8:29:16:a4:f7", Name = "CARCEB", LocationApRefId = locationAp.Id, CreatedDate = DateTime.Now },
                new Ap { MacAddress = "78:8a:20:70:e0:f9", Name = "CARCEB", LocationApRefId = locationAp.Id, CreatedDate = DateTime.Now},
                new Ap { MacAddress = "dc:9f:db:8a:1f:2c", Name = "CARCEB", LocationApRefId = locationAp.Id, CreatedDate = DateTime.Now}
            };

            await context.Aps.AddRangeAsync(aps);
            await context.SaveChangesAsync();






            var stationuser = new ApplicationUser
            {
                UserName = "CARCEB",
                Email = "station@centenel.com",
                FirstName = "Station",
                LastName = "Careta",
                UserType = UserType.STATION,
                CreatedDate = DateTime.Now
            };

            string stationPassword = "stationcaretacebu";
            var station = await UserManager.FindByNameAsync("CARCEB");
            if (station == null)
            {
                var createPowerUser = await UserManager.CreateAsync(stationuser, stationPassword);
                if (createPowerUser.Succeeded)
                {
                    try
                    {
                        await UserManager.AddToRoleAsync(stationuser, "Station");

                        var stationInfo = new Station { IdentityId = stationuser.Id, Name = "STAT_CARCEB", LocationStationRefId = locationStation.Id };
                        await context.Stations.AddAsync(stationInfo);

                        await context.SaveChangesAsync();
                        var prices = new List<Price>
                        {
                            new Price { StationRefId = stationInfo.Id, CoinIdentity = coinIdentities[0], BandwidthUp = 512, BandwidthDown = 512, TotalMinutes = 60 },
                            new Price { StationRefId = stationInfo.Id, CoinIdentity = coinIdentities[1], BandwidthUp = 512, BandwidthDown = 512, TotalMinutes = 300 },
                            new Price { StationRefId = stationInfo.Id, CoinIdentity = coinIdentities[2], BandwidthUp = 512, BandwidthDown = 512, TotalMinutes = 600 }
                        };

                        await context.Prices.AddRangeAsync(prices);

                        await context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }




            var stationuser2 = new ApplicationUser
            {
                UserName = "CARCEB2",
                Email = "station2@centenel.com",
                FirstName = "Station2",
                LastName = "Careta",
                UserType = UserType.STATION,
                CreatedDate = DateTime.Now
            };

            string stationPassword2 = "station2caretacebu";
            var station2 = await UserManager.FindByNameAsync("CARCEB2");
            if (station2 == null)
            {
                var createPowerUser = await UserManager.CreateAsync(stationuser2, stationPassword2);
                if (createPowerUser.Succeeded)
                {
                    try
                    {
                        await UserManager.AddToRoleAsync(stationuser, "Station");

                        var stationInfo = new Station { IdentityId = stationuser2.Id, Name = "STAT_CARCEB2", LocationStationRefId = locationStation.Id };
                        await context.Stations.AddAsync(stationInfo);

                        await context.SaveChangesAsync();
                        var prices = new List<Price>
                        {
                            new Price { StationRefId = stationInfo.Id, CoinIdentity = coinIdentities[0], BandwidthUp = 512, BandwidthDown = 512, TotalMinutes = 60 },
                            new Price { StationRefId = stationInfo.Id, CoinIdentity = coinIdentities[1], BandwidthUp = 512, BandwidthDown = 512, TotalMinutes = 300 },
                            new Price { StationRefId = stationInfo.Id, CoinIdentity = coinIdentities[2], BandwidthUp = 512, BandwidthDown = 512, TotalMinutes = 600 }
                        };

                        await context.Prices.AddRangeAsync(prices);

                        await context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

        }
    }
}
