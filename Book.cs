using System.Diagnostics;

namespace TLibrary;

public class Book
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public bool IsFavorite {get; set; } = false;


    public override string ToString()
    {
        string status = IsRead ? "[Прочитано]" : "[Не прочитано]";
        string favorite = IsFavorite ? "★" : "";
        return $"{status}{favorite} {Title} — {Author} ({Year}) | Жанр: {Genre}";
    }
}