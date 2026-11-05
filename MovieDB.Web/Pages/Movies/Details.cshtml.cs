using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using MovieDB.Entities;

namespace MovieDB.Web.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly MovieDbContext _context;

        public DetailsModel(MovieDbContext context)
        {
            _context = context;
        }

        public Movie Movie { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Studio)
                    .ThenInclude(s => s.Country)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            Movie = movie;

            return Page();
        }
    }
}