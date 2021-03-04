using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Repository
{
    public class TypeExigenceRepository : GenericRepository<TypeExigence>
    {
        public TypeExigenceRepository(DbConfig dbContext) : base(dbContext)
        {

        }

    }
}
