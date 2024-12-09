using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Score
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } // Foreign key to the Identity user

    [ForeignKey("UserId")]
    public IdentityUser User { get; set; }

    public int Points { get; set; }
    public DateTime DateAchieved { get; set; }
}
