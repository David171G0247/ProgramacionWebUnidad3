using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FruitStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FruitStore.Models;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FruitStore.Controllers
{
    public class ProductosController : Controller
    {
        public IHostingEnvironment Environment { get; set; }

        public ProductosController(IHostingEnvironment env)
        {
            Environment = env;
        }

        [Route("Productos")]
        public IActionResult Index()
        {
            ProductosIndexViewModel vm = new ProductosIndexViewModel();
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository categoriasRepos = new CategoriasRepository(context);
            ProductosRepository productosRepos = new ProductosRepository(context);
            int? id = null;
            vm.Categorias = categoriasRepos.GetAll();
            vm.Productos = productosRepos.GetProductosByCategoria(id);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Index(ProductosIndexViewModel vm)
        {
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository categoriasRepos = new CategoriasRepository(context);
            ProductosRepository productosRepos = new ProductosRepository(context);
            vm.Categorias = categoriasRepos.GetAll();
            vm.Productos = productosRepos.GetProductosByCategoria(vm.IdCategoria);
            return View(vm);
        }
        public IActionResult Agregar()
        {
            ProductosViewModel vm = new ProductosViewModel();
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository categoriasRepos = new CategoriasRepository(context);
            vm.Categorias = categoriasRepos.GetAll();
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(ProductosViewModel vm)
        {
            fruteriashopContext context = new fruteriashopContext();
            if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
            {
                ModelState.AddModelError("", "Debe seleccionar un archivo tipo .jpg menor de 2MB.");
                CategoriasRepository categoriasRepos = new CategoriasRepository(context);
                vm.Categorias = categoriasRepos.GetAll();
                return View(vm);
            }
            try
            {
                ProductosRepository repos = new ProductosRepository(context);
                repos.Insert(vm.Producto);
                // Guardar archivo
                FileStream fs = new FileStream
                    (Environment.WebRootPath + "/img_frutas/" + vm.Producto.Id + ".jpg", FileMode.Create);
                vm.Archivo.CopyTo(fs);
                fs.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CategoriasRepository categoriasRepos = new CategoriasRepository(context);
                vm.Categorias = categoriasRepos.GetAll();
                return View(vm);
            }

        }
        public IActionResult Editar(int id)
        {
            fruteriashopContext context = new fruteriashopContext();
            ProductosViewModel vm = new ProductosViewModel();
            ProductosRepository productosRepos = new ProductosRepository(context);
            vm.Producto = productosRepos.Get(id);
            if (vm.Producto == null)
            {
                return RedirectToAction("Index");
            }
            CategoriasRepository categoriasRepos = new CategoriasRepository(context);
            vm.Categorias = categoriasRepos.GetAll();
            if (System.IO.File.Exists(Environment.WebRootPath + "/img_frutas/" + vm.Producto.Id + ".jpg"))
            {
                vm.Imagen = vm.Producto.Id + ".jpg";
            }
            else
            {
                vm.Imagen = "no-disponible.png";
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(ProductosViewModel vm)
        {
            fruteriashopContext context = new fruteriashopContext();
            if (vm.Archivo != null)
            {
                if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                {
                    ModelState.AddModelError("", "Debe seleccionar un archivo tipo .jpg menor de 2MB.");
                    CategoriasRepository categoriasRepos = new CategoriasRepository(context);
                    vm.Categorias = categoriasRepos.GetAll();
                    return View(vm);
                }
            }
            try
            {
                ProductosRepository repos = new ProductosRepository(context);
                var p = repos.Get(vm.Producto.Id);
                if (p != null)
                {
                    p.Nombre = vm.Producto.Nombre;
                    p.IdCategoria = vm.Producto.IdCategoria;
                    p.Precio = vm.Producto.Precio;
                    p.UnidadMedida = vm.Producto.UnidadMedida;
                    p.Descripcion = vm.Producto.Descripcion;
                    repos.Update(p);
                }
                // Sobreescribir archivo
                if (vm.Archivo != null)
                {
                    FileStream fs = new FileStream(Environment.WebRootPath + "/img_frutas/" + vm.Producto.Id + ".jpg", FileMode.Create);
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CategoriasRepository categoriasRepos = new CategoriasRepository(context);
                vm.Categorias = categoriasRepos.GetAll();
                return View(vm);
            }
        }
        public IActionResult Eliminar(int id)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                var p = repos.Get(id);
                if (p != null)
                {
                    return View(p);
                }
                else
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Productos p)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                var producto = repos.Get(p.Id);
                if (producto != null)
                {
                    repos.Delete(producto);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "El producto no existe o ya ha sido eliminado.");
                    return View(p);
                }
            }
        }
    }
}