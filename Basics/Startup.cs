using Basics.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Basics
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication("CookieAuth")
        .AddCookie("CookieAuth", config =>
        {
          config.Cookie.Name = "Grandmas.Cookie";
          config.LoginPath = "/Home/Authenticate";
        });


      services.AddAuthorization(config =>
      {
   
   
        //config.DefaultPolicy = defaultAuthPolicy;


        //config.AddPolicy("Claim.DoB", policyBuilder =>
        //{
        //  policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
        //});

        config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));


        config.AddPolicy("Claim.DoB", policyBuilder =>
        {
          policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
        });


      });


      services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

      services.AddControllersWithViews(config =>
      {
        ////global authorization filter
        //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
        //var defaultAuthPolicy = defaultAuthBuilder
        //.RequireAuthenticatedUser()
        //.RequireClaim(ClaimTypes.DateOfBirth)
        //.Build();



        //config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));

      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      //who are you
      app.UseAuthentication();

      //are you allowed
      app.UseAuthorization();


      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
      });
    }
  }
}