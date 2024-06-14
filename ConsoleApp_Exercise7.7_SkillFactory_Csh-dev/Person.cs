using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Exercise7._7_SkillFactory_Csh_dev
{
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
}
