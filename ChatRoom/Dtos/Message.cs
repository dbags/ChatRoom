using System;
using System.ComponentModel.DataAnnotations;

namespace ChatRoom.Dtos
{
    public class Message : Dto<int>
    {
        [Required]
        public int ChatroomID { get; set; }
        public string ChatroomTitle { get; set; }

        [Required]
        public string PosterID { get; set; }
        public string PosterName { get; set; }

        [Required]
        public DateTime PostDate { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Content { get; set; }

    }
}
