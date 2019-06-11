using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.Controllers
{
  public class HelloWorldController : Controller
  {
    // GET helloworld
    [HttpGet]
    public IActionResult Index() => View();

    // GET helloworld/welcome
    // https://localhost:xxxx/HelloWorld/Welcome?name=Rick&numtimes=4
    [HttpGet]
    public IActionResult Welcome(string name, int numTimes = 1)
    {
      ViewData["Message"] = "Hello " + name;
      ViewData["NumTimes"] = numTimes;
      return View();
    }
  }
}