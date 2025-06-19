using System;
using System.ComponentModel.DataAnnotations;

namespace BasketForum.Models
{
    public class Kommentar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime Skapad { get; set; } = DateTime.Now;
        // Koppling till användare
        public int UserId { get; set; }
        public string Anvandarnamn { get; set; } // redundans för enkelhet

        // Koppling till inlägg
        public int InlaggId { get; set; }
    }
}
