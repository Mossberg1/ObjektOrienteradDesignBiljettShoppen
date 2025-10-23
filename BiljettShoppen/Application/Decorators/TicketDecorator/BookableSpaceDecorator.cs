using Models.Entities.Base;

namespace Application.Decorators.TicketDecorator;

public class BookableSpaceDecorator : TicketDecorator
{
    private readonly BookableSpace _bookableSpace;

    public BookableSpaceDecorator(ITicketComponent wrappedTicket, BookableSpace bookableSpace) : base(wrappedTicket)
    {
        _bookableSpace = bookableSpace;
    }

    public override decimal GetPrice() => base.GetPrice() + _bookableSpace.Price;
    public override string GetDescription() => $"{base.GetDescription()}, Plats: {_bookableSpace}";
}