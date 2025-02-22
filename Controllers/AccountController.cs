using CustomerManagerWeb.Models;
using CustomerManagerWeb.Services;
using CustomerManagerWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CustomerManagerWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;
        private readonly string _BaseUrl;
        private readonly ISessionService _sessionService;
        public AccountController(IAuthService authService, ILogger<AccountController> logger, IOptions<ServiceSettings> BaseUrl, ISessionService sessionService)
        {
            _authService = authService;
            _logger = logger;
            _BaseUrl = BaseUrl.Value.BaseUrl;
            _sessionService = sessionService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var UserToken = await _authService.LoginAsync(model);

            if (!string.IsNullOrEmpty(UserToken))
            {
                _sessionService.SetToken(UserToken);
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                ViewBag.Error = "Usuário ou senha inválidos!";
                return View();
            }
        }

        public ActionResult Logout()
        {
            _sessionService.ClearSession();
            return RedirectToAction("Login");
        }
    }
}
