using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mvc.Models;

namespace Mvc.Db
{
  public static class SeedData
  {
    private static Movie[] movies = new Movie[] {
      new Movie {
        Title = "Why Herry Met Sally",
        ReleaseDate = DateTime.Parse("1989-2-12"),
        Genre = "Romantic Comedy",
        Price = 7.99M
      },
      new Movie {
        Title = "Ghostbusters",
        ReleaseDate = DateTime.Parse("1984-3-13"),
        Genre = "Comedy",
        Price = 8.99M
      },
      new Movie {
        Title = "Ghostbusters 2",
        ReleaseDate = DateTime.Parse("1986-2-23"),
        Genre = "Comedy",
        Price = 9.99M
      },
      new Movie {
        Title = "Rio Bravo",
        ReleaseDate = DateTime.Parse("1959-4-15"),
        Genre = "Western",
        Price = 3.99M
      }
    };

    public static void Initialize(IServiceProvider serviceProvider)
    {
      var requiredService =
        serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

      using (var dbContext = new ApplicationDbContext(requiredService))
      {
        if (dbContext.Movies.Any()) return;

        dbContext.Movies.AddRange(movies);
        dbContext.SaveChanges();
      }
    }
  }
}