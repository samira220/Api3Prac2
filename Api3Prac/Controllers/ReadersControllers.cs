using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContextApi;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadersController : Controller
    {
        private readonly TestApiDb _context;

        public ReadersController(TestApiDb context)
        {
            _context = context;
        }

        // GET /api/readers — получить список всех читателей
        [HttpGet]
        public async Task<IActionResult> GetAllReaders()
        {
            var readers = await _context.Readers.ToListAsync();
            return Ok(new
            {
                readers = readers,
                status = true
            });
        }

        // GET /api/readers/{id} — получить читателя по его ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReaderById(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { message = "Reader not found" });
            }
            return Ok(new
            {
                reader = reader,
                status = true
            });
        }

        // POST /api/readers — зарегистрировать нового читателя
        [HttpPost]
        public async Task<IActionResult> AddReader([FromBody] Readers newReader)
        {
            _context.Readers.Add(newReader);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Reader added successfully",
                status = true
            });
        }

        // PUT /api/readers/{id} — обновить данные читателя
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReader(int id, [FromBody] Readers updatedReader)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { message = "Reader not found" });
            }

            reader.FirstName = updatedReader.FirstName;
            reader.LastName = updatedReader.LastName;
            reader.DateOfBirth = updatedReader.DateOfBirth;
            reader.ContactInfo = updatedReader.ContactInfo;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Reader updated successfully",
                status = true
            });
        }

        // DELETE /api/readers/{id} — удалить читателя
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { message = "Reader not found" });
            }

            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Reader deleted successfully",
                status = true
            });

            [HttpGet("filter")]
                 async Task<IActionResult> FilterReaders([FromQuery] DateTime? registeredAfter)
            {
                var query = _context.Readers.AsQueryable();

                if (registeredAfter.HasValue)
                {
                    query = query.Where(r => r.RegistrationDate >= registeredAfter.Value);
                }

                var readers = await query.ToListAsync();
                return Ok(new { readers, status = true });
            }

        }
    }

}
