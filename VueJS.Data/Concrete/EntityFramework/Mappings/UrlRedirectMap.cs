using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class UrlRedirectMap : IEntityTypeConfiguration<UrlRedirect>
    {
        public void Configure(EntityTypeBuilder<UrlRedirect> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.OldUrl).IsRequired();
            builder.Property(u => u.NewUrl).IsRequired();
            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.CreatedDate).IsRequired();
            builder.Property(u => u.ModifiedDate).IsRequired();
            builder.Property(u => u.Description).HasMaxLength(500);
            builder.ToTable("UrlRedirects");
        }
    }
}