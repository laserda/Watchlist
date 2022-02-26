using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Watchlist.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            this.Watchlist = new HashSet<UserMovie>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserMovie> Watchlist { get; set; }

    }
}
