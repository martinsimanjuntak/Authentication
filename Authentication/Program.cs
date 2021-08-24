using System;

namespace Authentication
{
    class Program : Menu
    {
        static Menu mainMenu = new Menu();
        static void Main(string[] args)
        {
            mainMenu.MainMenu();
        }
    }
}
