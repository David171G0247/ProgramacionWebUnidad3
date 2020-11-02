using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZooPlanet.Models;
using ZooPlanet.Models.ViewModels;
using ZooPlanet.Repositories;

namespace ZooPlanet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdministradorController : Controller
    {
        public IWebHostEnvironment Environment { get; set; }
        public AdministradorController(IWebHostEnvironment env)
        {
            Environment = env;
        }
        public IActionResult Index()
        {
            animalesContext context = new animalesContext();
            EspeciesRepository o = new EspeciesRepository(context);
            return View(o.GetAll());
        }
        public IActionResult Agregar()
        {
            EspeciesViewModel vm = new EspeciesViewModel();
            animalesContext context = new animalesContext();
            ClasesRepository clasesRepos = new ClasesRepository(context);
            vm.Clases = clasesRepos.GetAll();
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(EspeciesViewModel vm)
        {
            animalesContext context = new animalesContext();
            try
            {
                ClasesRepository clasesRepos = new ClasesRepository(context);
                vm.Clases = clasesRepos.GetAll();
                EspeciesRepository especiesRepos = new EspeciesRepository(context);
                especiesRepos.Insert(vm.Especie);
                return RedirectToAction("Index", "Administrador");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ClasesRepository clasesRepos = new ClasesRepository(context);
                vm.Clases = clasesRepos.GetAll();
                return View(vm);
            }
        }
        public IActionResult Editar(int id)
        {
            animalesContext context = new animalesContext();
            EspeciesViewModel vm = new EspeciesViewModel();
            EspeciesRepository especiesRepos = new EspeciesRepository(context);
            vm.Especie = especiesRepos.GetById(id);
            if (vm.Especie == null)
            {
                return RedirectToAction("Index", "Administrador");
            }
            ClasesRepository clasesRepos = new ClasesRepository(context);
            vm.Clases = clasesRepos.GetAll();
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(EspeciesViewModel vm)
        {
            animalesContext context = new animalesContext();
            try
            {
                ClasesRepository clasesRepos = new ClasesRepository(context);
                vm.Clases = clasesRepos.GetAll();
                EspeciesRepository especiesRepos = new EspeciesRepository(context);
                var o = especiesRepos.GetById(vm.Especie.Id);
                if (o != null)
                {
                    o.Especie = vm.Especie.Especie;
                    o.IdClase = vm.Especie.IdClase;
                    o.Habitat = vm.Especie.Habitat;
                    o.Peso = vm.Especie.Peso;
                    o.Tamaño = vm.Especie.Tamaño;
                    o.Observaciones = vm.Especie.Observaciones;
                    especiesRepos.Update(o);
                }
                return RedirectToAction("Index", "Administrador");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ClasesRepository clasesRepos = new ClasesRepository(context);
                vm.Clases = clasesRepos.GetAll();
                return View(vm);
            }
        }
        public IActionResult Eliminar(int id)
        {
            using (animalesContext context = new animalesContext())
            {
                EspeciesRepository repos = new EspeciesRepository(context);
                var o = repos.GetById(id);
                if (o != null)
                {
                    return View(o);
                }
                else
                    return RedirectToAction("Index", "Administrador");
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Especies o)
        {
            using (animalesContext context = new animalesContext())
            {
                EspeciesRepository repos = new EspeciesRepository(context);
                var especie = repos.GetById(o.Id);
                if (especie != null)
                {
                    repos.Delete(especie);
                    return RedirectToAction("Index", "Administrador");
                }
                else
                {
                    ModelState.AddModelError("", "El animal no existe o ya ha sido eliminado.");
                    return View(o);
                }
            }
        }
        public IActionResult Imagen(int id)
        {
            animalesContext context = new animalesContext();
            EspeciesViewModel vm = new EspeciesViewModel();
            EspeciesRepository repos = new EspeciesRepository(context);
            vm.Especie = repos.GetById(id);
            if (System.IO.File.Exists(Environment.WebRootPath + "/especies/" + vm.Especie.Id + ".jpg"))
            {
                vm.Imagen = vm.Especie.Id + ".jpg";
            }
            else
            {
                vm.Imagen = "nophoto.jpg";
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult Imagen(EspeciesViewModel vm)
        {
            try
            {
                if (vm.Archivo == null)
                {
                    ModelState.AddModelError("", "Seleccione una imagen de la especie.");
                    return View(vm);
                }
                else
                {
                    if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un archivo tipo .jpg menor de 2MB.");
                        return View(vm);
                    }
                }
                if (vm.Archivo != null)
                {
                    FileStream fs = new FileStream
                        (Environment.WebRootPath + "/especies/" + vm.Especie.Id + ".jpg", FileMode.Create);
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index", "Administrador");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }
    }
}