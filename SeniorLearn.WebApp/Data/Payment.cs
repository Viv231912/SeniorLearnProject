namespace SeniorLearn.WebApp.Data
{
    public abstract class Payment
    {
        public enum PaymentMedia { Cash, Cheque, CreditCard, ElectronicFundTransfer }

        public int Id { get; set; }
        public int MemberId { get; set; }
        public virtual Member Member { get; set; } = default!;
        public abstract PaymentMedia Media { get;}
        public abstract bool Approved { get; }  
        public decimal  Amount { get; set; }
        public DateTime PaymentDate { get; set; }   
       
    }
}
