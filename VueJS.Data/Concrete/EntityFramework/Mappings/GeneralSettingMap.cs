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
            builder.Property(gs => gs.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(gs => gs.IsUseLogoAdminPanel).IsRequired();
            builder.Property(gs => gs.IsUseIconAdminPanel).IsRequired();
            builder.Property(gs => gs.CreatedDate).IsRequired();
            builder.Property(gs => gs.ModifiedDate).IsRequired();
            builder.ToTable("GeneralSettings");

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