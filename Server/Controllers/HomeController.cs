﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
  public class HomeController : Controller
  {
    private readonly IAuthorizationService _authorizationService;

    public HomeController(IAuthorizationService authorizationService)
    {
      _authorizationService = authorizationService;
    }

    public IActionResult Index()
    {
      return View();
    }

    [Authorize]
    public IActionResult Secret()
    {
      return View();
    }

    public IActionResult Authenticate()
    {
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
        new Claim("granny", "cookie"),
      };


      var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);

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

      var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

      return Ok(new { accesstoken = tokenJson });
    }


    public IActionResult Decode (string part)
    {
      var bytes = Convert.FromBase64String(part);
      return Ok(Encoding.UTF8.GetString(bytes));
    }
  }
}