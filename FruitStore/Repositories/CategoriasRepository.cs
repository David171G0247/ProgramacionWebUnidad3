using FruitStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FruitStore.Repositories
{
    public class CategoriasRepository : Repository<Categorias>
    {
        public CategoriasRepository(fruteriashopContext context) : base(context)
        {

        }
        public override bool Validate(Categorias entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("No escribió el nombre de la categoría.");
            if (Context.Categorias.Any(x => x.Nombre == entidad.Nombre && x.Id != entidad.Id))
                throw new Exception("Ya existe una categoría con el mismo nombre.");
            return true;
        }
    }
}
