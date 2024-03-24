using AMaz.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMaz.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly FileService _fileService;
   
        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
