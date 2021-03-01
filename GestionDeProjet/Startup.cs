using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using GestionDeProjet.DbContextImplementation;
using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using Microsoft.AspNetCore.Identity;

namespace GestionDeProjet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddMvc();

            Tools.AddDefaultConfig(services);

            
            InsertData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void InsertData()
        {
      
            using (var context = new DbConfig())
            {
                if(context.Database.EnsureCreated())
                {
                    context.RoleUsers.Add(new RoleUsers
                    {
                        NameRole = "Chef de projet"
                    });

                    context.RoleUsers.Add(new RoleUsers
                    {
                        NameRole = "Responsable de projet"
                    });

                    context.RoleUsers.Add(new RoleUsers
                    {
                        NameRole = "Développeur"
                    });

                    context.User.Add(new User
                    {
                        Trigramme = "TSp",
                        Firstname = "Tristan",
                        Lastname = "Spinnewyn",
                        Email = "tspinnewyn@esimed.fr",
                        RoleUserId = 1,
                        Password = new PasswordHasher<object?>().HashPassword(null,"admin")
                    });
                    context.User.Add(new User
                    {
                        Trigramme = "TSp",
                        Firstname = "Tristan",
                        Lastname = "Spinnewyn",
                        Email = "tspinnewyn2@esimed.fr",
                        RoleUserId = 2,
                        Password = new PasswordHasher<object?>().HashPassword(null, "admin")
                    });

                    context.SaveChanges();
                }
               
            }
        }
    }
}
