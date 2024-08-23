using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebAppl10MoviesRazorPages.Data;
using WebAppl10MoviesRazorPages.Models;

namespace WebAppl10MoviesRazorPages.Pages
{
    public class CreateModel : PageModel
    {
        private readonly MovieContext _context;

        public CreateModel(MovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MovieViewModel MovieViewModel { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

           
            var movie = await _context.Movies.FindAsync(MovieViewModel.Id);
            if (movie == null)
            {
                return NotFound();  
            }

           
            movie.Title = MovieViewModel.Title;
            movie.Director = MovieViewModel.Director;
            movie.Genre = MovieViewModel.Genre;
            movie.Year = MovieViewModel.Year;
            movie.Description = MovieViewModel.Description;

            
            if (MovieViewModel.Poster != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(MovieViewModel.Poster.FileName);
                var extension = Path.GetExtension(MovieViewModel.Poster.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                var uploadsFolder = Path.Combine("wwwroot/images");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await MovieViewModel.Poster.CopyToAsync(stream);
                }

                
                movie.PosterPath = $"/images/{uniqueFileName}";
            }

            
            _context.Attach(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}