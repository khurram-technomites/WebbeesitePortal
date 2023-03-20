using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices
{
    public interface IFTPUpload
    {
        public List<string> FetchGallery();
        bool UploadToDraft(IFormFile file, ref string AbsoluteUri, bool isWebpRequire = false);
        bool UploadToDirectory(IFormFile file, ref string DestinationDirectory);
        bool UploadToDirectory(byte[] file, ref string DestinationDirectory, string Extension);
        bool CreateFolder(string Path);
        bool DirectoryExist(string dirPath);
        bool DeleteFile(string Path);
        bool MoveFile(string SourcePath, ref string DestinationDirectory);
    }
}
