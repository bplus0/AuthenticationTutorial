using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
  public class OAuthController : Controller
  {
    [HttpGet]
    public IActionResult Authorize(
                       string response_Type, // authorization flow type
                       string client_id, // identifier for the client
                       string redirect_uri, //
                       string scope, // what info i am requesting (email, cookie, phone number)
                       string state)  //random string to confirm that we are going back to the same client

    {

      // ?a=foo&bar
      var query = new QueryBuilder();
      query.Add("redirectUri", redirect_uri);
      query.Add("state", state);
      var qs = query.ToQueryString();

      return View(model: query.ToString());
    }

    [HttpPost]
    public IActionResult Authorize(string username, string redirectUri, string state)
    {

      const string code = "BABABAAABA";

      var query = new QueryBuilder();
      query.Add("code", code);
      query.Add("state", state);

      return Redirect($"{redirectUri}{query.ToString()}");
    }

    public async Task<IActionResult> Token(string grant_type,  // flow of access_token request
                                string code, 
                                string redirect_uri, 
                                string client_id)
    {


      //some mechanism for validating the code

      var claims = new[]
     {
        new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
        new Claim("granny", "cookie"),
      };


      var secretBytes = System.Text.Encoding.UTF8.GetBytes(Constants.Secret);

      var key = new SymmetricSecurityKey(secretBytes);
      var algorithim = SecurityAlgorithms.HmacSha256;

      var signingCredentials = new SigningCredentials(key, algorithim);

      var token = new JwtSecurityToken(
        Constants.Issuer,
        Constants.Audiance,
        claims,
        notBefore: DateTime.Now,
        expires: DateTime.Now.AddDays(1),
        signingCredentials);

      var access_token = new JwtSecurityTokenHandler().WriteToken(token);

      var responseObject = new
      {
        access_token,
        token_type = "Bearer",
        raw_claim = "oauthTutorial"
      };

      var responseJson = JsonConvert.SerializeObject(responseObject);
      var responseBytes = Encoding.UTF8.GetBytes(responseJson);
      await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

      return Redirect(redirect_uri);
    }
  }
}