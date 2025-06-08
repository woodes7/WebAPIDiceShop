using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPaymentMethodService
    {
        public List<PaymentMethodDto> GetPaymentMethods();

        public PaymentMethodDto GetPaymentMethod(int id);

        public bool AddPaymentMethod(PaymentMethodDto paymentMethodDto);

        public bool UpdatePaymentMethod(PaymentMethodDto paymentMethodDto);

        public bool DeletePaymentMethod(int id);
    }
}
