using Data;
using DataModel;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Stripe;
using Stripe.Checkout;

namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodController : ControllerBase
    {

        private IPaymentMethodService paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            this.paymentMethodService = paymentMethodService;
        }


        [HttpGet("paymentMethod")]
        public List<PaymentMethodDto> GetPaymentMehods()
        {
            return this.paymentMethodService.GetPaymentMethods();
        }

        [HttpGet("user/{id}")]
        public PaymentMethodDto GetPayamentMethod(int id)
        {
            return this.paymentMethodService.GetPaymentMethod(id);
        }

        [HttpPost("add")]
        public bool PostPaymentMehtod(PaymentMethodDto paymentMethodDto)
        {
            var createPayment = paymentMethodService.AddPaymentMethod(paymentMethodDto);
            return createPayment;
        }

        [HttpPut("edit")]
        public bool EditPaymentMehtod(PaymentMethodDto paymentMethodDto)
        {
            return this.paymentMethodService.UpdatePaymentMethod(paymentMethodDto);
        }

        [HttpDelete("delete/{id}")]
        public bool DeletePaymentMehtod(int id)
        {
            return this.paymentMethodService.DeletePaymentMethod(id);
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
                SuccessUrl = "http://localhost:4200/views/payment-success",
                CancelUrl = "http://localhost:4200/views/payment-cancel"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Ok(new { url = session.Url });
        }


        /* [HttpPost("create-checkout-session")]
         public IActionResult CreateCheckoutSession(OrderRequest order)
         {

             var options = new SessionCreateOptions
             {
                 PaymentMethodTypes = new List<string> { "card" },
                 LineItems = new List<SessionLineItemOptions>
                 {
                     new SessionLineItemOptions
                     {
                         PriceData = new SessionLineItemPriceDataOptions
                         {
                             UnitAmount = (long)order.TotalAmount,
                             Currency = "eur",
                             ProductData = new SessionLineItemPriceDataProductDataOptions
                             {
                                 Name = "Producto de prueba"
                             }
                         },
                         Quantity = 1
                     }
                 },
                     Mode = "payment",
                     SuccessUrl = "http://localhost:4200/views/payment-success",
                     CancelUrl = "http://localhost:4200/views/payment-cancel"
             };
             var service = new SessionService();
             Session session = service.Create(options);

             return Ok(new { url = session.Url });
         }*/

    }
}
