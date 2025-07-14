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

            // Seed data for Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

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
                .WithMany() // Pas de navigation inverse côté Article (choix assumé)
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
        }
    }
}
