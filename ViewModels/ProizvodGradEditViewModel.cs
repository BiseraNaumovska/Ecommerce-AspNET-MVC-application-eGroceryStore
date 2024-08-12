using GradinataNaBabaRatka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GradinataNaBabaRatka.ViewModels
{
    public class ProizvodGradEditViewModel
    {
        public Proizvod Proizvod { get; set; }

        public IEnumerable<int>? SelectedGrads { get; set; }
        public IEnumerable<SelectListItem>? GradList { get; set; }
    }
}
