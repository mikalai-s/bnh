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

        public IEnumerable<FileViewModel> Folders { get; set; }

        public IEnumerable<FileViewModel> Files { get; set; }

        public IEnumerable<FileViewModel> BreadCrumbs { get; set; }
        
        public FilesViewModel(string relativePath, string fullPath, string uploadsFolder)
        {
            this.CurrentPath = relativePath;

            if (Directory.Exists(fullPath))
            {
                Folders = Directory.GetDirectories(fullPath).Select(d =>
                    {
                        var folderName = Path.GetFileName(d);
                        return new FileViewModel
                        {
                            Name = folderName,
                            Path = Path.Combine(relativePath, folderName)
                        };
                    });
                Files = Directory.GetFiles(fullPath).Select(f =>
                {
                    var fileName = Path.GetFileName(f);
                    return new FileViewModel
                    {
                        Name = fileName,
                        Path = Path.Combine(uploadsFolder, relativePath, fileName)
                    };
                });
                var bc = new List<FileViewModel>();
                var di = new DirectoryInfo(fullPath);
                while (true)
                {
                    di = di.Parent;
                    relativePath = Path.GetDirectoryName(relativePath);
                }
                //BreadCrumbs = 
            }
        }
    }

    public class FileViewModel
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }
}