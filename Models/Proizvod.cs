using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GradinataNaBabaRatka.Models
{
    public class Proizvod
    {
        public int Id { get; set; }


        [MaxLength(100)]
        [Display(Name = "Ime na proizvod")]
        [Required]
        public string Ime { get; set; }


        [Display(Name = "Data na proizvodstvo")]
        public int? Data { get; set; }


        [Display(Name = "Zaliha")]
        [Required]
        public int? Zaliha { get; set; }



        [Display(Name = "Opis na proizvod")]
        [Required]
        public string? Opis { get; set; }


        [Display(Name = "Slika")]
        [Required]
        public string? Slika { get; set; }


        [Display(Name = "Cena")]
        [Required]
        public int? Cena { get; set; }



        [Display(Name = "ProdavacId")]
        public int ProdavacId { get; set; }
        public Prodavac? Prodavac { get; set; }


        public ICollection<ProizvodGrad>? ProizvodGrads { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        public ICollection<Kupuvac>? Kupuvacs { get; set; }

    }
}
