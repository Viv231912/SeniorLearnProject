
using Microsoft.AspNetCore.Mvc.Rendering;
using static SeniorLearn.WebApp.Data.Payment;

namespace SeniorLearn.WebApp.Areas.Administration.Models.Payment
{
    public class Manage 
    {
        public int MemberId { get; set; }
        public int MediaTypeId { get; set; }

        public bool Approved { get; }
        public decimal Amount { get; set; }

        public decimal OutstandingFees { get; set; }
        //Cheuqe
        public bool Cleared { get; set; }
        //Credit card
        public string CardIssuer { get; set; } = "Issuer";
        public int AuthorisationNumber { get; set; }

        //ETF
        public string ReferenceNumber { get; set; } = "Ref";


        public SelectList PaymentMediaTypes => new SelectList(PaymentMediaItem(), "Value", "Text");

        private IEnumerable<SelectListItem> PaymentMediaItem()
        {
            return Enum.GetValues(typeof(PaymentMedia))
                .Cast<PaymentMedia>()
                .Select(p => new SelectListItem
                {
                    Text = p.ToString(),
                    Value = ((int)p).ToString()
                });
        }

    }
}
