using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class GalleryMap : IEntityTypeConfiguration<Gallery>
    {
        public void Configure(EntityTypeBuilder<Gallery> builder)
        {
            builder.HasKey(g => new { g.PostId, g.UploadId });
            builder.Property(g => g.PostId).IsRequired(true);
            builder.Property(g => g.UploadId).IsRequired(true);
            builder.ToTable("Galleries");
        }
    }
}