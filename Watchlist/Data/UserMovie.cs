namespace Watchlist.Data
{
    public class UserMovie
    {
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public bool Watched { get; set; }
        public int Rating { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
