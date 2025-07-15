using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlanchisserieBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Firstname = table.Column<string>(type: "text", nullable: false),
                    Lastname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Civilite = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Motif = table.Column<string>(type: "text", nullable: true),
                    Commentary = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientOrderArticles",
                columns: table => new
                {
                    ClientOrderId = table.Column<int>(type: "integer", nullable: false),
                    ArticleId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientOrderArticles", x => new { x.ClientOrderId, x.ArticleId });
                    table.ForeignKey(
                        name: "FK_ClientOrderArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientOrderArticles_ClientOrders_ClientOrderId",
                        column: x => x.ClientOrderId,
                        principalTable: "ClientOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Description", "ImagePath", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Produit utilisé pour le lavage du linge, efficace contre les taches tenaces.", "images/articles/detergent.jpg", "Détergent", 12.00m },
                    { 2, "Produit permettant de rendre le linge plus doux et agréable au toucher.", "images/articles/assouplissant.jpg", "Assouplissant", 10.50m },
                    { 3, "Sac destiné au transport ou au stockage du linge sale ou propre.", "images/articles/sac_linge.jpg", "Sac à linge", 5.00m },
                    { 4, "Panier robuste pour collecter et déplacer le linge dans la blanchisserie.", "images/articles/panier_linge.jpg", "Panier à linge", 15.00m },
                    { 5, "Gants utilisés pour protéger les mains lors du traitement du linge.", "images/articles/gants_protection.jpg", "Gants de protection", 3.50m },
                    { 6, "Masque de protection pour garantir l’hygiène et la sécurité du personnel.", "images/articles/masque.jpg", "Masque", 2.00m },
                    { 7, "Tablier de travail pour protéger les vêtements lors des opérations de blanchisserie.", "images/articles/tablier.jpg", "Tablier", 8.00m },
                    { 8, "Brosse utilisée pour détacher et nettoyer le linge avant lavage.", "images/articles/brosse_linge.jpg", "Brosse à linge", 4.00m },
                    { 9, "Produit spécialisé pour éliminer les taches difficiles sur le linge.", "images/articles/produit_detachant.jpg", "Produit détachant", 7.50m },
                    { 10, "Spray utilisé pour désinfecter les surfaces et le matériel de blanchisserie.", "images/articles/spray_desinfectant.jpg", "Spray désinfectant", 6.00m }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Civilite", "Email", "Firstname", "Lastname", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, 1, "alice.martin@blanchisserie.com", "Alice", "Martin", "$2a$11$0uqFJ4DMnI6ippubd3HNEOXi7aCcj0eP16Z6bmifWOcWJUSFXGiEq", 1 },
                    { 2, 0, "bob.durand@blanchisserie.com", "Bob", "Durand", "$2a$11$N.YaNzC2QaNh20cIc2m1MOdis67GzK50nOfBhRVIE1W/S.JtdNm4W", 2 },
                    { 3, 1, "claire.dupont@blanchisserie.com", "Claire", "Dupont", "$2a$11$8uVtwZGRx.Sj5OPE9ZbiMuDksgpsXpHcA/zaUQEaKY1lclyUwIByS", 2 },
                    { 4, 0, "david.bernard@blanchisserie.com", "David", "Bernard", "$2a$11$ST.WyRYn1hVJjzvUiigKWuGW/uEjTDKUQDCs.4xlmr3XDr.UTU.NC", 2 },
                    { 5, 1, "emma.petit@blanchisserie.com", "Emma", "Petit", "$2a$11$DalMeeldVQ7jzW56VWAnV.TOtnprQhMQnkFL4W6iY8vYmWHzDlniC", 2 }
                });

            migrationBuilder.InsertData(
                table: "ClientOrders",
                columns: new[] { "Id", "Commentary", "Motif", "OrderDate", "TotalPrice", "UserId" },
                values: new object[] { 1, "Livrer avant midi", "Livraison urgente", new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 39.00m, 1 });

            migrationBuilder.InsertData(
                table: "ClientOrders",
                columns: new[] { "Id", "Commentary", "Motif", "OrderDate", "Status", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 2, null, "Commande régulière", new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, 38.50m, 2 },
                    { 3, "Remplacer les paniers cassés", "Remplacement matériel", new DateTime(2025, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, 44.00m, 3 }
                });

            migrationBuilder.InsertData(
                table: "ClientOrders",
                columns: new[] { "Id", "Commentary", "Motif", "OrderDate", "TotalPrice", "UserId" },
                values: new object[] { 4, "Essai du nouveau détergent", "Test produit", new DateTime(2025, 7, 4, 0, 0, 0, 0, DateTimeKind.Utc), 31.50m, 4 });

            migrationBuilder.InsertData(
                table: "ClientOrders",
                columns: new[] { "Id", "Commentary", "Motif", "OrderDate", "Status", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 5, "Réapprovisionnement annuel", null, new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 53.00m, 5 },
                    { 6, "Demande spécifique client", "Commande spéciale", new DateTime(2025, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, 12.00m, 1 }
                });

            migrationBuilder.InsertData(
                table: "ClientOrders",
                columns: new[] { "Id", "Commentary", "Motif", "OrderDate", "TotalPrice", "UserId" },
                values: new object[] { 7, null, null, new DateTime(2025, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), 27.00m, 2 });

            migrationBuilder.InsertData(
                table: "ClientOrders",
                columns: new[] { "Id", "Commentary", "Motif", "OrderDate", "Status", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 8, "Routine mensuelle", "Commande mensuelle", new DateTime(2025, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, 60.00m, 3 },
                    { 9, "Client VIP", "Demande client", new DateTime(2025, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, 13.00m, 4 }
                });

            migrationBuilder.InsertData(
                table: "ClientOrders",
                columns: new[] { "Id", "Commentary", "Motif", "OrderDate", "TotalPrice", "UserId" },
                values: new object[] { 10, "Stock faible", "Réapprovisionnement", new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc), 75.00m, 5 });

            migrationBuilder.InsertData(
                table: "ClientOrderArticles",
                columns: new[] { "ArticleId", "ClientOrderId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 4, 1, 1 },
                    { 2, 2, 3 },
                    { 5, 2, 2 },
                    { 4, 3, 2 },
                    { 7, 3, 1 },
                    { 10, 3, 1 },
                    { 1, 4, 1 },
                    { 6, 4, 2 },
                    { 8, 4, 2 },
                    { 9, 4, 1 },
                    { 2, 5, 1 },
                    { 3, 5, 4 },
                    { 5, 5, 2 },
                    { 8, 5, 2 },
                    { 9, 5, 1 },
                    { 6, 6, 3 },
                    { 10, 6, 1 },
                    { 5, 7, 2 },
                    { 7, 7, 1 },
                    { 8, 7, 3 },
                    { 1, 8, 1 },
                    { 2, 8, 2 },
                    { 8, 8, 3 },
                    { 9, 8, 2 },
                    { 3, 9, 1 },
                    { 8, 9, 2 },
                    { 1, 10, 2 },
                    { 2, 10, 2 },
                    { 5, 10, 3 },
                    { 9, 10, 1 },
                    { 10, 10, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrderArticles_ArticleId",
                table: "ClientOrderArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrders_UserId",
                table: "ClientOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientOrderArticles");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "ClientOrders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
