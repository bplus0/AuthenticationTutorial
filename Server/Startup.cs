using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication("OAuth")
        .AddJwtBearer("OAuth", config =>
        {
          var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
          var key = new SymmetricSecurityKey(secretBytes);

          // how to pass the bearer token in a url
          config.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
          {
            OnMessageReceived = context =>
            {
              if (context.Request.Query.ContainsKey("access_token"))
              {
                context.Token = context.Request.Query["access_token"];
              }

              return Task.CompletedTask;
            }
          };

          config.TokenValidationParameters = new TokenValidationParameters()
          {
            ValidIssuer = Constants.Issuer,
            ValidAudience = Constants.Audiance,
            IssuerSigningKey = key
          };
        });

      services.AddControllersWithViews(config =>
      {
      }).AddRazorRuntimeCompilation();
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