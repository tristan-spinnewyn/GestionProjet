using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Repository
{
    public class ExigenceRepository : GenericRepository<Exigence>
    {
        public ExigenceRepository(DbConfig dbContext) : base(dbContext)
        {

        }

        public List<Exigence> GetExigencesForProject(int id)
        {
            return this._dbSet.Where(e => e.ProjectId == id).ToList();
        }

    }
}
