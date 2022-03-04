using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;
using System;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.YoutubeLink).HasMaxLength(250);
            builder.Property(u => u.TwitterLink).HasMaxLength(250);
            builder.Property(u => u.InstagramLink).HasMaxLength(250);
            builder.Property(u => u.FacebookLink).HasMaxLength(250);
            builder.Property(u => u.LinkedInLink).HasMaxLength(250);
            builder.Property(u => u.GitHubLink).HasMaxLength(250);
            builder.Property(u => u.WebsiteLink).HasMaxLength(250);
            builder.Property(u => u.FirstName).HasMaxLength(30);
            builder.Property(u => u.LastName).HasMaxLength(30);
            builder.Property(u => u.About).HasMaxLength(1000);
            builder.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            builder.Property(u => u.UserName).HasMaxLength(50);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(50);
            builder.Property(u => u.Email).HasMaxLength(100);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(100);
            builder.ToTable("Users");

            builder.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            builder.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            builder.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

            var adminUser = new User
            {
                Id = 1,
                UserName = "adminuser",
                NormalizedUserName = "ADMINUSER",
                Email = "adminuser@gmail.com",
                NormalizedEmail = "ADMINUSER@GMAIL.COM",
                PhoneNumber = "+905555555555",
                FirstName = "Admin",
                LastName = "User",
                About = "Admin User of WebApp",
                TwitterLink = "https://twitter.com/adminuser",
                InstagramLink = "https://instagram.com/adminuser",
                YoutubeLink = "https://youtube.com/adminuser",
                GitHubLink = "https://github.com/adminuser",
                LinkedInLink = "https://linkedin.com/adminuser",
                WebsiteLink = "https://VueJS.com/",
                FacebookLink = "https://facebook.com/adminuser",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = CreatePasswordHash(adminUser, "Erdal4157+");
            builder.HasData(adminUser);
        }

        private static string CreatePasswordHash(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}