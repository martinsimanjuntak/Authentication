using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication
{
    class Menu
    {
        static Authenticator auth = new Authenticator();
        static bool loggedIn = false;
        static string user = "";
        static string pass = "";
        public void AddAccount()
        {
            start:
            Console.Clear();
            Console.WriteLine("==CREATE USER==");
            Console.Write("Firstname : ");
            string namaDepan = Console.ReadLine();
            Console.Write("Lastname : ");
            string namaBelakang = Console.ReadLine();
            Console.Write("Password : ");
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine("");
            string passHash = BCrypt.Net.BCrypt.HashPassword(pass);
            try
            {
                auth.AddAcc(namaDepan, namaBelakang, passHash);
            }
            catch (Exception) 
            {
                Console.WriteLine("ERROR : Input Not Valid");
                Console.ReadKey();
                goto start;
            }
        }
        public void List()
        {
            Console.Clear();
            Console.WriteLine("==SHOW USER==");
            auth.AccountsList();
            Console.ReadKey();
        }
        public void Search()
        {
            start:
            Console.Clear();
            Console.WriteLine("==SEARCH USER==");
            Console.Write("Search : ");
            string user = Console.ReadLine();
            if (string.IsNullOrEmpty(user)) 
            {
                Console.WriteLine("Cant be Empty");
                Console.ReadKey();
                goto start;
            }
            else
                auth.Match(user);
            Console.ReadKey();
        }
        public void Login()
        {
            if (Menu.loggedIn == false) 
            {
                Console.Clear();
                Console.WriteLine("==LOGIN==");
                Console.Write("USERNAME : ");
                user = Console.ReadLine();
                Console.Write("PASSWORD : ");
                pass = string.Empty;
                ConsoleKey key;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        Console.Write("\b \b");
                        pass = pass[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        pass += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);
                Console.WriteLine("");
                Menu.loggedIn = auth.LoginAuth(user, pass);
                if(Menu.loggedIn)
                    UserMenu(user, pass);
            }
            else
                UserMenu(user, pass);
        }
        public void UserMenu(string username, string password) 
        {
            int inputUserMenu = 0;
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("==USER INFO==");
                    int userIndex = auth.UserInfo(username, password);
                    auth.PrintUserInfo(userIndex);
                    Console.WriteLine("=============");
                    Console.WriteLine("What do you want to do : ");
                    Console.WriteLine("1. Update\n2. Delete\n3. Logout\n4. Back to Main Menu");
                    Console.Write("Input : ");
                    inputUserMenu = Convert.ToInt32(Console.ReadLine());
                    switch (inputUserMenu)
                    {
                        case 1:
                            UpdateMenu(userIndex);
                            break;
                        case 2:
                            auth.Delete(userIndex);
                            Menu.loggedIn = false;
                            break;
                        case 3:
                            Menu.loggedIn = false;
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Input Error");
                    Console.ReadKey();
                }
            } while (inputUserMenu < 2 || inputUserMenu > 4);  
        }
        public void UpdateMenu(int index) 
        {
            start:
            Console.Clear();
            Console.WriteLine("==EDIT USER==");
            Console.Write("NEW Firstname : ");
            string namaDepan = Console.ReadLine();
            Console.Write("NEW Lastname : ");
            string namaBelakang = Console.ReadLine();
            Console.Write("NEW Username : ");
            string username = Console.ReadLine();
            Console.Write("NEW Password : ");
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine("");
            if (string.IsNullOrEmpty(namaDepan) || string.IsNullOrEmpty(namaBelakang) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass))
            {
                Console.WriteLine("Input can't be empty");
                goto start;
            }
            else 
            {
                string fullName = $"{namaDepan} {namaBelakang}";
                auth.Update(index, fullName, username, pass);
            }  
        }
        public void MainMenu() 
        {
            int input = 0;
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("==BASIC AUTHENTICATION==");
                    Console.WriteLine($"1. Create User\n2. Show User\n3. Search\n4. Login\n5. Exit");
                    Console.Write("Input : ");
                    input = Convert.ToInt32(Console.ReadLine());
                    switch (input)
                    {
                        case 1:
                            AddAccount();
                            break;
                        case 2:
                            List();
                            break;
                        case 3:
                            Search();
                            break;
                        case 4:
                            Login();
                            break;
                    }
                }
                catch(Exception)
                {
                    Console.WriteLine("Input Error");
                    Console.ReadKey();
                }
            } while (input != 5);
        }
    }
}
