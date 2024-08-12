using GradinataNaBabaRatka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GradinataNaBabaRatka.ViewModels
{
    public class ProdavacNameSurnameNationality
    {
        public IList<Prodavac> Prodavacs { get; set; }
        public SelectList Nationalities { get; set; }
        public string ProdavacNationality { get; set; }
        public string SearchStringName { get; set; }

        public string SearchStringSurname { get; set; }
    }
}
