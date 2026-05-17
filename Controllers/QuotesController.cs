using Microsoft.AspNetCore.Mvc;
using MovieQuotesAPI.Data;
using MovieQuotesAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using MovieQuotesAPI.Models;

namespace MovieQuotesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuotesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuoteCreateDto dto)
        {
            var movie = await _context.Movies.FindAsync(dto.MovieId);

            if (movie == null)
                return NotFound("Movie not found!");

            var quote = new Quote
            {
                QuoteEn = dto.QuoteEn,
                QuoteKa = dto.QuoteKa,
                ImagUrl  = dto.ImagUrl,
                MovieId = dto.MovieId
            };

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            return Ok(new QuotesResponseDto
            {
               Id = quote.Id,
               QuoteEn =  quote.QuoteEn,
               QuoteKa = quote.QuoteKa,
               ImagUrl = quote.ImagUrl,
               MovieId =  quote.MovieId
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var quotes = await _context.Quotes
                .Include(q => q.Movie)
                .Select(q => new QuotesResponseDto
                {
                    Id = q.Id,
                    QuoteEn = q.QuoteEn,
                    QuoteKa = q.QuoteKa,
                    ImagUrl = q.ImagUrl,
                    MovieTitle = q.Movie.TitleEn
                })
                .ToListAsync();
            return Ok(quotes);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetByMovie(int movieId)
        {
            var quote = await _context.Quotes
                .Where(q => q.MovieId == movieId)
                .Include(q => q.Movie)
                .Select(q => new QuotesResponseDto
                {
                    Id = q.Id,
                    QuoteEn = q.QuoteEn,
                    QuoteKa = q.QuoteKa,
                    ImagUrl = q.ImagUrl,
                    MovieTitle = q.Movie.TitleEn
                })
                .ToListAsync();

            return Ok(quote);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandom()
        {
            var quotes = await _context.Quotes.ToListAsync();

            if (quotes.Count == 0)
                return NotFound();

            var random = new Random();
            var quote = quotes[random.Next(quotes.Count)];

            return Ok(new QuotesResponseDto
            {
                Id = quote.Id,
                QuoteEn = quote.QuoteEn,
                QuoteKa = quote.QuoteKa,
                ImagUrl = quote.ImagUrl,
                MovieId = quote.MovieId
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, QuoteUpdateDto dto)
        {
            var quote = await _context.Quotes.FindAsync(id);

            if (quote == null)
                return NotFound();

            quote.QuoteEn = dto.QuoteEn;
            quote.QuoteKa = dto.QuoteKa;
            quote.ImagUrl = dto.ImagUrl;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);

            if (quote == null)
                return NotFound();
            _context.Quotes.Remove(quote);

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
