using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;
using System;
using VueJS.Entities.ComplexTypes;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class SeoGeneralSettingMap : IEntityTypeConfiguration<SeoGeneralSetting>
    {
        public void Configure(EntityTypeBuilder<SeoGeneralSetting> builder)
        {
            builder.HasKey(sgs => sgs.Id);
            builder.Property(sgs => sgs.Id).ValueGeneratedOnAdd();
            builder.Property(sgs => sgs.GoogleVerificationCode).HasMaxLength(100);
            builder.Property(sgs => sgs.YandexVerificationCode).HasMaxLength(100);
            builder.Property(sgs => sgs.BingVerificationCode).HasMaxLength(100);
            builder.Property(sgs => sgs.BaiduVerificationCode).HasMaxLength(100);
            builder.Property(sgs => sgs.IsActiveSitemap).IsRequired();
            builder.Property(sgs => sgs.IsActiveRobotsTxt).IsRequired();
            builder.Property(sgs => sgs.IsActiveArticleSearchEngineIndex).IsRequired();
            builder.Property(sgs => sgs.IsActiveArticleSeoSetting).IsRequired();
            builder.Property(sgs => sgs.ArticleDefaultSeoTitle).HasMaxLength(100);
            builder.Property(sgs => sgs.ArticleDefaultSeoDescription).HasMaxLength(150);
            builder.Property(sgs => sgs.IsActivePageSearchEngineIndex).IsRequired();
            builder.Property(sgs => sgs.IsActivePageSeoSetting).IsRequired();
            builder.Property(sgs => sgs.PageDefaultSeoTitle).HasMaxLength(100);
            builder.Property(sgs => sgs.PageDefaultSeoDescription).HasMaxLength(150);
            builder.Property(sgs => sgs.IsActiveCategorySearchEngineIndex).IsRequired();
            builder.Property(sgs => sgs.IsActiveCategorySeoSetting).IsRequired();
            builder.Property(sgs => sgs.CategoryDefaultSeoTitle).HasMaxLength(100);
            builder.Property(sgs => sgs.CategoryDefaultSeoDescription).HasMaxLength(150);
            builder.Property(sgs => sgs.IsActiveTagSearchEngineIndex).IsRequired();
            builder.Property(sgs => sgs.IsActiveTagSeoSetting).IsRequired();
            builder.Property(sgs => sgs.TagDefaultSeoTitle).HasMaxLength(100);
            builder.Property(sgs => sgs.TagDefaultSeoDescription).HasMaxLength(150);
            builder.Property(sgs => sgs.FacebookPageLink).HasMaxLength(100);
            builder.Property(sgs => sgs.TwitterUserName).HasMaxLength(100);
            builder.Property(sgs => sgs.InstagramProfilLink).HasMaxLength(100);
            builder.Property(sgs => sgs.LinkedInProfilLink).HasMaxLength(100);
            builder.Property(sgs => sgs.PinterestProfilLink).HasMaxLength(100);
            builder.Property(sgs => sgs.YoutubeChannelLink).HasMaxLength(100);
            builder.Property(sgs => sgs.IsActiveOpenGraph).IsRequired();
            builder.Property(sgs => sgs.IsActiveTwitterOpenGraph).IsRequired();
            builder.Property(sgs => sgs.TwitterCardType).IsRequired();
            builder.Property(sgs => sgs.PinterestConfirmationCode).HasMaxLength(100);
            builder.Property(sgs => sgs.CreatedDate).IsRequired();
            builder.Property(sgs => sgs.ModifiedDate).IsRequired();
            builder.ToTable("SeoGeneralSettings");

            builder.HasData(
                new SeoGeneralSetting
                {
                    Id = 1,
                    UserId = 1,
                    IsActiveSitemap = false,
                    IsActiveRobotsTxt = false,
                    IsActiveArticleSearchEngineIndex = true,
                    IsActiveArticleSeoSetting = true,
                    IsActivePageSearchEngineIndex = true,
                    IsActivePageSeoSetting = true,
                    IsActiveCategorySearchEngineIndex = true,
                    IsActiveCategorySeoSetting = true,
                    IsActiveTagSearchEngineIndex = true,
                    IsActiveTagSeoSetting = true,
                    IsActiveOpenGraph = false,
                    IsActiveTwitterOpenGraph = false,
                    TwitterCardType = TwitterCardType.Summary,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                });            
        }
    }
}