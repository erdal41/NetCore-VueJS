using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class MenuMap : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(m => m.Name).IsRequired();
            builder.ToTable("Menus");

            //builder.HasOne<Term>(t => t.Parent).WithMany(t => t.Children);
            //builder.HasMany<Term>().WithOne(t => t.Parent).HasForeignKey(t => t.ParentId);

        }
    }
}