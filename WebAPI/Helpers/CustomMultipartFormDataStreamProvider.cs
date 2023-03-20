using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public string filePath { set; get; }
        public List<PathNameList> filePathList { set; get; } = new List<PathNameList>();
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            //if (headers.ContentDisposition.FileName.Contains("."))
            //{

            var fileName = string.Format("/{0}-{1}", Guid.NewGuid().ToString(), headers.ContentDisposition.FileName.Replace("\"", string.Empty));
            filePath = fileName;

            filePathList.Add(new PathNameList
            {
                Filename = string.IsNullOrEmpty(headers.ContentDisposition.FileName) ? "" : headers.ContentDisposition.FileName,
                Type = headers.ContentDisposition.Name,
                LocalPath = fileName,
                UploadedFileName = headers.ContentDisposition.FileName.Replace("\"", string.Empty)
            });
            return fileName;
        }
    }

    public class PathNameList
    {
        public string Type { get; set; }
        public string LocalPath { get; set; }
        public string Filename { get; set; }
        public string UploadedFileName { get; set; }
    }
}
