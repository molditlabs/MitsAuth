using Auth.Level01.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Level01.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateUserAsync(SignupUserModel userModel)
        {
            var user = new IdentityUser()
            {
                UserName = userModel.Username
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            return result;
        }
    }
}
