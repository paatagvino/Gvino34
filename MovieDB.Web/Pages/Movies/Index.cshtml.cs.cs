using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using MovieDB.Entities;

namespace MovieDB.Web.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly MovieDbContext _context;

        public IndexModel(MovieDbContext context)
        {
            _context = context;
        }

        public List<Movie> Movies { get; set; } = new();

        public async Task OnGetAsync()
        {
            Movies = await _context.Movies
                .Include(m => m.Studio)
                .Include(m => m.Actors)
                .OrderBy(m => m.Title)
                .ToListAsync();
        }
    }
}