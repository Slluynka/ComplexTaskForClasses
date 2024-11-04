using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTaskForClasses
{
    public interface IProduct
    {
        string Name { get; set; }
        decimal Price { get; set; }
        decimal CalculateDiscount(decimal percentage);
    }
    public abstract class Product : IProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        protected Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва товару не може бути порожньою.");
            if (price < 0)
                throw new ArgumentException("Ціна не може бути від'ємною.");

            Name = name;
            Price = price;
        }

        public decimal CalculateDiscount(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("Знижка повинна бути в межах від 0 до 100.");

            return Price * (percentage / 100);
        }

        public abstract decimal CalculateTotalPrice();
    }
    public class Book : Product
    {
        public int NumberOfPages { get; set; }

        public Book(string name, decimal price, int numberOfPages)
            : base(name, price)
        {
            NumberOfPages = numberOfPages;
        }

        public override decimal CalculateTotalPrice()
        {
            return Price; // Просте ціноутворення для книг
        }
    }
    public class Electronics : Product
    {
        public int MemorySize { get; set; } // в ГБ

        public Electronics(string name, decimal price, int memorySize)
            : base(name, price)
        {
            MemorySize = memorySize;
        }

        public override decimal CalculateTotalPrice()
        {
            return Price; // Просте ціноутворення для електроніки
        }
    }
    public class Clothing : Product
    {
        public string Size { get; set; }

        public Clothing(string name, decimal price, string size)
            : base(name, price)
        {
            Size = size;
        }

        public override decimal CalculateTotalPrice()
        {
            return Price; // Просте ціноутворення для одягу
        }
    }
    public class Order
    {
        public int OrderNumber { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalPrice => CalculateTotalPrice();

        public delegate void OrderStatusChangedHandler(string status);
        public event OrderStatusChangedHandler OrderStatusChanged;

        public Order(int orderNumber)
        {
            OrderNumber = orderNumber;
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public decimal CalculateTotalPrice()
        {
            decimal total = 0;
            foreach (var product in Products)
            {
                total += product.CalculateTotalPrice();
            }
            return total;
        }

        public void ChangeStatus(string status)
        {
            OrderStatusChanged?.Invoke(status);
        }
    }
    public class OrderProcessor
    {
        public void ProcessOrder(Order order)
        {
            // Обробка замовлення (наприклад, розрахунок вартості)
            Console.WriteLine($"Обробка замовлення #{order.OrderNumber}...");
            Console.WriteLine($"Загальна вартість: {order.TotalPrice:C}");

            // Зміна статусу замовлення
            order.ChangeStatus("Замовлення оброблене");
        }
    }
    public class NotificationService
    {
        public void SendNotification(string status)
        {
            Console.WriteLine($"Сповіщення: Статус замовлення змінився на: {status}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо продукти
            Book book1 = new Book("1984", 15.99m, 328);
            Electronics phone = new Electronics("iPhone", 999.99m, 128);
            Clothing shirt = new Clothing("Футболка", 19.99m, "M");

            // Створюємо замовлення
            Order order = new Order(1);
            order.AddProduct(book1);
            order.AddProduct(phone);
            order.AddProduct(shirt);

            // Створюємо обробник замовлення
            OrderProcessor processor = new OrderProcessor();

            // Створюємо сервіс сповіщень
            NotificationService notificationService = new NotificationService();

            // Підписуємося на зміни статусу замовлення
            order.OrderStatusChanged += notificationService.SendNotification;

            // Обробляємо замовлення
            processor.ProcessOrder(order);

            // Виводимо інформацію про замовлення
            Console.WriteLine($"Замовлення #{order.OrderNumber} оброблено.");
        }
    }
}
