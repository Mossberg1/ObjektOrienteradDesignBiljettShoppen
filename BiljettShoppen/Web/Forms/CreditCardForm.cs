namespace Web.Forms
{
    public class CreditCardForm
    {
        public string Email { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string Cvc { get; set; } = string.Empty;

        public CreditCardForm() { }

        public CreditCardForm(string email, string cardNumber, string name, string expiryDate, string cvc)
        {
            Email = email;
            CardNumber = cardNumber;
            Name = name;
            ExpiryDate = expiryDate;
            Cvc = cvc;
        }
    }
}
