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
    internal class PaymentMethodService : IPaymentMethodService
    {
        private DiceShopContext diceShopContext;
        public PaymentMethodService(DiceShopContext diceShopContext)
        {
            this.diceShopContext = diceShopContext;
        }

        public bool AddPaymentMethod(PaymentMethodDto paymentMethodDto)
        {
            var paymentEntity = paymentMethodDto.Adapt<PaymentMethod>();
            diceShopContext.Paymentmethods.Add(paymentEntity);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool DeletePaymentMethod(int id)
        {
            var payment = diceShopContext.Paymentmethods.Find(id);
            if (payment == null) return false;

            diceShopContext.Paymentmethods.Remove(payment);
            return diceShopContext.SaveChanges() > 0;
        }

        public PaymentMethodDto GetPaymentMethod(int id)
        {
            return diceShopContext.Paymentmethods.Find(id).Adapt<PaymentMethodDto>();
        }

        public List<PaymentMethodDto> GetPaymentMethods()
        {
            var paymentDto = diceShopContext.Paymentmethods.ProjectToType<PaymentMethodDto>().ToList();
            return paymentDto;
        }

        public bool UpdatePaymentMethod(PaymentMethodDto paymentMethodDto)
        {
            var payment = diceShopContext.Paymentmethods.FirstOrDefault(c => c.Id == paymentMethodDto.Id);
            if (payment == null) return false;
            paymentMethodDto.Adapt(payment);
            diceShopContext.Update(payment);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}
