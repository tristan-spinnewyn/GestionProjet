using System;
using System.Collections.Generic;
using System.Text;

namespace GestionDeProjet.DbContextImplementation.Model
{
    public class TypeExigence
    {
        public int Id { get; set; }

        public string NameExigence { get; set; }
        
        public ICollection<Exigence> Exigences { get; set; } = new HashSet<Exigence>();

    }
}
