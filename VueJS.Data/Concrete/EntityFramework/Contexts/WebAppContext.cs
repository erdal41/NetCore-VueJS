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
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuDetail> MenuDetails { get; set; }

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
            modelBuilder.Entity<Term>().HasMany(t => t.Children).WithOne(t => t.Parent).HasForeignKey(t => t.ParentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PostTerm>().HasOne(pt => pt.Term).WithMany(t => t.PostTerms).HasForeignKey(pt => pt.TermId).OnDelete(DeleteBehavior.Cascade);

            // POST
            modelBuilder.Entity<Post>().HasMany(p => p.Children).WithOne(p => p.Parent).HasForeignKey(p => p.ParentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PostTerm>().HasOne(pt => pt.Post).WithMany(p => p.PostTerms).HasForeignKey(pt => pt.PostId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Gallery>().HasOne(g => g.Post).WithMany(p => p.Galleries).HasForeignKey(g => g.PostId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);

            // COMMENT            
            modelBuilder.Entity<Comment>().HasMany(c => c.Children).WithOne(c => c.Parent).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.Restrict);

            //MENU
            modelBuilder.Entity<MenuDetail>().HasOne(md => md.Menu).WithMany(m => m.MenuDetails).HasForeignKey(md => md.MenuId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MenuDetail>().HasMany(md => md.Children).WithOne(md => md.Parent).HasForeignKey(md => md.ParentId).OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region ONE TO ONE

            // USER
            modelBuilder.Entity<GeneralSetting>().HasOne(gs => gs.User).WithOne(u => u.GeneralSetting).HasForeignKey<GeneralSetting>(gs => gs.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoGeneralSetting>().HasOne(sgs => sgs.User).WithOne(u => u.SeoGeneralSetting).HasForeignKey<SeoGeneralSetting>(sgs => sgs.UserId).OnDelete(DeleteBehavior.Restrict);

            // UPLOAD
            modelBuilder.Entity<GeneralSetting>().HasOne(gs => gs.Logo).WithOne(u => u.Logo).HasForeignKey<GeneralSetting>(gs => gs.LogoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GeneralSetting>().HasOne(gs => gs.MobileLogo).WithOne(u => u.MobileLogo).HasForeignKey<GeneralSetting>(gs => gs.MobileLogoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GeneralSetting>().HasOne(gs => gs.Icon).WithOne(u => u.Icon).HasForeignKey<GeneralSetting>(gs => gs.IconId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoGeneralSetting>().HasOne(sgs => sgs.OpenGraphImage).WithOne(u => u.OpenGraphSetting).HasForeignKey<SeoGeneralSetting>(sgs => sgs.OpenGraphImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoGeneralSetting>().HasOne(sgs => sgs.SiteMainImage).WithOne(u => u.SiteMainSetting).HasForeignKey<SeoGeneralSetting>(sgs => sgs.SiteMainImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoGeneralSetting>().HasOne(sgs => sgs.PageSocialImage).WithOne(u => u.PageSocialSetting).HasForeignKey<SeoGeneralSetting>(sgs => sgs.PageSocialImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoGeneralSetting>().HasOne(sgs => sgs.ArticleSocialImage).WithOne(u => u.ArticleSocialSetting).HasForeignKey<SeoGeneralSetting>(sgs => sgs.ArticleSocialImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoGeneralSetting>().HasOne(sgs => sgs.CategorySocialImage).WithOne(u => u.CategorySocialSetting).HasForeignKey<SeoGeneralSetting>(sgs => sgs.CategorySocialImageId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SeoGeneralSetting>().HasOne(sgs => sgs.TagSocialImage).WithOne(u => u.TagSocialSetting).HasForeignKey<SeoGeneralSetting>(sgs => sgs.TagSocialImageId).OnDelete(DeleteBehavior.Restrict);


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
            modelBuilder.ApplyConfiguration(new MenuMap());
            modelBuilder.ApplyConfiguration(new MenuDetailMap());
        }
    }
}