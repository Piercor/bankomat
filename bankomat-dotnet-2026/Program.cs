
using ATM;

var account = new Account(5000);
var card = new Card("1234-5678", "1234", account);
var atm = new AtmService(11000);

ConsoleRunner.Run(atm, card);

