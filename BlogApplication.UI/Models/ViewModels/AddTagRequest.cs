using System.ComponentModel.DataAnnotations;

namespace BlogApplication.UI.Models.ViewModels
{
    public class AddTagRequest
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? DisplayName { get; set; }
    }
}
