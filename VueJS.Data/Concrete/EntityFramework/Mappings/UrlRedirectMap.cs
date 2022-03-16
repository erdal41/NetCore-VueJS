using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class UrlRedirectMap : IEntityTypeConfiguration<UrlRedirect>
    {
        public void Configure(EntityTypeBuilder<UrlRedirect> builder)
        {
            builder.HasKey(ur => ur.Id);
            builder.Property(ur => ur.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(ur => ur.OldUrl).IsRequired();
            builder.Property(ur => ur.NewUrl).IsRequired();
            builder.Property(ur => ur.UserId).IsRequired();
            builder.Property(ur => ur.CreatedDate).IsRequired();
            builder.Property(ur => ur.ModifiedDate).IsRequired();
            builder.Property(ur => ur.Description).HasMaxLength(500);
            builder.ToTable("UrlRedirects");
        }
    }
}