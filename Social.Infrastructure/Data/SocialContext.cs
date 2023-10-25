using Microsoft.EntityFrameworkCore;

namespace Social.Infrastructure.Data;

public class SocialContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity
                .HasIndex(e => e.Email, "IX_Accounts_Email_Unique")
                .IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .Property(e => e.Id)
                .ValueGeneratedNever();

            entity
                .HasOne(d => d.Account)
                .WithOne(p => p.User)
                .HasForeignKey<User>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity
                .Property(e => e.Image)
                .HasDefaultValue("'https://static.productionready.io/images/smiley-cyrus.jpg'");
        });

        base.OnModelCreating(modelBuilder);
    }

    public SocialContext(DbContextOptions<SocialContext> options) : base(options) { }
}