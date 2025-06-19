using System.ComponentModel.DataAnnotations;
using System;

namespace BasketForum.Models
{
    public class Inlagg
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Titel är obligatoriskt.")]
        public string titel { get; set; }

        [Required(ErrorMessage = "Ämne är obligatoriskt.")]
        public string amne { get; set; }

        [Required(ErrorMessage = "Innehåll är obligatoriskt.")]
        public string innehall { get; set; }

        [Required(ErrorMessage = "Kategori är obligatoriskt.")]
        public string kategori { get; set; }

        public string? skapare { get; set; } 

        public DateTime skapad { get; set; } = DateTime.Now;
    }
}
