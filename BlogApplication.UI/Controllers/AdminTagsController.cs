using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.UI.Controllers
{
    public class AdminTagsController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
