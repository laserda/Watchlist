using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Watchlist.Data;
using Microsoft.AspNetCore.Authorization;
using Watchlist.Models;
using Microsoft.AspNetCore.Identity;
using Watchlist.Services;
using Watchlist.Repositories;

namespace Watchlist.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserMovieRepository _userMovieRepository;
        private readonly IUserService _userService;

        public MoviesController(
            IMovieRepository movieRepository,
            IUserMovieRepository userMovieRepository,
            IUserService userService
        )
        {
            _movieRepository = movieRepository;
            _userService = userService;
            _userMovieRepository = userMovieRepository;
             
        }


        // GET: Movies
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _movieRepository.GetMoviesAsync(HttpContext));
        }


        [Authorize]
        [HttpGet]
        public async Task<JsonResult> AddRemove(int id,int val)
        {
            int retval = -1;
            var userId = await _userService.GetCurrentUserIdAsync(HttpContext);
            if (val == 1)
            {
                // if a record exists in UserMovies that contains both the user’s
                // and movie’s Ids, then the movie is in the watchlist and can
                // be removed
                var movie = await _userMovieRepository.GetUserMovieAsync(userId, id);
                if (movie != null)
                {
                    await _userMovieRepository.DeleteAsync(movie);
                    retval = 0;
                }

            }
            else
            {
                // the movie is not currently in the watchlist, so we need to
                // build a new UserMovie object and add it to the database
                await _userMovieRepository.CreateAsync(
                    new UserMovie
                    {
                        UserId = userId,
                        MovieId = id,
                        Watched = false,
                        Rating = 0
                    }
                );
                retval = 1;
            }

            // and our return value (-1, 0, or 1) back to the script that called
            // this method from the Index page
            return Json(retval);
        }

        // GET: Movies/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepository.GetMoviesAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Year")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _movieRepository.CreateAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepository.GetMoviesAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Year")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _movieRepository.UpdateAsync(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_movieRepository.MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepository.GetMoviesAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieRepository.GetMoviesAsync(id);
            await _movieRepository.CreateAsync(movie);
            return RedirectToAction(nameof(Index));
        }

    }
}
