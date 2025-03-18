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
            int Id = Program.orders.Count > 0 ? Program.orders.Max(o => o.Id) + 1 : 1;


            Console.Write("\nPodaj nazwę produktu: ");
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

            Order order = new Order(Id, price, productName, customerType, adress, paymentMethod, orderStatus);
            Program.orders.Add(order);

            Console.WriteLine("\nZamówienie zostało utworzone!\n");
        }

        public static void ProcessToWarehouse()
        {
            var orders = Program.orders.Where(o => o.OrderStatus == OrderStatus.Nowe).ToList();
            if (orders.Count == 0)
            {
                Console.WriteLine("Nie ma żadnego zamówienia do przekazania do magazynu!");
                return;
            }

            Console.WriteLine("\nDostępne zamówienia możliwe do przekazania do magazynu: ");
            foreach (var order in orders)
            {
                Console.WriteLine($"ID: {order.Id}, Produkt: {order.ProductName}, Kwota: {order.Price}, Klient: {order.CustomerType}, Status: {order.OrderStatus}");
            }

            Console.Write("\nPodaj ID zamówienia do przekazania do magazynu: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("\nNiepoprawne ID zamówienia!\n");
                return;
            }

            var orderToProcess = orders.FirstOrDefault(o => o.Id == orderId);

            if (orderToProcess == null)
            {
                Console.WriteLine("\nNie znaleziono zamówienia o podanym ID lub zamówienie nie jest w statusie 'Nowe'!");
                return;
            }

            if (orderToProcess.Price >= 2500 && orderToProcess.PaymentMethod == PaymentMethod.GotowkaPrzyOdbiorze)
            {
                orderToProcess.OrderStatus = OrderStatus.Zwrocono;
                Console.WriteLine("Zamówienie zostało zwrócone do klienta z powodu zbyt wysokiej ceny przy płatności gotówką");
            }

            else if (string.IsNullOrEmpty(orderToProcess.ProductName))
            {
                orderToProcess.OrderStatus = OrderStatus.Blad;
                Console.WriteLine("Błąd: brak podania nazwy produktu");
            }

            else if (string.IsNullOrEmpty(orderToProcess.DeliveryAdress))
            {
                orderToProcess.OrderStatus= OrderStatus.Blad;
                Console.WriteLine("Błąd: brak podania adresu dostawy");
            }

            else
            {
                orderToProcess.OrderStatus = OrderStatus.WMagazynie;
                Console.WriteLine("Zamówienie zostało pomyślnie przekazane do magazynu!");
            }
        }
        
        public static async Task ProcessToShipping()
        {
            var orders = Program.orders.Where(o => o.OrderStatus == OrderStatus.WMagazynie).ToList();

            if (orders.Count == 0)
            {
                Console.WriteLine("Brak zamówień w statusie 'WMagazynie' do wysyłki.");
                return;
            }

            Console.WriteLine("\nDostępne zamówienia do wysyłki:");

            foreach (var order in orders)
            {
                Console.WriteLine($"ID: {order.Id}, Produkt: {order.ProductName}, Kwota: {order.Price}, Klient: {order.CustomerType}, Status: {order.OrderStatus}");
            }

            Console.Write("\nPodaj ID zamówienia do wysyłki: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("\nNiepoprawne ID zamówienia!\n");
                return;
            }

            var orderToShip = orders.FirstOrDefault(o => o.Id == orderId);
            if (orderToShip == null)
            {
                Console.WriteLine("\nNie znaleziono zamówienia o podanym ID lub zamówienie nie jest w statusie 'WMagazynie'!");
                return;
            }

            orderToShip.OrderStatus = OrderStatus.WWysylce;
            Console.WriteLine("Zamówienie w trakcie wysyłania!");
            await Task.Delay(3000);

            orderToShip.OrderStatus = OrderStatus.Wyslane;
            Console.WriteLine("\nZamówienie pomyślnie wysłane!");
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
                Console.WriteLine($"ID: {order.Id}, Produkt: {order.ProductName}, Kwota: {order.Price}, Klient: {order.CustomerType}, Status: {order.OrderStatus}");
            }
        }
    }
}
