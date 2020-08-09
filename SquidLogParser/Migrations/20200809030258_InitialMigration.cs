using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace SquidLogParser.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "squid_log");

            migrationBuilder.CreateTable(
                name: "access_log_entry",
                schema: "squid_log",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    time = table.Column<DateTime>(type: "timestamp", nullable: false),
                    milliseconds = table.Column<int>(nullable: false),
                    duration = table.Column<long>(nullable: false),
                    client_address = table.Column<string>(type: "varchar(200)", nullable: true),
                    result_code = table.Column<string>(type: "varchar(150)", nullable: true),
                    bytes = table.Column<long>(nullable: false),
                    request_method = table.Column<string>(type: "varchar(50)", nullable: true),
                    url = table.Column<string>(type: "varchar(500)", nullable: true),
                    user = table.Column<string>(type: "varchar(50)", nullable: true),
                    peer = table.Column<string>(type: "varchar(100)", nullable: true),
                    type = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_log_entry", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_access_bytes",
                schema: "squid_log",
                table: "access_log_entry",
                column: "bytes");

            migrationBuilder.CreateIndex(
                name: "idx_access_log_client_address",
                schema: "squid_log",
                table: "access_log_entry",
                column: "client_address");

            migrationBuilder.CreateIndex(
                name: "idx_access_log_time",
                schema: "squid_log",
                table: "access_log_entry",
                column: "time");

            migrationBuilder.CreateIndex(
                name: "idx_access_log_url",
                schema: "squid_log",
                table: "access_log_entry",
                column: "url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_log_entry",
                schema: "squid_log");
        }
    }
}
