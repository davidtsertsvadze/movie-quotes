using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 


namespace MovieQuotesAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string TitleEn { get; set; }

        [Required]
        [MaxLength(200)]
        public string TitleKa { get; set; }

        // Relationship: ერთი ფილმს შეიძლება ჰქონდეს ბევრი Quote
        public List<Quote> Quotes { get; set; }
    }
}
