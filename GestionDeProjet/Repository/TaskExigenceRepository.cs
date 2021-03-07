using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Repository
{
    public class TaskExigenceRepository : GenericRepository<TaskExigence>
    {
        public TaskExigenceRepository(DbConfig dbContext) : base(dbContext)
        {
        }

        public List<TaskExigence> GetByTask(int idTask)
        {
            return this._dbSet.Where(t => t.TaskId == idTask).ToList();
        }
    }
}
