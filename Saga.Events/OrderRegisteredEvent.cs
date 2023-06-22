namespace Saga.Events;

public class OrderRegisteredEvent
{
    public Guid Id { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
}

