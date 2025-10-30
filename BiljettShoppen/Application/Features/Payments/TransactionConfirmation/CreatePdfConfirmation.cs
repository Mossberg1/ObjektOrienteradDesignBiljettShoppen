using Models.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Features.Payments.TransactionConfirmation
{
    public class CreatePdfConfirmation
    {
        static CreatePdfConfirmation()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] GeneratePdf(Booking booking, string? transactionId = null)
        {
            var tickets = booking.TicketsNavigation ?? new List<Ticket>();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Header()
                        .Text("Biljetter")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Text("Det här är bekräftelsen att transaktionen är genomförd, och dela ut biljetterna");
                            column.Item().Text("Transaktion Detaljer:");
                            column.Item().Text($" - Total pris: {booking.TotalPrice}kr");
                            column.Item().Text(" - Datum: " + DateTime.Now.ToString("dd/MM/yyyy"));

                            column.Item().PaddingTop(5, Unit.Millimetre).Text("Biljetter:").FontSize(14).SemiBold();

                            foreach (var t in tickets)
                            {

                                var eventName = t.EventNavigation?.Name ?? "N/A";
                                string place;
                                if (t.BookableSpaceNavigation is Models.Entities.Seat seat)
                                    place = $"Row {seat.RowNumber}, Col {seat.ColNumber}";
                                else
                                    place = t.BookableSpaceNavigation?.GetType().Name ?? $"Id {t.BookableSpaceId}";

                                column.Item()
                                    .Border(1, Colors.Grey.Darken2)
                                    .Background(Colors.Grey.Lighten4)
                                    .Padding(8)
                                    .Column(tc =>
                                    {
                                        tc.Item().Text(x =>
                                        {
                                            x.Span("Evenemang: ").SemiBold();
                                            x.Span(eventName);
                                        });

                                        tc.Item().Text(x =>
                                        {
                                            x.Span("Plats: ").SemiBold();
                                            x.Span(place);
                                        });

                                        tc.Item().Text(x =>
                                        {
                                            x.Span("Pris: ").SemiBold();
                                            x.Span($"{t.Price:C}");
                                        });
                                    });

                                column.Item().Height(6);
                            }

                            if (tickets.Count == 0)
                                column.Item().Text("Inga biljetter hittades för denna bokning.");
                        });


                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}
