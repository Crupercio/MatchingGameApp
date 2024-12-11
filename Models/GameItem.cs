namespace MatchingGameApp.Models
{
    public class GameItem
    {
        public int Id { get; set; }
        public string? Name { get; set; } // Name of the item (e.g., "Dog")
        public string? ImageUrl { get; set; } // URL for the item's image
        public string? Category { get; set; } // Category for sorting
    }

}
