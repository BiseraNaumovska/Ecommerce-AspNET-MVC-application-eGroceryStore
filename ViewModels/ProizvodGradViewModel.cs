using GradinataNaBabaRatka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GradinataNaBabaRatka.ViewModels
{
    public class ProizvodGradViewModel
    {
        public IList<Proizvod>? Proizvods { get; set; }
        public SelectList? Grads { get; set; }
        public string ProizvodGradString { get; set; }
        public string? SearchString { get; set; }
    }
}
