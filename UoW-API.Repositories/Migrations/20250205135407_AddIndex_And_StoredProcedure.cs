using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UoW_API.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex_And_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string getUserSP = @"CREATE PROCEDURE GET_USERS
                  @userId INT      
                AS
                BEGIN
                    SELECT * FROM USERS WHERE Id = @userId
                END";

            migrationBuilder.Sql(getUserSP);


            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_From_To_State",
                table: "Projects",
                columns: new[] { "From", "To", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name_From",
                table: "Projects",
                columns: new[] { "Name", "From" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name_From_To_State",
                table: "Projects",
                columns: new[] { "Name", "From", "To", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name_To",
                table: "Projects",
                columns: new[] { "Name", "To" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_State",
                table: "Projects",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_To",
                table: "Projects",
                column: "To");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Name",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Projects_From_To_State",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Name",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Name_From",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Name_From_To_State",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Name_To",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_State",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_To",
                table: "Projects");
        }
    }
}
