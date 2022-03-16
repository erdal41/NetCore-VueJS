using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class MenuDetailMap : IEntityTypeConfiguration<MenuDetail>
    {
        public void Configure(EntityTypeBuilder<MenuDetail> builder)
        {
            builder.HasKey(md => md.Id);
            builder.Property(md => md.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(md => md.MenuId).IsRequired();
            builder.Property(md => md.MenuOrder).IsRequired();
            builder.ToTable("MenuDetails");
        }
    }
}