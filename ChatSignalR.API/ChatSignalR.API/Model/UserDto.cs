using System.ComponentModel.DataAnnotations;

namespace ChatSignalR.API.Model
{
    public class UserDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Name must be at least")]
        public string Name { get; set; }
    }
}
