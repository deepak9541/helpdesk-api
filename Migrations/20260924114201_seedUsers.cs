using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HelpdeskApi.Migrations
{
    /// <inheritdoc />
    public partial class seedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, "admin@helpdesk.com", "Admin User", "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK", "Admin" },
                    { 2, "rahul@helpdesk.com", "Rahul Employee", "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK", "Employee" },
                    { 3, "priya@helpdesk.com", "Priya Employee", "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK", "Employee" },
                    { 4, "amit@helpdesk.com", "Amit Developer", "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK", "Developer" },
                    { 5, "neha@helpdesk.com", "Neha Developer", "$2a$11$hwhUhSevm2S09j51PTx1.OqSKNuU1ZT7HqVRmiCbAtDFZpF52mNmK", "Developer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
