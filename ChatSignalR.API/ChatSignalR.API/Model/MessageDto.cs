using System.ComponentModel.DataAnnotations;

namespace ChatSignalR.API.Model
{
    public class MessageDto
    {
        [Required]
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
    }
}
