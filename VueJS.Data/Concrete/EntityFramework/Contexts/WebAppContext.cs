using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VueJS.Data.Concrete.EntityFramework.Mappings;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Contexts
{
    public class WebAppContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<UrlRedirect> UrlRedirects { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<PostTerm> PostTerms { get; set; }
        public DbSet<SeoGeneralSetting> SeoGeneralSettings { get; set; }
        public DbSet<SeoObjectSetting> SeoObjectSettings { get; set; }

        public WebAppContext(DbContextOptions<WebAppContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region ONE TO MANY

            //USER
            modelBuilder.Entity<Post>().HasOne(p => p.User).WithMany(u => u.Posts).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Upload>().HasOne(u => u.User).WithMany(u => u.Uploads).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoObjectSetting>().HasOne(sos => sos.User).WithMany(u => u.SeoObjectSettings).HasForeignKey(sos => sos.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UrlRedirect>().HasOne(ur => ur.User).WithMany(u => u.UrlRedirects).HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Restrict);

            // UPLOAD
            modelBuilder.Entity<Post>().HasOne(p => p.FeaturedImage).WithMany(u => u.Posts).HasForeignKey(p => p.FeaturedImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasOne(u => u.ProfileImage).WithMany(u => u.Users).HasForeignKey(u => u.ProfileImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Gallery>().HasOne(g => g.Upload).WithMany(u => u.Galleries).HasForeignKey(g => g.UploadId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SeoObjectSetting>().HasOne(sos => sos.OpenGraphImage).WithMany(u => u.OpenGraphImages).HasForeignKey(sos => sos.OpenGraphImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoObjectSetting>().HasOne(sos => sos.TwitterImage).WithMany(u => u.TwitterImages).HasForeignKey(sos => sos.TwitterImageId).OnDelete(DeleteBehavior.Restrict);


            // TERM
            modelBuilder.Entity<Term>().HasOne<Term>().WithMany(t => t.Children);
            modelBuilder.Entity<Term>().HasOne(t => t.Parent).WithMany().HasForeignKey(t => t.ParentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PostTerm>().HasOne(pt => pt.Term).WithMany(t => t.PostTerms).HasForeignKey(pt => pt.TermId).OnDelete(DeleteBehavior.Cascade);

            // POST
            modelBuilder.Entity<Post>().HasOne<Post>().WithMany(p => p.Children);
            modelBuilder.Entity<Post>().HasOne(p => p.Parent).WithMany().HasForeignKey(p => p.ParentId).OnDelete(DeleteBehavior.Restrict); ;
            modelBuilder.Entity<PostTerm>().HasOne(pt => pt.Post).WithMany(p => p.PostTerms).HasForeignKey(pt => pt.PostId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Gallery>().HasOne(g => g.Post).WithMany(p => p.Galleries).HasForeignKey(g => g.PostId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);

            // COMMENT
            modelBuilder.Entity<Comment>().HasOne<Comment>().WithMany(c => c.Children);
            modelBuilder.Entity<Comment>().HasMany<Comment>().WithOne(c => c.Parent).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region ONE TO ONE

            // USER
            modelBuilder.Entity<User>().HasOne(u => u.GeneralSetting).WithOne(gs => gs.User).HasForeignKey<GeneralSetting>(gs => gs.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasOne(u => u.SeoGeneralSetting).WithOne(sgs => sgs.User).HasForeignKey<SeoGeneralSetting>(sgs => sgs.UserId).OnDelete(DeleteBehavior.Restrict);

            // UPLOAD
            modelBuilder.Entity<Upload>().HasOne(u => u.Logo).WithOne(gs => gs.Logo).HasForeignKey<GeneralSetting>(gs => gs.LogoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Upload>().HasOne(u => u.MobileLogo).WithOne(gs => gs.MobileLogo).HasForeignKey<GeneralSetting>(gs => gs.MobileLogoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Upload>().HasOne(u => u.Icon).WithOne(gs => gs.Icon).HasForeignKey<GeneralSetting>(gs => gs.IconId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Upload>().HasOne(u => u.SiteMainSetting).WithOne(sgs => sgs.SiteMainImage).HasForeignKey<SeoGeneralSetting>(gs => gs.SiteMainImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Upload>().HasOne(u => u.OpenGraphSetting).WithOne(sgs => sgs.OpenGraphImage).HasForeignKey<SeoGeneralSetting>(gs => gs.OpenGraphImageId).OnDelete(DeleteBehavior.Restrict);

            #endregion

            //modelBuilder.Entity<User>().HasOne(u => u.ProfileImage).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Upload>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Post>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Post>().HasOne(u => u.FeaturedImage).WithMany().OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<PostTerm>().HasOne(u => u.Post).WithMany().OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<PostTerm>().HasOne(u => u.Term).WithMany().OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Comment>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Comment>().HasOne(u => u.Post).WithMany().OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Gallery>().HasOne(u => u.Post).WithMany().OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Gallery>().HasOne(u => u.Upload).WithMany().OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<GeneralSetting>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<GeneralSetting>().HasOne(u => u.Logo).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<GeneralSetting>().HasOne(u => u.MobileLogo).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<GeneralSetting>().HasOne(u => u.Icon).WithMany().OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<SeoGeneralSetting>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<SeoGeneralSetting>().HasOne(u => u.OpenGraphImage).WithMany().OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<SeoObjectSetting>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<SeoObjectSetting>().HasOne(u => u.OpenGraphImage).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<SeoObjectSetting>().HasOne(u => u.TwitterImage).WithMany().OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UrlRedirect>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);


            modelBuilder.ApplyConfiguration(new CommentMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleClaimMap());
            modelBuilder.ApplyConfiguration(new UserClaimMap());
            modelBuilder.ApplyConfiguration(new UserLoginMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            modelBuilder.ApplyConfiguration(new UserTokenMap());
            modelBuilder.ApplyConfiguration(new LogMap());
            modelBuilder.ApplyConfiguration(new UploadMap());
            modelBuilder.ApplyConfiguration(new UrlRedirectMap());
            modelBuilder.ApplyConfiguration(new GeneralSettingMap());
            modelBuilder.ApplyConfiguration(new GalleryMap());
            modelBuilder.ApplyConfiguration(new PostMap());
            modelBuilder.ApplyConfiguration(new TermMap());
            modelBuilder.ApplyConfiguration(new PostTermMap());
            modelBuilder.ApplyConfiguration(new SeoGeneralSettingMap());
            modelBuilder.ApplyConfiguration(new SeoObjectSettingMap());
        }
    }
}