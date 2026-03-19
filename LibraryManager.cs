using System.Text.Json;
using System.Linq;

namespace TLibrary;

public class LibraryManager
{
    private List<Book> _books = new();
    private readonly string _filePath;

    // Публичное свойство для доступа к количеству книг (только для чтения)
    public int Count => _books.Count;

    public LibraryManager(string fileName)
    {
        // Проверка расширения файла
        _filePath = fileName.EndsWith(".json") ? fileName : fileName + ".json";
        LoadFromFile();
    }

    public void AddBook(Book book) => _books.Add(book);

    public bool RemoveBook(string title)
    {
        // Ищем книгу без учета регистра
        var book = _books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (book != null) return _books.Remove(book);
        return false;
    }

    public List<Book> GetAll(string sortBy = "title") => sortBy.ToLower() switch
    {
        "author" => _books.OrderBy(b => b.Author).ToList(),
        "year" => _books.OrderBy(b => b.Year).ToList(),
        _ => _books.OrderBy(b => b.Title).ToList()
    };

    public List<Book> Search(string query)
    {
        return _books.Where(b => 
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) || 
            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Description.Contains(query, StringComparison.OrdinalIgnoreCase)
        ).ToList();
    }

    public void SaveToFile()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_books, options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Ошибка записи на диск: {ex.Message}");
        }
    }

    private void LoadFromFile()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            string json = File.ReadAllText(_filePath);
            _books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }
        catch 
        { 
            // Если файл поврежден, начинаем с чистого листа
            _books = new List<Book>(); 
        }
    }
}