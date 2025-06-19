using System.Collections.Generic;

namespace BasketForum.Models
{
    public class StartSidaViewModel
    {
        public Dictionary<string, Inlagg> SenasteInlagg { get; set; }
        public Dictionary<string, int> InlaggPerKategori { get; set; }
    }
}
