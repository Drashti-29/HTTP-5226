using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalArtShowcase.Data.Migrations
{
    /// <inheritdoc />
    public partial class Artwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkExhibition_Exhibitions_ExhibitionId",
                table: "ArtworkExhibition");

            migrationBuilder.RenameColumn(
                name: "ExhibitionId",
                table: "ArtworkExhibition",
                newName: "ExhibitionsExhibitionId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkExhibition_ExhibitionId",
                table: "ArtworkExhibition",
                newName: "IX_ArtworkExhibition_ExhibitionsExhibitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkExhibition_Exhibitions_ExhibitionsExhibitionId",
                table: "ArtworkExhibition",
                column: "ExhibitionsExhibitionId",
                principalTable: "Exhibitions",
                principalColumn: "ExhibitionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtworkExhibition_Exhibitions_ExhibitionsExhibitionId",
                table: "ArtworkExhibition");

            migrationBuilder.RenameColumn(
                name: "ExhibitionsExhibitionId",
                table: "ArtworkExhibition",
                newName: "ExhibitionId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtworkExhibition_ExhibitionsExhibitionId",
                table: "ArtworkExhibition",
                newName: "IX_ArtworkExhibition_ExhibitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtworkExhibition_Exhibitions_ExhibitionId",
                table: "ArtworkExhibition",
                column: "ExhibitionId",
                principalTable: "Exhibitions",
                principalColumn: "ExhibitionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
