using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorerWebUI.Models
{
    public class ExplorerViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public bool isDirectory { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public static IEnumerable<ExplorerViewModel> GetDirectory(string path = "")
        {
            List<ExplorerViewModel> providerViewModels = new List<ExplorerViewModel>();

            if (path == "")
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    providerViewModels.Add(new ExplorerViewModel()
                    {
                        Name = drive.Name,
                        isDirectory = true,
                        Path = drive.Name,
                        Size = drive.TotalSize - drive.TotalFreeSpace
                    });
                }
                return providerViewModels.OrderByDescending(x => x.Size);
            }

            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var dirInfo = new DirectoryInfo(dir);
                if ((dirInfo.Attributes & FileAttributes.Hidden) > 0)
                    continue;

                providerViewModels.Add(new ExplorerViewModel()
                {
                    Name = dirInfo.Name,
                    isDirectory = true,
                    Path = dir
                }) ;
            }

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                providerViewModels.Add(new ExplorerViewModel()
                {
                    Name = fileInfo.Name,
                    isDirectory = false,
                    Path = file,
                    Size = file.Length
                });
            }
            return providerViewModels.OrderByDescending(x => x.Size);
        }

        public static IEnumerable<FileInfo> SafeEnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            Stack<DirectoryInfo> dirs = new Stack<DirectoryInfo>();
            dirs.Push(new DirectoryInfo(path));

            while (dirs.Count > 0)
            {
                DirectoryInfo currentDirInfo = dirs.Pop();
                if (searchOption == SearchOption.AllDirectories)
                {
                    try
                    {
                        foreach (DirectoryInfo subDirInfo in currentDirInfo.EnumerateDirectories())
                        {
                            dirs.Push(subDirInfo);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                FileInfo[] files = null;
                try
                {
                    files = currentDirInfo.GetFiles(searchPattern);
                }
                catch
                {
                    continue;
                }

                foreach (FileInfo fileInfo in files)
                {
                    yield return fileInfo;
                }
            }
        }
    }
}