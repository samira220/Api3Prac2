using WebApplication1.Model;

namespace WebApplication1.Interface
{
    public interface IBooksService
    {
        Task<IEnumerable<Books>> GetAllBooksAsync();
        Task<Books> GetBookByIdAsync(int id);
        Task AddBookAsync(Books newBook);
        Task UpdateBookAsync(int id, Books updatedBook);
        Task DeleteBookAsync(int id);
        Task<IEnumerable<Books>> GetBooksByGenreAsync(int genreId);
        Task<IEnumerable<Books>> SearchBooksAsync(string query);
        Task<IEnumerable<Books>> SearchBooksAsync(string author, string genre, int? year);
        Task<IEnumerable<Books>> GetBooksPaginatedAsync(int page, int pageSize);
        Task ImportBooksAsync(IEnumerable<Books> books);
        Task<IEnumerable<Books>> ExportBooksAsync();

    }
}
