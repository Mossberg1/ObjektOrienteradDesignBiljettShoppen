namespace Application.Services;

/// <summary>
/// Klass för att validera kreditkortsuppgifter.
/// Med hjälp av Luhns algoritm. Om man vill 
/// testa kortnummer under utveckling kan man 
/// hämta testnummer från: https://docs.stripe.com/testing
/// </summary>
public class CardValidator
{
    /// <summary>
    /// Validerar ett kortnummer med luhns algoritm.
    /// </summary>
    /// <param name="cardNumber">Kortnummer i sträng format</param>
    /// <returns>true om kortnummret är korrekt, annars false.</returns>
    public static bool ValidateCardNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        var cleaned = new string(cardNumber.Where(char.IsDigit).ToArray());

        if (cleaned.Length < 13 || cleaned.Length > 19)
            return false;

        return CheckCard(cleaned);
    }

    /// <summary>
    /// Privat hjälpmetod för att kontrollera kortnummer med Luhns algoritm.
    /// </summary>
    private static bool CheckCard(string cardNumber)
    {
        int sum = 0;
        bool isEven = false;

        for (int i = cardNumber.Length - 1; i >= 0; i--)
        {
            int digit = cardNumber[i] - '0';

            if (isEven)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            isEven = !isEven;
        }

        return sum % 10 == 0;
    }

    /// <summary>
    /// Hämtar korttyp baserat på kortnummer.
    /// </summary>
    /// <param name="cardNumber">Kortnummret i sträng format</param>
    /// <returns>Korttyp i sträng format</returns>
    public static string GetCardType(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return "Okänd";

        var cleaned = new string(cardNumber.Where(char.IsDigit).ToArray());

        if (cleaned.StartsWith("4"))
            return "Visa";
        if (cleaned.StartsWith("5") && cleaned.Length >= 2)
        {
            int secondDigit = cleaned[1] - '0';
            if (secondDigit >= 1 && secondDigit <= 5)
                return "Mastercard";
        }

        if (cleaned.StartsWith("34") || cleaned.StartsWith("37"))
            return "American Express";
        if (cleaned.StartsWith("6011") || cleaned.StartsWith("65"))
            return "Discover";

        return "Okänd";
    }

    /// <summary>
    /// Kontrollerar om utgångsdatum är giltigt och inte har passerat.
    /// </summary>
    /// <param name="expiryDate">Utgångsdatum i sträng format 'MM/YY'</param>
    /// <returns>true om valideringen har lyckats annars false.</returns>
    public static bool ValidateExpiryDate(string expiryDate)
    {
        if (string.IsNullOrWhiteSpace(expiryDate))
            return false;

        var cleaned = expiryDate.Replace("/", "").Replace(" ", "");

        if (cleaned.Length != 4)
            return false;

        if (!int.TryParse(cleaned.Substring(0, 2), out int month))
            return false;
        if (!int.TryParse(cleaned.Substring(2, 2), out int year))
            return false;

        if (month < 1 || month > 12)
            return false;

        int fullYear = 2000 + year;

        var expiryDateTime = new DateTime(fullYear, month, DateTime.DaysInMonth(fullYear, month));

        return expiryDateTime >= DateTime.Now.Date;
    }

    /// <summary>
    /// Validerar CVC kod.
    /// </summary>
    public static bool ValidateCvc(string cvc, string cardNumber = null)
    {
        if (string.IsNullOrWhiteSpace(cvc))
            return false;

        var cleaned = new string(cvc.Where(char.IsDigit).ToArray());

        var cardType = !string.IsNullOrEmpty(cardNumber) ? GetCardType(cardNumber) : null;

        if (cardType == "American Express")
            return cleaned.Length == 4;

        return cleaned.Length == 3;
    }
}
