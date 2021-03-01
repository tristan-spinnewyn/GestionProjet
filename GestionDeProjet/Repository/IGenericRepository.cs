using System.Collections.Generic;

namespace GestionDeProjet.Repository
{
    public interface IGenericRepository<Tentity>
    {
        public List<Tentity> GetAll();

        public void Add(Tentity entity);

        public void Update(Tentity entity);

        public void Delete(Tentity entity);

        public Tentity GetById(int id);
    }
}