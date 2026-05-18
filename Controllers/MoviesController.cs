using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieQuotesAPI.Data;
using MovieQuotesAPI.Models;
using MovieQuotesAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace MovieQuotesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _context.Movies
                .Include(m => m.Quotes)
                .Select(m => new MovieResponseDto
                {
                    Id = m.Id,
                    TitleEn = m.TitleEn,
                    TitleKa = m.TitleKa,
                    Quotes = m.Quotes.Select(q => new QuoteDto
                    {
                        Id = q.Id,
                        QuoteEn = q.QuoteEn,
                        QuoteKa = q.QuoteKa,
                        ImgUrl = q.ImagUrl
                    }).ToList()
                })
                .ToListAsync();

            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Quotes)
                .Where(m => m.Id == id)
                .Select(m => new MovieResponseDto
                {
                    Id = m.Id,
                    TitleEn = m.TitleEn,
                    TitleKa = m.TitleKa,
                    Quotes = m.Quotes.Select(q => new QuoteDto
                    {
                        Id = q.Id,
                        QuoteEn = q.QuoteEn,
                        QuoteKa = q.QuoteKa,
                        ImgUrl = q.ImagUrl
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (movie == null)
                return NotFound ();
            return Ok(movie);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                TitleEn = dto.TitleEn,
                TitleKa = dto.TitleKa
            };  

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            movie.TitleEn = dto.TitleEn;
            movie.TitleKa = dto.TitleKa;

            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
                
            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok(movie);
        }
    }

}
