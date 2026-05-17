using System.ComponentModel.DataAnnotations;

namespace MovieQuotesAPI.DTOs
{
    public class MovieUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string TitleEn { get; set; }

        [Required]
        [MaxLength(200)]
        public string TitleKa { get; set; }
    }
}
