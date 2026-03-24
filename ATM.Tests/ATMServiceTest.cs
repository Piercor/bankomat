namespace ATM.Test;

public class ATMServiceTest
{
  AtmService ATMService;
  Account Account;
  Card Card;

  public ATMServiceTest()
  {
    ATMService = new AtmService(11000);
    Account = new Account(9000);
    Card = new Card("1234-5678", "1234", Account);

  }
  [Fact]
  public void InsertCardTest()
  {
    ATMService.InsertCard(Card);
    Assert.True(ATMService.HasCardInserted);
  }

  [Fact]
  public void EnterCorrectPinTest()
  {
    ATMService.InsertCard(Card);
    Assert.True(ATMService.EnterPin("1234"));
  }

  [Fact]
  public void EnterWrongtPinTest()
  {
    ATMService.InsertCard(Card);
    Assert.False(ATMService.EnterPin("4321"));
  }

  [Fact]
  public void AccountCorrectWithdrawTest()
  {
    ATMService.InsertCard(Card);
    ATMService.EnterPin("1234");

    Assert.True(ATMService.Withdraw(5000));
  }

  [Fact]
  public void EjectCard()
  {
    ATMService.InsertCard(Card);
    ATMService.EnterPin("1234");
    ATMService.EjectCard();

    Assert.False(ATMService.HasCardInserted);
  }

  [Fact]
  public void AccountWrongtWithdrawTest()
  {
    ATMService.InsertCard(Card);
    ATMService.EnterPin("1234");
    Assert.True(ATMService.Withdraw(5000));

    ATMService.EjectCard();

    ATMService.InsertCard(Card);
    ATMService.EnterPin("1234");
    Assert.False(ATMService.Withdraw(7000));
    Assert.False(ATMService.Withdraw(6000));

    ATMService.EjectCard();
  }
}