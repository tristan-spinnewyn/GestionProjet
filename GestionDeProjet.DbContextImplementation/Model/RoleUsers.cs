using System;
using System.Collections.Generic;
using System.Text;

namespace GestionDeProjet.DbContextImplementation.Model
{
    public class RoleUsers
    {
        public int Id { get; set; }

        public string NameRole { get; set; }

        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
