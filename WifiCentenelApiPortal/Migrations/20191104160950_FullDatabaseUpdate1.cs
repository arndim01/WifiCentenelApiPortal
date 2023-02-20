using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WifiCentenelApiPortal.Migrations
{
    public partial class FullDatabaseUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoinIdentities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AmountCode = table.Column<string>(nullable: true),
                    AmountIdentity = table.Column<double>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinIdentities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserType = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(maxLength: 10, nullable: false),
                    Address = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CRoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CRoleClaim_CRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdentityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_CUser_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "CUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CUserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CUserClaim_CUser_UserId",
                        column: x => x.UserId,
                        principalTable: "CUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_CUserLogin_CUser_UserId",
                        column: x => x.UserId,
                        principalTable: "CUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CUserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_CUserRole_CRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CUserRole_CUser_UserId",
                        column: x => x.UserId,
                        principalTable: "CUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CUserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_CUserToken_CUser_UserId",
                        column: x => x.UserId,
                        principalTable: "CUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdentityId = table.Column<string>(nullable: true),
                    MacAddress = table.Column<string>(maxLength: 20, nullable: false),
                    Hostname = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_CUser_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "CUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LegalTerms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AcceptTerms = table.Column<bool>(nullable: false),
                    TermsLink = table.Column<string>(nullable: true),
                    AcceptDate = table.Column<DateTime>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    IdentityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalTerms_CUser_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "CUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationAps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LocationRefId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationAps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationAps_Locations_LocationRefId",
                        column: x => x.LocationRefId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationStations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LocationRefId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationStations_Locations_LocationRefId",
                        column: x => x.LocationRefId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoinLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceRef = table.Column<long>(nullable: false),
                    CoinIdentityRef = table.Column<long>(nullable: false),
                    InsertedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinLogs_CoinIdentities_CoinIdentityRef",
                        column: x => x.CoinIdentityRef,
                        principalTable: "CoinIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoinLogs_Devices_DeviceRef",
                        column: x => x.DeviceRef,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternetAuthorizations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceRef = table.Column<long>(nullable: false),
                    EndDateAuthorized = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternetAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternetAuthorizations_Devices_DeviceRef",
                        column: x => x.DeviceRef,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MacAddress = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocationApRefId = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aps_LocationAps_LocationApRefId",
                        column: x => x.LocationApRefId,
                        principalTable: "LocationAps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdentityId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    LocationStationRefId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_CUser_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "CUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stations_LocationStations_LocationStationRefId",
                        column: x => x.LocationStationRefId,
                        principalTable: "LocationStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StationRefId = table.Column<long>(nullable: false),
                    CoinIdentityRefId = table.Column<long>(nullable: false),
                    BandwidthUp = table.Column<int>(nullable: false),
                    BandwidthDown = table.Column<int>(nullable: false),
                    TotalMinutes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_CoinIdentities_CoinIdentityRefId",
                        column: x => x.CoinIdentityRefId,
                        principalTable: "CoinIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_Stations_StationRefId",
                        column: x => x.StationRefId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_IdentityId",
                table: "Accounts",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Aps_LocationApRefId",
                table: "Aps",
                column: "LocationApRefId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinLogs_CoinIdentityRef",
                table: "CoinLogs",
                column: "CoinIdentityRef");

            migrationBuilder.CreateIndex(
                name: "IX_CoinLogs_DeviceRef",
                table: "CoinLogs",
                column: "DeviceRef");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "CRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CRoleClaim_RoleId",
                table: "CRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "CUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "CUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUserClaim_UserId",
                table: "CUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CUserLogin_UserId",
                table: "CUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CUserRole_RoleId",
                table: "CUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_IdentityId",
                table: "Devices",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_InternetAuthorizations_DeviceRef",
                table: "InternetAuthorizations",
                column: "DeviceRef");

            migrationBuilder.CreateIndex(
                name: "IX_LegalTerms_IdentityId",
                table: "LegalTerms",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationAps_LocationRefId",
                table: "LocationAps",
                column: "LocationRefId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationStations_LocationRefId",
                table: "LocationStations",
                column: "LocationRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CoinIdentityRefId",
                table: "Prices",
                column: "CoinIdentityRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_StationRefId",
                table: "Prices",
                column: "StationRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_IdentityId",
                table: "Stations",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_LocationStationRefId",
                table: "Stations",
                column: "LocationStationRefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Aps");

            migrationBuilder.DropTable(
                name: "CoinLogs");

            migrationBuilder.DropTable(
                name: "CRoleClaim");

            migrationBuilder.DropTable(
                name: "CUserClaim");

            migrationBuilder.DropTable(
                name: "CUserLogin");

            migrationBuilder.DropTable(
                name: "CUserRole");

            migrationBuilder.DropTable(
                name: "CUserToken");

            migrationBuilder.DropTable(
                name: "InternetAuthorizations");

            migrationBuilder.DropTable(
                name: "LegalTerms");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "LocationAps");

            migrationBuilder.DropTable(
                name: "CRole");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "CoinIdentities");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "CUser");

            migrationBuilder.DropTable(
                name: "LocationStations");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
