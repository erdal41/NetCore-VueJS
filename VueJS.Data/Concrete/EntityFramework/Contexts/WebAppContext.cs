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
            modelBuilder.Entity<User>().HasOne(u => u.ProfileImage).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Upload>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Post>().HasOne(u => u.FeaturedImage).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PostTerm>().HasOne(u => u.Post).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PostTerm>().HasOne(u => u.Term).WithMany().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne(u => u.Post).WithMany().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Gallery>().HasOne(u => u.Post).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Gallery>().HasOne(u => u.Upload).WithMany().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GeneralSetting>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GeneralSetting>().HasOne(u => u.Logo).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GeneralSetting>().HasOne(u => u.MobileLogo).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GeneralSetting>().HasOne(u => u.Icon).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SeoGeneralSetting>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoGeneralSetting>().HasOne(u => u.OpenGraphImage).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SeoObjectSetting>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoObjectSetting>().HasOne(u => u.OpenGraphImage).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoObjectSetting>().HasOne(u => u.TwitterImage).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UrlRedirect>().HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);


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