using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;
using System;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class GeneralSettingMap : IEntityTypeConfiguration<GeneralSetting>
    {
        public void Configure(EntityTypeBuilder<GeneralSetting> builder)
        {
            builder.HasKey(gs => gs.Id);
            builder.Property(gs => gs.Id).ValueGeneratedOnAdd();
            builder.Property(gs => gs.IsUseLogoAdminPanel).IsRequired();
            builder.Property(gs => gs.IsUseIconAdminPanel).IsRequired();
            builder.Property(gs => gs.CreatedDate).IsRequired();
            builder.Property(gs => gs.ModifiedDate).IsRequired();
            builder.ToTable("GeneralSettings");

            builder.HasOne<User>(gs => gs.User).WithOne(u => u.GeneralSetting).HasForeignKey<GeneralSetting>(gs => gs.UserId);
            builder.HasOne<Upload>(gs => gs.Logo).WithOne(u => u.Logo).HasForeignKey<GeneralSetting>(gs => gs.LogoId);
            builder.HasOne<Upload>(gs => gs.MobileLogo).WithOne(u => u.MobileLogo).HasForeignKey<GeneralSetting>(gs => gs.MobileLogoId);
            builder.HasOne<Upload>(gs => gs.Icon).WithOne(u => u.Icon).HasForeignKey<GeneralSetting>(gs => gs.IconId);

            builder.HasData(
                new GeneralSetting
                {
                    Id = 1,
                    UserId = 1,
                    LogoId = 3,
                    MobileLogoId = 3,
                    IconId = 3,
                    IsUseLogoAdminPanel = false,
                    IsUseIconAdminPanel = false,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                });
        }
    }
}