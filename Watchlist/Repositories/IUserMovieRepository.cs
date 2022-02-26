using System.Threading.Tasks;
using Watchlist.Data;
using System.Collections.Generic;
using Watchlist.Models;

namespace Watchlist.Repositories
{
    public interface IUserMovieRepository
    {
        Task CreateAsync(UserMovie userMovie);
        Task DeleteAsync(UserMovie userMovie);
        bool GetUserInWatchlist(Movie x, string userId);
        IEnumerable<UserMovieViewModel> GetUserMovieAsync(string userId);
        Task<UserMovie> GetUserMovieAsync(string userId, int movieId);
    }
}
