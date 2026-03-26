namespace ATM;

using System.Globalization;
using System.Security.Cryptography;

public static class ConsoleRunner
{
    public static void Run(AtmService atm, List<Card> cardList)
    {
        bool running = true;

        while (running)
        {
            if (!atm.HasCardInserted)
            {
                running = ShowWelcomeMenu(atm, cardList);
            }
            else if (!atm.IsAuthenticated)
            {
                ShowPinMenu(atm);
            }
            else
            {
                ShowMainMenu(atm);
            }
        }

        Console.WriteLine("Tack och hej!");

    }

    private static bool ShowWelcomeMenu(AtmService atm, List<Card> cardList)
    {
        Console.WriteLine();
        Console.WriteLine("=== BANKOMAT ===");
        Console.WriteLine("1. Mata in kort");
        Console.WriteLine("2. Aktivera kort");
        Console.WriteLine("3. Skapa konto");
        Console.WriteLine("0. Avsluta");
        Console.Write("\nVal: ");

        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                {
                    Card? cardToInsert = null;
                    Console.Write("\nAnge ditt kortnummer (8 siffror): ");
                    string? cardInput = Console.ReadLine();

                    if (!atm.CheckCardFormat(cardInput))
                    {
                        Console.Write("Fel inmatning. Tryck [ENTER] för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }

                    string formattedCard = cardInput.Insert(4, "-");

                    if (atm.CardStatus(formattedCard) == "active")
                    {
                        cardToInsert = atm.GetCard(formattedCard);
                    }
                    else if (atm.CardStatus(formattedCard) == "inactive")
                    {
                        Console.Write("Kortet är inte aktiverat. Tryck [ENTER] för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }
                    else
                    {
                        Console.Write("Inget kort hittat. Tryck [ENTER] för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }

                    if (cardToInsert != null)
                    {
                        atm.InsertCard(cardToInsert);
                        Console.WriteLine("Kort inmatat.");
                    }
                    return true;

                }

            case "2":
                {
                    Console.Write("\nAnge ditt kortnummer (8 siffror): ");
                    string? cardInput = Console.ReadLine();

                    if (!atm.CheckCardFormat(cardInput))
                    {
                        Console.Write("Fel inmatning. Tryck [ENTER] för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }

                    string formattedCard = cardInput.Insert(4, "-");

                    if (atm.CardStatus(formattedCard) == "active")
                    {
                        Console.Write("Kortet är redan aktiverat. Tryck [ENTER] för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }
                    else if (atm.CardStatus(formattedCard) == "notfound")
                    {
                        Console.Write("Inget kort hittat. Tryck [ENTER] för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }

                    Card? cardToActivate = atm.GetCard(formattedCard);

                    bool createPin = true;
                    while (createPin)
                    {
                        Console.Write("\nAnge din pinkod (4 siffror). Tryck [X] + [ENTER] för att avbryta: ");
                        string? newPin = Console.ReadLine();
                        if (newPin.ToLower().Trim() == "x")
                        {
                            Console.Write("\nKort aktivering avbröts. Tryckt [ENTER] för att fortsätta ");
                            Console.ReadLine();
                            createPin = false;
                        }
                        else if (atm.CheckPinFormat(newPin))
                        {
                            Console.Write("\nAnge din pinkod igen: ");
                            string? repeatPin = Console.ReadLine();
                            if (atm.CheckPinFormat(repeatPin))
                            {
                                if (atm.CheckPinMatches(newPin, repeatPin))
                                {
                                    Console.Write("\nKort aktiverat! Tryck [ENTER] för att fortsätta. ");
                                    Console.ReadLine();
                                    cardToActivate.SetPinCode(repeatPin);
                                    cardToActivate.ActivateCard();
                                    createPin = false;
                                }
                                else
                                {
                                    Console.Write("Fel inmatning. Tryck [ENTER] för att fortsätta. ");
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                Console.Write("Fel inmatning. Tryck [ENTER] för att fortsätta. ");
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.Write("Fel inmatning. Tryck [ENTER] för att fortsätta. ");
                            Console.ReadLine();
                        }
                    }
                    return true;
                }

            case "3":

                string cardNumber;
                do
                {
                    cardNumber = RandomNumberGenerator.GetInt32(0, 100000000).ToString("D8").Insert(4, "-");
                }
                while (atm.CardStatus(cardNumber) != "notfound");

                atm.CreateAccount(cardNumber, "", cardList);

                Console.WriteLine($"Ditt konto har skapats! Ditt kortnummer är: {cardNumber}. Kom ihåg det!");
                Console.Write("Tryck [ENTER] för att fortsätta.");
                Console.ReadLine();
                return true;

            case "0":
                return false;

            default:
                Console.WriteLine("Ogiltigt val.");
                return true;
        }
    }

    private static void ShowPinMenu(AtmService atm)
    {
        Console.WriteLine();
        Console.Write("Ange PIN: ");
        string? pin = Console.ReadLine();

        bool ok = atm.EnterPin(pin ?? "");

        if (ok)
        {
            Console.WriteLine("PIN korrekt.");
        }
        else
        {
            Console.WriteLine("Fel PIN.");
            Console.WriteLine("Kortet matas ut.");
            atm.EjectCard();
        }
    }

    private static void ShowMainMenu(AtmService atm)
    {
        Console.WriteLine();
        Console.WriteLine("=== HUVUDMENY ===");
        Console.WriteLine("1. Visa saldo");
        Console.WriteLine("2. Ta ut pengar");
        Console.WriteLine("3. Sätt in pengar");
        Console.WriteLine("4. Inaktivera kort");
        Console.WriteLine("0. Mata ut kort");
        Console.Write("\nVal: ");

        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                ShowBalance(atm);
                break;
            case "2":
                WithdrawFlow(atm);
                break;
            case "3":
                DepositFlow(atm);
                break;
            case "4":
                CardDeactivationFlow(atm);
                break;
            case "0":
                atm.EjectCard();
                Console.WriteLine("Kortet är utmatat.");
                break;
            default:
                Console.WriteLine("Ogiltigt val.");
                break;
        }
    }

    private static void ShowBalance(AtmService atm)
    {
        int balance = atm.GetBalance();
        Console.WriteLine($"Ditt saldo är: {balance} kr");
    }

    private static void WithdrawFlow(AtmService atm)
    {
        Console.Write("Ange belopp att ta ut: ");
        string? input = Console.ReadLine();

        int amount = int.Parse(input);

        bool success = atm.Withdraw(amount);

        if (success)
        {
            Console.WriteLine("Varsågod, ta dina pengar.");
        }
        else
        {
            Console.WriteLine("Uttaget kunde inte genomföras.");
        }
    }

    private static void DepositFlow(AtmService atm)
    {
        Console.Write("Ange belopp att sätta in: ");
        string? input = Console.ReadLine();

        int amount = int.Parse(input);

        bool success = atm.Deposit(amount);

        if (success)
        {
            Console.WriteLine("Insättning genomförd.");
        }
        else
        {
            Console.WriteLine("Insättningen kunde inte genomföras.");
        }
    }

    private static void CardDeactivationFlow(AtmService atm)
    {
        Console.Write("\nÄr du säkert att du vill inaktivera ditt kort? [J/N]: ");
        switch (Console.ReadLine().ToLower().Trim())
        {
            case "j":
                Console.WriteLine("Ange din pinkod");
                string pin1 = Console.ReadLine();

                if (atm.CheckPinFormat(pin1))
                {

                    Console.WriteLine("Ange din pinkod igen");
                    string pin2 = Console.ReadLine();
                    if (atm.CheckPinFormat(pin2))
                    {
                        if (atm.CheckPinMatches(pin1, pin2))
                        {
                            if (atm.GetCurrentCard().PinCode == pin2)
                            {
                                atm.GetCurrentCard().InactivateCard();
                                Console.WriteLine("\nKort inaktiverat. Ditt kort mattas ut.");
                                Console.Write("Tryck [ENTER] för att fortsätta. ");
                                Console.ReadLine();
                                atm.EjectCard();
                            }
                            else
                            {
                                Console.WriteLine("\nFel pin. Transaktionen avbröts.");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nPinkod matchar inte. Transaktionen avbröts.");
                            return;
                        }
                    }
                    else
                    {
                        Console.Write("\nFel inmatning. Tryck [ENTER] för att fortsätta. ");
                        Console.ReadLine();
                    }

                }
                else
                {
                    Console.Write("\nFel inmatning. Tryck [ENTER] för att fortsätta. ");
                    Console.ReadLine();
                }

                break;
            case "n":
                break;
            default:
                Console.WriteLine("\nOgiltigt val.");
                break;
        }
    }

}