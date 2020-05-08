using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace API.Controllers
{
  public class SecretController : Controller
  {


    [Authorize]
    public string Index()
    {
      return "secret message";
    }
  }
}