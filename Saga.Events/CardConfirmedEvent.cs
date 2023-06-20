namespace Saga.Events;

public class CardConfirmedEvent
{
    public List<Item> Items { get; set; } = new List<Item>();
}

public class Item
{
    public string Name { get; set; } = string.Empty;
    public Decimal Price { get; set; } = default;
}

