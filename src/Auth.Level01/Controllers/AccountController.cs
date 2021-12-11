using Auth.Level01.Models;
using Auth.Level01.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Level01.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Route("signup")]
        public IActionResult Signup()
        {
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(SignupUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }

                    return View(userModel);
                }

                ModelState.Clear();
                return View();
            }
            return View(userModel);
        }

        [Route("signin")]
        public IActionResult Signin()
        {
            return View();
        }

        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> Signin(SigninUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSigninAsync(userModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid Credentials");
            }
            return View(userModel);
        }

        [Route("signout")]
        public async Task<IActionResult> Signout()
        {
            await _accountRepository.SignoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
