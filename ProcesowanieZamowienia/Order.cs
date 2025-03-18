using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesowanieZamowienia
{
    enum OrderStatus { Nowe, WMagazynie, WWysylce, Wyslane, Zwrocono, Blad, Zamkniete }
    enum PaymentMethod { Karta, GotowkaPrzyOdbiorze }
    enum CustomerType { Firma, OsobaFizyczna }
    class Order
    {
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public CustomerType CustomerType { get; set; }
        public string DeliveryAdress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Nowe;



        public Order(decimal price, string productName, CustomerType customerType, string deliveryAdress, PaymentMethod paymentMethod, OrderStatus orderStatus)
        {
            Price = price;
            ProductName = productName;
            CustomerType = customerType;
            DeliveryAdress = deliveryAdress;
            PaymentMethod = paymentMethod;
            OrderStatus = orderStatus;
        }


    }
}
