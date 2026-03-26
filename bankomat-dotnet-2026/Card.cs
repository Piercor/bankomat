namespace ATM;

public class Card
{
    public enum CardStatus { Active, Inactive }
    public string CardNumber { get; }
    public string PinCode { get; set; }
    public Account Account { get; }
    public CardStatus Status { get; set; }

    public Card(string cardNumber, string pinCode, Account account, CardStatus status)
    {
        CardNumber = cardNumber;
        PinCode = pinCode;
        Account = account;
        Status = status;
    }

    public bool MatchesPin(string pinCode)
    {
        return PinCode == pinCode;
    }

    public void SetPinCode(string pin)
    {
        PinCode = pin;
    }

    public void ActivateCard()
    {
        Status = CardStatus.Active;
    }
    public void InactivateCard()
    {
        Status = CardStatus.Inactive;
    }
}