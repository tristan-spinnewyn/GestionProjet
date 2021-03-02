using GestionDeProjet.DbContextImplementation.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Repository
{
    public class GenericRepository<Tentity> : IGenericRepository<Tentity> where Tentity : class
    {
        protected DbSet<Tentity> _dbSet;

        private DbConfig _dbContext;

        public GenericRepository(DbConfig dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Tentity>();
        }
        public void Add(Tentity entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(Tentity entity)
        {
            if(_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public List<Tentity> GetAll()
        {
            return _dbSet.ToList();
        }

        public Tentity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Detach(Tentity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
         public void Update(Tentity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
