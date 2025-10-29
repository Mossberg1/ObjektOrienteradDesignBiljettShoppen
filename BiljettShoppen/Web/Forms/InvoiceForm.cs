namespace Web.Forms
{
    public class InvoiceForm
    {
        public string Email { get; set; } = string.Empty;

        public InvoiceForm() { }

        public InvoiceForm(string email)
        {
            Email = email;
        }
    }
}
