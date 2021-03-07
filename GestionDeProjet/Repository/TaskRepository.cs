using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = GestionDeProjet.DbContextImplementation.Model.Task;

namespace GestionDeProjet.Repository
{
    public class TaskRepository : GenericRepository<Task>
    {
        public TaskRepository(DbConfig dbContext) : base(dbContext)
        {

        }

        public List<Task> GetTaskForJalon(int id)
        {
            return this._dbSet.Where(e => e.JalonId == id).ToList();
        }

        public Task getTaskForUser(int jalonId,int userID)
        {
            return this._dbSet.Where(e => (e.JalonId == jalonId) && (e.UserId == userID)).First();

        }
    }
}
