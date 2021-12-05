using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorerWebUI.Models
{
    public enum ProviderType
    {
        Directory,
        File
    } 

    public class ProviderViewModel
    {
        public string Name { get; set; }
        public ProviderType Type { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public static List<ProviderViewModel> GetDrives()
        {
            List<ProviderViewModel> drives = new List<ProviderViewModel>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                drives.Add(new ProviderViewModel()
                {
                    Name = drive.Name,
                    Type = ProviderType.Directory,
                    Path = drive.Name,
                    Size = drive.TotalSize - drive.TotalFreeSpace
                });
            }
            return drives.OrderByDescending(x => x.Size).ToList();
        }

        public static List<ProviderViewModel> GetDirectoryFiles(string path)
        {
            List<ProviderViewModel> providerViewModels = new List<ProviderViewModel>();

            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var dirInfo = new DirectoryInfo(dir);
                if ((dirInfo.Attributes & FileAttributes.Hidden) > 0)
                    continue;

                providerViewModels.Add(new ProviderViewModel()
                {
                    Name = dirInfo.Name,
                    Type = ProviderType.Directory,
                    Path = dir,
                    Size = ProviderViewModel.GetDirectorySize(dir)
                });
            }

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                providerViewModels.Add(new ProviderViewModel()
                {
                    Name = fileInfo.Name,
                    Type = ProviderType.File,
                    Path = file,
                    Size = file.Length
                });
            }
            return providerViewModels.OrderByDescending(x => x.Size).ToList();
        }

        public static string BytesToString(long bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024;
            }

            return String.Format("{0:0.##} {1}", dblSByte, suffix[i]);
        }

        public static long GetDirectorySize(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return SafeEnumerateFiles(directoryInfo, "*.*", SearchOption.AllDirectories).Sum(x => x.Length);
        }

        private static IEnumerable<FileInfo> SafeEnumerateFiles(DirectoryInfo dirInfo, string searchPattern, SearchOption searchOption)
        {
            Stack<DirectoryInfo> dirs = new Stack<DirectoryInfo>();
            dirs.Push(dirInfo);

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
