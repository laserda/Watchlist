using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Watchlist.Data;
using Watchlist.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Watchlist.Services;
using Watchlist.Repositories;

namespace Watchlist.Controllers
{
    [Authorize]
    public class WatchlistController : Controller
    {
        private readonly IUserMovieRepository _userMovieRepository;
        private readonly IUserService _userService;

        public WatchlistController(
            IUserMovieRepository userMovieRepository,
            IUserService userService
        )
        {
            _userMovieRepository = userMovieRepository;
            _userService = userService;
        }


        public async Task<IActionResult> Index()
        {
            var id = await _userService.GetCurrentUserIdAsync(HttpContext);

            //var userMovies = _context.UserMovies.Select(x => new MovieViewModel
            //{
            //    UserId = x.UserId,
            //    MovieId = x.MovieId,
            //    Title = x.Movie.Title,
            //    Year = x.Movie.Year,
            //    Watched = x.Watched,
            //    InWatchlist = true,
            //    Rating = x.Rating
            //});

            //var model = userMovies.Where(u => u.UserId == id).ToList();

            return View(await _userMovieRepository.GetUserMovieAsync(id));
        }
    }
}
