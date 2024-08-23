using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppl10MoviesRazorPages.Data;
using WebAppl10MoviesRazorPages.Models;

namespace WebAppl10MoviesRazorPages.Pages
{
    public class EditModel : PageModel
    {
        private readonly MovieContext _context;

        public EditModel(MovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MovieViewModel MovieViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();  // Если фильм не найден, возвращаем 404
            }

            MovieViewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = movie.Director,
                Genre = movie.Genre,
                Year = movie.Year,
                PosterPath = movie.PosterPath,
                Description = movie.Description
            };

            return Page();  // Возвращаем страницу с данными
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("Начало выполнения OnPostAsync");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Модель не валидна");
                return Page();  // Если модель не валидна, возвращаем ту же страницу с ошибками
            }

            // Загружаем фильм из базы данных по идентификатору
            var movie = await _context.Movies.FindAsync(MovieViewModel.Id);
            if (movie == null)
            {
                Console.WriteLine($"Фильм с ID {MovieViewModel.Id} не найден");
                return NotFound();  // Если фильм не найден, возвращаем 404
            }

            Console.WriteLine($"Фильм найден: {movie.Title}");

            // Обновляем данные фильма
            movie.Title = MovieViewModel.Title;
            movie.Director = MovieViewModel.Director;
            movie.Genre = MovieViewModel.Genre;
            movie.Year = MovieViewModel.Year;
            movie.PosterPath = MovieViewModel.PosterPath;
            movie.Description = MovieViewModel.Description;

            Console.WriteLine("Данные фильма обновлены");

            // Обработка загруженного нового постера, если он есть
            if (MovieViewModel.Poster != null)
            {
                Console.WriteLine("Обработка загруженного постера");

                var fileName = Path.GetFileNameWithoutExtension(MovieViewModel.Poster.FileName);
                var extension = Path.GetExtension(MovieViewModel.Poster.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                var uploadsFolder = Path.Combine("wwwroot/images");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                Console.WriteLine($"Сохранение постера в {filePath}");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                    Console.WriteLine($"Создана папка: {uploadsFolder}");
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await MovieViewModel.Poster.CopyToAsync(stream);
                }

                movie.PosterPath = $"/images/{uniqueFileName}";
                Console.WriteLine("Путь постера обновлен");
            }

            // Обновляем запись в базе данных
            _context.Attach(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            Console.WriteLine("Изменения сохранены в базе данных");

            return RedirectToPage("./Index");  // После успешного сохранения перенаправляем на индексную страницу
        }
    }
}