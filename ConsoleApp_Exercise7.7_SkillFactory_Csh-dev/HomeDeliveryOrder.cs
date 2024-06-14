using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Exercise7._7_SkillFactory_Csh_dev
{
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
}
