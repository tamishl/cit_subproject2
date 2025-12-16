namespace WebServiceLayer.DTOs
{
    public class UserRatingDto
    {
        public string TitleId { get; set; }
        public string? TitleUrl { get; set; }
        public string? TitleName { get; set; }
        public string? Plot { get; set; }
        public string? Poster { get; set; }
        public int RatingValue { get; set; }
        public double AverageRating { get; set; }
        public int Votes { get; set; }


    }
}
