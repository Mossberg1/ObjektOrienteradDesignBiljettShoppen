using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CreditCardPaymentStubService : ICreditCardPaymentService
    {
        private readonly ILogger<CreditCardPaymentStubService> _logger;

        public CreditCardPaymentStubService(ILogger<CreditCardPaymentStubService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> PayAsync(string cardNumber, string expiryDate, string cvc, string name)
        {
            var numberResult = CardValidator.ValidateCardNumber(cardNumber);
            var expiryDateResult = CardValidator.ValidateExpiryDate(expiryDate);
            var cvcResult = CardValidator.ValidateCvc(cvc, cardNumber);
            string type = CardValidator.GetCardType(cardNumber);

            if (!numberResult || !expiryDateResult || !cvcResult)
            {
                _logger.LogWarning("Ogiltig kortinformation.");
                return false;
            }

            return true;
        }
    }
}
