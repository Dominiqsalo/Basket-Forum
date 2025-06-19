using System.Collections.Generic;
namespace BasketForum.Models
{
    public class UserProfileViewModel
    {
        public Users User { get; set; }
        public int AntalInlagg { get; set; }
        public int AntalKommentarer { get; set; }

        public List<Kommentar> Kommentarer { get; set; } = new List<Kommentar>();
    }
}
