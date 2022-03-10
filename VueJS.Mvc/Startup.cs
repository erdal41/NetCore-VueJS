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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using VueJS.Mvc.Areas.Admin.Helper.Abstract;
using VueJS.Mvc.Areas.Admin.Helper.Concrete;
using System.Text;
using VueJS.Shared.Utilities.Extensions;
using VueJS.Entities.Concrete;

namespace VueJS.Mvc
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.Configure<ArticleRightSideBarWidgetOptions>(Configuration.GetSection("ArticleRightSideBarWidgetOptions"));
            services.Configure<ReCaptcha>(Configuration.GetSection("ReCaptcha"));
            services.ConfigureWritable<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.ConfigureWritable<ArticleRightSideBarWidgetOptions>(Configuration.GetSection("ArticleRightSideBarWidgetOptions"));
            services.ConfigureWritable<ReCaptcha>(Configuration.GetSection("ReCaptcha"));
            services.AddAuthorization()
                .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtConfig:Secret"]))
                };
            });            

            //services.AddCors(c =>
            //{
            //    c.AddPolicy("Default", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:50598/"));
            //});
            services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement, UnicodeRanges.LatinExtendedA }));
            services.AddAutoMapper(typeof(CommentProfile), typeof(UploadProfile), typeof(UrlRedirectProfile), typeof(SettingProfile), typeof(PostProfile), typeof(TermProfile), typeof(SeoProfile), typeof(UserProfile));
            services.LoadMyServices(connectionString: Configuration.GetConnectionString("DefaultConnection"));
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IJwtHelper, JwtHelper>();
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors("Default");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
