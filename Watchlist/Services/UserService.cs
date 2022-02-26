using Microsoft.AspNetCore.Identity;
using Watchlist.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Watchlist.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        private async Task CreationRole()
        {
            var role = new IdentityRole();
            var roleAdminExiste = await _roleManager.RoleExistsAsync("Administrator");
            if (!roleAdminExiste)
            {
                role.Name = "Administrator";
                await _roleManager.CreateAsync(role);
            }

        }

        public async Task<string> GetCurrentUserIdAsync(HttpContext httpContext)
        {

            await CreationRole();

            var user = await GetCurrentUserAsync(httpContext);
            return user?.Id;
        }

        private Task<ApplicationUser> GetCurrentUserAsync(HttpContext httpContext) 
            => _userManager.GetUserAsync(httpContext.User);

        
    }
}
