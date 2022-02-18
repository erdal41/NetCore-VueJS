using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class GalleryMap : IEntityTypeConfiguration<Gallery>
    {
        public void Configure(EntityTypeBuilder<Gallery> builder)
        {
            builder.HasKey(pg => pg.Id);
            builder.Property(pg => pg.Id).ValueGeneratedOnAdd();
            builder.Property(pg => pg.PostId).IsRequired(true);
            builder.Property(pg => pg.UploadId).IsRequired(true);
            builder.HasKey(pg => new { pg.PostId, pg.UploadId });
            builder.ToTable("Galleries");
        }
    }
}