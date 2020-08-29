using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace TM470.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private IHostingEnvironment _env;

        public FileUploadController(IHostingEnvironment environment)
        {
            _env = environment;
        }

        [HttpPost]
        public ObjectResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Uploads";
                string webRootPath = _env.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                string fileName = "";
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok(fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
