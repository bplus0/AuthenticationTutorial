using API.AuthRequirement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication("DefaultAuth")
        .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("DefaultAuth",null);

      services.AddAuthorization(config =>
      {
        var defaultauthBuilder = new AuthorizationPolicyBuilder();
        var defaultauthPolicy = defaultauthBuilder
        .AddRequirements(new JwtRequirement())
        .Build();

        config.DefaultPolicy = defaultauthPolicy;
      });

      services.AddScoped<IAuthorizationHandler, JwtRequirementHandler>();

      services.AddHttpClient()
        .AddHttpContextAccessor();

      services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
      });
    }
  }
}