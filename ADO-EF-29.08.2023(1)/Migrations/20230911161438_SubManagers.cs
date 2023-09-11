using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADO_EF_29._08._2023_1_.Migrations
{
    /// <inheritdoc />
    public partial class SubManagers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Managers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ChiefId",
                table: "Managers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SecDepId",
                table: "Managers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_ChiefId",
                table: "Managers",
                column: "ChiefId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_SecDepId",
                table: "Managers",
                column: "SecDepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Departments_SecDepId",
                table: "Managers",
                column: "SecDepId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Managers_ChiefId",
                table: "Managers",
                column: "ChiefId",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Departments_SecDepId",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Managers_ChiefId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_ChiefId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_SecDepId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "ChiefId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "SecDepId",
                table: "Managers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
