using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderModule
{
    public enum OrderPaymentStatus
    {
        Pending = 0, // معلقه
        PaymentReceived = 1, // تم التوصيل
        PaymentFailed = 2 // فشل التوصيل
    }
}
