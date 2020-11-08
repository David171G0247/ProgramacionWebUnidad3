using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazasPerros.Models.ViewModels
{
    public class InfoPerroViewModel
    {
        public Razas Raza { get; set; }
        public IEnumerable<RazaViewModel> OtrasRazas { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
        public IEnumerable<RazaViewModel> Razas { get; set; }
        public IFormFile Archivo { get; set; }
        public string Imagen { get; set; }
    }
}