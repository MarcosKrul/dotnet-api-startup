using System.ComponentModel.DataAnnotations;

namespace TucaAPI.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 characters")]
        [MaxLength(180, ErrorMessage = "Title cannot be over 180 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be 5 characters")]
        [MaxLength(280, ErrorMessage = "Contet cannot be over 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}
