using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bankomat
{
    class App
    {
        Bank bank;
        Bankomat bankomat;

        public App()
        {
            bank = new Bank();
            bankomat = new Bankomat();
        }

        string[] menuApp = { "В банк", "В банкомат", "Выход" };
        public void MainMenu()
        {
            bank.LoadFile();
            while (true)
            {
                Console.Clear();
                int c = Menu.VerticalMenu(menuApp);
                switch (c)
                {
                    case 0:
                        bank.MainMenu();
                        break;
                    case 1:
                        bankomat.MainMenu(bank);
                        break;
                    case 2:
                        return;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    class Bankomat
    {
        Bank bank;
        string[] menuBankomat = { "Снять сумму", "Положить сумму", "Баланс", "Выход" };

        public Account EnterAcc()
        {
            Console.Clear();
            Console.WriteLine("Введите номер счета: ");
            string str = Console.ReadLine();

            while (str.Length <= 0) str = Console.ReadLine();
            int numAcc = int.Parse(str);

            return checkAcc(numAcc);
        }

        Account checkAcc(int numAcc)
        {
            if (bank.checkAcc(numAcc) == null)
            {
                Console.WriteLine("Такого номера счета нет! Попробуйте еще раз!");
                Thread.Sleep(2000);
                return null;
            }
            else
                return bank.checkAcc(numAcc);
        }

        public bool EnterPIN(Account acc)
        {
            Console.Write("Введите ПИН: ");
            int pin = Convert.ToInt32(Console.ReadLine());
            if (pin == acc.PIN)
                return true;
            return false;
        }

        public void MainMenu(Bank bank)
        {
            this.bank = bank;
            bank.LoadFile();
            Account acc = EnterAcc();
            if (acc != null)
            {
                int Count = 0;
                bool p = true;
                while (p)
                {
                    if (Count == 3)
                    {
                        Console.WriteLine("Использовано все три попытки!");
                        Thread.Sleep(2000);
                        return;
                    }
                    else if (EnterPIN(acc))
                        p = false;
                    else
                    {
                        p = true;
                        Count++;
                    }
                }
            }
            else
            {
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Account: {acc.Acc}");
                Console.WriteLine("===================");
                int c = Menu.VerticalMenu(menuBankomat);
                switch (c)
                {
                    case 0:
                        acc.Withdraw();
                        bank.SaveFile();
                        break;
                    case 1:
                        acc.addSum();
                        bank.SaveFile();
                        break;
                    case 2:
                        acc.GetBalance();
                        break;
                    case 3:
                        return;
                    default:
                        break;
                }
            }
        }
    }

    class Bank
    {
        int countAcc = 0;
        Account[] accounts;
        string[] menuBank = { "Открыть счет", "Закрыть счет", "Печать счетов", "Выход" };

        public Account checkAcc(int numAcc)
        {
            foreach (var item in accounts)
            {
                if (item.Acc == numAcc)
                    return item;
            }
            return null;
        }



        public void AddAccount()
        {
            Array.Resize(ref accounts, ++countAcc);
            int pin = (new Random()).Next(1000, 10000);
            int accNum = (new Random()).Next(262000, 262099);
            Account acc = new Account(accNum, pin);
            accounts[accounts.Length - 1] = acc;

            Console.WriteLine($"Account Number: {accNum}  PIN: {pin}");
            Console.Read();
            SaveFile();
        }

        public void DellAccount()
        {
            Console.Clear();
            Console.WriteLine("Введите номер счет для закрытия: ");
            int acc = int.Parse(Console.ReadLine());
            Account[] newAcc = new Account[accounts.Length - 1];
            int del = 0;
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Acc == acc)
                    del = i;
            }
            for (int i = 0; i < del; i++)
            {
                newAcc[i] = accounts[i];
            }
            for (int i = del; i < newAcc.Length; i++)
            {
                newAcc[i] = accounts[i + 1];
            }
            accounts = newAcc;
            Console.WriteLine();
            Console.WriteLine("Номер счета закрыт!!!");
            SaveFile();
        }

        public void MainMenu()
        {
            Console.Clear();
            int c = Menu.VerticalMenu(menuBank);
            switch (c)
            {
                case 0:
                    AddAccount();
                    break;
                case 1:
                    DellAccount();
                    break;
                case 2:
                    Print();
                    break;
                case 3:
                    return;
                default:
                    break;
            }
        }

        public void SaveFile()
        {
            BinaryFormatter formater = new BinaryFormatter();
            using (FileStream fs = new FileStream("accounts.data", FileMode.Create))
            {
                formater.Serialize(fs, this.accounts);
            }
        }

        public void LoadFile()
        {
            if (System.IO.File.Exists("accounts.data"))
            {
                BinaryFormatter formater = new BinaryFormatter();
                using (FileStream fs = new FileStream("accounts.data", FileMode.OpenOrCreate))
                {
                    this.accounts = (Account[])formater.Deserialize(fs);
                }
            }
            else
            {
                Console.WriteLine("Такого файла нет!");
            }
        }

        public void Print()
        {
            Console.Clear();
            Console.WriteLine("\tБАЗА СЧЕТОВ БАНКА");
            Console.WriteLine("\t-----------------");
            Console.WriteLine();
            Console.WriteLine("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550" +
                              "\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.WriteLine("\u2551 Account \u2551 PIN \u2551 Summa \u2551");
            Console.WriteLine("\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550" +
                              "\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550");
            for (int i = 0; i < this.accounts.Length; i++)
            {
                Console.WriteLine($"\u2551 {accounts[i].Acc} \u2551 {accounts[i].PIN} \u2551 {accounts[i].Summa} \u2551");
                Console.WriteLine("\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550" +
                                  "\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550");
            }

            Console.ReadLine();
        }
    }

    [Serializable]
    class Account
    {
        public int Acc { get; private set; }
        public int PIN { get; private set; }
        public int Summa { get; set; }
        public Account(int acc, int pin)
        {
            Acc = acc;
            PIN = pin;
        }

        public void Withdraw()
        {
            Console.WriteLine("Введите сумму: ");
            string str = Console.ReadLine();
            while (str.Length <= 0) str = Console.ReadLine();
            int sum = int.Parse(str);
            if (sum < Summa)
                Summa -= sum;
        }

        public void GetBalance()
        {
            Console.WriteLine($"Баланс: {Summa}");
            Console.Read();
        }

        public void addSum()
        {
            Console.WriteLine("Введите сумму: ");
            string str = Console.ReadLine();
            while (str.Length <= 0) str = Console.ReadLine();
            int sum = int.Parse(str);
            Summa += sum;
        }




    }
}
