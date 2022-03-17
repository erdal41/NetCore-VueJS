using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class SeoObjectSettingMap : IEntityTypeConfiguration<SeoObjectSetting>
    {
        public void Configure(EntityTypeBuilder<SeoObjectSetting> builder)
        {
            builder.HasKey(sos => sos.Id);
            builder.Property(sos => sos.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(sos => sos.Permalink).HasColumnType("NVARCHAR(MAX)");
            builder.Property(sos => sos.UserId).IsRequired();
            builder.Property(sos => sos.SeoTitle).HasMaxLength(100);
            builder.Property(sos => sos.SeoDescription).HasMaxLength(250);
            builder.Property(sos => sos.CanonicalUrl).HasColumnType("NVARCHAR(MAX)");
            builder.Property(sos => sos.FocusKeyword).HasMaxLength(100);
            builder.Property(sos => sos.IsRobotsNoIndex).IsRequired();
            builder.Property(sos => sos.IsRobotsNoFollow).IsRequired();
            builder.Property(sos => sos.IsRobotsNoArchive).IsRequired();
            builder.Property(sos => sos.IsRobotsNoImageIndex).IsRequired();
            builder.Property(sos => sos.IsRobotsNoSnippet).IsRequired();
            builder.Property(sos => sos.SchemaPageType).IsRequired();
            builder.Property(sos => sos.SchemaArticleType).IsRequired();
            builder.Property(sos => sos.OpenGraphTitle).HasColumnType("NVARCHAR(MAX)");
            builder.Property(sos => sos.OpenGraphDescription).HasColumnType("NVARCHAR(MAX)");
            builder.Property(sos => sos.TwitterTitle).HasColumnType("NVARCHAR(MAX)");
            builder.Property(sos => sos.TwitterDescription).HasColumnType("NVARCHAR(MAX)");
            builder.Property(sos => sos.CreatedDate).IsRequired();
            builder.Property(sos => sos.ModifiedDate).IsRequired();
            builder.ToTable("SeoObjectSettings");

            builder.HasData(
                new SeoObjectSetting
                {
                    Id = 1,
                    SeoTitle = "Örnek Makale",
                    ObjectType= ObjectType.article,
                    ObjectId = 1,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                },
                new SeoObjectSetting
                {
                    Id = 2,
                    SeoTitle = "Örnek Sayfa",
                    ObjectType = ObjectType.page,
                    ObjectId = 2,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                },
                new SeoObjectSetting
                {
                    Id = 3,
                    SeoTitle = "Anasayfa",
                    ObjectType = ObjectType.basepage,
                    ObjectId = 3,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                },
                new SeoObjectSetting
                {
                    Id = 4,
                    SeoTitle = "Hakkımızda",
                    ObjectType = ObjectType.basepage,
                    ObjectId = 4,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                },
                new SeoObjectSetting
                {
                    Id = 5,
                    SeoTitle = "Fuarlarımız",
                    ObjectType = ObjectType.basepage,
                    ObjectId = 5,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                },
                new SeoObjectSetting
                {
                    Id = 6,
                    SeoTitle = "Referanslar",
                    ObjectType = ObjectType.basepage,
                    ObjectId = 6,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                },
                new SeoObjectSetting
                {
                    Id = 7,
                    SeoTitle = "İletişim",
                    ObjectType = ObjectType.basepage,
                    ObjectId = 7,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                },
                new SeoObjectSetting
                {
                    Id = 8,
                    SeoTitle = "Örnek Kategori",
                    ObjectType = ObjectType.category,
                    ObjectId = 1,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                },
                new SeoObjectSetting
                {
                    Id = 9,
                    SeoTitle = "Örnek Etiket",
                    ObjectType = ObjectType.tag,
                    ObjectId = 2,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                });
        }
    }
}