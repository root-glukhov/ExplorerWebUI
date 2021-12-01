using System;
using System.IO;
using System.Linq;

namespace ExplorerWebUI.Models
{
    public enum ProviderType
    {
        Directory,
        File
    } 

    public class ProviderModel
    {
        public string Name { get; set; }
        public ProviderType Type { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

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

        public static long GetDirectorySize(string folderPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            long dirSize = directoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(x => x.Length);
            return dirSize;
        }
    }
}
