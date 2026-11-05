using System;
using System.Text;
using System.Threading.Tasks;
using MovieDB.Data;
using MovieDB.DTOs;
using MovieDB.Services;
using MovieDB.Entities;
using System.Collections.Generic;

namespace MovieDB
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;


        using var context = new MovieDbContext();
            var movieService = new MovieService(context);

            // სატესტო მონაცემების დამატება
            await SeedData.AddTestDataAsync(context);

            Console.WriteLine();
            Console.WriteLine("=== MovieDB ===");
            Console.WriteLine("1. ფილმის განახლება");
            Console.WriteLine("2. ფილმის წაშლა");
            Console.WriteLine("3. ძიება სტუდიის მიხედვით");
            Console.WriteLine("4. ძიება ქვეყნის მიხედვით");
            Console.WriteLine("5. გაფართოებული ძიება");
            Console.Write("აირჩიე მოქმედება: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("ფილმის ID: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("არასწორი ID.");
                    return;
                }

                Console.Write("ახალი სათაური: ");
                string title = Console.ReadLine() ?? string.Empty;

                Console.Write("გამოშვების წელი: ");

                if (!int.TryParse(Console.ReadLine(), out int year))
                {
                    Console.WriteLine("არასწორი წელი.");
                    return;
                }

                Console.Write("Studio ID: ");

                if (!int.TryParse(Console.ReadLine(), out int studioId))
                {
                    Console.WriteLine("არასწორი Studio ID.");
                    return;
                }

                var dto = new UpdateMovieDto
                {
                    Title = title,
                    ReleaseYear = year,
                    StudioId = studioId
                };

                bool updated = await movieService.UpdateMovie(id, dto);

                Console.WriteLine(updated
                    ? "ფილმი წარმატებით განახლდა!"
                    : "ფილმი ვერ მოიძებნა.");
            }
            else if (choice == "2")
            {
                Console.Write("წასაშლელი ფილმის ID: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("არასწორი ID.");
                    return;
                }

                Console.Write("დარწმუნებული ხარ, რომ გსურს წაშლა? (კი/არა): ");

                string? confirmation = Console.ReadLine();

                if (confirmation?.Trim().Equals(
                    "კი", StringComparison.OrdinalIgnoreCase) == true)
                {
                    bool deleted = await movieService.DeleteMovie(id);

                    Console.WriteLine(deleted
                        ? "ფილმი წარმატებით წაიშალა!"
                        : "ფილმი ვერ მოიძებნა.");
                }
                else
                {
                    Console.WriteLine("წაშლა გაუქმებულია.");
                }
            }
            else if (choice == "3")
            {
                Console.Write("მინიმალური გამოშვების წელი: ");

                if (!int.TryParse(Console.ReadLine(), out int year))
                {
                    Console.WriteLine("არასწორი წელი.");
                    return;
                }

                Console.Write("სტუდიის სახელი: ");
                string studioName = Console.ReadLine() ?? string.Empty;

                Console.Write("მსახიობების მინიმალური რაოდენობა: ");

                if (!int.TryParse(Console.ReadLine(), out int minActors))
                {
                    Console.WriteLine("არასწორი რაოდენობა.");
                    return;
                }

                var movies = await movieService.SearchMoviesByStudioAsync(
                    year, studioName, minActors);

                PrintMovies(movies);
            }
            else if (choice == "4")
            {
                Console.Write("ქვეყნის სახელი: ");
                string countryName = Console.ReadLine() ?? string.Empty;

                Console.Write("მინიმალური გამოშვების წელი: ");

                if (!int.TryParse(Console.ReadLine(), out int minYear))
                {
                    Console.WriteLine("არასწორი წელი.");
                    return;
                }

                Console.Write("მსახიობების მაქსიმალური რაოდენობა: ");

                if (!int.TryParse(Console.ReadLine(), out int maxActors))
                {
                    Console.WriteLine("არასწორი რაოდენობა.");
                    return;
                }

                var movies = await movieService.SearchMoviesByCountryAsync(
                    countryName, minYear, maxActors);

                PrintMovies(movies);
            }
            else if (choice == "5")
            {
                Console.Write("საწყისი წელი: ");

                if (!int.TryParse(Console.ReadLine(), out int fromYear))
                {
                    Console.WriteLine("არასწორი წელი.");
                    return;
                }

                Console.Write("საბოლოო წელი: ");

                if (!int.TryParse(Console.ReadLine(), out int toYear))
                {
                    Console.WriteLine("არასწორი წელი.");
                    return;
                }

                if (fromYear > toYear)
                {
                    Console.WriteLine(
                        "საწყისი წელი საბოლოო წელზე მეტი ვერ იქნება.");
                    return;
                }

                Console.Write("ქვეყნის სახელი: ");
                string countryName = Console.ReadLine() ?? string.Empty;

                Console.Write("სათაურის საძიებელი ტექსტი: ");
                string titleText = Console.ReadLine() ?? string.Empty;

                Console.Write("მსახიობების მინიმალური რაოდენობა: ");

                if (!int.TryParse(Console.ReadLine(), out int minActors))
                {
                    Console.WriteLine("არასწორი რაოდენობა.");
                    return;
                }

                var movies = await movieService.SearchMoviesAdvancedAsync(
                    fromYear,
                    toYear,
                    countryName,
                    titleText,
                    minActors);

                PrintMovies(movies);
            }
            else
            {
                Console.WriteLine("არასწორი არჩევანი.");
            }

            Console.WriteLine();
            Console.WriteLine(
                "პროგრამის დასასრულებლად დააჭირე ნებისმიერ ღილაკს...");

            Console.ReadKey();
        }

        static void PrintMovies(ICollection<Movie> movies)
        {
            if (movies.Count == 0)
            {
                Console.WriteLine(
                    "შესაბამისი ფილმები ვერ მოიძებნა.");

                return;
            }

            Console.WriteLine();
            Console.WriteLine("=== ძიების შედეგები ===");

            foreach (var movie in movies)
            {
                Console.WriteLine(
                    $"ID: {movie.Id} | " +
                    $"სათაური: {movie.Title} | " +
                    $"წელი: {movie.ReleaseYear} | " +
                    $"სტუდია: {movie.Studio.Name} | " +
                    $"მსახიობები: {movie.Actors.Count}");
            }

            Console.WriteLine();
            Console.WriteLine($"სულ მოიძებნა: {movies.Count} ფილმი.");
        }
    }


}
