// See https://aka.ms/new-console-template for more information
using System.Reflection;

Console.WriteLine("Hello!\n");
List<Product<ProductInCartInfo>> cart = Shop.AddProductsToCart();
Console.WriteLine("\n--------ОФОРМЛЕНИЕ ЗАКАЗА--------");
Console.WriteLine("Введите Ваше имя:");
Customer Customer = new Customer(Console.ReadLine());
Delivery delivery;
int deliveryChoosen = Shop.ChooseDeliverType();
switch (deliveryChoosen)
{
    case 1:
        HomeDelivery homeDelivery = new HomeDelivery();
        homeDelivery.SetDeliveryAdress();
        homeDelivery.SetDeliveryTime();
        homeDelivery.SetDeliveryDate();
        delivery = homeDelivery;
        break;
    case 2:
        PickPointDelivery pickPointDelivery = new PickPointDelivery();
        pickPointDelivery.SetPickPoint();
        pickPointDelivery.SetDeliveryDate();
        delivery = pickPointDelivery;
        break;
    case 3:
        ShopDelivery shopDelivery = new ShopDelivery();
        shopDelivery.SetShopLocation();
        shopDelivery.SetDeliveryDate();
        delivery = shopDelivery;
        break;
    default:
        throw new ArgumentException("Неверный тип доставки.");
}
Order<Delivery, ProductInCartInfo> order = new Order<Delivery, ProductInCartInfo>(delivery, 1, "Заказ", Customer, DateTime.Now, cart);
Console.WriteLine("\nЗАКАЗА СОЗДАН");
Console.WriteLine("\nВаш заказ:");
order.DisplayOrderInfo();
Console.WriteLine("\nБлагодарим Вас за заказ и поздравляем с покупкой!");
Console.WriteLine("До свидания!\n");
order.DisplayOrderInfo();
Console.ReadKey();

//Добавление еще одного продукта в заказ
Product<ProductInfo> workProduct = Shop.GetProduct(0);
ProductInCartInfo info = new ProductInCartInfo { Name = workProduct.Info.Name, Price = workProduct.Info.Price, Amount = 1 };
Product<ProductInCartInfo> product = new Product<ProductInCartInfo>(info);
order = order + product;
order.DisplayOrderInfo();
Console.ReadKey();


abstract class Delivery //Использование абстрактных классов
{
    private protected string address; //использование модификаторов элементов класса(чтобы важные поля не были доступны для полного контроля извне, использование protected);
    public string Address
    {
        get { return address; }
        set { address = value; }
    }

    private DateOnly deliveryDate;
    public DateOnly DeliveryDate
    {
        get { return deliveryDate; }
        set { deliveryDate = value; }
    }

    public void SetDeliveryAdress()
    {
        Console.WriteLine("\nВведите адрес доставки:");
        address = Console.ReadLine();
    }

    public void SetDeliveryDate()
    {
        DateOnly date;
        Console.WriteLine("\nВведите дату доставки:");
        bool exit = false;
        string input;
        while (!exit)
        {
            input = Console.ReadLine();
            if (DateOnly.TryParse(input, out date))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    deliveryDate = date;
                    exit = true;
                }
                else
                {
                    Console.WriteLine("На выходные доставка не осуществляется");
                }
            }
            else
            {
                Console.WriteLine("Некорректная дата доставки\n");
            }
        }
    }
}

class HomeDelivery : Delivery
{
    private TimeOnly deliveryTime;
    public TimeOnly DeliveryTime
    {
        get { return deliveryTime; }
        set {deliveryTime = value; }
    }
    public void SetDeliveryTime()
    {
        TimeOnly time;
        Console.WriteLine("\nВведите время доставки:");
        bool exit = false;
        string input;
        while (!exit)
        {
            input = Console.ReadLine();
            if (TimeOnly.TryParse(input, out time))
            {
                if (time.Hour > 9 && time.Hour <= 21)
                {
                    deliveryTime = time;
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Доставка осуществляется с 9:00 до 21:00");
                }
            }
            else
            {
                Console.WriteLine("Некорректное время доставки\n");
            }
        }
    }
}

class PickPointDelivery : Delivery
{
    private string pickPoint;

    public void SetPickPoint()
    {
        Console.WriteLine("\nВыберетите пункт выдачи из списка:");
        pickPoint = Console.ReadLine();
    }
    public void GetPickPoint()
    {
        Console.WriteLine(pickPoint);
    }
}

class ShopDelivery : Delivery
{
    private string shopLocation;
    public void SetShopLocation()
    {
        Console.WriteLine("\nВыберите магазин из списка:");
        shopLocation = Console.ReadLine();
    }
    public void GetShopLocation()
    {
        Console.WriteLine(shopLocation);
    }
}

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
    public static Order<TDelivery,TStruct> operator +(Order<TDelivery,TStruct> order,Product<TStruct> product)
    {
        order.AddProduct(product);
        return order;
    }

}
class HomeDeliveryOrder<TStruct> : Order<HomeDelivery, TStruct> //Использование наследования обобщений
{
    public string Email { get; set; }
    public string Phone { get; set; }

    public override void DisplayOrderInfo()
    {
        base.DisplayOrderInfo();
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Телефон: {Phone}");
    }
}


//Мой классы
public enum DeliveryType
{
        Home,
        PickPoint,
        Shop
 };
public struct ProductInfo
{
    public string Name;
    public decimal Price;
}
public struct ProductInCartInfo
{
    public string Name;
    public int Amount;
    public decimal Price;
}
public class Product<TStruct>
{
    public TStruct Info { get; set; }

    public Product(TStruct info)
    {
        Info = info;
    }
    public string GetName()
    {
        FieldInfo nameProp = Info.GetType().GetField("Name");
        if (nameProp == null)
        {
            throw new ArgumentException("FieldInfo doesn't contain Name");
        }
        return nameProp.GetValue(Info)?.ToString();
    }
    public decimal GetPrice()
    {
        FieldInfo priceProp = Info.GetType().GetField("Price");
        if (priceProp == null)
        {
            throw new ArgumentException("FieldInfo doesn't contain Price");
        }
        return (decimal)priceProp.GetValue(Info);
    }
    public decimal GetAmount()
    {
        FieldInfo priceProp = Info.GetType().GetField("Amount");
        if (priceProp == null)
        {
            throw new ArgumentException("FieldInfo doesn't contain Amount");
        }
        return (decimal)priceProp.GetValue(Info);
    }


}
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
abstract class Person
{
    private protected string name;
    public string Name { get { return name; } set { name = value; } }

    private protected string phone;
    public string Phone { get { return phone; } set { phone = value; } }

    private protected string email;
    public string Email { get { return email; } set { email = value; } }
    public Person(string Name) { name = Name; }
    public void DisplayName()
    {
        Console.WriteLine(name);
    }
}
class Customer : Person
{
    public Customer(string name) : base(name) { }
}
