using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Repository
{
    public class ProjectRepository : GenericRepository<Project>
    {
        public ProjectRepository(DbConfig db) : base(db)
        {
        }

        public List<Project> GetProjectAssign(int UserId)
        {
            return this._dbSet.Where(p => p.UserId == UserId).ToList();
        }
    }
}
