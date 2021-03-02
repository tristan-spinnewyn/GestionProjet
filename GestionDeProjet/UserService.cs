using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using GestionDeProjet.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProjet
{
    public interface IUserService
    {
        User Auth(string login, string password,DbConfig dbConfig);
    }
    public class UserService : IUserService
    {
        private Parametres Parametres { get; }

        public User Auth(string login,string password,DbConfig DbConfig)
        {
            User ?user = DbConfig.User.FirstOrDefault(u => u.Email == login);
            JwtSecurityTokenHandler tokenHandler;
            SecurityTokenDescriptor tokenDescriptor;
            SecurityToken token;
            byte[] key;

            if(user == null)
            {
                return null;
            }

            var ph = new PasswordHasher<object?>();

            var res = ph.VerifyHashedPassword(user, user.Password, password);
            if (res == PasswordVerificationResult.Failed)
            {
                return null;
            }

            tokenHandler = new JwtSecurityTokenHandler();
            key = Encoding.ASCII.GetBytes(this.Parametres.Cle);
            tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("roleId", user.RoleUserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;

            return user;
        }

        public UserService(IOptions<Parametres> appSettings)
        {
            this.Parametres = appSettings.Value;
        }
    }
}
