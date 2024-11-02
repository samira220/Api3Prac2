using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContextApi;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private readonly TestApiDb _context;

        public GenresController(TestApiDb context)
        {
            _context = context;
        }

        // GET /api/genres — получить список всех жанров
        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _context.Genres.ToListAsync();
            return Ok(new
            {
                genres = genres,
                status = true
            });
        }

        // POST /api/genres — добавить новый жанр
        [HttpPost]
        public async Task<IActionResult> AddGenre([FromBody] Genres_Books newGenre)
        {
            _context.Genres.Add(newGenre);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Genre added successfully",
                status = true
            });
        }

        // PUT /api/genres/{id} — обновить жанр
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] Genres_Books updatedGenre)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound(new { message = "Genre not found" });
            }

            genre.Name = updatedGenre.Name;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Genre updated successfully",
                status = true
            });
        }

        // DELETE /api/genres/{id} — удалить жанр
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound(new { message = "Genre not found" });
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Genre deleted successfully",
                status = true
            });
        }
    }

}
