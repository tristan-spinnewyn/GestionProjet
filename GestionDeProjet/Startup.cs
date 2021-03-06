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
using GestionDeProjet.Properties;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
            IConfigurationSection section;
            byte[] cle;
            Parametres listeParametres;

            // Obtention des paramètres de l'application.
            section = Configuration.GetSection(nameof(Parametres));
            services.Configure<Parametres>(section);
            listeParametres = section.Get<Parametres>();
            cle = Encoding.ASCII.GetBytes(listeParametres.Cle);

            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
               {
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                   x.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(cle),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });

            // Activation des services.
            services.AddCors();
            services.AddMvc();
            services.AddScoped<IUserService, UserService>();

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


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configuration CORS.
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Activation de l'authentification.
            app.UseAuthentication();

            app.UseAuthorization();

            // Accès aux ressources statiques.
            app.UseStaticFiles();

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

                    context.TypeExigences.Add(new TypeExigence
                    {
                        NameExigence = "Données"
                    });

                    context.TypeExigences.Add(new TypeExigence
                    {
                        NameExigence = "Performances"
                    });

                    context.TypeExigences.Add(new TypeExigence
                    {
                        NameExigence = "Interface utilisateurs"
                    });

                    context.TypeExigences.Add(new TypeExigence
                    {
                        NameExigence = "Qualités"
                    });

                    context.TypeExigences.Add(new TypeExigence
                    {
                        NameExigence = "Services"
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
                        Trigramme = "TS2",
                        Firstname = "Tristan",
                        Lastname = "Spinnewyn",
                        Email = "tspinnewyn2@esimed.fr",
                        RoleUserId = 2,
                        Password = new PasswordHasher<object?>().HashPassword(null, "admin")
                    });

                    context.User.Add(new User
                    {
                        Trigramme = "TS3",
                        Firstname = "Tristan",
                        Lastname = "Spinnewyn",
                        Email = "tspinnewyn3@esimed.fr",
                        RoleUserId = 2,
                        Password = new PasswordHasher<object?>().HashPassword(null, "admin")
                    });

                    context.SaveChanges();
                }
               
            }
        }
    }
}
