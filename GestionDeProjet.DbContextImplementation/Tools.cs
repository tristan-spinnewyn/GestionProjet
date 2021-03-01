using GestionDeProjet.DbContextImplementation.DataContext;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GestionDeProjet.DbContextImplementation
{
    public class Tools
    {
        public static IServiceCollection AddDefaultConfig(IServiceCollection container)
        {

            //On ajoute la config de bdd ect....
            container.AddDbContext<DbConfig>();
            
            return container;
        }
    }
}
