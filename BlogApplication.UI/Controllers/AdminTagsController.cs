using BlogApplication.UI.Data;
using BlogApplication.UI.Models.Domain;
using BlogApplication.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.UI.Controllers
{
    public class AdminTagsController : Controller
    {
        private BlogDbContext dbContext;
        public AdminTagsController(BlogDbContext blogDbContext)
        {
            dbContext = blogDbContext;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            dbContext.Tags.Add(tag);
            dbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            // Use dbContext to read the tags 

            var tags = dbContext.Tags.ToList();

            return View(tags);
        } 

    }
}
