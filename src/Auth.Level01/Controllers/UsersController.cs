using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Level01.Controllers
{
    public class UsersController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
                SignInManager<IdentityUser> signInManager,
                UserManager<IdentityUser> userManager,
                ILogger<UsersController> logger
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Add()
        {
            var username = "sysadmin";
            var email = "sysadmin@app.co";
            var password = "Password1@";

            var user = new IdentityUser {
                UserName = username,
                Email = email,                
            };

            var result  = await _userManager.CreateAsync(user, password);


            return View();
        }
    }
}
