namespace MatchingGameApp.Models
{
    public class SaveScoreDto
    {
        public int Points { get; set; }
        public bool IsFinal { get; set; }
        public string Category { get; set; } // Ensure this property exists and is populated
    }


}

