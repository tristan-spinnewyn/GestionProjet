using System;
using System.Collections.Generic;
using System.Text;

namespace GestionDeProjet.DbContextImplementation.Model
{
    public class User 
    {
        public int Id { get; set; }

        public string Trigramme { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public RoleUsers RoleUsers { get; set; }

        public int RoleUserId { get; set; }

        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public ICollection<Jalon> Jalons { get; set; } = new HashSet<Jalon>();

        public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();

    }
}
