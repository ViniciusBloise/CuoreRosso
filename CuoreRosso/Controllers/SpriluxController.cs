using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuoreRosso.Filters;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CuoreRosso.Controllers
{
    [ActionFilter]
    public class SpriluxController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewData["Title"] = "Sprilux";
            Console.Write(ViewData["Title"]);
            return View();
        }
    }
}
