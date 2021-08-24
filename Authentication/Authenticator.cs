using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Authentication
{
    class Authenticator
    {
        public static List<Account> acc = new List<Account>();
        public void AddAcc(string inputFirst, string inputLast, string inputPass)
        {
            string fullName = $"{inputFirst} {inputLast}";
            string username = $"{inputFirst.Substring(0, 2)}{inputLast.Substring(0, 2)}";
            bool match = true;
            Random rnd = new Random();
            if (acc.Count > 0)
            {
                do
                {
                    match = false;
                    for (int i = 0; i < acc.Count; i++)
                    {
                        int random = rnd.Next(0, 100);
                        if (username == acc[i].Username)
                        {
                            username = username + random;
                            match = true;
                        }
                    }
                } while (match == true);
            }
            acc.Add(new Account { Name = fullName, Username = username, Password = inputPass });
        }
        public void AccountsList()
        {
            for (int i = 0; i < acc.Count; i++)
            {
                Console.WriteLine("===================");
                Console.WriteLine($"NAME : {acc[i].Name}\nUSERNAME : {acc[i].Username}\nPASSWORD : {acc[i].Password}");
                Console.WriteLine("===================\n");
            }
        }
        public bool LoginAuth(string username, string password)
        {
            bool notFound = true;
            if (acc.Count > 0)
            {
                for (int i = 0; i < acc.Count; i++)
                {
                    if (username == acc[i].Username)
                    {
                        notFound = false;
                        if (BCrypt.Net.BCrypt.Verify(password,acc[i].Password))
                        {
                            Console.WriteLine("MESSAGE : LOGIN SUCCESFULLY !!!");
                            Console.ReadKey();
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("MESSAGE : WRONG PASSWORD !!!");
                            Console.ReadKey();
                        }
                    }
                }
                if (notFound == true) 
                {
                    Console.WriteLine("MESSAGE : USERNAME NOT FOUND !!!");
                    Console.ReadKey();
                }   
            }
            else
            {
                Console.WriteLine("MESSAGE : USERNAME NOT FOUND !!!");
                Console.ReadKey();
            }
            return false;
        }
        public void Match(string userSearch)
        {
            bool match = false;
            int searchcount = 0;
            for (int i = 0; i < acc.Count; i++)
            {
                match = acc[i].Username.Contains(userSearch, System.StringComparison.CurrentCultureIgnoreCase);
                if (match)
                {
                    Console.WriteLine("===================");
                    Console.WriteLine($"NAME : {acc[i].Name}\nUSERNAME : {acc[i].Username}\nPASSWORD : {acc[i].Password}");
                    Console.WriteLine("===================\n");
                    searchcount++;
                }
            }
            Console.WriteLine($"FOUND : {searchcount} DATA");
            Console.ReadKey();
        }

        public int UserInfo(string username, string password)
        {
            for (int i = 0; i < acc.Count; i++)
            {
                if (username == acc[i].Username && password == acc[i].Password)
                    return i;
            }
            return 0;
        }
        public void PrintUserInfo(int index) 
        {
            Console.WriteLine($"Name : {acc[index].Name}");
            Console.WriteLine($"Username : {acc[index].Username}");
            Console.WriteLine($"Password : {acc[index].Password}");
        }

        public void Delete(int index)
        {
            acc.RemoveAt(index);
        }

        public void Update(int index,string name, string username, string password)
        {
            bool match = false;
            Random rnd = new Random();
            do
            {
                match = false;
                for (int i = 0; i < acc.Count; i++)
                {
                    int random = rnd.Next(0, 100);
                    if (username == acc[i].Username)
                    {
                        username = username + random;
                        match = true;
                    }
                }
            } while (match == true);
            acc[index].Name = name;
            acc[index].Username = username;
            acc[index].Password = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
