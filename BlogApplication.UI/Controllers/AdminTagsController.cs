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

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
           var tag = dbContext.Tags.FirstOrDefault(t => t.Id == id);

            if (tag! == null)
            {
                var editTaqRequest = new EditTagRequest {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTaqRequest);
            }
            return View(null);
        }

        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

           var existingTag = dbContext.Tags.Find(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                dbContext.SaveChanges();
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }

        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = dbContext.Tags.Find(editTagRequest.Id);
            if (tag != null)
            {
                dbContext.Tags.Remove(tag);
                dbContext.SaveChanges();

                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }

    }
}
