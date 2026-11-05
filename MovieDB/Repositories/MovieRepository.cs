
using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Repositories
{
    public class MovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        // დავალება 1
        public async Task<ICollection<MovieDB.Entities.Movie>>
            SearchMoviesByStudioAsync(
                int year,
                string studioName,
                int minimumActorCount)
        {
            return await _context.Movies
                .Where(m =>
                    m.ReleaseYear >= year &&
                    m.Studio.Name == studioName &&
                    m.Actors.Count >= minimumActorCount)
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }

        // დავალება 2
        public async Task<ICollection<MovieDB.Entities.Movie>>
            SearchMoviesByCountryAsync(
                string countryName,
                int minimumYear,
                int maximumActorCount)
        {
            return await _context.Movies
                .Where(m =>
                    m.Studio.Country.Name == countryName &&
                    m.ReleaseYear >= minimumYear &&
                    m.Actors.Count <= maximumActorCount)
                .OrderBy(m => m.Actors.Count)
                .ThenByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }

        // დავალება 3
        public async Task<ICollection<MovieDB.Entities.Movie>>
            SearchMoviesAdvancedAsync(
                int fromYear,
                int toYear,
                string countryName,
                string titleText,
                int minimumActorCount)
        {
            return await _context.Movies
                .Where(m =>
                    m.ReleaseYear >= fromYear &&
                    m.ReleaseYear <= toYear &&
                    m.Studio.Country.Name == countryName &&
                    m.Title.Contains(titleText) &&
                    m.Actors.Count >= minimumActorCount)
                .OrderByDescending(m => m.Actors.Count)
                .ThenByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Studio.Name)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }
    }
}
