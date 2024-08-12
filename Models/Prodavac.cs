using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GradinataNaBabaRatka.Models
{
    public class Prodavac
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }



        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }



        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }



        [MaxLength(50)]
        public string? Nationality { get; set; }


        [MaxLength(50)]
        public string? Gender { get; set; }



        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public ICollection<Proizvod>? Proizvods { get; set; }
        public string Grad { get; internal set; }
    }
}
