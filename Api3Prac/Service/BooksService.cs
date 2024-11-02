using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DbContextApi;
using WebApplication1.Interface;
using WebApplication1.Model;

public class BooksService : IBooksService  // Реализуем интерфейс IBooksService
{
    private readonly TestApiDb _context;  // Это будет наш контекст базы данных

    public BooksService(TestApiDb context)  // Внедряем зависимость контекста базы данных
    {
        _context = context;
    }

    public async Task<IEnumerable<Books>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();  // Возвращаем все книги
    }

    public async Task<Books> GetBookByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);  // Ищем книгу по её ID
    }

    public async Task AddBookAsync(Books newBook)
    {
        if (newBook != null)
        {
            await _context.Books.AddAsync(newBook);  // Добавляем новую книгу в базу данных
            await _context.SaveChangesAsync();  // Сохраняем изменения
        }
    }

    public async Task UpdateBookAsync(int id, Books updatedBook)
    {
        var book = await _context.Books.FindAsync(id);  // Ищем книгу по ID
        if (book != null)
        {
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Genre_ID = updatedBook.Genre_ID;
            book.PublicationYear = updatedBook.PublicationYear;
            book.Description = updatedBook.Description;
            book.AvailableCopies = updatedBook.AvailableCopies;

            await _context.SaveChangesAsync();  // Сохраняем изменения
        }
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);  // Ищем книгу по ID
        if (book != null)
        {
            _context.Books.Remove(book);  // Удаляем книгу из базы данных
            await _context.SaveChangesAsync();  // Сохраняем изменения
        }
    }

    public async Task<IEnumerable<Books>> GetBooksByGenreAsync(int genreId)
    {
        return await _context.Books.Where(b => b.Genre_ID.ToString() == genreId.ToString()).ToListAsync();
    }

    public async Task<IEnumerable<Books>> SearchBooksAsync(string query)
    {
        return await _context.Books
            .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
            .ToListAsync();
    }

    public async Task<IEnumerable<Books>> SearchBooksAsync(string author, string genre, int? year)
    {
        var query = _context.Books.AsQueryable();  // Используем LINQ для динамической фильтрации

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

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Books>> GetBooksPaginatedAsync(int page, int pageSize)
    {
        return await _context.Books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task ImportBooksAsync(IEnumerable<Books> books)
    {
        _context.Books.AddRange(books);  // Импортируем список книг в базу данных
        await _context.SaveChangesAsync();  // Сохраняем изменения
    }

    public async Task<IEnumerable<Books>> ExportBooksAsync()
    {
        return await _context.Books.ToListAsync();  // Экспортируем все книги
    }
}

