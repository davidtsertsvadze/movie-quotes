using System.ComponentModel.DataAnnotations;

namespace MovieQuotesAPI.DTOs
{
    public class QuoteCreateDto
    {
        public string QuoteEn { get; set; }
        public string QuoteKa { get; set; }
        public string ImagUrl  { get; set; }

        public int MovieId    { get; set; }
    }
}
