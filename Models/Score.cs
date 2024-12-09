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
        public string UserId { get; set; } // Foreign key to the Identity user

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; } // Navigation property to IdentityUser

        public int Points { get; set; }

        [Required]
        public string Category { get; set; } // Add Category property

        public DateTime DateAchieved { get; set; }
    }
}
