using System;
using Watchlist.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Watchlist.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace WatchlistTests
{
    public class UserMovieRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UserMovieRepository _userMovieRepository;
        private readonly string[] _userList = new string[] { "user1", "user2", "user3" };

        public UserMovieRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _userMovieRepository = new UserMovieRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        private async Task CreateUserMovie()
        {

            var userMovies = new List<UserMovie>()
            {
                new UserMovie
                {
                    MovieId = 1,
                    UserId = _userList[0],
                    Rating = 3,
                    Watched = true

                },

                new UserMovie
                {
                    MovieId = 2,
                    UserId = _userList[0],
                    Rating = 0,
                    Watched = false

                },

                new UserMovie
                {
                    MovieId = 3,
                    UserId = _userList[0],
                    Rating = 0,
                    Watched = false

                },
                new UserMovie
                {
                    MovieId = 1,
                    UserId = _userList[1],
                    Rating = 3,
                    Watched = true

                },

                new UserMovie
                {
                    MovieId = 3,
                    UserId = _userList[1],
                    Rating = 0,
                    Watched = false

                },
                new UserMovie
                {
                    MovieId = 1,
                    UserId = _userList[2],
                    Rating = 3,
                    Watched = true

                },

                new UserMovie
                {
                    MovieId = 2,
                    UserId = _userList[2],
                    Rating = 0,
                    Watched = false

                },

                new UserMovie
                {
                    MovieId = 3,
                    UserId = _userList[2],
                    Rating = 0,
                    Watched = false

                },
                new UserMovie
                {
                    MovieId = 4,
                    UserId = _userList[2],
                    Rating = 0,
                    Watched = false

                }
            };

            foreach (var uM in userMovies)
                await _userMovieRepository.CreateAsync(uM);
        }

        [Fact]
        public async void GetUserMovieAsync_ShouldReturnAllUserMovie_WhenUseMovieExste()
        {
            //Arrange
            await CreateUserMovie();

            //Act
            var result = await _userMovieRepository.GetUserMovieAsync(_userList[0]);

            //Assert
            Assert.IsAssignableFrom<IEnumerable<UserMovie>>(result);
            Assert.Equal(3, result.ToList().Count);
        }

        [Fact]
        public async void GetUserMovieAsync_ShouldReturnOnUserMovie_WhenUserMovieExiste()
        {
            //Arrange
            await CreateUserMovie();

            //Act
            var result = await _userMovieRepository.GetUserMovieAsync(_userList[0], 1);

            //Assert
            Assert.IsType<UserMovie>(result);
            Assert.Equal(_userList[0], result.UserId);
            Assert.Equal(1, result.MovieId);
            Assert.Equal(3, result.Rating);
            Assert.True(result.Watched);

        }

        [Fact]
        public async void GetUserMovieAsync_ShouldReturnNull_WhenUserMovieExiste()
        {
            //Arrange
            await CreateUserMovie();

            //Act
            var result = await _userMovieRepository.GetUserMovieAsync(_userList[0], 10);

            //Assert
            Assert.Null(result);

        }


        [Fact]
        public async void DeleteAsync_ShouldDeleteOneUserMovie_WenWeDeleteOneUserMovie()
        {
            //Arrange
            await CreateUserMovie();
            var userMovie = await _userMovieRepository.GetUserMovieAsync(_userList[0], 1);
            await _userMovieRepository.DeleteAsync(userMovie);

            //Act
            var result = await _userMovieRepository.GetUserMovieAsync(_userList[0]);
            var result2 = await _userMovieRepository.GetUserMovieAsync(_userList[2]);

            //Assert
            Assert.IsAssignableFrom<IEnumerable<UserMovie>>(result);
            Assert.Equal(2, result.ToList().Count);
            Assert.Equal(4, result2.ToList().Count);
        }

        [Fact]
        public async void GetUserInWatchlist_ShouldReturnTrue_WhenMovieExiste()
        {
            //Arrange
            await CreateUserMovie();
            var movie = new Movie
            {
                Id = 1,
                Title = "Test ",
                Year = 2021
            };

            //Act
            var result = _userMovieRepository.GetUserInWatchlist(movie, _userList[0]);

            //Assert
            Assert.True(result);

        }

        [Fact]
        public async void GetUserInWatchlist_ShouldReturnFalse_WhenMovieNotExiste()
        {
            //Arrange
            await CreateUserMovie();
            var movie = new Movie
            {
                Id = 10,
                Title = "Test ",
                Year = 2021
            };

            //Act
            var result = _userMovieRepository.GetUserInWatchlist(movie, _userList[0]);

            //Assert
            Assert.False(result);


        }
    }
}
