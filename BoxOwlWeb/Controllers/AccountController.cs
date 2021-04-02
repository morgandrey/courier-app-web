using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BoxOwlWeb.Models;
using BoxOwlWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static BoxOwlWeb.Utils.Utils;

namespace BoxOwlWeb.Controllers {
    public class AccountController : Controller {

        private readonly BoxOwlDbContext dbContext;
        private readonly ILogger<AccountController> logger;

        public AccountController(BoxOwlDbContext dbContext, ILogger<AccountController> logger) {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) {
            if (dbContext.Client.Any(x => x.ClientEmail == loginViewModel.ClientEmail)) {
                var clientDb = await dbContext.Client.FirstOrDefaultAsync(x => x.ClientEmail == loginViewModel.ClientEmail);
                var clientPostHash = Convert.ToBase64String(
                    SaltHashPassword(Encoding.ASCII.GetBytes(loginViewModel.ClientPassword),
                        Convert.FromBase64String(clientDb.ClientSalt)));
                if (clientPostHash == clientDb.ClientPassword) {
                    await Authenticate(clientDb);
                    return RedirectToAction("Profile");
                }
                return View();
            }
            return View();
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string email) {
            return dbContext.Client.Any(x => x.ClientEmail == email) ? Json(false)
                    : Json(true);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ClientViewModel clientViewModel) {
            if (ModelState.IsValid) {
                try {
                    var client = new Client {
                        ClientName = clientViewModel.ClientName,
                        ClientSurname = clientViewModel.ClientSurname,
                        ClientEmail = clientViewModel.ClientEmail,
                        ClientPhone = clientViewModel.ClientPhone,
                        ClientSalt = Convert.ToBase64String(GetRandomSalt(16))
                    };
                    client.ClientPassword = Convert.ToBase64String(SaltHashPassword(
                        Encoding.ASCII.GetBytes(clientViewModel.ClientPassword),
                        Convert.FromBase64String(client.ClientSalt)));
                    await dbContext.AddAsync(client);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("Login");
                } catch (Exception ex) {
                    logger.LogTrace(ex.Message);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile() {
            return View(ToClientViewModel(await dbContext.Client
                .FirstAsync(x => x.ClientEmail == User.Identity.Name)));
        }

        private async Task Authenticate(Client client) {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, client.ClientEmail)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private static ClientViewModel ToClientViewModel(Client client) {
            return new ClientViewModel {
                ClientName = client.ClientName,
                ClientSurname = client.ClientSurname,
                ClientPhone = client.ClientPhone,
                ClientEmail = client.ClientEmail
            };
        }
    }
}
