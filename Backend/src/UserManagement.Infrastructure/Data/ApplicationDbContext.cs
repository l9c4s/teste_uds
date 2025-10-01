using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public DbSet<User> Users { get; set; }
    public DbSet<AccessLevel> AccessLevels { get; set; }
    public DbSet<UserAccessLevel> UserAccessLevels { get; set; }


protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar a tabela Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(255).IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(500).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
            entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
            
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configurar a tabela AccessLevels
        modelBuilder.Entity<AccessLevel>(entity =>
        {
            entity.ToTable("AccessLevels");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
            entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        });

        // Configurar a tabela UserAccessLevels
        modelBuilder.Entity<UserAccessLevel>(entity =>
        {
            entity.ToTable("UserAccessLevels");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.Property(e => e.AccessLevelId).HasColumnName("AccessLevelId");
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.AssignedAt).HasColumnName("AssignedAt");
            entity.Property(e => e.RevokedAt).HasColumnName("RevokedAt");

            // Configurar relacionamentos
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserAccessLevels)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.AccessLevel)
                .WithMany()
                .HasForeignKey(e => e.AccessLevelId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}