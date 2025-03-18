using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProcesowanieZamowienia
{
    internal class Program
    {
        public static List<Order> orders = new List<Order>();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n1. Utwórz zamówienie");
                Console.WriteLine("2. Przekaż do magazynu");
                Console.WriteLine("3. Przekaż do wysyłki");
                Console.WriteLine("4. Przegląd zamówień");
                Console.WriteLine("5. Wyjście");

                switch (Console.ReadLine())
                {
                    case "1": Functions.CreateOrder(); break;
                    case "2": Functions.ProcessToWarehouse(); break;
                    case "3": Functions.ProcessToShipping(); break;
                    case "4": Functions.ReviewOrders(); break;
                    case "5": return;
                    default: Console.WriteLine("Niepoprawny wybór, wybierz jedną z podanych opcji!"); break;
                }
            }
        }
    }
}
