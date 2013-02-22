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

        public IEnumerable<FileViewModel> Files { get; set; }

        public IEnumerable<FileViewModel> BreadCrumbs { get; set; }
        
        public FilesViewModel(string relativePath, string fullPath, string uploadsFolder)
        {
            this.CurrentPath = relativePath;

            if (Directory.Exists(fullPath))
            {
                Files = EnumerateFolders(fullPath, relativePath).Union(EnumerateFiles(fullPath, relativePath, uploadsFolder));
                //var bc = new List<FileViewModel>();
                //var di = new DirectoryInfo(fullPath);
                //while (true)
                //{
                //    di = di.Parent;
                //    relativePath = Path.GetDirectoryName(relativePath);
                //}
                //BreadCrumbs = 
            }
        }

        private IEnumerable<FileViewModel> EnumerateFolders(string fullPath, string relativePath)
        {
            return Directory.GetDirectories(fullPath).Select(d =>
                    {
                        var folderName = Path.GetFileName(d);
                        return new FileViewModel
                        {
                            IsFile = false,
                            Name = folderName,
                            Path = Path.Combine(relativePath, folderName)
                        };
                    });
        }

        private IEnumerable<FileViewModel> EnumerateFiles(string fullPath, string relativePath, string uploadsFolder)
        {
            return Directory.GetFiles(fullPath).Select(f =>
                {
                    var fileName = Path.GetFileName(f);
                    return new FileViewModel
                    {
                        IsFile = true,
                        Name = fileName,
                        Path = Path.Combine(uploadsFolder, relativePath, fileName)
                    };
                });
        }
    }

    public class FileViewModel
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsFile { get; set; }
    }
}