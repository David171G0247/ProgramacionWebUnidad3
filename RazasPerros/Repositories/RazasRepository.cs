using System;
using System.Collections.Generic;
using System.Linq;
using RazasPerros.Models;
using RazasPerros.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace RazasPerros.Repositories
{
    public class RazasRepository : Repository<Razas>
    {
        public RazasRepository(sistem14_razasContext context) : base(context) { }
        public new IEnumerable<RazaViewModel> GetAll()
        {
            return Context.Razas.OrderBy(x => x.Nombre)
                .Select(x => new RazaViewModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                });
        }
        public IEnumerable<RazaViewModel> GetRazasByLetraInicial(string letra)
        {
            return GetAll().Where(x => x.Nombre.StartsWith(letra));
        }
        public IEnumerable<char> GetLetrasIniciales()
        {
            return Context.Razas.OrderBy(x => x.Nombre).Select(x => x.Nombre.First()).Distinct();
        }
        public Razas GetRazaByNombre(string nombre)
        {
            nombre = nombre.Replace("-", " ");
            return (Razas)Context.Razas
                .Include(x => x.Estadisticasraza)
                .Include(x => x.Caracteristicasfisicas)
                .Include(x => x.IdPaisNavigation)
                .Include(x => x.Nombre == nombre);
        }
        public IEnumerable<RazaViewModel> Get4RandomRazasExcept(string nombre)
        {
            nombre = nombre.Replace("-", " ");
            Random r = new Random();
            return Context.Razas
                .Where(x => x.Nombre != nombre).ToList()
                .OrderBy(x => r.Next())
                .Take(4)
                .Select(x => new RazaViewModel { Id = x.Id, Nombre = x.Nombre });
        }
        public IEnumerable<Paises> GetPaisesByPais()
        {
            return Context.Paises.Include(x => x.Razas).OrderBy(x => x.Nombre);
        }
        public IEnumerable<Paises> GetPaises()
        {
            return Context.Paises.OrderBy(x => x.Nombre);
        }
        public virtual bool Validate(Razas entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("Ingrese un nombre.");
            if (string.IsNullOrWhiteSpace(entidad.Descripcion))
                throw new Exception("Ingrese una descripción.");
            if (string.IsNullOrWhiteSpace(entidad.OtrosNombres))
                throw new Exception("Ingrese otro nombre de la raza.");
            if (!Context.Paises.Any(x => x.Id == entidad.IdPais && entidad.Eliminado == false))
                throw new Exception("No existe el país específicada.");
            if (entidad.PesoMin <= 0)
                throw new Exception("Ingrese un peso mínimo válido.");
            if (entidad.PesoMax <= 0)
                throw new Exception("Ingrese un peso máximo válido.");
            if (entidad.AlturaMin <= 0)
                throw new Exception("Ingrese una altura mínima válida.");
            if (entidad.AlturaMax <= 0)
                throw new Exception("Ingrese una altura máxima válida.");
            if (entidad.EsperanzaVida <= 0)
                throw new Exception("Ingrese una esperanza de vida válida.");
            if (Context.Razas.Any(x => x.Nombre == entidad.Nombre && x.Id != entidad.Id))
                throw new Exception("Ya existe una raza registrada con el mismo nombre.");
            return true;
        }
    }
}