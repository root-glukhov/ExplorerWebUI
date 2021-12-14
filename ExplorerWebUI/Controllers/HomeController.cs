using ExplorerWebUI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExplorerWebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(ExplorerViewModel.GetDirectory());
        }

        [HttpPost]
        public IActionResult GetDirectory(string path)
        {
            return PartialView("_Explorer", ExplorerViewModel.GetDirectory(path));
        }
    }
}