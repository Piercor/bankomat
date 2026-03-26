
using ATM;

List<Card> cardList =
[
  new Card("1234-5678", "1234", new Account(9000), Card.CardStatus.Active),
  new Card("8765-4321", "", new Account(0), Card.CardStatus.Inactive),
];

var atm = new AtmService(11000, cardList);

ConsoleRunner.Run(atm, cardList);

