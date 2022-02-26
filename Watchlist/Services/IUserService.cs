
using System.Threading.Tasks;
using Watchlist.Data;
using Microsoft.AspNetCore.Http;

namespace Watchlist.Services
{
    public interface IUserService
    {
        public Task<string> GetCurrentUserIdAsync(HttpContext httpContext);
    }
}
