namespace DataServiceLayer.DTOs;

public class TitleSummaryDto
{
    public string Id { get; set; }
    public string PrimaryTitle { get; set; }
    public string? StartYear { get; set; }
    public string? EndYear { get; set; }
    public int? RuntimeMinutes { get; set; }
    public string? Poster { get; set; }
    public TitleRatingDto? TitleRating { get; set; }
    public string TypeId { get; set; }
 
}
