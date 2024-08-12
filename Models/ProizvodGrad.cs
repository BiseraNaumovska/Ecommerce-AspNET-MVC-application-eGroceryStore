namespace GradinataNaBabaRatka.Models
{
    public class ProizvodGrad
    {
        public int Id { get; set; }

        public int ProizvodId { get; set; }
        public Proizvod? Proizvod { get; set; }
        public int GradId { get; set; }
        public Grad? Grad { get; set; }
    }
}
