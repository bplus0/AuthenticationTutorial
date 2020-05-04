using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Controllers
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

    [Authorize(Policy = "Claim.Dob")]
    public IActionResult SecretPolicy()
    {
      return View("Secret");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult SecretRole()
    {
      return View("Secret");
    }

    public IActionResult Authenticate()
    {
      var grandmaClaims = new List<Claim>()
      {
        new Claim(ClaimTypes.Name, "bob"),
        new Claim(ClaimTypes.Email, "bob@mail.com"),
        new Claim(ClaimTypes.DateOfBirth, "6/12/92"),
        new Claim(ClaimTypes.Role, "Admin"),
        new Claim("Grandma.Says", "very nice boi.")
      };

      var licenseClaims = new List<Claim>()
      {
        new Claim(ClaimTypes.Name, "DriverBen"),
        new Claim("DrivingLicense", "A=")
      };

      var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
      var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

      var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

      HttpContext.SignInAsync(userPrincipal);

      return RedirectToAction("Index");
    }




    public async Task<IActionResult> DoStuff([FromServices] IAuthorizationService authorizationService)
    {
      //we are doin BL work here

      var builder = new AuthorizationPolicyBuilder("Schema");
      var custompolicy = builder.RequireClaim("Hello").Build();

     var authresult = await _authorizationService.AuthorizeAsync(HttpContext.User, custompolicy);

      if (authresult.Succeeded)
      {
        return View("Index");
      }


      return View("Index");
    }


  }
}