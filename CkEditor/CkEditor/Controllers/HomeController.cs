using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using CkEditor.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace CkEditor.Controllers
{

    [Route("home")]
        public class HomeController : Controller
        {
            private readonly IWebHostEnvironment _webHostEnvironment;

            public HomeController (IWebHostEnvironment webHostEnvironment)
            {
                _webHostEnvironment= webHostEnvironment;
            }
        
            [Route("index")]
            [Route("")]
            [Route("~/")]
            public IActionResult Index()
            {
                return View();
            }
        
            [Route("uploadedimages")]
            [HttpGet]
            public ActionResult UploadedImages()
            {
                var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), _webHostEnvironment.WebRootPath, "uploads"));
                ViewBag.fileInfos = dir.GetFiles();
                return View();
            }
            [Route("uploadimage")]
            [HttpPost]
            public async Task<ActionResult> UploadImage(IFormFile upload)
            {
                if(upload!=null)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), _webHostEnvironment.WebRootPath, "uploads",fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await upload.CopyToAsync(stream);
                    }
                    return new JsonResult(new {path = "/uploads/"+fileName});
                }
                return new JsonResult("Herhangi bir dosya seçiniz");
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

    }