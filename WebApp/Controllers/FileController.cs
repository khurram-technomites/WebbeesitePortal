using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Controllers
{
	public class FileController : Controller
	{
		private readonly IFileUpload _fileUpload;

		public FileController(IFileUpload fileUpload)
		{
			_fileUpload = fileUpload;
		}

		public async Task<string> Upload(IFormFile file)
		{
			if (file is null)
			{
				return "";
			}
			string asd = await _fileUpload.UploadFile(file);

			return asd;
		}

		[HttpGet]
		public async Task<string> Delete(string path, string table = "optional")
		{
			if (string.IsNullOrEmpty(path))
				return "";

			return await _fileUpload.DeleteFile(path.Replace(" ", "%20"), table);
		}

		[HttpGet]
		public async Task<object> Gallery()
		{
			return await _fileUpload.Gallery();
		}
	}
}
