using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watchlist.Data;
using Watchlist.Models;
using Watchlist.Services;

namespace Watchlist.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IUserMovieRepository _userMovieRepository;

        public MovieRepository(
            ApplicationDbContext context,
            IUserMovieRepository userMovieRepository,
            IUserService userService
        )
        {
            _context = context;
            _userMovieRepository = userMovieRepository;
            _userService = userService;
        }

        /// <summary>Pour la création des films
        /// 
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task CreateAsync(Movie movie)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Pour la suppression des films
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Pour récupérer la liste des films
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserMovieViewModel>> GetMoviesAsync(HttpContext httpContext)
        {
            var userId = await _userService.GetCurrentUserIdAsync(httpContext);
            var movie = await _context.Movies.ToListAsync();
            var model = movie.Select(x => new UserMovieViewModel
            {
                MovieId = x.Id,
                Title = x.Title,
                Year = x.Year,
                InWatchlist = _userMovieRepository.GetUserInWatchlist(x, userId)
            }).ToList();


            return model;
        }

        /// <summary>
        /// Pour récupérer un film
        /// </summary>
        /// <param name="id">Id du film</param>
        /// <returns></returns>
        public async Task<Movie> GetMoviesAsync(int? id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            return movie;
        }

        /// <summary>
        /// Pour modifier un film
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Movie movie)
        {
            _context.Update(movie);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Pour vérifier si un film existe
        /// </summary>
        /// <param name="id">Id du film</param>
        /// <returns></returns>
        public bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
