using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;
using System;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class UploadMap : IEntityTypeConfiguration<Upload>
    {
        public void Configure(EntityTypeBuilder<Upload> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.FileName).IsRequired();
            builder.Property(u => u.FileName).HasMaxLength(250);
            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.CreatedDate).IsRequired();
            builder.Property(u => u.AltText).HasMaxLength(100);
            builder.Property(u => u.Title).HasMaxLength(100);
            builder.Property(u => u.SubTitle).HasMaxLength(100);
            builder.Property(u => u.FileUrl).HasMaxLength(250);
            builder.Property(u => u.ContentType).HasMaxLength(20);
            builder.Property(u => u.CreatedDate).IsRequired();
            builder.Property(u => u.ModifiedDate).IsRequired();
            builder.Property(u => u.Description).HasMaxLength(500);
            builder.ToTable("Uploads");
            builder.HasOne<User>(u => u.User).WithMany(u => u.Uploads).HasForeignKey(u => u.UserId);

            builder.HasData(
                new Upload
                {
                    Id = 1,
                    FileName = "default-user-image.png",
                    AltText = "Varsayılan Kullanıcı Resmi",
                    Title = "Varsayılan Kullanıcı Resmi",
                    SubTitle = "Varsayılan Kullanıcı Resmi",
                    FileUrl = "/assets/ap/img/default-user-image.png",
                    ContentType = "image/png",
                    Size = 1,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Description = "Bu resim varsayılan kullanıcı resmidir."
                },
                new Upload
                {
                    Id = 2,
                    FileName = "default-post-image.jpg",
                    AltText = "Varsayılan Sayfa Resmi",
                    Title = "Varsayılan Sayfa Resmi",
                    SubTitle = "Varsayılan Sayfa Resmi",
                    FileUrl = "/assets/ap/img/default-post-image.jpg",
                    ContentType = "image/jpeg",
                    Size = 1,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Description = "Bu resim varsayılan sayfa resmidir."
                },
                new Upload
                {
                    Id = 3,
                    FileName = "default-logo.png",
                    AltText = "Varsayılan Logo",
                    Title = "Varsayılan Logo Resmi",
                    SubTitle = "Varsayılan Logo Resmi",
                    FileUrl = "/assets/ap/img/default-logo.png",
                    ContentType = "image/png",
                    Size = 1,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Description = "Bu resim varsayılan logo resmidir."
                }                
            );
        }
    }
}