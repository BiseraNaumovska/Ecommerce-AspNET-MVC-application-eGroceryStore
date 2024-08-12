using GradinataNaBabaRatka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GradinataNaBabaRatka.ViewModels
{
    public class ProizvodImeGradSearchViewModel
    {
        public IList<Proizvod> Proizvods { get; set; }
        public SelectList Grads { get; set; }
        public string ProizvodGrad { get; set; }
        public string SearchString { get; set; }
    }
}
