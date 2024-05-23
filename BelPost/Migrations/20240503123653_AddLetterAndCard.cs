using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BelPost.Migrations
{
    /// <inheritdoc />
    public partial class AddLetterAndCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StampsUsers_AspNetUsers_IdUserNavigationId",
                table: "StampsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_StampsUsers_PostageStamps_IdPostageStampNavigationId",
                table: "StampsUsers");

            migrationBuilder.DropIndex(
                name: "IX_StampsUsers_IdPostageStampNavigationId",
                table: "StampsUsers");

            migrationBuilder.DropIndex(
                name: "IX_StampsUsers_IdUserNavigationId",
                table: "StampsUsers");

            migrationBuilder.DropColumn(
                name: "IdPostageStampNavigationId",
                table: "StampsUsers");

            migrationBuilder.DropColumn(
                name: "IdUserNavigationId",
                table: "StampsUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UrlImg",
                table: "PostageStamps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageStorageName",
                table: "PostageStamps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "PostageStamps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "Img",
                table: "PostageStamps",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Letter",
                table: "PostageStamps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UrlImg",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageStorageName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CountIn",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Img",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostalLetterCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Letter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Img = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalLetterCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCardLetter = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    IdCardNavigationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCards_PostalLetterCards_IdCardNavigationId",
                        column: x => x.IdCardNavigationId,
                        principalTable: "PostalLetterCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCards_IdCardNavigationId",
                table: "UserCards",
                column: "IdCardNavigationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCards");

            migrationBuilder.DropTable(
                name: "PostalLetterCards");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "PostageStamps");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "PostageStamps");

            migrationBuilder.DropColumn(
                name: "Letter",
                table: "PostageStamps");

            migrationBuilder.DropColumn(
                name: "CountIn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "IdPostageStampNavigationId",
                table: "StampsUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdUserNavigationId",
                table: "StampsUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UrlImg",
                table: "PostageStamps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageStorageName",
                table: "PostageStamps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UrlImg",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageStorageName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StampsUsers_IdPostageStampNavigationId",
                table: "StampsUsers",
                column: "IdPostageStampNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_StampsUsers_IdUserNavigationId",
                table: "StampsUsers",
                column: "IdUserNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StampsUsers_AspNetUsers_IdUserNavigationId",
                table: "StampsUsers",
                column: "IdUserNavigationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StampsUsers_PostageStamps_IdPostageStampNavigationId",
                table: "StampsUsers",
                column: "IdPostageStampNavigationId",
                principalTable: "PostageStamps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
