using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GradinataNaBabaRatka.Models
{
    public class Review
    {
        public int Id { get; set; }



        [Display(Name = "App User")]
        [Required]
        [MaxLength(450)]
        public string AppUser { get; set; }



        [Display(Name = "Comment")]
        [Required]
        [MaxLength(500)]
        public string Comment { get; set; }



        [Display(Name = "Rating")]
        [Range(1, 10)]
        public int? Rating { get; set; }




        public int ProizvodId { get; set; }
        public Proizvod? Proizvod { get; set; }
        public Kupuvac Kupuvac { get; set; }
    }
}
