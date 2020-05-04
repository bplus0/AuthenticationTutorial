using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

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

      return View(qs);
    }

    [HttpPost]
    public IActionResult Authorize(string username, string redirectUri, string state)
    {

      const string code = "BABABAAABA";

      return Redirect($"");
    }

    public IActionResult Token()
    {
      return View();
    }
  }
}