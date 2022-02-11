using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using VueCliMiddleware;
using VueJS.Data.Concrete.EntityFramework.Contexts;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Mvc.Helpers.Concrete;
using VueJS.Services.AutoMapper.Profiles;
using VueJS.Services.Extensions;
using VueJS.Mvc.AutoMapper.Profiles;

namespace VueJS.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("Default", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:50598/"));
            });
            services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement, UnicodeRanges.LatinExtendedA }));
            services.AddAutoMapper(typeof(CommentProfile), typeof(UploadProfile), typeof(UrlRedirectProfile), typeof(SettingProfile), typeof(PostProfile), typeof(TermProfile), typeof(SeoProfile), typeof(UserProfile));
            services.LoadMyServices(connectionString: Configuration.GetConnectionString("DefaultConnection"));
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddControllers();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            //services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });







            // Make sure you call this previous to AddMvc
            //services.ConfigureApplicationCookie(options =>
            //{
            //    //options.LoginPath = new PathString("/admin/auth/login");
            //    //options.LogoutPath = new PathString("/admin/auth/logout");
            //    options.Cookie = new CookieBuilder
            //    {
            //        Name = "VueJSWebApp",
            //        HttpOnly = true,
            //        SameSite = SameSiteMode.Strict,
            //        SecurePolicy = CookieSecurePolicy.SameAsRequest // Always
            //    };
            //    options.SlidingExpiration = true;
            //    options.ExpireTimeSpan = System.TimeSpan.FromHours(3);
            //    //options.AccessDeniedPath = new PathString("/admin/auth/unauthorizedaccess");
            //});
            //services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("Default");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseStatusCodePages();
            }

            //app.UseMvc();
            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseAuthentication();  
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<WebAppContext>();
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory()))
            //});

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "ClientApp/";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }
            });
        }
    }
}
