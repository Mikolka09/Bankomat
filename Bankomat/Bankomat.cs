using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            Account acc = EnterAcc();
            if (acc != null)
                EnterPIN(acc);
          else
            {

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
                        break;
                    case 1:
                        acc.addSum();
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
        string[] menuBank = { "Открыть счет", "Закрыть счет", "Выход" };

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
                    //bankomat.Menu();
                    break;
                case 2:
                    return;
                    break;
                default:
                    break;
            }
        }
    }


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
