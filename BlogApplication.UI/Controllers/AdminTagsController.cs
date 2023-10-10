using BlogApplication.UI.Data;
using BlogApplication.UI.Models.Domain;
using BlogApplication.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // Use dbContext to read the tags 

            var tags = await dbContext.Tags.ToListAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
           var tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

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
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

           var existingTag = await dbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await dbContext.Tags.FindAsync(editTagRequest.Id);
            if (tag != null)
            {
                dbContext.Tags.Remove(tag);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }

    }
}
