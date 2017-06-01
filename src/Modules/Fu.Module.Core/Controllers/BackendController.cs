using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Fu.Module.Core.Controllers
{
    [Route("Backend/[controller]/[action]/{id?}")]
    [Authorize(Roles = ("Administrator"))]
    public abstract class BackendController : Controller
    { 
        public BackendController()
        {

        }
    }
}
