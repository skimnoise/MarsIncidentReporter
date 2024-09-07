using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
  public IActionResult Index()
  {
    return View();
  }

  [Authorize(Policy = "Admin")]
  public IActionResult AdminOnly()
  {
    return View();
  }

  [Authorize(Policy = "Reader")]
  public IActionResult ReaderOnly()
  {
    return View();
  }
}
