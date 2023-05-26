using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Завдання 1:
//Створіть клас «П'єса», який має зберігати таку інформацію:
// назва п'єси;
// П.І.Б. автора;
// жанр;
// рік.
//Реалізуйте у класі методи та властивості, необхідні для функціонування класу.
//Додайте до класу деструктор. Напишіть код для тестування функціональності класу. Напишіть код для деструктора.
//Завдання 3:
//Додайте до першого завдання реалізацію інтерфейсу IDisposable.  Напишіть код для тестування нових можливостей.

namespace PlayClass
{
    class Play : IDisposable
    {
        private bool disposedValue;
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }

        public Play(string title, string author, string genre, int year)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Пьеса: {Title}\t Автор: {Author} \t({Year})") ;
            Console.WriteLine("Жанр: " + Genre);
        }

        ~Play() // финализатор
        {
            Console.WriteLine("Finalizator for object: " + Title);
            Dispose(disposing: false);
        }

        protected virtual void Dispose(bool disposing)
        {
            Console.WriteLine("вызов dispose для объекта: " + Title);
            if (!disposedValue) // проверка того, что объект еще не был "очищен"
            {
                if (disposing)
                {
                    Console.WriteLine("dispose освобождение управляемых ресурсов  объекта: " + Title);
                }

                Console.WriteLine("dispose освобождение НЕуправляемых ресурсов  объекта: " + Title);
                disposedValue = true; // флаг - признак "очистки" объекта
            }
            else Console.WriteLine("dispose уже вызывался ранее для этого объекта: " + Title);
        }


        public void Dispose()
        {
            Dispose(disposing: true);
            Console.WriteLine("блокирование вызова финализатора для объекта: " + Title);
            GC.SuppressFinalize(this);
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            // без прямого/автоматического вызова dispose (метод вызывается самим финализатором - для избежания дублирования кода)
            Play play = new Play("Вишневый сад", "Антон Чехов", "Драма", 1904);
            play.PrintInfo();
            Console.WriteLine("Памяти в куче занято (в байтах): {0}", GC.GetTotalMemory(false));
            Console.WriteLine("Имеется поколений: {0}", GC.MaxGeneration + 1);
            Console.WriteLine($"Объект ({play.Title}) в поколении : {GC.GetGeneration(play)}");
            play = null;
            GC.Collect();
            GC.WaitForPendingFinalizers(); // вызов финализатора здесь не гарантируется!
            Console.WriteLine("Памяти в куче занято после вызова GC.Collect (в байтах): {0}", GC.GetTotalMemory(false));

            Console.WriteLine("____________________________________________________________________________________");
            //с использованием косвенного/прямого вызова  dispose
            using (var play2 = new Play("Чайка", "Антон Чехов", "Трагикомедия", 1896))
            {
                play2.PrintInfo();
                Console.WriteLine("Памяти в куче занято (в байтах): {0}", GC.GetTotalMemory(false));
                Console.WriteLine("Имеется поколений: {0}", GC.MaxGeneration + 1);
                Console.WriteLine($"Объект ({play2.Title}) в поколении : {GC.GetGeneration(play2)}");
                play2.Dispose(); // прямой вызов dispose
            }
            GC.Collect();
            Console.WriteLine("____________________________________________________________________________________");
            Console.WriteLine("Памяти в куче занято (в байтах): {0}", GC.GetTotalMemory(false));

        }
    }
}
