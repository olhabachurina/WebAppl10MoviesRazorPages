using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebAppl10MoviesRazorPages.Models;

namespace WebAppl10MoviesRazorPages.Data
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }
    }
}
