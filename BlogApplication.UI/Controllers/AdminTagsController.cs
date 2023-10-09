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

            return View("Add");
        }
    }
}
