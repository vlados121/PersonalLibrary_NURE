using System;

namespace PersonalLibrary
{
    // Клас, що описує сутність книги в бібліотеці 
    public class Book
    {
        public int Id { get; set; } // Унікальний ідентифікатор книги
        public string Author { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; } // Чи знаходиться книга вдома
        public int Rating { get; set; }
        public string BorrowedBy { get; set; } // Ім'я того, кому віддали книгу

        public Book() { } // Порожній конструктор для JSON-серіалізації

        // Конструктор для створення нової книги
        public Book(int id, string author, string title, string publisher, string category, int rating)
        {
            Id = id;
            Author = author;
            Title = title;
            Publisher = publisher;
            Category = category;
            Rating = rating;
            IsAvailable = true; // За замовчуванням книга вдома
            BorrowedBy = "Нікому";
        }

        // Перевизначений метод для зручного виведення інформації про книгу
        public override string ToString()
        {
            string status = IsAvailable ? "Є вдома" : $"Відсутня (У кого зараз: {BorrowedBy})";
            return $"[ID: {Id}] {Author} - \"{Title}\"\n  Видавництво: {Publisher}\n  Розділ: {Category}\n  Оцінка: {Rating}/5\n  Статус: {status}\n--------------------------------------------------";
        }
    }
}
