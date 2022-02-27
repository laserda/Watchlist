using System.Collections.Generic;
using System.Threading.Tasks;
using Watchlist.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Watchlist.Models;
using System;

namespace Watchlist.Repositories
{
    public class UserMovieRepository : IUserMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public UserMovieRepository(
            ApplicationDbContext context
        )
        {
            _context = context;
        }


        /// <summary>
        /// Pour la création des films par utilisateur connecter
        /// </summary>
        /// <param name="userMovie"></param>
        /// <returns></returns>
        public async Task CreateAsync(UserMovie userMovie)
        {
            _context.Add(userMovie);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Pour la suppression des films par utilisateur connecter
        /// </summary>
        /// <param name="userMovie"></param>
        /// <returns></returns>
        public async Task DeleteAsync(UserMovie userMovie)
        {
            _context.UserMovies.Remove(userMovie);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Pour vérifier si le film a été choisi par l'utilisateur connecter ou pas
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="userId">L'Id de l'utilisateur</param>
        /// <returns>True/False</returns>
        public bool GetUserInWatchlist(Movie movie, string userId)
        {
            return _context.UserMovies.Any(u => u.MovieId == movie.Id && u.UserId == userId);
        }


        /// <summary>
        /// Pour récupérer la liste des films par utilisateur connecter
        /// </summary>
        /// <param name="userId">L'Id de l'utilisateur</param>
        /// <returns>List de UserMovie</returns>

        public async Task<IEnumerable<UserMovieViewModel>> GetUserMovieAsync(string userId)
        {
            var userMovies = await _context.UserMovies.Where(u => u.UserId == userId).ToListAsync();


            var model = (from u in userMovies
                         join m in _context.Movies on u.MovieId equals m.Id
                         select new UserMovieViewModel
                         {
                             UserId = u.UserId,
                            MovieId = m.Id,
                            Title = m.Title,
                            Year = m.Year,
                            Watched = u.Watched,
                            InWatchlist = true,
                            Rating = u.Rating
                        }).ToList();

            
            return model;
        }
        /// <summary>
        /// Pour récupérer une film en fonction de l'utilisateur connecter
        /// </summary>
        /// <param name="userId">L'Id de l'utilisateur </param>
        /// <param name="movieId">L'Id du film</param>
        /// <returns></returns>
        public async Task<UserMovie> GetUserMovieAsync(string userId, int movieId)
        {
            return await _context.UserMovies.FirstOrDefaultAsync(um => um.UserId == userId && um.MovieId == movieId);
        }
    }
}
