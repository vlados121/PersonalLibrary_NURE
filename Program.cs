using System;
using System.Collections.Generic;

namespace PersonalLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Library myLibrary = new Library();
            bool isRunning = true;

            Console.WriteLine("=== Програма 'Особиста бібліотека' ===");

            while (isRunning)
            {
                Console.WriteLine("\nОберіть дію:");
                Console.WriteLine("1. Додати книгу");
                Console.WriteLine("2. Показати всі книги");
                Console.WriteLine("3. Редагувати книгу");
                Console.WriteLine("4. Видалити книгу");
                Console.WriteLine("5. Знайти книгу (за автором або назвою)");
                Console.WriteLine("6. Відмітити, що дав книгу другові");
                Console.WriteLine("7. Відмітити, що книгу повернули");
                Console.WriteLine("8. Фільтр за оцінкою");
                Console.WriteLine("9. Показати загальну кількість книг");
                Console.WriteLine("0. Вийти");
                Console.Write("\nВаш вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddBookMenu(myLibrary); break;
                    case "2": PrintBooks(myLibrary.GetAllBooks(), "\n--- Список всіх книг ---"); break;
                    case "3": EditBookMenu(myLibrary); break;
                    case "4": RemoveBookMenu(myLibrary); break;
                    case "5": SearchMenu(myLibrary); break;
                    case "6": MarkBorrowedMenu(myLibrary); break;
                    case "7": MarkReturnedMenu(myLibrary); break;
                    case "8": FilterMenu(myLibrary); break;
                    case "9": Console.WriteLine($"\nЗагальна кількість книг у вашій бібліотеці: {myLibrary.GetBookCount()}"); break;
                    case "0":
                        isRunning = false;
                        Console.WriteLine("Вихід з програми. До побачення!");
                        break;
                    default: Console.WriteLine("Невідома команда. Спробуйте ще раз."); break;
                }
            }
        }

        static void AddBookMenu(Library library)
        {
            Console.WriteLine("\n--- Додавання нової книги ---");
            Console.Write("Автор: ");
            string author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author)) author = "Невідомий автор";
            
            Console.Write("Назва: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title)) title = "Без назви";
            
            Console.Write("Видавництво: ");
            string publisher = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(publisher)) publisher = "Невідомо";
            
            Console.Write("Розділ (хобі, белетристика тощо): ");
            string category = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(category)) category = "Загальне";
            
            Console.Write("Оцінка (1-5): ");
            int.TryParse(Console.ReadLine(), out int rating);
            if (rating < 1 || rating > 5) rating = 0;

            library.AddBook(author, title, publisher, category, rating);
            Console.WriteLine("Книгу успішно додано!");
        }

        static void EditBookMenu(Library library)
        {
            Console.Write("\nВведіть ID книги для редагування: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Book book = library.GetBookById(id);
                if (book != null)
                {
                    Console.WriteLine("Залиште поле порожнім, якщо не хочете його змінювати.");
                    Console.Write($"Автор ({book.Author}): ");
                    string author = Console.ReadLine();
                    Console.Write($"Назва ({book.Title}): ");
                    string title = Console.ReadLine();
                    Console.Write($"Оцінка ({book.Rating}): ");
                    int.TryParse(Console.ReadLine(), out int rating);

                    library.EditBook(id, author, title, rating);
                    Console.WriteLine("Книгу оновлено!");
                }
                else Console.WriteLine("Книгу з таким ID не знайдено.");
            }
        }

        static void RemoveBookMenu(Library library)
{
    Console.Write("\nВведіть ID книги для видалення: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        Book book = library.GetBookById(id);
        if (book != null)
        {
            Console.Write($"Ви дійсно хочете видалити книгу '{book.Title}'? (т/н): ");
            string confirm = Console.ReadLine().ToLower();
            
            if (confirm == "т" || confirm == "y")
            {
                library.RemoveBook(id);
                Console.WriteLine("Книгу видалено!");
            }
            else
            {
                Console.WriteLine("Видалення скасовано.");
            }
        }
        else 
        {
            Console.WriteLine("Книгу з таким ID не знайдено.");
        }
    }
    else
    {
        Console.WriteLine("Помилка: ID має бути числом.");
    }
}

        static void SearchMenu(Library library)
        {
            Console.Write("\nВведіть автора або назву для пошуку: ");
            string query = Console.ReadLine() ?? "";
            PrintBooks(library.Search(query), "\nРезультати пошуку:");
        }

        static void MarkBorrowedMenu(Library library)
        {
            Console.Write("\nВведіть ID книги, яку віддаєте: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Кому віддаєте? Введіть ім'я: ");
                string person = Console.ReadLine();
                if (library.MarkAsBorrowed(id, person)) Console.WriteLine($"Відмічено! Тепер книга у {person}.");
                else Console.WriteLine("Книгу з таким ID не знайдено.");
            }
        }

        static void MarkReturnedMenu(Library library)
        {
            Console.Write("\nВведіть ID книги, яку повернули: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (library.MarkAsReturned(id)) Console.WriteLine("Відмічено! Книга знову вдома.");
                else Console.WriteLine("Книгу з таким ID не знайдено.");
            }
        }

        static void FilterMenu(Library library)
        {
            Console.Write("\nВведіть оцінку для фільтру (1-5): ");
            if (int.TryParse(Console.ReadLine(), out int rating))
                PrintBooks(library.FilterByRating(rating), $"\nКниги з оцінкою {rating}:");
        }

        static void PrintBooks(List<Book> books, string title)
        {
            Console.WriteLine(title);
            if (books.Count == 0) { Console.WriteLine("Нічого не знайдено або список порожній."); return; }
            foreach (var book in books) Console.WriteLine(book.ToString());
        }
    }
}

