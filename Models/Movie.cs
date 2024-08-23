using System.ComponentModel.DataAnnotations;

namespace WebAppl10MoviesRazorPages.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Director { get; set; }

        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [Required]
        [Range(1888, 2100)]
        public int Year { get; set; }


        public string PosterPath { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}

