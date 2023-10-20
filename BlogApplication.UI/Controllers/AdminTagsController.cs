using BlogApplication.UI.Data;
using BlogApplication.UI.Models.Domain;
using BlogApplication.UI.Models.ViewModels;
using BlogApplication.UI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly BlogDbContext dbContext;


        public AdminTagsController(ITagRepository tagRepository, BlogDbContext blogDbContext)
        {
            this.tagRepository = tagRepository;
            this.dbContext = blogDbContext;

        }

       
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            ValidateAddTagRequest(addTagRequest);
            if (ModelState.IsValid == false)
            {
                return View();
            }

            // Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // Use dbContext to read the tags 

            var tags = await tagRepository.GetAllTagsAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag! == null)
            {
                var editTaqRequest = new EditTagRequest
                {
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

            var updatedTag = await tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                // Show Succsess
                return RedirectToAction("List");
            }
            else
            {
                // Show Error
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }

           
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                // Show succsess
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
        }


        private void ValidateAddTagRequest(AddTagRequest request) 
        {
            if (request.Name is not null && request.DisplayName is not null)
            {
                if (request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name can not be the same as DisplayName");
                }
            }
        }

    }
}
