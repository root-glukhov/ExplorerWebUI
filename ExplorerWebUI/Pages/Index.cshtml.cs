using ExplorerWebUI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;

namespace ExplorerWebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileProvider _fileProvider;

        public List<ProviderModel> providerModel { get; private set; }

        public IndexModel(IWebHostEnvironment env, IFileProvider fileProvider)
        {
            _env = env;
            _fileProvider = fileProvider;
            providerModel = new List<ProviderModel>();
        }

        public void OnGet(string path = "")
        {
            IDirectoryContents contents = _fileProvider.GetDirectoryContents(path);

            foreach (IFileInfo item in contents)
            {
                providerModel.Add(new ProviderModel() {
                    Name = item.Name,
                    Type = item.IsDirectory ? ProviderType.Directory : ProviderType.File,
                    Path = item.PhysicalPath.Replace(_env.ContentRootPath, String.Empty),
                    Size = item.IsDirectory ? ProviderModel.GetDirectorySize(@item.PhysicalPath) : item.Length
                });
            }
        }
    }
}
