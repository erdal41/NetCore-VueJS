using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.Property(c => c.Text).HasMaxLength(1000);
            builder.Property(c => c.Email).HasMaxLength(100);
            builder.Property(c => c.IsPersonalInformationsSharing).IsRequired();
            builder.Property(c => c.CreatedDate).IsRequired();
            builder.Property(c => c.ModifiedDate).IsRequired();
            builder.Property(c => c.CommentStatus).IsRequired();
            builder.ToTable("Comments");

            builder.HasData(
                new Comment
                {
                    Id = 1,
                    Name = "InitialCreate",
                    Email = "ornek@mail.com",
                    Text = "Bu bir örnek yorum içeriğidir.",
                    PostId = 1,
                    IsPersonalInformationsSharing = true,
                    CommentStatus = CommentStatus.moderated,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                }
            );
        }
    }
}