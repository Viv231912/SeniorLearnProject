namespace SeniorLearn.WebApp.Data
{
    public class ElectronicFundTransfer : Payment
    {
        public override PaymentMedia Media => PaymentMedia.ElectronicFundTransfer;

        public override bool Approved => true;
        public string ReferenceNumber { get; set; } = string.Empty;

    }
}
