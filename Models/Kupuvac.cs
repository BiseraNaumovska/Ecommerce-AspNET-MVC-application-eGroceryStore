using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GradinataNaBabaRatka.Models
{
    public class Kupuvac
    {
        public int Id { get; set; }

        [Display(Name = "App User")]
        [Required]
        [MaxLength(450)]
        public string AppUser { get; set; }


        public int ProizvodId { get; set; }
        public Proizvod? Proizvod { get; set; }
    }
}
