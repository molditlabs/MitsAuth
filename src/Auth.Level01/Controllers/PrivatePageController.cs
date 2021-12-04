using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Level01.Controllers
{
    [Authorize]
    public class PrivatePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
