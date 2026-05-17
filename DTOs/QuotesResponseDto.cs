namespace MovieQuotesAPI.DTOs
{
    public class QuotesResponseDto
    {
            public int Id { get; set; }
            public string QuoteEn { get; set; }
            public string QuoteKa { get; set; }
            public string ImagUrl { get; set; }
            public int MovieId { get; set; }
            public string MovieTitle { get; set; }   
    }
}
