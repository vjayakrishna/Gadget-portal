using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineGadgetStore.Data.Migrations
{
    public partial class ProductInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductInfo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Color = table.Column<string>(nullable: true),
                    DisplaySize = table.Column<string>(nullable: true),
                    DisplayType = table.Column<string>(nullable: true),
                    GPSSupport = table.Column<string>(nullable: true),
                    InternalStorage = table.Column<string>(nullable: true),
                    InternetConnectivity = table.Column<string>(nullable: true),
                    MapSupport = table.Column<string>(nullable: true),
                    ModelName = table.Column<string>(nullable: true),
                    NetworkType = table.Column<string>(nullable: true),
                    OperatingSystem = table.Column<string>(nullable: true),
                    PrimaryCamera = table.Column<string>(nullable: true),
                    Processor = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: false),
                    Resolution = table.Column<string>(nullable: true),
                    ResolutionType = table.Column<string>(nullable: true),
                    SecondaryCamera = table.Column<string>(nullable: true),
                    Sensors = table.Column<string>(nullable: true),
                    SupportedNetworks = table.Column<string>(nullable: true),
                    Touchscreen = table.Column<string>(nullable: true),
                    Warranty = table.Column<string>(nullable: true),
                    WiFi = table.Column<string>(nullable: true),
                    WiFiVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInfo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductInfo_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductInfo_ProductID",
                table: "ProductInfo",
                column: "ProductID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductInfo");
        }
    }
}
