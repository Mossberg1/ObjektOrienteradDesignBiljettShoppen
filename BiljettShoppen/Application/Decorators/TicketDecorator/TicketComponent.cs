using Models.Entities;

namespace Application.Decorators.TicketDecorator;

/// <summary>
/// TicketComponent som är den konkreta implementation av ITicketComponent.
/// Som ska dekoreras av de olika Decorators som finns för prisberäkning.
/// </summary>
public class TicketComponent : ITicketComponent
{
    private readonly Event _event;

    public TicketComponent(Event ev)
    {
        _event = ev;
    }

    public decimal GetPrice()
    {
        return _event.Price;
    }

    public string GetDescription()
    {
        return $"Event: {_event.Name}";
    }
}