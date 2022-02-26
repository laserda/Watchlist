using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Watchlist.Data;
using Watchlist.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Watchlist.Services;

namespace Watchlist.Controllers
{
    [Authorize]
    public class WatchlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public WatchlistController(
            ApplicationDbContext context,
            IUserService userService
        )
        {
            _context = context;
            _userService = userService;
        }


        public async Task<IActionResult> Index()
        {
            var id = await _userService.GetCurrentUserIdAsync(HttpContext);

            var userMovies = _context.UserMovies.Where(u => u.UserId == id);

            var model = userMovies.Select(x => new MovieViewModel
            {
                MovieId = x.MovieId,
                Title = x.Movie.Title,
                Year = x.Movie.Year,
                Watched = x.Watched,
                InWatchlist = true,
                Rating = x.Rating
            }).ToList();

            return View(model);
        }
    }
}
