using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Models;

namespace BlanchisserieBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ClientOrderArticle> ClientOrderArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Default values for timestamps (database level)

            modelBuilder.Entity<ClientOrder>()
                .Property(clientOrder => clientOrder.OrderDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<ClientOrder>()
                .Property(clientOrder => clientOrder.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<ClientOrder>()
                .Property(clientOrder => clientOrder.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            modelBuilder.Entity<Article>()
                .Property(article => article.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Article>()
                .Property(article => article.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            modelBuilder.Entity<ClientOrderArticle>()
                .Property(clientOrderArticle => clientOrderArticle.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<ClientOrderArticle>()
                .Property(clientOrderArticle => clientOrderArticle.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            modelBuilder.Entity<User>()
                .Property(user => user.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<User>()
                .Property(user => user.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Role>()
                .Property(role => role.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Role>()
                .Property(role => role.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Composite key definition for the join table
            modelBuilder.Entity<ClientOrderArticle>()
                .HasKey(clientOrderArticle => new { clientOrderArticle.ClientOrderId, clientOrderArticle.ArticleId });

            // Relationship ClientOrder → User
            modelBuilder.Entity<ClientOrder>()
                .HasOne(clientOrder => clientOrder.User)
                .WithMany(user => user.ClientOrders)
                .HasForeignKey(clientOrder => clientOrder.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship ClientOrderArticle → ClientOrder
            modelBuilder.Entity<ClientOrderArticle>()
                .HasOne(clientOrderArticle => clientOrderArticle.ClientOrder)
                .WithMany(clientOrder => clientOrder.ClientOrderArticles)
                .HasForeignKey(clientOrderArticle => clientOrderArticle.ClientOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship ClientOrderArticle → Article
            modelBuilder.Entity<ClientOrderArticle>()
                .HasOne(clientOrderArticle => clientOrderArticle.Article)
                .WithMany()
                .HasForeignKey(clientOrderArticle => clientOrderArticle.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship User → Role
            modelBuilder.Entity<User>()
                .HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint on User Email
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            // Default value for ClientOrder status
            modelBuilder.Entity<ClientOrder>()
            .Property(co => co.Status)
            .HasDefaultValue(ClientOrderStatus.PendingValidation);

            // Seed data for Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            // Seed data for Articles
            modelBuilder.Entity<Article>().HasData(
                new Article { Id = 1, Name = "Détergent", Description = "Produit utilisé pour le lavage du linge, efficace contre les taches tenaces.", Price = 12.00m, ImagePath = "images/articles/detergent.jpg" },
                new Article { Id = 2, Name = "Assouplissant", Description = "Produit permettant de rendre le linge plus doux et agréable au toucher.", Price = 10.50m, ImagePath = "images/articles/assouplissant.jpg" },
                new Article { Id = 3, Name = "Sac à linge", Description = "Sac destiné au transport ou au stockage du linge sale ou propre.", Price = 5.00m, ImagePath = "images/articles/sac_linge.jpg" },
                new Article { Id = 4, Name = "Panier à linge", Description = "Panier robuste pour collecter et déplacer le linge dans la blanchisserie.", Price = 15.00m, ImagePath = "images/articles/panier_linge.jpg" },
                new Article { Id = 5, Name = "Gants de protection", Description = "Gants utilisés pour protéger les mains lors du traitement du linge.", Price = 3.50m, ImagePath = "images/articles/gants_protection.jpg" },
                new Article { Id = 6, Name = "Masque", Description = "Masque de protection pour garantir l’hygiène et la sécurité du personnel.", Price = 2.00m, ImagePath = "images/articles/masque.jpg" },
                new Article { Id = 7, Name = "Tablier", Description = "Tablier de travail pour protéger les vêtements lors des opérations de blanchisserie.", Price = 8.00m, ImagePath = "images/articles/tablier.jpg" },
                new Article { Id = 8, Name = "Brosse à linge", Description = "Brosse utilisée pour détacher et nettoyer le linge avant lavage.", Price = 4.00m, ImagePath = "images/articles/brosse_linge.jpg" },
                new Article { Id = 9, Name = "Produit détachant", Description = "Produit spécialisé pour éliminer les taches difficiles sur le linge.", Price = 7.50m, ImagePath = "images/articles/produit_detachant.jpg" },
                new Article { Id = 10, Name = "Spray désinfectant", Description = "Spray utilisé pour désinfecter les surfaces et le matériel de blanchisserie.", Price = 6.00m, ImagePath = "images/articles/spray_desinfectant.jpg" }
            );

            // For development testing, passwords are the firstname in lowercase + "123", example: "alice123"
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Firstname = "Alice", Lastname = "Martin", Email = "alice.martin@blanchisserie.com", Password = "$2a$11$0uqFJ4DMnI6ippubd3HNEOXi7aCcj0eP16Z6bmifWOcWJUSFXGiEq", Civilite = 1, RoleId = 1 },
                new User { Id = 2, Firstname = "Bob", Lastname = "Durand", Email = "bob.durand@blanchisserie.com", Password = "$2a$11$N.YaNzC2QaNh20cIc2m1MOdis67GzK50nOfBhRVIE1W/S.JtdNm4W", Civilite = 0, RoleId = 2 },
                new User { Id = 3, Firstname = "Claire", Lastname = "Dupont", Email = "claire.dupont@blanchisserie.com", Password = "$2a$11$8uVtwZGRx.Sj5OPE9ZbiMuDksgpsXpHcA/zaUQEaKY1lclyUwIByS", Civilite = 1, RoleId = 2 },
                new User { Id = 4, Firstname = "David", Lastname = "Bernard", Email = "david.bernard@blanchisserie.com", Password = "$2a$11$ST.WyRYn1hVJjzvUiigKWuGW/uEjTDKUQDCs.4xlmr3XDr.UTU.NC", Civilite = 0, RoleId = 2 },
                new User { Id = 5, Firstname = "Emma", Lastname = "Petit", Email = "emma.petit@blanchisserie.com", Password = "$2a$11$DalMeeldVQ7jzW56VWAnV.TOtnprQhMQnkFL4W6iY8vYmWHzDlniC", Civilite = 1, RoleId = 2 }
            );

            // Seed data for ClientOrders
            modelBuilder.Entity<ClientOrder>().HasData(
                new ClientOrder { Id = 1, TotalPrice = 39.00m, Motif = "Livraison urgente", Commentary = "Livrer avant midi", Status = ClientOrderStatus.PendingValidation, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 1), DateTimeKind.Utc), UserId = 1 },
                new ClientOrder { Id = 2, TotalPrice = 38.50m, Motif = "Commande régulière", Status = ClientOrderStatus.Validated, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 2), DateTimeKind.Utc), UserId = 2 },
                new ClientOrder { Id = 3, TotalPrice = 44.00m, Motif = "Remplacement matériel", Commentary = "Remplacer les paniers cassés", Status = ClientOrderStatus.Rejected, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 3), DateTimeKind.Utc), UserId = 3 },
                new ClientOrder { Id = 4, TotalPrice = 31.50m, Motif = "Test produit", Commentary = "Essai du nouveau détergent", Status = ClientOrderStatus.PendingValidation, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 4), DateTimeKind.Utc), UserId = 4 },
                new ClientOrder { Id = 5, TotalPrice = 53.00m, Commentary = "Réapprovisionnement annuel", Status = ClientOrderStatus.Validated, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 5), DateTimeKind.Utc), UserId = 5 },
                new ClientOrder { Id = 6, TotalPrice = 12.00m, Motif = "Commande spéciale", Commentary = "Demande spécifique client", Status = ClientOrderStatus.Rejected, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 6), DateTimeKind.Utc), UserId = 1 },
                new ClientOrder { Id = 7, TotalPrice = 27.00m, Status = ClientOrderStatus.PendingValidation, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 7), DateTimeKind.Utc), UserId = 2 }, 
                new ClientOrder { Id = 8, TotalPrice = 60.00m, Motif = "Commande mensuelle", Commentary = "Routine mensuelle", Status = ClientOrderStatus.Validated, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 8), DateTimeKind.Utc), UserId = 3 },
                new ClientOrder { Id = 9, TotalPrice = 13.00m, Motif = "Demande client", Commentary = "Client VIP", Status = ClientOrderStatus.Rejected, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 9), DateTimeKind.Utc), UserId = 4 },
                new ClientOrder { Id = 10, TotalPrice = 75.00m, Motif = "Réapprovisionnement", Commentary = "Stock faible", Status = ClientOrderStatus.PendingValidation, OrderDate = DateTime.SpecifyKind(new DateTime(2025, 7, 10), DateTimeKind.Utc), UserId = 5 }
            );


            // Seed data for ClientOrderArticles
            modelBuilder.Entity<ClientOrderArticle>().HasData(
                // Order 1 (2 articles)
                new ClientOrderArticle { ClientOrderId = 1, ArticleId = 1, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 1, ArticleId = 4, Quantity = 1 },

                // Order 2 (2 articles)
                new ClientOrderArticle { ClientOrderId = 2, ArticleId = 2, Quantity = 3 },
                new ClientOrderArticle { ClientOrderId = 2, ArticleId = 5, Quantity = 2 },

                // Order 3 (3 articles)
                new ClientOrderArticle { ClientOrderId = 3, ArticleId = 4, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 3, ArticleId = 7, Quantity = 1 },
                new ClientOrderArticle { ClientOrderId = 3, ArticleId = 10, Quantity = 1 },

                // Order 4 (4 articles)
                new ClientOrderArticle { ClientOrderId = 4, ArticleId = 1, Quantity = 1 },
                new ClientOrderArticle { ClientOrderId = 4, ArticleId = 6, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 4, ArticleId = 8, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 4, ArticleId = 9, Quantity = 1 },

                // Order 5 (5 articles)
                new ClientOrderArticle { ClientOrderId = 5, ArticleId = 3, Quantity = 4 },
                new ClientOrderArticle { ClientOrderId = 5, ArticleId = 8, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 5, ArticleId = 2, Quantity = 1 },
                new ClientOrderArticle { ClientOrderId = 5, ArticleId = 5, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 5, ArticleId = 9, Quantity = 1 },

                // Order 6 (2 articles)
                new ClientOrderArticle { ClientOrderId = 6, ArticleId = 6, Quantity = 3 },
                new ClientOrderArticle { ClientOrderId = 6, ArticleId = 10, Quantity = 1 },

                // Order 7 (3 articles)
                new ClientOrderArticle { ClientOrderId = 7, ArticleId = 5, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 7, ArticleId = 7, Quantity = 1 },
                new ClientOrderArticle { ClientOrderId = 7, ArticleId = 8, Quantity = 3 },

                // Order 8 (4 articles)
                new ClientOrderArticle { ClientOrderId = 8, ArticleId = 2, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 8, ArticleId = 8, Quantity = 3 },
                new ClientOrderArticle { ClientOrderId = 8, ArticleId = 1, Quantity = 1 },
                new ClientOrderArticle { ClientOrderId = 8, ArticleId = 9, Quantity = 2 },

                // Order 9 (2 articles)
                new ClientOrderArticle { ClientOrderId = 9, ArticleId = 3, Quantity = 1 },
                new ClientOrderArticle { ClientOrderId = 9, ArticleId = 8, Quantity = 2 },

                // Order 10 (5 articles)
                new ClientOrderArticle { ClientOrderId = 10, ArticleId = 1, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 10, ArticleId = 9, Quantity = 1 },
                new ClientOrderArticle { ClientOrderId = 10, ArticleId = 2, Quantity = 2 },
                new ClientOrderArticle { ClientOrderId = 10, ArticleId = 5, Quantity = 3 },
                new ClientOrderArticle { ClientOrderId = 10, ArticleId = 10, Quantity = 2 }
            );

            
        }
    }
}
