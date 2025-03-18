using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProcesowanieZamowienia
{
    public class Functions
    {
        public static void CreateOrder()
        {
            Console.Write("Podaj nazwę produktu: ");
            string? productName = Console.ReadLine();
            if (string.IsNullOrEmpty(productName))
            {
                Console.WriteLine("\nNazwa produktu nie może być pusta!\n");
                return;
            }
            if (!productName.All(char.IsLetter))
            {
                Console.WriteLine("\nNazwa produktu może zawierać tylko litery!\n");
                return;
            }


            Console.Write("\nPodaj kwotę zamówienia: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.Write("\nPodana kwota nie jest poprawna!\n");
                return;
            }


            Console.Write("\nJakiego typu klientem jesteś? (1. Firma, 2. Osoba fizyczna): ");
            CustomerType customerType;
            string? input = Console.ReadLine();

            if (input == "1" || input == "2")
            {
                customerType = (input == "1") ? CustomerType.Firma : CustomerType.OsobaFizyczna;
            }
            else
            {
                Console.WriteLine("\nNiepoprawny wybór!\n");
                return;
            }


            Console.Write("\nPodaj adres dostawy: ");
            string? adress = Console.ReadLine();
            if (string.IsNullOrEmpty(adress))
            {
                Console.WriteLine("\nAdres nie może być pusty!\n");
                return;
            }
            if (!adress.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '/' || c == ' '))
            {
                Console.WriteLine("\nAdres może zawierać tylko litery, cyfry, '-', i '/'.\n");
                return;
            }

            Console.Write("\nPodaj sposób płatności (1. Karta, 2. Gotówka przy odbiorze): ");
            PaymentMethod paymentMethod;
            input = Console.ReadLine();

            if (input == "1" || input == "2")
            {
                paymentMethod = (input == "1") ? PaymentMethod.Karta : PaymentMethod.GotowkaPrzyOdbiorze;
            }
            else
            {
                Console.WriteLine("\nNiepoprawny wybór!\n");
                return;
            }


            OrderStatus orderStatus = OrderStatus.Nowe;

            Order order = new Order(price, productName, customerType, adress, paymentMethod, orderStatus);
            Program.orders.Add(order);

            Console.WriteLine("Zamówienie zostało utworzone!");
        }

        public static void ProcessToWarehouse()
        {
            var order = Program.orders.FirstOrDefault(o => o.OrderStatus == OrderStatus.Nowe);
            if (order == null)
            {
                Console.WriteLine("Nie ma żadnego zamówienia do przekazania do magazynu!");
                return;
            }

            if (order.Price >= 2500 && order.PaymentMethod == PaymentMethod.GotowkaPrzyOdbiorze)
            {
                order.OrderStatus = OrderStatus.Zwrocono;
                Console.WriteLine("Zamówienie zostało zwrócone do klienta z powodu zbyt wysokiej ceny przy płatności gotówką");
            }

            else if (string.IsNullOrEmpty(order.ProductName))
            {
                order.OrderStatus = OrderStatus.Blad;
                Console.WriteLine("Błąd: brak podania nazwy produktu");
            }

            else if (string.IsNullOrEmpty(order.DeliveryAdress))
            {
                order.OrderStatus= OrderStatus.Blad;
                Console.WriteLine("Błąd: brak podania adresu dostawy");
            }

            else
            {
                order.OrderStatus = OrderStatus.WMagazynie;
                Console.WriteLine("Zamówienie zostało pomyślnie przekazane do magazynu!");
            }
        }
        
        public static async Task ProcessToShipping()
        {
            var order = Program.orders.FirstOrDefault(o => o.OrderStatus == OrderStatus.WMagazynie);
            
            if (order == null)
            {
                Console.WriteLine("Brak zamówień do wysyłki");
                return;
            }

            order.OrderStatus = OrderStatus.WWysylce;
            Console.WriteLine("Zamówienie w trakcie wysyłania!");
            await Task.Delay(3000);

            order.OrderStatus = OrderStatus.Wyslane;
            Console.WriteLine("Zamówienie pomyślnie wysłane!");
        }

        public static void ReviewOrders()
        {
            if (Program.orders.Count == 0)
            {
                Console.WriteLine("Brak zamówień do obejrzenia!");
                return;
            }

            Console.WriteLine("\n");

            foreach (var order in Program.orders)
            {
                Console.WriteLine($"Produkt: {order.ProductName}, Kwota: {order.Price}, Klient: {order.CustomerType}, Status: {order.OrderStatus}");
            }
        }
    }
}
