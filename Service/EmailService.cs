using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace Service
{
    internal class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public bool SendEmail(string to, string subject, string body)
        {
            try
            {
                var smtpHost = _config["Smtp:Host"];
                var smtpPort = int.Parse(_config["Smtp:Port"]);
                var smtpUser = _config["Smtp:Username"];
                var smtpPass = _config["Smtp:Password"];
                var from = _config["Smtp:From"];

                var mail = new MailMessage
                {
                    From = new MailAddress(from),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(to);

                using var smtp = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                Console.WriteLine($"[ERROR] Fallo al enviar el correo a '{to}': {ex.Message}");
                // Opcional: lanzar excepción si quieres que el proceso lo maneje a nivel superior
                // throw;
            }
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
