using HelperClasses.Classes;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;

namespace WebAPI.Services
{
    public class FTPUpload : IFTPUpload
    {
        private readonly ILogger<FTPUpload> _logger;
        private string _host, _username, _password;
        private int _rootPosition, _rootPositionForDelete;
        public FTPUpload(IConfiguration _config, ILogger<FTPUpload> logger)
        {
            _logger = logger;
            IConfigurationSection Section = _config.GetSection("FTPCredentials");

            _host = Section.GetValue<string>("Host");
            _username = Section.GetValue<string>("UserName");
            _password = Section.GetValue<string>("Password");

            _rootPosition = _config.GetValue<int>("RootInsert");
            _rootPositionForDelete = _config.GetValue<int>("RootInsertForDelete");
        }

        public bool CreateFolder(string Path)
        {
            bool IsCreated = true;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_host + Path);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(_username, _password);
                //request.EnableSsl = true;
                using var resp = (FtpWebResponse)request.GetResponse();
                Path = resp.ResponseUri.AbsoluteUri;
            }
            catch (WebException ex)
            {
                _logger.LogError("FTPDirectory creation failed , StatusDescription = " + ex);
                IsCreated = false;
            }
            return IsCreated;
        }

        public bool DeleteFile(string Path)
        {
            bool IsDeleted;
            try
            {
                if (!Path.Contains("wwwroot"))
                    Path = Path.Replace("https://cdn.demowbs.com", "ftp://cdn.demowbs.com");
                else
                    Path = Path.Replace("https://", "ftp://");

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + Path);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(_username, _password);
                //request.EnableSsl = true;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    IsDeleted = true;
                }
            }
            catch (WebException ex)
            {
                IsDeleted = false;
            }

            return IsDeleted;
        }

        public bool DirectoryExist(string Path)
        {
            bool isexist;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_host + Path);
                request.Credentials = new NetworkCredential(_username, _password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                //request.EnableSsl = true;
                using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                isexist = true;
            }
            catch (WebException ex)
            {
                isexist = false;
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {

                    }
                }
            }
            return isexist;
        }

        public bool MoveFile(string SourcePath, ref string DestinationDirectory)
        {
            while (DestinationDirectory[DestinationDirectory.Length - 2] == ' ' && DestinationDirectory.EndsWith("/"))
                DestinationDirectory = DestinationDirectory.Remove(DestinationDirectory.Length - 2, 1);

            DestinationDirectory = DestinationDirectory.RemoveSpecialCharacters();

            bool IsUploaded = false;

            SourcePath = Uri.UnescapeDataString(SourcePath).Replace("https://cdn.demowbs.com", "ftp://cdn.demowbs.com");
            try
            {
                string Extension = System.IO.Path.GetExtension(SourcePath);
                Guid fileName = Guid.NewGuid();

                if (!DirectoryExist(DestinationDirectory))
                    CreateFolder(DestinationDirectory);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(SourcePath);
                request.Method = WebRequestMethods.Ftp.Rename;
                //request.EnableSsl = true;
                request.Credentials = new NetworkCredential(_username, _password);

                request.RenameTo = "/wwwroot" + DestinationDirectory + fileName + Extension;

                using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                DestinationDirectory = (_host + DestinationDirectory + fileName + Extension).Replace("ftp://waws-prod-dxb-005.ftp.azurewebsites.windows.net/site/wwwroot/wwwroot", "https://cdn.demowbs.com").Replace("ftp://cdn.demowbs.com/wwwroot", "https://cdn.demowbs.com/wwwroot");

                IsUploaded = true;

                DeleteFile(SourcePath.Remove(0, 6));
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                _logger.LogError($"FTP File upload failed to Directory: {DestinationDirectory}, StatusDescription = " + status);
            }

            return IsUploaded;
        }

        public bool UploadToDirectory(IFormFile file, ref string DestinationDirectory)
        {
            bool IsUploaded = false;
            try
            {
                string Extension = System.IO.Path.GetExtension(file.FileName);
                Guid fileName = Guid.NewGuid();
                Uri path = new Uri(_host + DestinationDirectory + fileName.ToString() + Extension);

                if (!DirectoryExist(DestinationDirectory))
                    CreateFolder(DestinationDirectory);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(_username, _password);
                //request.EnableSsl = true;
                Stream ftpStream = request.GetRequestStream();
                Stream fs = file.OpenReadStream();

                byte[] buffer = new byte[1024];
                int byteRead = 0;
                do
                {
                    byteRead = fs.Read(buffer, 0, 1024);
                    ftpStream.Write(buffer, 0, byteRead);
                }
                while (byteRead != 0);
                fs.Close();
                ftpStream.Close();

                using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                DestinationDirectory = (response.ResponseUri.AbsoluteUri).Replace("ftp://waws-prod-dxb-005.ftp.azurewebsites.windows.net/site/wwwroot/wwwroot", "https://cdn.demowbs.com").Replace("ftp://cdn.demowbs.com/wwwroot", "https://cdn.demowbs.com/wwwroot");

                IsUploaded = true;
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                _logger.LogError("FTP File upload failed to Draft, StatusDescription = " + status);
            }

            return IsUploaded;
        }
        public bool UploadToDirectory(byte[] file, ref string DestinationDirectory, string Extension)
        {
            bool IsUploaded = false;
            try
            {
                Guid fileName = Guid.NewGuid();
                Uri path = new Uri(_host + DestinationDirectory + fileName.ToString() + Extension);

                if (!DirectoryExist(DestinationDirectory))
                    CreateFolder(DestinationDirectory);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(_username, _password);
                //request.EnableSsl = true;
                Stream ftpStream = request.GetRequestStream();
                Stream fs = new MemoryStream(file);

                byte[] buffer = new byte[1024];
                int byteRead = 0;
                do
                {
                    byteRead = fs.Read(buffer, 0, 1024);
                    ftpStream.Write(buffer, 0, byteRead);
                }
                while (byteRead != 0);
                fs.Close();
                ftpStream.Close();

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    DestinationDirectory = (response.ResponseUri.AbsoluteUri).Replace("ftp://waws-prod-dxb-005.ftp.azurewebsites.windows.net/site/wwwroot/wwwroot", "https://cdn.demowbs.com").Replace("ftp://cdn.demowbs.com/wwwroot", "https://cdn.demowbs.com/wwwroot");

                    IsUploaded = true;
                }
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                _logger.LogError("FTP File upload failed to Draft, StatusDescription = " + status);
            }

            return IsUploaded;
        }

        public bool UploadToDraft(IFormFile file, ref string AbsoluteUri, bool isWebpRequire = false)
        {
            bool IsUploaded = false;
            try
            {
                string Extension = "";
                if (isWebpRequire)
                    Extension = ".webp";
                else
                    Extension = System.IO.Path.GetExtension(file.FileName);
                Guid fileName = Guid.NewGuid();

                Uri path = new(_host + "/Draft/" + fileName.ToString() + Extension);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
                //request.EnableSsl = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_username, _password);

                Stream ftpStream = request.GetRequestStream();
                Stream fs = file.OpenReadStream();

                //WebP Code
                MemoryStream streamReader = new();

                if (Enum.IsDefined(typeof(ImageFormats), Extension.Replace(".", "").ToUpper()))
                {
                    using ImageFactory imageFactory = new(preserveExifData: false);
                    imageFactory.Load(fs)
                                .Format(new WebPFormat())
                                .Save(streamReader);

                    byte[] buffer = new byte[1024];
                    int byteRead = 0;

                    do
                    {
                        byteRead = streamReader.Read(buffer, 0, 1024);
                        ftpStream.Write(buffer, 0, byteRead);
                    }
                    while (byteRead != 0);
                }
                else
                {
                    byte[] buffer = new byte[1024];
                    int byteRead = 0;
                    do
                    {
                        byteRead = fs.Read(buffer, 0, 1024);
                        ftpStream.Write(buffer, 0, byteRead);
                    }
                    while (byteRead != 0);
                }

                fs.Close();
                ftpStream.Close();

                using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                AbsoluteUri = (response.ResponseUri.AbsoluteUri).Replace("ftp://waws-prod-dxb-005.ftp.azurewebsites.windows.net/site/wwwroot/wwwroot", "https://cdn.demowbs.com").Replace("ftp://cdn.demowbs.com/wwwroot", "https://cdn.demowbs.com/wwwroot");

                IsUploaded = true;
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                _logger.LogError("FTP File upload failed to Draft, StatusDescription = " + status);
            }

            return IsUploaded;
        }

        public List<string> FetchGallery()
        {
            try
            {
                Uri path = new Uri(_host + "/Gallery/");

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential(_username, _password);
                //request.EnableSsl = true;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(i => _host.Replace("ftp", "https") + "/Gallery/" + i).ToList();
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                _logger.LogError("FTP File upload failed to Draft, StatusDescription = " + status);
            }

            return new List<string>();
        }
    }
}