using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppl10MoviesRazorPages.Data;
using WebAppl10MoviesRazorPages.Models;

namespace WebAppl10MoviesRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MovieContext _context;

        public IndexModel(MovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public string SearchTitle { get; set; }
        public string SearchYear { get; set; }
        public string SearchGenre { get; set; }

        public async Task OnGetAsync(string searchTitle, string searchYear, string searchGenre)
        {
            var movies = from m in _context.Movies select m;

            if (!string.IsNullOrEmpty(searchTitle))
            {
                movies = movies.Where(s => s.Title.Contains(searchTitle));
            }

            if (!string.IsNullOrEmpty(searchYear))
            {
                movies = movies.Where(s => s.Year.ToString() == searchYear);
            }

            if (!string.IsNullOrEmpty(searchGenre))
            {
                movies = movies.Where(s => s.Genre == searchGenre);
            }

            Movie = await movies.ToListAsync();
            Genres = await _context.Movies.Select(m => m.Genre).Distinct().ToListAsync();

            SearchTitle = searchTitle;
            SearchYear = searchYear;
            SearchGenre = searchGenre;
        }
    }
}