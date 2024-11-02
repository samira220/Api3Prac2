using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContextApi;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class History_Book_RentalControllers : Controller
    {
        private readonly TestApiDb _context;

        public History_Book_RentalControllers(TestApiDb context)
        {
            _context = context;
        }

        // POST /api/rentals — арендовать книгу (читатель, книга, срок аренды)
        [HttpPost]
        public async Task<IActionResult> RentBook([FromBody] History_Book_Rental newRental)
        {
            _context.History_Book_Rentals.Add(newRental);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Book rented successfully",
                status = true
            });
        }

        // PUT /api/rentals/return — возврат книги
        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var rental = await _context.History_Book_Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound(new { message = "Rental record not found" });
            }

            rental.ReturnDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Book returned successfully",
                status = true
            });
        }

        // GET /api/rentals/history/reader/{readerId} — получить историю аренды для читателя
        [HttpGet("history/reader/{readerId}")]
        public async Task<IActionResult> GetRentalHistoryForReader(int readerId)
        {
            var rentals = await _context.History_Book_Rentals
                .Where(r => r.Reader_ID == readerId)
                .ToListAsync();

            return Ok(new
            {
                rentals = rentals,
                status = true
            });
        }

        // GET /api/rentals/history/book/{bookId} — получить историю аренды для книги
        [HttpGet("history/book/{bookId}")]
        public async Task<IActionResult> GetRentalHistoryForBook(int bookId)
        {
            var rentals = await _context.History_Book_Rentals
                .Where(r => r.Books_Id == bookId)
                .ToListAsync();

            return Ok(new
            {
                rentals = rentals,
                status = true
            });
        }

        // GET /api/rentals/current — получить список текущих аренд
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentRentals()
        {
            var rentals = await _context.History_Book_Rentals
                .Where(r => r.ReturnDate == null)
                .ToListAsync();

            return Ok(new
            {
                rentals = rentals,
                status = true
            });
        }
    }


}
