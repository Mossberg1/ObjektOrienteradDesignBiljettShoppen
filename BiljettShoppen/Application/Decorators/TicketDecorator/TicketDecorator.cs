namespace Application.Decorators.TicketDecorator;

/// <summary>
/// Bas klass för prisberäknings decorator.
/// </summary>
public abstract class TicketDecorator : ITicketComponent
{
    protected readonly ITicketComponent _wrappedTicket;

    protected TicketDecorator(ITicketComponent wrappedTicket) => _wrappedTicket = wrappedTicket;

    public virtual decimal GetPrice() => _wrappedTicket.GetPrice();
    public virtual string GetDescription() => _wrappedTicket.GetDescription();
}