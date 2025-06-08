using Data;
using DataModel;
using Mapster;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class PaymentService : IPaymentService
    {
        private readonly IUserService userService;
        private readonly IShoppingCartItemService shoppingCartItemService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly IOrdetailService ordetailService;
        private readonly IBillingAddressService billingAddressService;
        private readonly IEmailService emailService;
        public PaymentService(IUserService userService, IShoppingCartItemService shoppingCartItemService,
            IShoppingCartService shoppingCartService, IProductService productService, IOrderService orderService, IOrdetailService ordetailService,
            IBillingAddressService billingAddressService, IEmailService emailService)
        {
            
            this.userService = userService;
            this.shoppingCartService = shoppingCartService;
            this.shoppingCartItemService = shoppingCartItemService;
            this.productService = productService;
            this.orderService = orderService;
            this.ordetailService = ordetailService;
            this.billingAddressService = billingAddressService;
            this.emailService = emailService;
        }

        public bool PostPurchase(int userId)
        {
            var user = userService.GetUser(userId);
            var billingAddress = billingAddressService.GetPrimaryBillingaddress(userId);
            var cart = shoppingCartService.GetShoppingcartActiveByUser(userId);
            var items = shoppingCartItemService.GetItemsByCartId(cart.Id, true);
            var details = new List<OrderdetailDto>();

            decimal total = 0;
            var modify = false;

            foreach (var item in items)
            {
                total += (item.Quantity * item.UnitPrice);
            }

            var order = new OrderDto
            {
                UserId = userId,
                BillingAddress = $"{billingAddress.Street} {billingAddress.StreetNumber}\n {billingAddress.City}, {billingAddress.State}\n{billingAddress.PostalCode}, {billingAddress.Country}",
                OrderDate = DateTime.Now,
                OrderStatus = "Preparando",
                TotalAmount = total
            };

            var orderDto = orderService.AddOrder(order);

            foreach (var item in items)
            {
                var product = productService.GetProductDto(item.ProductId);
                product.Stock = product.Stock - item.Quantity;
                modify = productService.UpdateProductDto(product);

                OrderdetailDto orderDetail = new OrderdetailDto
                {
                    OrderId = orderDto.Id,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = item.Quantity * item.UnitPrice,
                };
                modify = ordetailService.AddOrderDetail(orderDetail);
                details.Add(orderDetail);
                modify = shoppingCartItemService.DeleteItemDto(item.Id);
            }

            var emailBody = GenerateInvoiceEmailBody(user, orderDto, details);
            emailService.SendEmail(user.Email, "Factura de tu pedido", emailBody);

            return modify;
        }

        private string GenerateInvoiceEmailBody(UserDto user, OrderDto order, List<OrderdetailDto> details)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<h2>Gracias por tu compra</h2>");
            sb.AppendLine($"<p><strong>Cliente:</strong> {user.FullName} ({user.Email})</p>");
            sb.AppendLine($"<p><strong>Fecha:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</p>");
            sb.AppendLine($"<p><strong>Dirección de facturación:</strong><br>{order.BillingAddress.Replace("\n", "<br>")}</p>");
            sb.AppendLine($"<p><strong>Estado:</strong> {order.OrderStatus}</p>");
            sb.AppendLine("<br>");

            sb.AppendLine("<h3>Resumen de productos:</h3>");
            sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
            sb.AppendLine("<thead><tr><th>Producto</th><th>Descripción</th><th>Cantidad</th><th>Precio unitario</th><th>Subtotal</th></tr></thead>");
            sb.AppendLine("<tbody>");

            foreach (var detail in details)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{detail.ProductName}</td>");
                sb.AppendLine($"<td>{detail.ProductDescription}</td>");
                sb.AppendLine($"<td>{detail.Quantity}</td>");
                sb.AppendLine($"<td>{detail.UnitPrice:C}</td>");
                sb.AppendLine($"<td>{detail.Subtotal:C}</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody></table>");
            sb.AppendLine($"<p><strong>Total:</strong> {order.TotalAmount:C}</p>");
            sb.AppendLine("<p>Gracias por confiar en nosotros.</p>");

            return sb.ToString();
        }


    }
}
