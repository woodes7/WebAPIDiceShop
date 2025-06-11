using Data;
using DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Stripe;
using Stripe.Checkout;

namespace WebAPIDiceShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {

        private IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession(OrderRequest order)
        {
            var lineItems = order.Items.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.UnitPrice * 100), // Stripe usa centavos
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Name
                    }
                },
                Quantity = item.Quantity
            }).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://pablorg.xyz/views/payment-success",
                CancelUrl = "https://pablorg.xyz/views/payment-cancel"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Ok(new { url = session.Url });
        }

        [HttpGet("postPurchase")]
        public bool PostPurchase(int userId)
        {
            return paymentService.PostPurchase(userId);
        }

    

    }
}
