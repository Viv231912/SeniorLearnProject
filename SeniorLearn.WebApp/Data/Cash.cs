namespace SeniorLearn.WebApp.Data
{
    public class Cash : Payment
    {
        public override PaymentMedia Media => PaymentMedia.Cash;

        public override bool Approved => true;


    }
}
