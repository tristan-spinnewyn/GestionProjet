using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionDeProjet.Controllers
{
    public class AbstractController : ControllerBase
    {
        protected bool IsChief()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity.FindFirst("roleId").Value == "1")
            {
                return true;
            }
            return false;
        }

        protected int GetId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                return Int32.Parse(identity.FindFirst("id").Value);
            }

            throw new Exception();
        }
    }
}
