using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zhankui_Wang_ProblemAssignment2.Migrations
{
    /// <inheritdoc />
    public partial class add_enum_default : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Students_Status",
                table: "Students");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Students_Status",
                table: "Students",
                sql: "_Status IN ('InvitationSent', 'InvitationRejected', 'Enrolled', 'JustCreated')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Students_Status",
                table: "Students");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Students_Status",
                table: "Students",
                sql: "_Status IN ('InvitationSent', 'InvitationRejected', 'Enrolled')");
        }
    }
}
