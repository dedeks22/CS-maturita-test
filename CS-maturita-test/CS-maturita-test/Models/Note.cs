using System.ComponentModel.DataAnnotations;

namespace CS_maturita_test.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}
