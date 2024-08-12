using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GradinataNaBabaRatka.Models
{
    public class Grad
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Grad")]
        public string GradIme { get; set; }

        public ICollection<ProizvodGrad>? ProizvodGrads { get; set; }
    }
}
