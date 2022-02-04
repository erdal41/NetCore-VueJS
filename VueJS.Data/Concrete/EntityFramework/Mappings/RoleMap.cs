using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;
using System;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
            builder.Property(u => u.Name).HasMaxLength(100);
            builder.Property(u => u.NormalizedName).HasMaxLength(100);
            builder.ToTable("Roles");

            builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            builder.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.HasData(
                new Role
                {
                    Id = 1,
                    Name = "All.Manage",
                    NormalizedName = "ALL.MANAGE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
            new Role
            {
                Id = 2,
                Name = "Dashboard.read",
                NormalizedName = "DASHBOARD.READ",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
                new Role
                {
                    Id = 3,
                    Name = "Category.create",
                    NormalizedName = "CATEGORY.CREATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 4,
                    Name = "Category.read",
                    NormalizedName = "CATEGORY.READ",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
            new Role
            {
                Id = 5,
                Name = "Category.update",
                NormalizedName = "CATEGORY.UPDATE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
                new Role
                {
                    Id = 6,
                    Name = "Category.delete",
                    NormalizedName = "CATEGORY.DELETE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                 new Role
                 {
                     Id = 7,
                     Name = "Tag.create",
                     NormalizedName = "TAG.CREATE",
                     ConcurrencyStamp = Guid.NewGuid().ToString()
                 },
                new Role
                {
                    Id = 8,
                    Name = "Tag.read",
                    NormalizedName = "TAG.READ",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
            new Role
            {
                Id = 9,
                Name = "Tag.update",
                NormalizedName = "TAG.UPDATE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
                new Role
                {
                    Id = 10,
                    Name = "Tag.delete",
                    NormalizedName = "TAG.DELETE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                 new Role
                 {
                     Id = 11,
                     Name = "BasePage.create",
                     NormalizedName = "BASEPAGE.CREATE",
                     ConcurrencyStamp = Guid.NewGuid().ToString()
                 },
            new Role
            {
                Id = 12,
                Name = "BasePage.read",
                NormalizedName = "BASEPAGE.READ",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 13,
                Name = "BasePage.update",
                NormalizedName = "BASEPAGE.UPDATE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 14,
                Name = "BasePage.delete",
                NormalizedName = "BASEPAGE.DELETE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
             new Role
             {
                 Id = 15,
                 Name = "OtherPage.create",
                 NormalizedName = "OTHERPAGE.CREATE",
                 ConcurrencyStamp = Guid.NewGuid().ToString()
             },
            new Role
            {
                Id = 16,
                Name = "OtherPage.read",
                NormalizedName = "OTHERPAGE.READ",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 17,
                Name = "OtherPage.update",
                NormalizedName = "OTHERPAGE.UPDATE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 18,
                Name = "OtherPage.delete",
                NormalizedName = "OTHERPAGE.DELETE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 19,
                Name = "Article.create",
                NormalizedName = "ARTICLE.CREATE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 20,
                Name = "Article.read",
                NormalizedName = "ARTICLE.READ",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 21,
                Name = "Article.update",
                NormalizedName = "ARTICLE.UPDATE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 22,
                Name = "Article.delete",
                NormalizedName = "ARTICLE.DELETE",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
                new Role
                {
                    Id = 23,
                    Name = "User.create",
                    NormalizedName = "USER.CREATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 24,
                    Name = "User.read",
                    NormalizedName = "USER.READ",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 25,
                    Name = "User.update",
                    NormalizedName = "USER.UPDATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 26,
                    Name = "User.delete",
                    NormalizedName = "USER.DELETE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 27,
                    Name = "Role.create",
                    NormalizedName = "ROLE.CREATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 28,
                    Name = "Role.read",
                    NormalizedName = "ROLE.READ",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 29,
                    Name = "Role.update",
                    NormalizedName = "ROLE.UPDATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 30,
                    Name = "Role.delete",
                    NormalizedName = "ROLE.DELETE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 31,
                    Name = "Comment.create",
                    NormalizedName = "COMMENT.CREATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 32,
                    Name = "Comment.read",
                    NormalizedName = "COMMENT.READ",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 33,
                    Name = "Comment.update",
                    NormalizedName = "COMMENT.UPDATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 34,
                    Name = "Comment.delete",
                    NormalizedName = "COMMENT.DELETE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }, new Role
                {
                    Id = 35,
                    Name = "UrlRedirect.create",
                    NormalizedName = "URLREDIRECT.CREATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 36,
                    Name = "UrlRedirect.read",
                    NormalizedName = "URLREDIRECT.READ",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 37,
                    Name = "UrlRedirect.update",
                    NormalizedName = "URLREDIRECT.UPDATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 38,
                    Name = "UrlRedirect.delete",
                    NormalizedName = "URLREDIRECT.DELETE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }, new Role
                {
                    Id = 39,
                    Name = "Seo.create",
                    NormalizedName = "SEO.CREATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 40,
                    Name = "Seo.read",
                    NormalizedName = "SEO.READ",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 41,
                    Name = "Seo.update",
                    NormalizedName = "SEO.UPDATE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Id = 42,
                    Name = "Seo.delete",
                    NormalizedName = "SEO.DELETE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });                
        }
    }
}