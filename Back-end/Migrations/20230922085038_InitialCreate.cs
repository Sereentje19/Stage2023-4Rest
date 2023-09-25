using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_end.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(24)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            //insert method Table Users
            migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "UserId", "Email", "Password" },
            values: new object[,]
            {
                    { 1, "Serena@Kenter.nl", "12345" },
                    { 2, "Kerena@Senter.nl", "11111" },
            });

            //insert method Table Customers
            migrationBuilder.InsertData(
            table: "Customers",
            columns: new[] { "CustomerId", "Email", "Name" },
            values: new object[,]
            {
                    { 1, "Lisa@Bakker.nl", "Lisa Bakker" },
                    { 2, "Bla@Bla.nl", "Bla Bla" },
            });

            //insert method Table Documents
            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "DocumentId", "Image", "Date", "CustomerId", "Type" },
                values: new object[,]
                {
        { 1, new byte[1], new DateTime(2023, 10, 18), 1, Models.Type.Contract.ToString() },
        { 2, new byte[1], new DateTime(2023, 10, 18), 1, Models.Type.Contract.ToString() },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
