using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Core.DependencyResolvers;
using ShopApp.Core.Extensions;
using ShopApp.Core.Utilities.IoC;
using ShopApp.Core.Utilities.Security.Encyption;
using ShopApp.Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShopApp.WebUI.AutoMappers;

namespace ShopApp.WebUI
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
            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("https://localhost:44392"));
            });
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddCookie()
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidIssuer = tokenOptions.Issuer,
            //            ValidAudience = tokenOptions.Audience,
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
            //        };
            //    });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/login";
                    options.LogoutPath = "/auth/logout";
                    options.Cookie.Name = "AshProgHelp";
                });

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule(),
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                services.AddHttpContextAccessor();
                services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.ConfigureCustomExceptionMiddleware();

            app.UseCors(builder => builder.WithOrigins("https://localhost:44392").AllowAnyHeader());
            app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?code={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
