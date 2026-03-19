using System.ComponentModel.DataAnnotations;

namespace MovieQuotesAPI.Models
{
    public class Quote
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string QuoteEn {  get; set; }

        [Required]
        [MaxLength (500)]
        public string QuoteKa { get; set; }

        public string ImagUrl { get; set; }

        // Foreign Key
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

    }
}
