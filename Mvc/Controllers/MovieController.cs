using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc.Db;
using Mvc.Models;

namespace Mvc.Controllers
{
  public class MovieController : Controller
  {
    private readonly ApplicationDbContext _context;

    public MovieController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: Movie
    public async Task<IActionResult> Index(string movieGenre, string searchString)
    {
      IQueryable<string> genreQuery =
        from movie in _context.Movies
        orderby movie.Genre
        select movie.Genre;

      var movies =
        from movie in _context.Movies
        select movie;

      if (!String.IsNullOrEmpty(searchString))
      {
        movies = movies.Where(_movie => _movie.Title.Contains(searchString));
      }

      if (!string.IsNullOrEmpty(movieGenre))
      {
        movies = movies.Where(x => x.Genre == movieGenre);
      }

      var movieGenreViewModel = new MovieGenreViewModel
      {
        Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
        Movies = await movies.ToListAsync()
      };

      return View(movieGenreViewModel);
    }

    // GET: Movie/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var movie = await _context.Movies
          .FirstOrDefaultAsync(m => m.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      return View(movie);
    }

    // GET: Movie/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Movie/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
    {
      if (ModelState.IsValid)
      {
        _context.Add(movie);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(movie);
    }

    // GET: Movie/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var movie = await _context.Movies.FindAsync(id);
      if (movie == null)
      {
        return NotFound();
      }
      return View(movie);
    }

    // POST: Movie/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
    {
      if (id != movie.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(movie);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!MovieExists(movie.Id))
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

    // GET: Movie/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var movie = await _context.Movies
          .FirstOrDefaultAsync(m => m.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      return View(movie);
    }

    // POST: Movie/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var movie = await _context.Movies.FindAsync(id);
      _context.Movies.Remove(movie);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
      return _context.Movies.Any(e => e.Id == id);
    }
  }
}
