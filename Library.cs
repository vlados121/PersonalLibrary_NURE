using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PersonalLibrary
{
    // Клас, що керує колекцією книг та зберігає їх у файл
    public class Library
    {
        private List<Book> books; // Список усіх книг
        private int nextId; // Змінна-лічильник для генерації унікального ID
        private readonly string filePath = "library.json";

        public Library()
        {
            books = new List<Book>();
            nextId = 1;
            LoadData(); // Завантажуємо книги при старті програми
        }

        // Додавання нової книги
        public void AddBook(string author, string title, string publisher, string category, int rating)
        {
            Book newBook = new Book(nextId++, author, title, publisher, category, rating);
            books.Add(newBook);
            SaveData(); // Зберігаємо результат у файл
        }

        public List<Book> GetAllBooks() => books;

        // Оновлення полів існуючої книги
        public bool EditBook(int id, string newAuthor, string newTitle, string newPublisher, string newCategory, int newRating)
        {
            Book book = FindBookById(id);
            if (book != null)
            {
                if (!string.IsNullOrWhiteSpace(newAuthor)) book.Author = newAuthor;
                if (!string.IsNullOrWhiteSpace(newTitle)) book.Title = newTitle;
                if (!string.IsNullOrWhiteSpace(newPublisher)) book.Publisher = newPublisher;
                if (!string.IsNullOrWhiteSpace(newCategory)) book.Category = newCategory;
                if (newRating >= 1 && newRating <= 5) book.Rating = newRating;
                SaveData();
                return true;
            }
            return false;
        }

        public Book GetBookById(int id) => FindBookById(id);

        // Видалення книги зі списку
        public bool RemoveBook(int id)
        {
            Book book = FindBookById(id);
            if (book != null)
            {
                books.Remove(book);
                SaveData();
                return true;
            }
            return false;
        }

        // Пошук книг частковим збігом (по автору або назві)
        public List<Book> Search(string query)
        {
            query = query.ToLower();
            return books.Where(b => b.Author.ToLower().Contains(query) || b.Title.ToLower().Contains(query)).ToList();
        }

        // Відмітити, що книгу фізично передали іншому
        public bool MarkAsBorrowed(int id, string person)
        {
            Book book = FindBookById(id);
            if (book != null)
            {
                book.IsAvailable = false;
                book.BorrowedBy = person;
                SaveData();
                return true;
            }
            return false;
        }

        // Відмітити, що книгу повернуто
        public bool MarkAsReturned(int id)
        {
            Book book = FindBookById(id);
            if (book != null)
            {
                book.IsAvailable = true;
                book.BorrowedBy = "Нікому";
                SaveData();
                return true;
            }
            return false;
        }

        // Отримати книги лише з певною оцінкою
        public List<Book> FilterByRating(int targetRating)
        {
            return books.Where(b => b.Rating == targetRating).ToList();
        }

        public int GetBookCount() => books.Count;

        // Пошук об'єкту книги за ідентифікатором (допоміжний приватний метод)
        private Book FindBookById(int id) => books.FirstOrDefault(b => b.Id == id);

        // Серіалізація списку книг та збереження у файл JSON
        private void SaveData()
        {
            string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Десеріалізація списку книг з файлу JSON
        private void LoadData()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
                    if (books.Count > 0)
                    {
                        nextId = books.Max(b => b.Id) + 1; // Корегуємо наступний ID
                    }
                }
            }
        }
    }
}
