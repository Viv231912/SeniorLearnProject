using static SeniorLearn.WebApp.Data.Payment;

namespace SeniorLearn.WebApp.Data
{
    public class Cheque : Payment
    {
        public override PaymentMedia Media => PaymentMedia.Cheque;

        public override bool Approved => Cleared;
        public bool Cleared { get; set; }

    }
}
