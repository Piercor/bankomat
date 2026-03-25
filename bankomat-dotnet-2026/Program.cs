
using ATM;

var account = new Account(9000);
var card = new Card("1234-5678", "1234", account);
List<Card> cardList = [card];
var atm = new AtmService(11000, cardList);

ConsoleRunner.Run(atm, cardList);

