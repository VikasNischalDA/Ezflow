using LeadManagementSystem.Common.Helpers;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.MVC.Models;
using LeadManagementSystem.MVC.Services;
using LeadManagementSystem.Shared.Contracts.Request;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LeadManagementSystem.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private string userCookie = "EzflowLogin";
        private readonly IAuthenticationService _authService;
        public LoginController(ILogger<LoginController> logger, IAuthenticationService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetValue("AuthToken", out byte[] token))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else if (Request.Cookies.TryGetValue(userCookie, out string remeberMeValue))
            {
                var value = remeberMeValue.Split("|");
                if (value?.Length == 2)
                {
                    ViewBag.RemeberMeUserName = value[0];
                    ViewBag.RememberMePassword = CryptographyHelper.Decrypt(value[1]);
                }
            }

            return View("Login");
        }
        [AuthorizeToken]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginRequest loginDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    loginDetails.Password = CryptographyHelper.Encrypt(loginDetails.Password);
                    var response = await _authService.AuthenticateAsync(loginDetails);

                    if (response != null && !string.IsNullOrEmpty(response?.Entity?.Token))
                    {
                        SetSessionProps(response);

                        if (loginDetails.RememberMe)
                            SetCookieRememberMe(loginDetails);
                        else
                            ClearCookieRememberMe();
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (response != null && response.Errors.Count > 0)
                    {
                        TempData["Message"] = response.Errors[0].ErrorMessage;
                    }
                    else
                        TempData["Message"] = "An unexpected error occurred.";
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["Message"] = $"Error connecting to the server. Please try again later.";
                TempData["ErrorDetails"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Message"] = "An unexpected error occurred.";
                TempData["ErrorDetails"] = ex.Message;
            }
            return RedirectToAction("Index");

        }

        private void SetSessionProps(Shared.Infrastructure.ActionResult<Shared.Contracts.Response.LoginResponse>? response)
        {
            HttpContext.Session.SetString("AuthToken", response.Entity.Token);
            HttpContext.Session.SetString("UserPrincipal", response.Entity.UserPrincipalName);
            HttpContext.Session.SetString("Desciption", response.Entity.Description);
            HttpContext.Session.SetString("Username", response.Entity.Username);
            TempData["AuthToken"] = response.Entity.Token;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AuthToken");
            return RedirectToAction("Index", "Login");
        }

        private void SetCookieRememberMe(LoginRequest login)
        {           
            Response.Cookies.Append(userCookie, $"{login.Username}|{login.Password}", new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                Expires = DateTime.UtcNow.AddDays(1),
                SameSite = Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax
            });
        }
        private void ClearCookieRememberMe()
        {
            Response.Cookies.Delete(userCookie);
        }
    }
}
