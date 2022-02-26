using System.Threading.Tasks;
using Watchlist.Data;
using System.Collections.Generic;

namespace Watchlist.Repositories
{
    public interface IUserMovieRepository
    {
        Task CreateAsync(UserMovie userMovie);
        Task DeleteAsync(UserMovie userMovie);
        bool GetUserInWatchlist(Movie x, string userId);
        Task<IEnumerable<UserMovie>> GetUserMovieAsync(string userId);
        Task<UserMovie> GetUserMovieAsync(string userId, int movieId);
    }
}
