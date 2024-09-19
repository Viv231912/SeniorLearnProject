namespace SeniorLearn.WebApp.Data
{
    public class CreditCard : Payment
    {
        public override PaymentMedia Media => PaymentMedia.CreditCard;
        public string CardIssuer { get; set; } = string.Empty;
        public override bool Approved =>  AuthorisationNumber > 0;
        public int AuthorisationNumber { get; set; }
    }
}
