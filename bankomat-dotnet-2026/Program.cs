
using ATM;

var account = new Account(9000);
var card = new Card("1234-5678", "1234", new(9000));
List<Card> cardList = new();
cardList.Add(card);
var atm = new AtmService(11000);

ConsoleRunner.Run(atm, cardList);

