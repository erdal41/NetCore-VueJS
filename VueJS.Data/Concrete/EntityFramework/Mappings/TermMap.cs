using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class TermMap : IEntityTypeConfiguration<Term>
    {
        public void Configure(EntityTypeBuilder<Term> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(t => t.Slug).IsRequired();
            builder.Property(a => a.Slug).HasColumnType("NVARCHAR(MAX)");
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(500);            
            builder.ToTable("Terms");

            //builder.HasOne<Term>(t => t.Parent).WithMany(t => t.Children);
            //builder.HasMany<Term>().WithOne(t => t.Parent).HasForeignKey(t => t.ParentId);
            builder.HasData(
                new Term
                {
                    Id = 1,
                    Slug = "ornek-kategori",
                    Name = "Örnek Kategori",
                    Description = "Kategori Açıklaması",
                    TermType = SubObjectType.category,
                    Count = 1
                },
                new Term
                {
                    Id = 2,
                    Slug = "ornek-etiket",
                    Name = "Örnek Etiket",
                    Description = "Etiket Açıklaması",
                    TermType = SubObjectType.tag,
                    Count = 1
                }
            );
        }
    }
}