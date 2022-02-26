using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Watchlist.Data;
using Microsoft.EntityFrameworkCore;
using Watchlist.Services;
using Watchlist.Repositories;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Watchlist.Models;
using System.Linq;

namespace WatchlistTests
{
    public class MovieRepositoryTests : IDisposable
    {
        public readonly ApplicationDbContext _context;
        public readonly MovieRepository _movieRepository;

        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<IUserMovieRepository> _userMovieRepositoryMock = new Mock<IUserMovieRepository>();
        private readonly Mock<HttpContext> _httpContextMock = new Mock<HttpContext>();

        public MovieRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _movieRepository = new MovieRepository(_context, _userMovieRepositoryMock.Object, _userServiceMock.Object);
        }



        private async Task CreateMovie()
        {

            await _movieRepository.CreateAsync(new Movie
            {
                Id = 1,
                Title = "Koman",
                Year = 2021
            });

            await _movieRepository.CreateAsync(new Movie
            {
                Id = 2,
                Title = "Igor",
                Year = 2020
            });

            await _movieRepository.CreateAsync(new Movie
            {
                Id = 3,
                Title = "Atsé",
                Year = 2019
            });
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Fact]
        public async void GetMovieAsync_ShouldReturnAllMovie_WhenMovieExiste()
        {
            //Arrange
            await CreateMovie();
            //httpcontext.Setup(mock => mock.User).Returns(() => user);

            //Act
            var result = await _movieRepository.GetMoviesAsync(_httpContextMock.Object);


            //Assert
            Assert.IsAssignableFrom<IEnumerable<UserMovieViewModel>>(result);
            Assert.Equal(3, result.ToList().Count);

        }


        [Fact]
        public async void GetMovieAsync_ShouldReturnOneMovie_WhenMovieExiste()
        {
            //Arrange
            await CreateMovie();


            //Act
            var result = await _movieRepository.GetMoviesAsync(1);


            //Assert
            Assert.IsType<Movie>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Koman", result.Title);
            Assert.Equal(2021, result.Year);

        }

        [Fact]
        public async void GetMovieAsync_ShouldReturnNull_WhenMovieNotExiste()
        {
            //Arrange
            await CreateMovie();


            //Act
            var result = await _movieRepository.GetMoviesAsync(10);


            //Assert
            Assert.Null(result);

        }

        [Fact]
        public async void UpdateAsync_ShouldReturnMovieUpdate_WhenWeUpdateMovie()
        {
            //Arrange
            await CreateMovie();
            var movie = await _movieRepository.GetMoviesAsync(1);
            movie.Title = "koman atse fabrice igor";
            movie.Year = 1999;
            await _movieRepository.UpdateAsync(movie);


            //Act
            var result = await _movieRepository.GetMoviesAsync(1);


            //Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("koman atse fabrice igor", result.Title);
            Assert.Equal(1999, result.Year);

        }


        [Fact]
        public async void DeleteAsync_ShouldDeleteOneMovie_WenWeDeleteOneMovie()
        {
            //Arrange
            await CreateMovie();
            await _movieRepository.DeleteAsync(1);


            //Act
            var result = await _movieRepository.GetMoviesAsync(_httpContextMock.Object);


            //Assert
            Assert.IsAssignableFrom<IEnumerable<UserMovieViewModel>>(result);
            Assert.Equal(2, result.ToList().Count);

        }

        [Fact]
        public async void MovieExists_ShouldReturnTrue_WhenMovieExiste()
        {
            //arrange
            await CreateMovie();

            //Act
            var result = _movieRepository.MovieExists(1);

            //Assert
            Assert.True(result);

        }

        [Fact]
        public async void MovieExists_ShouldReturnFalse_WhenMovieExiste()
        {
            //arrange
            await CreateMovie();

            //Act
            var result = _movieRepository.MovieExists(1);

            //Assert
            Assert.True(result);

        }
    }
}
