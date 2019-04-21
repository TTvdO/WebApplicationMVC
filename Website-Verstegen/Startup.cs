using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Website_Verstegen.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Website_Verstegen
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            //start setting up the login provider
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie()
            //.AddOpenIdConnect("Auth0", options =>
            //{
            //    //setting all needed values for the login provider
            //    options.Authority = $"https://fudgebrownies-verstegen.eu.auth0.com";
            //    options.ClientId = $"PS9OlnEd7knwyiOmaeS_64UhetDH0jra";
            //    options.ClientSecret = $"JdhrvsNOWm_Bz0nb3Kf01L7uXjL4iNOU4tRicaL6oSEGSIyTs-Gt7gu2d7-f8xD6";
            //    options.SaveTokens = true;
            //    options.ResponseType = "code";

            //    options.Scope.Clear();
            //    options.Scope.Add("openid");

            //    options.CallbackPath = new PathString("/signin-auth0");
            //    options.ClaimsIssuer = "Auth0";

            //    options.Events = new OpenIdConnectEvents
            //    {
            //        OnRedirectToIdentityProviderForSignOut = (context) =>
            //        {
            //            //setting all necessary things to redirect the user to the right place upon logout
            //            context.ProtocolMessage.SetParameter("audience", "PS9OlnEd7knwyiOmaeS_64UhetDH0jra");
            //            var logoutUri = $"https://fudgebrownies-verstegen.eu.auth0.com/v2/logout?client_id=PS9OlnEd7knwyiOmaeS_64UhetDH0jra";
            //            var postLogoutUri = context.Properties.RedirectUri;

            //            if (!string.IsNullOrEmpty(postLogoutUri))
            //            {
            //                if (postLogoutUri.StartsWith("/"))
            //                {
            //                    var request = context.Request;
            //                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
            //                }
            //                logoutUri = postLogoutUri;
            //            }
            //            context.Response.Redirect(logoutUri);
            //            context.HandleResponse();

            //            return Task.CompletedTask;
            //        }

            //    };
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddDbContext<DatabaseContext>(options =>
            //{
            //    options.UseSqlServer("Data Source=tcp:srv-h-se-wdpr-a.database.windows.net,1433;Initial Catalog=grp17;User ID=grp17@srv-h-se-wdpr-a.database.windows.net;Password=8jhrewqmN");
            //});

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EFGetStarted.FudgeBrowniesLaatsteVersie;Trusted_Connection=True;MultipleActiveResultSets=true"));

            //services.AddDbContext<DatabaseContext>(options =>
            //{
            //    options.UseSqlServer(Configuration["ConnectionStrings:Default"]);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DatabaseContext>();
                context.Database.Migrate();
                DatabaseSeeding.SeedDatabase(context);
            }
        }
    }
}
