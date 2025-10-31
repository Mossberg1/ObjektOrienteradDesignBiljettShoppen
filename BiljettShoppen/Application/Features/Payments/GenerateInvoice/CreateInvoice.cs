using Models.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Features.Payments.GenerateInvoice
/// <summary>
/// Skapar en PDF-faktura för en bokning.
/// <para>
/// PDF:en innehåller bokningsinformation, biljetter och förfallodag.
/// </para>
/// </summary>
{
    public class CreateInvoice
    {
        static CreateInvoice()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] GenerateInvoice(Booking booking)
        {
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Header()
                        .Text("Faktura")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Text("Det här är din faktura för bokningen.");
                            column.Item().Text("Bokningsdetaljer:");
                            column.Item().Text($" - Boknings-ID: {booking.Id}");
                            column.Item().Text($" - Total pris: {booking.TotalPrice}kr");
                            column.Item().Text(" - Datum: " + DateTime.Now.ToString("dd/MM/yyyy"));
                            column.Item().Text(" - Förfallodag: " + DateTime.Now.AddDays(14).ToString("dd/MM/yyyy"));
                            column.Item().Column(list =>
                            {
                                list.Item().Text("Biljetter:");
                                if (booking.TicketsNavigation != null && booking.TicketsNavigation.Count > 0)
                                {
                                    var index = 1;
                                    foreach (var ticket in booking.TicketsNavigation)
                                    {
                                        string seatType = "Okänd typ";
                                        decimal price = ticket?.Price ?? 0m;

                                        if (ticket?.BookableSpaceNavigation is Seat seat)
                                        {
                                            seatType = seat.TypeToString();
                                            price = seat.Price;
                                        }
                                        else if (!string.IsNullOrEmpty(ticket?.Description))
                                        {
                                            seatType = ticket.Description;
                                        }
                                        list.Item().Text($"{index++}. {seatType} - {price:0.00} kr");
                                    }
                                }
                                else
                                {
                                    list.Item().Text("Inga biljetter hittades för denna bokning.");
                                }
                            }
                            );
                        });
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Tack för att du valde vår tjänst!");
                        });
                });
            });
            return pdfBytes.GeneratePdf();
        }
    }
}
