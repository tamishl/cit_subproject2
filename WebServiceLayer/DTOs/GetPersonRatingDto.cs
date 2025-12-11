namespace WebServiceLayer.DTOs
{
    public class GetPersonRatingDto
    {
        public string Url { get; set; }
        public string PersonId { get; set; }
        public double AverageRating { get; set; }
    }
}
