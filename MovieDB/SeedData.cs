using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using MovieDB.Entities;

namespace MovieDB
{
    public static class SeedData
    {
        public static async Task AddTestDataAsync(MovieDbContext context)
        {
            // თუ მონაცემები უკვე არსებობს, ხელახლა არ დაამატოს
            if (await context.Movies.AnyAsync())
            {
                Console.WriteLine("მონაცემთა ბაზაში ფილმები უკვე არსებობს.");
                return;
            }

            // ქვეყნები
            var usa = new Country
            {
                Name = "USA"
            };

            var uk = new Country
            {
                Name = "UK"
            };

            context.Countries.AddRange(usa, uk);
            await context.SaveChangesAsync();

            // სტუდიები
            var universal = new Studio
            {
                Name = "Universal",
                CountryId = usa.Id,
                StudioDetails = new StudioDetails
                {
                    LicenseNumber = "LIC-1001"
                }
            };

            var warner = new Studio
            {
                Name = "Warner Bros",
                CountryId = usa.Id,
                StudioDetails = new StudioDetails
                {
                    LicenseNumber = "LIC-1002"
                }
            };

            var pinewood = new Studio
            {
                Name = "Pinewood",
                CountryId = uk.Id,
                StudioDetails = new StudioDetails
                {
                    LicenseNumber = "LIC-1003"
                }
            };

            context.Studios.AddRange(universal, warner, pinewood);
            await context.SaveChangesAsync();

            // მსახიობები
            var actors = new List<Actor>
        {
            new Actor { FirstName = "Tom", LastName = "Hanks" },
            new Actor { FirstName = "Emma", LastName = "Watson" },
            new Actor { FirstName = "Leonardo", LastName = "DiCaprio" },
            new Actor { FirstName = "Kate", LastName = "Winslet" },
            new Actor { FirstName = "Morgan", LastName = "Freeman" },
            new Actor { FirstName = "Daniel", LastName = "Radcliffe" }
        };

            context.Actors.AddRange(actors);
            await context.SaveChangesAsync();

            // ფილმები
            var movies = new List<Movie>
        {
            new Movie
            {
                Title = "Forest Gump",
                ReleaseYear = 1994,
                StudioId = universal.Id,
                Actors = new List<Actor> { actors[0], actors[4] }
            },
            new Movie
            {
                Title = "Inception",
                ReleaseYear = 2010,
                StudioId = warner.Id,
                Actors = new List<Actor> { actors[2], actors[4] }
            },
            new Movie
            {
                Title = "Titanic",
                ReleaseYear = 1997,
                StudioId = universal.Id,
                Actors = new List<Actor> { actors[2], actors[3] }
            },
            new Movie
            {
                Title = "Harry Potter",
                ReleaseYear = 2001,
                StudioId = pinewood.Id,
                Actors = new List<Actor> { actors[1], actors[5] }
            },
            new Movie
            {
                Title = "The Dark Knight",
                ReleaseYear = 2008,
                StudioId = warner.Id,
                Actors = new List<Actor>
                {
                    actors[2],
                    actors[4],
                    actors[5]
                }
            }
        };

            context.Movies.AddRange(movies);
            await context.SaveChangesAsync();

            Console.WriteLine("სატესტო მონაცემები წარმატებით დაემატა!");
        }
    }

}