using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
namespace tris.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            //Make sure data passed into method is good!
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Make sure you entered a username and password";

                return View();
            }
            if (LoginUser(loginModel.Username, loginModel.Password))
            {
                //Add info about the user
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, loginModel.Username)
                 };


                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.Authentication.SignInAsync("TRIS", principal);

                //Just redirect to our index after logging in. 
                return RedirectToAction("index", "Users");
            }
            ViewBag.Error = "Username or password is incorrect.";
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
                await HttpContext.Authentication.SignOutAsync("TRIS");
                
                return RedirectToAction("index", "home");
        }

        private bool LoginUser(string username, string password)
        {
            return username == password;

            //You would got to the db here.
        }
    }
}
