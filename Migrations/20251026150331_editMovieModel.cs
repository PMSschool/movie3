using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie.Migrations
{
    /// <inheritdoc />
    public partial class editMovieModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubImages",
                table: "movies");

            migrationBuilder.AddForeignKey(
                name: "FK_movieSubImages_movies_MovieId",
                table: "movieSubImages",
                column: "MovieId",
                principalTable: "movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movieSubImages_movies_MovieId",
                table: "movieSubImages");

            migrationBuilder.AddColumn<string>(
                name: "SubImages",
                table: "movies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
