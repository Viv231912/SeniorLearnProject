using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;
using System.Security.Cryptography.Xml;

namespace SeniorLearn.WebApp.Areas.Administration.Controllers
{
    public class PaymentController : AdministrationAreaController
    {
        private readonly UserManager<User> _userManager;
        public PaymentController(ApplicationDbContext context, UserManager<User> userManager) : base(context)
        {
            _userManager = userManager; 
        }

        //List of the payment belongs to single member

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.MemberId = id;    
            var payments = await _context.Payments
                .Include(p => p.Member)
                .Where(p => p.MemberId == id)
                .ToListAsync();

            return View(payments);
        }

        //Search payment by date
        public async Task<IActionResult> Search(DateTime? paymentDate = null)
        {
            var payments = await _context.Payments
                .Include(p => p.Member)
                .OrderBy(p => p.PaymentDate)
                .ToListAsync();
                

            if (paymentDate.HasValue)
            {
                var results = SearchPaymentsByDate(payments, paymentDate.Value);

                if (!results.Any())
                {
                    ViewData["NotFoundMessage"] = $"No payment found for the date '{paymentDate.Value.ToShortDateString()}'";
                    
                    return View("Search", new List<Payment>());
                    
                    //return NotFound($"No payment found for the date '{paymentDate.Value.ToShortDateString()}'");
                }
                else 
                {
                    return View("Search", results);

                }
            }

            
            return View("Search", new List<Payment>());
        }




   
        


        //Create payments
        [HttpGet]
        public IActionResult Create(int id)
        {
            //assign id to MemberId from the model
            var model = new Models.Payment.Create { MemberId = id};
            return View(model);  
        }


        [HttpPost]
        public async Task<IActionResult> Create(Models.Payment.Create m)
        {
                
            var member = await _context.Members.Include(mem =>mem.Payments).FirstAsync(mem => mem.Id == m.MemberId);

            if (ModelState.IsValid)
            {
                ////check if member has outstanding balance 
                if (member.OutstandingFees <= 0)
                {
                    ModelState.AddModelError("", $"Membership is paid up to date. Next renewal date is {member.RenewalDate.ToShortDateString()}.");

                    return View(m);
                }



                //make sure the amount enter in greater than zero 
                if (m.Amount <= 0) 
                {
                    ModelState.AddModelError(nameof(m.Amount), "Amount must be greater than zero");
                    return View(m);
                }

                var paymentType = new Dictionary<Payment.PaymentMedia, Func<Payment>>
                {
                    { Payment.PaymentMedia.Cash, () => new Cash { Amount = m.Amount}  },
                    { Payment.PaymentMedia.Cheque, () => new Cheque { Amount = m.Amount, Cleared = m.Cleared} },
                    { Payment.PaymentMedia.CreditCard, () => new CreditCard { Amount = m.Amount, CardIssuer = m.CardIssuer, AuthorisationNumber = m.AuthorisationNumber} },
                    { Payment.PaymentMedia.ElectronicFundTransfer, () => new ElectronicFundTransfer { Amount = m.Amount, ReferenceNumber = m.ReferenceNumber}},

                };
                if (paymentType.TryGetValue((Payment.PaymentMedia)m.MediaTypeId, out var createPayment)) 
                {
                    var newPayment = createPayment();
                    newPayment.PaymentDate = DateTime.Now;
                    newPayment.Amount = m.Amount;
                    member.Payments.Add(newPayment);

                    //if payment status is approved, set the outstanding fees to zero, and extended the renew date for another 12mths.
                    if (newPayment.Approved) 
                    {
            
                        member.OutstandingFees -= newPayment.Amount;
                        member.RenewalDate = member.RenewalDate.AddYears(1);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = m.MemberId });
                };

               

                
            }
       
             return View(m);
        }


        private List<Payment> SearchPaymentsByDate(List<Payment> payments, DateTime paymentDate)
        {
            int left = 0;
            int right = payments.Count - 1;
            var results = new List<Payment>();

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                int result = DateTime.Compare(payments[mid].PaymentDate.Date, paymentDate.Date);

                if (result == 0)
                {
                    // Add the mid element
                    results.Add(payments[mid]);

                    // Search on the left side of mid
                    int temp = mid - 1;
                    while (temp >= 0 && DateTime.Compare(payments[temp].PaymentDate.Date, paymentDate.Date) == 0)
                    {
                        results.Add(payments[temp]);
                        temp--;
                    }

                    // Search on the right side of mid
                    temp = mid + 1;
                    while (temp < payments.Count && DateTime.Compare(payments[temp].PaymentDate.Date, paymentDate.Date) == 0)
                    {
                        results.Add(payments[temp]);
                        temp++;
                    }

                    break;
                }
                else if (result < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return results;
        }
    }
}
