using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICreditCardPaymentService
    {
        Task<bool> PayAsync(string cardNumber, string expiryDate, string cvc, string name);
    }
}
