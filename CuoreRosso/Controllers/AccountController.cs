using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuoreRosso.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CuoreRosso.Controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AccountViewModel model)
        {
            if(ModelState.IsValid)
            {
               // Console.Write(Thread.CurrentThread.CurrentUICulture);

            }
            return View(model);
        }
    }
}
