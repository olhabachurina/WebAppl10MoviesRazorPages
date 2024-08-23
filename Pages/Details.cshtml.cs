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
    public class DetailsModel : PageModel
    {
        private readonly MovieContext _context;

        public DetailsModel(MovieContext context)
        {
            _context = context;
        }

        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Movie = await _context.Movies.FindAsync(id);

            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}