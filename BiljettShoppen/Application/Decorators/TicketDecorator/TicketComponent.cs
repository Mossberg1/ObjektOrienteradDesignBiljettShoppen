using Models.Entities;

namespace Application.Decorators.TicketDecorator;

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