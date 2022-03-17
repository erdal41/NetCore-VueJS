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
            builder.HasKey(pt => new { pt.PostId, pt.TermId });
            builder.Property(pt => pt.PostId).IsRequired();
            builder.Property(pt => pt.TermId).IsRequired();
            builder.ToTable("PostTerms");

            builder.HasData(
                new PostTerm
                {
                    PostId = 1,
                    TermId = 1,
                    TermType = ObjectType.category,
                },
                new PostTerm
                {
                    PostId = 1,
                    TermId = 2,
                    TermType = ObjectType.tag
                }
                );
        }
    }
}