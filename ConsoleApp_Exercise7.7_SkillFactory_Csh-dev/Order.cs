using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Exercise7._7_SkillFactory_Csh_dev
{
    class Order<TDelivery, TStruct> where TDelivery : Delivery //Использование обобщений
    {
        public TDelivery Delivery;

        public int Number { get; } //Использование свойств
        public string Description { get; set; }
        public Customer Customer { get; }  //Использование агрегации классов
        public DateTime Date { get; }
        public decimal TotalAmount { get; }
        public int Count { get; }

        private List<Product<TStruct>> Products;


        public Order()
        {
            Products = new List<Product<TStruct>>();
        }

        //Использование конструкторов классов с параметрами
        public Order(TDelivery delivery, int number, string description, Customer customer, DateTime date, List<Product<TStruct>> products) //Использование агрегации классов (Customer;
        {
            Delivery = delivery;
            Number = number;
            Description = description;
            Customer = customer;
            Date = date;
            Products = products;
            Count = products.Count;
        }

        public Product<TStruct> this[int index] //Использование индексаторов
        {
            get
            {
                return Products[index];
            }
        }

        public void AddProduct(Product<TStruct> product)
        {
            Products.Add(product);
        }
        public void RemoveProduct(Product<TStruct> product)
        {
            Products.Remove(product);
        }
        public void ClearProducts()
        {
            Products.Clear();
        }

        public decimal GetTotalCost()
        {
            decimal total = 0;
            foreach (var product in Products)
            {
                FieldInfo priceProp = product.Info.GetType().GetField("Price");
                if (priceProp != null && priceProp.FieldType == typeof(decimal))
                {
                    total += (decimal)priceProp.GetValue(product.Info);
                }
            }
            return total;
        }
        public decimal GetTotalAmount()
        {
            int total = 0;
            foreach (var product in Products)
            {
                FieldInfo amountProp = product.Info.GetType().GetField("Amount");
                if (amountProp != null && amountProp.FieldType == typeof(int))
                {
                    total += (int)amountProp.GetValue(product.Info);
                }
            }
            return total;
        }
        public virtual void DisplayOrderInfo()
        {
            Console.WriteLine($"Заказ #{Number}");
            Console.WriteLine($"Имя покупателя: {Customer.Name}");
            Console.WriteLine($"Описание: {Description}");
            Console.WriteLine("\nТовары:");
            foreach (var product in Products)
            {
                FieldInfo nameProp = product.Info.GetType().GetField("Name");
                FieldInfo priceProp = product.Info.GetType().GetField("Price");
                FieldInfo amountProp = product.Info.GetType().GetField("Amount");
                if (nameProp != null && nameProp.FieldType == typeof(string)
                    && priceProp != null && priceProp.FieldType == typeof(decimal)
                    && amountProp != null && amountProp.FieldType == typeof(int))
                {
                    Console.WriteLine($" - {nameProp.GetValue(product.Info)}, {priceProp.GetValue(product.Info)} руб. x {amountProp.GetValue(product.Info)} шт.");
                }
            }
            Console.WriteLine($"\nОбщее количество товаров, шт.: {GetTotalAmount()}");
            Console.WriteLine($"Общая стоимость товаров, руб.: {GetTotalCost()}");
            Console.WriteLine($"\nИнформация о доставке");
            Console.WriteLine($"Дата доставки: {Delivery.DeliveryDate}");
            Console.WriteLine($"Адрес доставки: {Delivery.Address}");
            Console.WriteLine($"Выбранный способ доставки: {Delivery.GetType().Name}");
        }
        public void DisplayAddress()
        {
            Console.WriteLine(Delivery.Address);
        }
        public TStruct GetProductInfo(int index) //Использование обобщенных методов
        {
            if (index >= 0 && index < Products.Count)
            {
                return Products[index].Info;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
        public static Order<TDelivery, TStruct> operator +(Order<TDelivery, TStruct> order, Product<TStruct> product)
        {
            order.AddProduct(product);
            return order;
        }

    }
}
