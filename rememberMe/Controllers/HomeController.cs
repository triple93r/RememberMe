using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using rememberMe.Models;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;

namespace rememberMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            string culture = "or-IN";
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });
            //ViewData["projInfo"] = UserLoginClient.getProjectInfo();
            CultureInfo.CurrentCulture = new CultureInfo(culture);

            if (Request.Cookies.TryGetValue("RememberMeCredentials", out string rememberMeValue))
            {
                var values = rememberMeValue.Split('|');
                if (values.Length == 2)
                {
                    ViewBag.RememberMeEmail = values[0];
                    ViewBag.RememberMePassword = values[1];
                }
            }

            if (TempData.ContainsKey("LoginFlag"))
            {
                ViewBag.LoginFlag = TempData["LoginFlag"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usrs model)
        {

           if (model.uName == "Admin2" && model.uPassword=="password3")
            {
                HttpContext.Session.SetString("UserName", model.uName);
                HttpContext.Session.SetString("Email", model.uPassword);
                if (model.RememberMe)
                {
                    Response.Cookies.Append("RememberMeCredentials", $"{model.uName}|{model.uPassword}", new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(30),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    });
                }
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {// Clear session data
            HttpContext.Session.Clear();

            // Sign out the user
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Optionally, remove the "RememberMe" cookie
            Response.Cookies.Delete("RememberMeCredentials");
            return RedirectToAction("Login");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
