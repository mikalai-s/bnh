using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Bnh.ViewModels
{
    public class FilesViewModel
    {
        public string CurrentPath { get; set; }

        public FileViewModel[] Files { get; set; }

        public FileViewModel[] BreadCrumbs { get; set; }
        
        public FilesViewModel(string relativePath, string fullPath, string uploadsFolder)
        {
            this.CurrentPath = relativePath;

            if (Directory.Exists(fullPath))
            {
                Files = EnumerateFolders(fullPath, relativePath)
                    .Union(EnumerateFiles(fullPath, relativePath, uploadsFolder))
                    .ToArray();

                BreadCrumbs = EnumerateBreadCrumbs(relativePath, fullPath).Reverse().ToArray();
            }
        }

        private static IEnumerable<FileViewModel> EnumerateBreadCrumbs(string relativePath, string fullPath)
        {
            var di = new DirectoryInfo(fullPath);
            while (relativePath.Length > 0)
            {
                di = di.Parent;
                var name = Path.GetFileName(relativePath);

                yield return new FileViewModel
                {
                    IsFile = false,
                    Name = name,
                    Path = relativePath
                };

                relativePath = relativePath.Substring(0, relativePath.Length - name.Length).TrimEnd(Path.DirectorySeparatorChar);
                fullPath = fullPath.Substring(0, fullPath.Length - name.Length).TrimEnd(Path.DirectorySeparatorChar);
            }

            yield return new FileViewModel
            {
                IsFile = false,
                Name = "Uploads",
                Path = ""
            };
        }

        private IEnumerable<FileViewModel> EnumerateFolders(string fullPath, string relativePath)
        {
            return Directory.GetDirectories(fullPath).Select(d =>
                    {
                        var folderName = Path.GetFileName(d);
                        var folder = new DirectoryInfo(d);
                        return new FileViewModel
                        {
                            IsFile = false,
                            Name = folderName,
                            Path = Path.Combine(relativePath, folderName),
                            Added = folder.CreationTimeUtc.ToLocalTime()
                        };
                    });
        }

        private IEnumerable<FileViewModel> EnumerateFiles(string fullPath, string relativePath, string uploadsFolder)
        {
            return Directory.GetFiles(fullPath).Select(f =>
                {
                    var fileName = Path.GetFileName(f);
                    var file = new FileInfo(f);
                    return new FileViewModel
                    {
                        IsFile = true,
                        Name = fileName,
                        Path = Path.Combine(uploadsFolder, relativePath, fileName),
                        Added = file.CreationTimeUtc.ToLocalTime()
                    };
                });
        }
    }

    public class FileViewModel
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsFile { get; set; }

        public DateTime Added { get; set; }
    }
}