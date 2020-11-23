using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClient
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

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    // Identity Server (which is storing my users' identities)
                    options.Authority = "https://localhost:5001";

                    options.SignInScheme = "Cookies";
                    options.RequireHttpsMetadata = false;
                    
                    options.ClientId = "mvc"; // This is my name
                    options.ClientSecret = "secret"; // This is my secret we agreed ahead of time
                    //options.ResponseType = "code";
                    options.ResponseType = "token id_token"; // This respone type i am going to need
                    //options.ResponseType = "code id_token";

                    options.Scope.Add("profile"); // In required scopes please add user profile info
                    options.Scope.Add("api1"); // To call api from mvc client
                    options.GetClaimsFromUserInfoEndpoint = true; // To get additional claims

                    options.SaveTokens = true; // Save all information in authorization cookie
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                // Following code will be used to secure whole application
                //endpoints.MapDefaultControllerRoute()
                //    .RequireAuthorization();
            });
        }
    }
}
