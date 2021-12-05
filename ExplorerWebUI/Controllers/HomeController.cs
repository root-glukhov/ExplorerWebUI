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
        public IActionResult Index(string path)
        {
            List<ProviderViewModel> providerViewModels = path == null ? ProviderViewModel.GetDrives() : ProviderViewModel.GetDirectoryFiles(path);
            return View(providerViewModels);
        }

        //[HttpPost]
        //public IActionResult Index(string path)
        //{
        //    List<ProviderViewModel> providerViewModels = path == null ? ProviderViewModel.GetDrives() : ProviderViewModel.GetDirectoryFiles(path);
        //    return PartialView("_GetProvider", providerViewModels);
        //}
    }
}
