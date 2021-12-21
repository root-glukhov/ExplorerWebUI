using ExplorerWebUI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorerWebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string path = "")
        {
            return View(ExplorerViewModel.GetDirectory(path));
        }

        [HttpPost]
        public long GetSize(string path)
        {
            return ExplorerViewModel.SafeEnumerateFiles(path, "*.*", SearchOption.AllDirectories).Sum(x => x.Length);
        }
    }
}