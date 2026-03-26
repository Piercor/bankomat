namespace ATM;

public class AtmService
{
    private Card? _currentCard;
    private bool _isAuthenticated;
    private List<Card> _cardList;

    public bool HasCardInserted => _currentCard != null;
    public bool IsAuthenticated => _isAuthenticated;

    public int AtmBalance { get; private set; }

    public AtmService(int initialBalance, List<Card> cardList)
    {
        AtmBalance = initialBalance;
        _cardList = cardList;
    }

    public void InsertCard(Card card)
    {
        _currentCard = card;
        _isAuthenticated = false;
    }

    public void EjectCard()
    {
        _currentCard = null;
        _isAuthenticated = false;
    }

    public bool EnterPin(string pinCode)
    {
        if (_currentCard == null)
        {
            return false;
        }

        _isAuthenticated = _currentCard.MatchesPin(pinCode);
        return _isAuthenticated;
    }

    public int GetBalance()
    {
        EnsureAuthenticated();
        return _currentCard!.Account.GetBalance();
    }

    public bool Withdraw(int amount)
    {
        EnsureAuthenticated();
        if (AtmBalance - amount < 0)
        {
            return false;
        }
        AtmBalance -= amount;
        return _currentCard!.Account.Withdraw(amount);
    }

    public bool Deposit(int amount)
    {
        EnsureAuthenticated();
        AtmBalance += amount;
        return _currentCard!.Account.Deposit(amount);
    }

    public void EnsureAuthenticated()
    {
        if (_currentCard == null || !_isAuthenticated)
        {
            throw new InvalidOperationException("Ingen autentiserad session.");
        }
    }
    public bool CardExist(string checkCard)
    {
        foreach (Card card in _cardList)
        {
            if (checkCard == card.CardNumber)
            {
                return true;
            }
        }
        return false;
    }
    public Card GetCard(string cardNumber)
    {
        Card? cardToGet = null;
        foreach (Card card in _cardList)
        {
            if (cardNumber == card.CardNumber)
            {
                cardToGet = card;
                break;
            }
        }
        return cardToGet;
    }

    public bool CheckCardFormat(string card)
    {
        if (!int.TryParse(card, out int cardInput) || cardInput.ToString().Length != 8)
        { return false; }
        else { return true; }
    }
    public bool CheckPinFormat(string pin)
    {
        if (!int.TryParse(pin, out int pinInput) || pinInput.ToString().Length != 4)
        { return false; }
        else { return true; }
    }
    public bool CheckPinMatches(string pin1, string pin2)
    {
        if (pin1 == pin2) { return true; }
        else { return false; }
    }
    public void ActivateCard(string card, string pin, List<Card> cardList)
    {
        cardList.Add(new(card, pin, new(0)));
    }
}