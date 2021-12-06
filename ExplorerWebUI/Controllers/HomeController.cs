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
            List<ProviderViewModel> providerViewModels = ProviderViewModel.GetDirectory(path);
            return View(providerViewModels);
        }
    }
}
