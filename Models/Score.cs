using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MatchingGameApp.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int Points { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public DateTime DateAchieved { get; set; }
    }
}
