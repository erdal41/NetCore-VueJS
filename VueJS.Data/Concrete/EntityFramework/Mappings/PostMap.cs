using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.ModifiedDate).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.Content).HasColumnType("NVARCHAR(MAX)");
            builder.Property(p => p.PostStatus).IsRequired();
            builder.Property(p => p.CommentStatus).IsRequired();
            builder.Property(a => a.PostName).IsRequired();
            builder.Property(a => a.PostName).HasColumnType("NVARCHAR(MAX)");
            builder.Property(a => a.PostType).IsRequired();
            builder.Property(a => a.CommentCount).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.BottomContent).HasColumnType("NVARCHAR(MAX)");
            builder.Property(p => p.IsShowPage).IsRequired();
            builder.Property(p => p.IsShowFeaturedImage).IsRequired();
            builder.Property(p => p.IsShowSubPosts).IsRequired();
            builder.ToTable("Posts");

            builder.HasData(
                new Post
                {
                    Id = 1,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Title = "Örnek Makale",
                    Content = "Örnek makale içeriği.",
                    PostStatus = PostStatus.publish,
                    CommentStatus = true,
                    PostName = "ornek-makale",
                    PostType = SubObjectType.article,
                    CommentCount = 0,
                    FeaturedImageId = 2,
                    IsShowPage = false,
                    IsShowFeaturedImage = false,
                    IsShowSubPosts = false
                },
                new Post
                {
                    Id = 2,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Title = "Örnek Sayfa",
                    Content = "Örnek sayfa üst içeriği.",
                    PostStatus = PostStatus.publish,
                    CommentStatus = true,
                    PostName = "ornek-urun",
                    PostType = SubObjectType.page,
                    CommentCount = 0,
                    BottomContent = "Örnek sayfa alt içeriği.",
                    FeaturedImageId = 2,
                    IsShowPage = false,
                    IsShowFeaturedImage = false,
                    IsShowSubPosts = false
                },
                 new Post
                 {
                     Id = 3,
                     UserId = 1,
                     CreatedDate = DateTime.Now,
                     ModifiedDate = DateTime.Now,
                     Title = "Anasayfa",
                     PostStatus = PostStatus.publish,
                     CommentStatus = false,
                     PostName = "anasayfa",
                     PostType = SubObjectType.basepage,
                     CommentCount = 0,
                     FeaturedImageId = 2,
                     IsShowPage = false,
                     IsShowFeaturedImage = false,
                     IsShowSubPosts = false
                 },
                 new Post
                 {
                     Id = 4,
                     UserId = 1,
                     CreatedDate = DateTime.Now,
                     ModifiedDate = DateTime.Now,
                     Title = "Hakkımızda",
                     PostStatus = PostStatus.publish,
                     CommentStatus = false,
                     PostName = "hakkimizda",
                     PostType = SubObjectType.basepage,
                     CommentCount = 0,
                     FeaturedImageId = 2,
                     IsShowPage = false,
                     IsShowFeaturedImage = false,
                     IsShowSubPosts = false
                 },
                 new Post
                 {
                     Id = 5,
                     UserId = 1,
                     CreatedDate = DateTime.Now,
                     ModifiedDate = DateTime.Now,
                     Title = "Fuarlarımız",
                     PostStatus = PostStatus.publish,
                     CommentStatus = false,
                     PostName = "fuarlarimiz",
                     PostType = SubObjectType.basepage,
                     CommentCount = 0,
                     FeaturedImageId = 2,
                     IsShowPage = false,
                     IsShowFeaturedImage = false,
                     IsShowSubPosts = false
                 },
                 new Post
                 {
                     Id = 6,
                     UserId = 1,
                     CreatedDate = DateTime.Now,
                     ModifiedDate = DateTime.Now,
                     Title = "Referanslar",
                     PostStatus = PostStatus.publish,
                     CommentStatus = false,
                     PostName = "referanslar",
                     PostType = SubObjectType.basepage,
                     CommentCount = 0,
                     FeaturedImageId = 2,
                     IsShowPage = false,
                     IsShowFeaturedImage = false,
                     IsShowSubPosts = false
                 },
                 new Post
                 {
                     Id = 7,
                     UserId = 1,
                     CreatedDate = DateTime.Now,
                     ModifiedDate = DateTime.Now,
                     Title = "E-Katalog",
                     PostStatus = PostStatus.publish,
                     CommentStatus = false,
                     PostName = "e-katalog",
                     PostType = SubObjectType.basepage,
                     CommentCount = 0,
                     FeaturedImageId = 2,
                     IsShowPage = false,
                     IsShowFeaturedImage = false,
                     IsShowSubPosts = false
                 },
                 new Post
                 {
                     Id = 8,
                     UserId = 1,
                     CreatedDate = DateTime.Now,
                     ModifiedDate = DateTime.Now,
                     Title = "İletişim",
                     PostStatus = PostStatus.publish,
                     CommentStatus = false,
                     PostName = "iletisim",
                     PostType = SubObjectType.basepage,
                     CommentCount = 0,
                     FeaturedImageId = 2,
                     IsShowPage = false,
                     IsShowFeaturedImage = false,
                     IsShowSubPosts = false
                 });
        }
    }
}