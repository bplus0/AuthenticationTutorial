using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication(config =>
      {
        //we check the cookie to confirm that we are authenticated
        config.DefaultAuthenticateScheme = "ClientCookie";

        //our signin will deal out a cookie
        config.DefaultSignInScheme = "ClientCookie";

        //use this to check if we are allowed to do something
        config.DefaultChallengeScheme = "OurServer";
      })
        .AddCookie("ClientCookie")
        .AddOAuth("OurSever", config =>
        {
          config.ClientId = "client_id";
          config.ClientSecret = "client_secret";
          config.CallbackPath = "/oauth/callback";
          config.AuthorizationEndpoint = "https://localhost:44365/oauth/authorize";
          config.TokenEndpoint = "https://localhost:44365/oauth/token";
        });

      services.AddControllersWithViews().AddRazorRuntimeCompilation();
    }

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