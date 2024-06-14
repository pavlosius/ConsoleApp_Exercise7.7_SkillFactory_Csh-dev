using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Exercise7._7_SkillFactory_Csh_dev
{
    static class Shop //Использование статических классов
    {
        public static int MaxProductsCount = 5; //Использование статических элементов;

        static private List<Product<ProductInfo>> productList = new List<Product<ProductInfo>>()
    {
        new Product<ProductInfo>(new ProductInfo { Name = "Мяч футбольный", Price = 900m }),
        new Product<ProductInfo>(new ProductInfo { Name = "Мяч воллейбольный", Price = 600m }),
        new Product<ProductInfo>(new ProductInfo { Name = "Мяч баскетбольный", Price = 1100m }),
    };
        static public List<Product<ProductInfo>> ProductList       //Использование свойств
        {
            get { return productList; }
        }
        static public List<Product<ProductInfo>> GetProductList()
        {
            return productList;
        }
        static public Product<ProductInfo> GetProduct(int index)
        {
            return productList[index];
        }

        static public void DisplayProductsList()
        {
            Console.WriteLine(String.Format("{0,-5} {1,-20} {2,0}", "Номер", "Название товара", "Стоимость, руб."));
            int index = 1;
            foreach (Product<ProductInfo> product in productList)
            {
                Console.WriteLine(String.Format("{0,-5} {1,-20} {2,0}", index, product.Info.Name, product.Info.Price));
                index++;
            }
        }
        static public List<Product<ProductInCartInfo>> AddProductsToCart()
        {
            List<Product<ProductInCartInfo>> cart = new List<Product<ProductInCartInfo>>();
            Console.WriteLine("Посмотрите пожалуйста на список товаров, доступных для покупки:\n");
            List<Product<ProductInfo>> products = GetProductList();
            bool exit = false;
            int number;
            string input;
            while (!exit)
            {
                if (cart.Count <= MaxProductsCount)
                {
                    Console.WriteLine("Список товаров:");
                    DisplayProductsList();
                    Console.Write("Выход - команда закончить выбор\n");
                    Console.Write("\nВыберите номер товара: ");
                    input = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Достигнуто максимальное число товров в корзине");
                    input = "ВЫХОД";
                }
                if (int.TryParse(input, out number) && number > 0 && number <= products.Count)
                {
                    Console.Write("Введите количество: ");
                    input = Console.ReadLine();
                    int amount;
                    if (int.TryParse(input, out amount) && amount > 0)
                    {
                        ProductInCartInfo info = new ProductInCartInfo { Name = products[number - 1].GetName(), Price = products[number - 1].GetPrice(), Amount = amount };
                        Product<ProductInCartInfo> product = new Product<ProductInCartInfo>(info);
                        cart.Add(product);
                        Console.WriteLine($"Товар {products[number - 1].GetName()} ({products[number - 1].GetPrice()} руб.) добавлен в корзину в количестве: {amount} шт.");
                    }
                    else
                    {
                        Console.WriteLine("Некорректное количество товара\n");
                    }
                }
                else if (input.ToUpper().Contains("ВЫХОД"))
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Некорректный выбор товара");
                }
            }
            return cart;
        }

        static public int ChooseDeliverType()
        {
            Console.WriteLine("\nДоступны следующие способы доставки:\n");
            int i = 1;
            foreach (DeliveryType item in (DeliveryType[])Enum.GetValues(typeof(DeliveryType)))
            {
                Console.WriteLine(i + " " + item);
                i++;
            }
            bool isChosen = false;
            int deliveryChoosen = 0;
            do
            {
                Console.WriteLine("\nВведите номер способа доставки:");
                string answer = Console.ReadLine();
                if (int.TryParse(answer, out int j))
                {
                    if (j > 0 && j <= Enum.GetNames(typeof(DeliveryType)).Length)
                    {
                        deliveryChoosen = j;
                        isChosen = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неправильный выбор");
                    }
                }
                else
                {
                    Console.WriteLine("Неправильный выбор");
                }
            } while (!isChosen);
            return deliveryChoosen;
        }
    }
}
