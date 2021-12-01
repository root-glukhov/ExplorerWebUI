using ExplorerWebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorerWebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFileProvider _fileProvider;

        public List<ProviderModel> providerModel { get; private set; }

        public IndexModel(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
            providerModel = new List<ProviderModel>();
        }

        public void OnGet()
        {
            IDirectoryContents contents = _fileProvider.GetDirectoryContents("");

            foreach (IFileInfo item in contents)
            {
                providerModel.Add(new ProviderModel() {
                    Name = item.Name,
                    Type = item.IsDirectory ? ProviderType.Directory : ProviderType.File,
                    Size = item.IsDirectory ? ProviderModel.GetDirectorySize(@item.PhysicalPath) : item.Length
                });
            }
        }
    }
}
