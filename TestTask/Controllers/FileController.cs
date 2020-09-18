using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        UserContext db;
        IWebHostEnvironment _appEnvironment;
        public FileController(UserContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        [Authorize]
        [Route("GetFiles")]
        [HttpGet]
        public IActionResult GetFiles()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (db)
            {
                var Data = db.Files.Where(prop => prop.UserId.ToString() == userid).Select(z => z.FileName).ToList();
                return Json(Data);
            }
            
        }

        [Authorize]
        [Route("AddFile")]
        [HttpPost]
        public IActionResult AddFile()
        {
            var uploadedFile = Request.Form.Files[0];
            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    uploadedFile.CopyToAsync(fileStream);
                }
                Models.File file = new Models.File { UserId =  Guid.Parse(userid), FileName = uploadedFile.FileName, Path = path };
                db.Files.Add(file);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [Route("DownloadFile")]
        [HttpGet]
        public IActionResult DownloadFile(string FileName)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var file = db.Files.Where(prop => prop.UserId.ToString() == userid && prop.FileName == FileName)
                .Select(z => z.FileName).SingleOrDefault();

            try {
                string path = "/Files/" + file;
                var extention = MimeMapping.MimeUtility.GetMimeMapping(file);
                using (FileStream stream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    return File(stream, extention);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }


        }

    }
}
