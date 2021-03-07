using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Repository
{
    public class JalonRepository : GenericRepository<Jalon>
    {
        public JalonRepository(DbConfig dbContext) : base(dbContext)
        {

        }

        public List<Jalon> GetJalonForProject(int id)
        {
            return this._dbSet.Where(e => e.ProjectId == id).ToList();
        }

        public Jalon getForUser(int projectId,int userId)
        {
            return this._dbSet.Where(e => (e.ProjectId == projectId) && (e.UserId == userId)).First();

        }
    }
}
