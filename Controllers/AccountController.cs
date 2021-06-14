using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KezyMart.Models;
using Microsoft.AspNetCore.Identity;

namespace KezyMart.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("register")]
        public IActionResult Signup()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Signup(SignUpUserModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View();

            returnUrl ??= Url.Content("~/");

            var user = new IdentityUser { Email = userModel.Email, UserName = userModel.Email};

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                LocalRedirect(returnUrl);
            }
            
            foreach (var errorMessage in result.Errors)
            {
                ModelState.AddModelError("", errorMessage.Description);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SignInModel signInModel, string returnUrl = null)
        {
            if (!ModelState.IsValid) 
                return View(signInModel);

            var user = await _userManager.FindByEmailAsync(signInModel.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, signInModel.Password,
                    signInModel.RememberMe, false);

                if (result.Succeeded)
                {
                    returnUrl ??= Url.Content("~/");
                    LocalRedirect(returnUrl);
                }
            }

            ModelState.AddModelError("", "Invalid credentials");

            return View(signInModel);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
