using Auth.Level01.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Auth.Level01.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignupUserModel userModel);
    }
}