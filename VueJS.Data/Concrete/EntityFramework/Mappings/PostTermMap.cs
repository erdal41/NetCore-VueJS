using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class PostTermMap : IEntityTypeConfiguration<PostTerm>
    {
        public void Configure(EntityTypeBuilder<PostTerm> builder)
        {
            builder.HasKey(ac => ac.Id);
            builder.Property(ac => ac.Id).ValueGeneratedOnAdd();
            builder.Property(ac => ac.PostId).IsRequired(true);
            builder.Property(ac => ac.TermId).IsRequired(true);
            builder.HasKey(ac => new { ac.PostId, ac.TermId });
            builder.ToTable("PostTerms");

            builder.HasData(
                new PostTerm
                {
                    Id = 1,
                    PostId = 1,
                    TermId = 1,
                    TermType = SubObjectType.category,
                },
                new PostTerm
                {
                    Id = 1,
                    PostId = 1,
                    TermId = 2,
                    TermType = SubObjectType.tag
                }
                );
        }
    }
}