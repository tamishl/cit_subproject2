namespace WebServiceLayer.DTOs
{
    public class GetTitleRatingDto
    {
        public string Url { get; set; }
        public string TitleId { get; set; }
        public float AverageRating { get; set; }
        public int Votes { get; set; }
    }
}
