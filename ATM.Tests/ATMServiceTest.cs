namespace ATM.Test;

public class ATMServiceTest
{
  AtmService ATM;
  Account Account;
  Card Card;
  List<Card> CardList;

  public ATMServiceTest()
  {
    Account = new Account(9000);
    Card = new Card("1234-5678", "1234", Account);
    CardList = [Card];
    ATM = new AtmService(11000, CardList);

  }
  [Fact]
  public void InsertCardTest()
  {
    ATM.InsertCard(Card);
    Assert.True(ATM.HasCardInserted);
  }

  [Fact]
  public void EnterWrongtPinTest()
  {
    ATM.InsertCard(Card);
    Assert.False(ATM.EnterPin("4321"));
  }

  [Fact]
  public void EnterCorrectPinTest()
  {
    ATM.InsertCard(Card);
    Assert.True(ATM.EnterPin("1234"));
  }


  [Fact]
  public void AccountCorrectWithdrawTest()
  {
    ATM.InsertCard(Card);
    ATM.EnterPin("1234");
    ATM.Withdraw(5000);

    Assert.Equal(4000, ATM.GetBalance());
  }

  [Fact]
  public void EjectCard()
  {
    ATM.InsertCard(Card);
    ATM.EnterPin("1234");
    ATM.EjectCard();

    Assert.False(ATM.HasCardInserted);
  }

  [Fact]
  public void AccountWrongtWithdrawTest()
  {
    ATM.InsertCard(Card);
    ATM.EnterPin("1234");
    Assert.True(ATM.Withdraw(5000));

    ATM.EjectCard();

    ATM.InsertCard(Card);
    ATM.EnterPin("1234");
    Assert.False(ATM.Withdraw(7000));
    Assert.False(ATM.Withdraw(6000));

    ATM.EjectCard();
  }
  [Fact]
  public void CardActivationTest()
  {
    Assert.True(ATM.CheckCardFormat("87654321"));
    Assert.False(ATM.CardExist("8765-4321"));
    Assert.True(ATM.CheckPinFormat("4321"));
    Assert.True(ATM.CheckPinMatches("4321", "4321"));
    ATM.ActivateCard("8765-4321", "4321", CardList);
    Assert.True(ATM.CardExist("8765-4321"));
  }
}