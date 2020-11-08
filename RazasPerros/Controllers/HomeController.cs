﻿using Microsoft.AspNetCore.Mvc;
using RazasPerros.Models;
using RazasPerros.Models.ViewModels;
using RazasPerros.Repositories;

namespace RazasPerros.Controllers
{
    public class HomeController : Controller
    {
        sistem14_razasContext context = new sistem14_razasContext();
        public IActionResult Index(string id)
        {
            RazasRepository repos = new RazasRepository();
            IndexViewModel vm = new IndexViewModel
            {
                Razas = id == null ? repos.GetRazas() : repos.GetRazasByLetraInicial(id),
                LetrasIniciales = repos.GetLetrasIniciales()
            };
            return View(vm);
        }
        [Route("Raza/{id}")]
        public IActionResult InfoPerros(string id)
        {
            RazasRepository repos = new RazasRepository();
            InfoPerroViewModel vm = new InfoPerroViewModel();
            vm.Raza = repos.GetRazaByNombre(id);
            if (vm.Raza == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                vm.OtrasRazas = repos.Get4RandomRazasExcept(id);
                return View(vm);
            }
        }
        [Route("RazasPorPais")]
        public IActionResult RazasPorPais()
        {
            RazasRepository repos = new RazasRepository();
            RazasPorPaisViewModel vm = new RazasPorPaisViewModel();
            var paises = context.Paises.Include(x => x.Razas).OrderBy(x => x.Nombre)
                .Select(x => new RazasPorPaisViewModel { Pais=x.Nombre, Razas= x.Razas });
            return View(paises);
        }
    }
}
