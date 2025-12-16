namespace WebServiceLayer.DTOs
{
    public class BookmarkTitleDtoWsl
    {
        public string BookmarkUrl { get; set; }
        public string TitleUrl { get; set; }
        public string TitleId { get; set; }
        public string PrimaryTitle { get; set; }
        public string? Plot { get; set; }
        public string? Poster { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
