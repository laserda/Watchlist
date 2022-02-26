using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Watchlist.Data;
using Watchlist.Models;

namespace Watchlist.Repositories
{
    public interface IMovieRepository
    {
        Task CreateAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task<IEnumerable<UserMovieViewModel>> GetMoviesAsync(HttpContext httpContext);
        Task<Movie> GetMoviesAsync(int? id);
        Task DeleteAsync(int id);
        bool MovieExists(int id);
    }
}
