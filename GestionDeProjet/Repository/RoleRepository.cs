using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Repository
{
    public class RoleRepository : GenericRepository<RoleUsers>
    {
        public RoleRepository(DbConfig dbContext) : base(dbContext)
        {
            
        }
    }
}
