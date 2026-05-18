namespace MovieQuotesAPI.DTOs
{
    public class MovieResponseDto
    {
            public int Id { get; set; }
            public string TitleEn { get; set; }
            public string TitleKa { get; set; }

            public List<QuoteDto> Quotes { get; set; }

    }
}
