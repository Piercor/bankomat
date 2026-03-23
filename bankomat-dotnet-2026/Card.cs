namespace ATM;

public class Card
{
    public string CardNumber { get; }
    public string PinCode { get; }
    public Account Account { get; }

    public Card(string cardNumber, string pinCode, Account account)
    {
        CardNumber = cardNumber;
        PinCode = pinCode;
        Account = account;
    }

    public bool MatchesPin(string pinCode)
    {
        return PinCode == pinCode;
    }
}