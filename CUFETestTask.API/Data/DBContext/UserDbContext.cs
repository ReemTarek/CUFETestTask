using CUFETestTask.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CUFETestTask.API.Data.DBContext
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options)
           : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {  
            builder.Entity<UserModel>()
                .HasIndex(u => u.UserName)
                .IsUnique().IsUnique();
            builder.Entity<UserModel>(entity =>
            {
                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(150);
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.FatherName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.FamilyName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(int.MaxValue);
                entity.Property(e => e.Occupation)
                  .IsRequired()
                  .HasMaxLength(int.MaxValue);
                entity.Property(e => e.Address)
                                  .IsRequired()
                                  .HasMaxLength(int.MaxValue);
                entity.Property(e => e.BirthDate)
                                  .IsRequired().HasMaxLength(int.MaxValue);

                entity.HasKey(e=> e.UserId);
                entity.Property(e => e.UserId).UseIdentityColumn();

            });
        }
    }
}
