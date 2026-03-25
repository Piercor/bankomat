namespace ATM;

using System.Globalization;

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
        Console.WriteLine("0. Avsluta");
        Console.Write("Val: ");

        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                {
                    Card? cardToInsert = null;
                    Console.Write("\nAnge ditt kortnummer (8 siffror): ");

                    if (!int.TryParse(Console.ReadLine(), out int cardInput) || cardInput.ToString().Length != 8)
                    {
                        Console.Write("Fel inmatning. Tryck ENTER för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }

                    string formattedCard = cardInput.ToString().Insert(4, "-");

                    foreach (Card card in cardList)
                    {
                        if (formattedCard == card.CardNumber)
                        {
                            cardToInsert = card;
                            break;
                        }
                    }
                    if (cardToInsert != null)
                    {
                        atm.InsertCard(cardToInsert);
                        Console.WriteLine("Kort inmatat.");
                        return true;
                    }
                    else
                    {
                        Console.Write("Inget kort hittat. Tryck ENTER för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }
                }

            case "2":
                {
                    Console.Write("\nAnge ditt kortnummer (8 siffror): ");

                    if (!int.TryParse(Console.ReadLine(), out int cardInput) || cardInput.ToString().Length != 8)
                    {
                        Console.Write("Fel inmatning. Tryck ENTER för att fortsätta. ");
                        Console.ReadLine();
                        return true;
                    }

                    string formattedCard = cardInput.ToString().Insert(4, "-");
                    bool found = false;

                    foreach (Card card in cardList)
                    {
                        if (formattedCard == card.CardNumber)
                        {
                            Console.Write("Kortet är redan aktiverat. Tryck ENTER för att fortsätta. ");
                            Console.ReadLine();
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        bool createPin = true;
                        while (createPin)
                        {
                            Console.Write("\nAnge din pinkod (4 siffror): ");
                            if (!int.TryParse(Console.ReadLine(), out int pinInput) || pinInput.ToString().Length != 4)
                            {
                                Console.Write("Fel inmatning. Tryck ENTER för att fortsätta. ");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.Write("\nAnge din pinkod igen: ");
                                string? repeatPin = Console.ReadLine();
                                if (repeatPin == pinInput.ToString())
                                {
                                    Console.Write("\nKort aktiverat! Tryck ENTER för att fortsätta. ");
                                    Console.ReadLine();
                                    cardList.Add(new(formattedCard, repeatPin, new(0)));
                                    createPin = false;
                                }
                                else
                                {
                                    Console.Write("Fel inmatning. Tryck ENTER för att fortsätta. ");
                                    Console.ReadLine();
                                }
                            }
                        }
                    }
                    return true;
                }

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
        Console.WriteLine("4. Mata ut kort");
        Console.Write("Val: ");

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

}