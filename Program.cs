using TLibrary;

Console.Title = "T-Library Professional";
Console.WriteLine("=== T-Библиотека v1.1 ===");

// Умный выбор файла: если пусто - library.json
Console.Write("Введите имя базы данных [Enter для входа в библиотеку]: ");
string inputName = Console.ReadLine()?.Trim() ?? "";
string dbName = string.IsNullOrWhiteSpace(inputName) ? "library.json" : inputName;

LibraryManager manager = new LibraryManager(dbName);

bool running = true;
while (running)
{
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("\n" + new string('-', 40));
    Console.WriteLine($"База: {dbName} | Книг: {manager.Count}");
    Console.WriteLine("1. Добавить   2. Список   3. Поиск   4. Статус/Избранное   5. Удалить   0. Выход");
    Console.ResetColor();
    Console.Write("Команда: ");

    switch (Console.ReadLine())
    {
        case "1":
            AddNewBook(manager);
            break;
        case "2":
            ShowBooks(manager.GetAll());
            break;
        case "3":
            Console.Write("Поиск (запрос): ");
            ShowBooks(manager.Search(Console.ReadLine() ?? ""));
            break;
        case "4":
            UpdateBookStatus(manager);
            break;
        case "5":
            DeleteBook(manager);
            break;
        case "0":
            manager.SaveToFile();
            Console.WriteLine("[*] Данные сохранены. До встречи!");
            running = false;
            break;
        default:
            Console.WriteLine("[!] Неверная команда.");
            break;
    }
}

// --- Методы UI ---

void AddNewBook(LibraryManager m)
{
    Console.WriteLine("\n[ Новая книга ]");
    var b = new Book();
    Console.Write("Название: "); b.Title = Console.ReadLine() ?? "Untitled";
    Console.Write("Автор: "); b.Author = Console.ReadLine() ?? "Unknown";
    Console.Write("Жанр: "); b.Genre = Console.ReadLine() ?? "General";
    Console.Write("Год: "); b.Year = int.TryParse(Console.ReadLine(), out int y) ? y : 0;
    Console.Write("Описание: "); b.Description = Console.ReadLine() ?? "";
    
    m.AddBook(b);
    Console.WriteLine("[+] Книга добавлена в список.");
}

void ShowBooks(List<Book> books)
{
    if (books.Count == 0)
    {
        Console.WriteLine("[?] Книг не найдено.");
        return;
    }

    Console.WriteLine("\n{0,-20} | {1,-15} | {2,-6} | {3}", "Название", "Автор", "Год", "Статус");
    Console.WriteLine(new string('-', 60));
    foreach (var b in books)
    {
        string status = (b.IsRead ? "✓" : " ") + (b.IsFavorite ? " ★" : "");
        Console.WriteLine("{0,-20} | {1,-15} | {2,-6} | {3}", 
            b.Title.Length > 18 ? b.Title[..17] + ".." : b.Title, 
            b.Author, b.Year, status);
    }
}

void UpdateBookStatus(LibraryManager m)
{
    Console.Write("Введите название книги: ");
    string title = Console.ReadLine() ?? "";
    var books = m.Search(title);

    if (books.Count == 0) { Console.WriteLine("[!] Книга не найдена."); return; }
    
    var book = books[0]; // Берем первое совпадение
    Console.WriteLine($"Выбрано: {book.Title}");
    Console.WriteLine("1. Прочитано (да/нет)  2. В избранное (да/нет)");
    
    string choice = Console.ReadLine() ?? "";
    if (choice == "1") book.IsRead = !book.IsRead;
    if (choice == "2") book.IsFavorite = !book.IsFavorite;
    
    Console.WriteLine("[+] Статус обновлен.");
}

void DeleteBook(LibraryManager m)
{
    Console.Write("Введите название для удаления: ");
    if (m.RemoveBook(Console.ReadLine() ?? "")) Console.WriteLine("[+] Книга удалена.");
    else Console.WriteLine("[!] Ошибка удаления.");
}