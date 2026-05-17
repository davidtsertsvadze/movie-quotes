using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieQuotesAPI.Data;
using MovieQuotesAPI.Models;
using MovieQuotesAPI.DTOs;

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
                .ToListAsync();

            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Quotes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound ();
            return Ok(movie);
        }

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
