using Microsoft.EntityFrameworkCore;
using OAuthService.Server.Domain;

namespace OAuthService.Server
{
    public class OAuthContext : DbContext
    {
        public OAuthContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserClient>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ClientConfiguration>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<ClientConfiguration> ClientConfigurations { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserClient> UserClients { get; set; }

        public virtual DbSet<GlobalIdEntity> GlobalIdEntities { get; set; }
    }
}
