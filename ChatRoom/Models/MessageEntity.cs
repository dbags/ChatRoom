using System;
using System.ComponentModel.DataAnnotations;

namespace ChatRoom.Models
{
    public class MessageEntity : Entity<int>
    {
        [Required]
        public int ChatroomID { get; set; }
        public ChatroomEntity Chatroom { get; set; }

        [Required]
        public string PosterID { get; set; }
        public ApplicationUser Poster { get; set; }

        [Required]
        public DateTime PostDate { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Content { get; set; }
    }
}
