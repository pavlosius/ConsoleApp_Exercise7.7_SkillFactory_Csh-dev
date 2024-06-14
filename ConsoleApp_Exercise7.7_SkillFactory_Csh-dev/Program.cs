// See https://aka.ms/new-console-template for more information
using ConsoleApp_Exercise7._7_SkillFactory_Csh_dev;
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



//Мои классы
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



