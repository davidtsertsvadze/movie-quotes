using System.ComponentModel.DataAnnotations;    

namespace MovieQuotesAPI.DTOs
{
    public class QuoteUpdateDto
    {
        public string QuoteEn { get; set; }
        public string QuoteKa { get; set; }
        public string ImagUrl { get; set; }
    }
}
