using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using WebApplication1.DbContextApi;
using WebApplication1.Interface;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly TestApiDb _context;

        public BooksController(TestApiDb context, IBooksService booksService)
        {
            _context = context;
            _booksService = booksService;
        }





        // GET /api/books — получить список всех книг
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _booksService.GetAllBooksAsync();
            if (books is null)
                return NotFound();

            return Ok(new
            {
                books = books,
                status = true
            });
        }
        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllBooks()
        //{
        //    var books = await _context.Books.ToListAsync();
        //    return Ok(new
        //    {
        //        books = books,
        //        status = true
        //    });
        //}

        // GET /api/books/{id} — получить книгу по её ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            //var book = await _context.Books.FindAsync(id);
            var book = await _booksService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }
            return Ok(new
            {
                book = book,
                status = true
            });
        }

        // POST /api/books — добавить новую книгу
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Books newBook)
        {
            //_context.Books.Add(newBook);
            //await _booksService.AddBookAsync(newBook);
            //await _context.SaveChangesAsync();
            //return Ok(new
            //{
            //    message = "Книга успешно добавлена",
            //    status = true
            //});

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(x => x.ErrorMessage)),
                    Status = false
                });
            }

            await _booksService.AddBookAsync(newBook);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id_Books }, new { Book = newBook, Status = true });


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Books updatedBook)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Genre_ID = updatedBook.Genre_ID;
            book.PublicationYear = updatedBook.PublicationYear;
            book.Description = updatedBook.Description;
            book.AvailableCopies = updatedBook.AvailableCopies;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Book updated successfully",
                status = true
            });
        }

        // DELETE /api/books/{id} — удалить книгу
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Book deleted successfully",
                status = true
            });
        }

        // GET /api/books/genre/{genreId} — получить книги по жанру
        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetBooksByGenre(int genreId)
        {
            var books = await _context.Books.Where(b => b.Genre_ID.ToString() == genreId.ToString()).ToListAsync();
            return Ok(new
            {
                books = books,
                status = true
            });
        }

        // GET /api/books/search?query={query} — поиск книги по автору и названию
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string query)
        {
            var books = await _context.Books
                .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
                .ToListAsync();

            return Ok(new
            {
                books = books,
                status = true
            });
        }

        [HttpGet("search1")]
        public async Task<IActionResult> SearchBooks([FromQuery] string? author, [FromQuery] string? genre, [FromQuery] int? year)
        {
            var query = _context.Books.AsQueryable(); // Подготовка запросов для фильтрации

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(b => b.Author.Contains(author));
            }


            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(b => b.Genre_ID.Contains(genre));
            }

            if (year.HasValue)
            {
                query = query.Where(b => b.Year.Value.Year == year.Value);
            }

            var books = await query.ToListAsync();
            return Ok(new { books, status = true });
        }

        // GET /api/books/list — получить список книг с пагинацией
        [HttpGet("list")]
        public async Task<IActionResult> GetBookList([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var totalBooks = await _context.Books.CountAsync();
            var books = await _context.Books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                books,
                currentPage = page,
                totalItems = totalBooks,
                totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize),
                status = true
            });
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportBooks(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var books = csv.GetRecords<Books>().ToList();
                _context.Books.AddRange(books);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Books imported successfully", status = true });
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportBooks()
        {
            var books = await _context.Books.ToListAsync();

            var csvContent = new StringBuilder();
            csvContent.AppendLine("Title,Author,Genre,Year,Description");

            foreach (var book in books)
            {
                csvContent.AppendLine($"{book.Title},{book.Author},{book.Genre_ID},{book.Year},{book.Description}");
            }

            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", "books.csv");    
        }


    }

}
