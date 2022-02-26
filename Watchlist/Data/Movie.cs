using System.ComponentModel.DataAnnotations;

namespace Watchlist.Data
{
    public class Movie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "MissingTitle")]
        public string Title { get; set; }
        [Required(ErrorMessage = "MissingYear")]
        public int Year { get; set; }
    }
}
