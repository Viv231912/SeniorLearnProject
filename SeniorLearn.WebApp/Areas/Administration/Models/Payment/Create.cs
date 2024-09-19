using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static SeniorLearn.WebApp.Data.Payment;

namespace SeniorLearn.WebApp.Areas.Administration.Models.Payment
{
    public class Create
    {

        public int MemberId { get; set; }
        public int MediaTypeId { get; set; }

        public bool Approved { get; }
        public decimal Amount { get; set; }

        public decimal OutstandingFees { get; set; }
        public DateTime PaymentDate { get; set; }

        //Cheuqe
        public bool Cleared { get; set; }
        //Credit card
        public string CardIssuer { get; set; } = "Issuer";
        public int AuthorisationNumber { get; set; }

        //ETF
        public string ReferenceNumber { get; set; } = "Ref";

        public SelectList MediaTypes => GetMediaTypes();

        private SelectList GetMediaTypes()
        {
            var mediaTypes = Enum.GetValues<PaymentMedia>()
                .Select(m => new { Value = (int)m, Text = m.ToString() });

            return new SelectList(mediaTypes, "Value", "Text");
        }

      
    }
}
