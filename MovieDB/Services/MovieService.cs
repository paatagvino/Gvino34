
using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using MovieDB.DTOs;
using MovieDB.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDB.Services
{
    public class MovieService
    {
        private readonly MovieDbContext _context;

        public MovieService(MovieDbContext context)
        {
            _context = context;
        }

        // ფილმის განახლება
        public async Task<bool> UpdateMovie(int id, UpdateMovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return false;
            }

            movie.Title = dto.Title;
            movie.ReleaseYear = dto.ReleaseYear;
            movie.StudioId = dto.StudioId;

            await _context.SaveChangesAsync();

            return true;
        }

        // ფილმის წაშლა
        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return false;
            }

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return true;
        }

        // ფილმების ძებნა სტუდიის, წლისა და მსახიობების რაოდენობის მიხედვით
        public async Task<ICollection<Movie>> SearchMoviesByStudioAsync(
            int year,
            string studioName,
            int minimumActorCount)
        {
            return await _context.Movies
                .Include(m => m.Studio)
                .Include(m => m.Actors)
                .Where(m =>
                    m.ReleaseYear >= year &&
                    m.Studio.Name == studioName &&
                    m.Actors.Count >= minimumActorCount)
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }

        // ფილმების ძებნა ქვეყნის, წლისა და მსახიობების მაქსიმალური რაოდენობის მიხედვით
        public async Task<ICollection<Movie>> SearchMoviesByCountryAsync(
            string countryName,
            int minimumYear,
            int maximumActorCount)
        {
            return await _context.Movies
                .Include(m => m.Studio)
                .Include(m => m.Actors)
                .Where(m =>
                    m.Studio.Country.Name == countryName &&
                    m.ReleaseYear >= minimumYear &&
                    m.Actors.Count <= maximumActorCount)
                .OrderBy(m => m.Actors.Count)
                .ThenByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }

        // გაფართოებული ძებნა
        public async Task<ICollection<Movie>> SearchMoviesAdvancedAsync(
            int fromYear,
            int toYear,
            string countryName,
            string titleText,
            int minimumActorCount)
        {
            return await _context.Movies
                .Include(m => m.Studio)
                .Include(m => m.Actors)
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

