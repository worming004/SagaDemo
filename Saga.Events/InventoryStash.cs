namespace Saga.Events;

public class InventoryStash
{
    public Guid OriginEventId { get; set; }
    public List<InventoryStashItem> Items { get; set; }
}

public class InventoryStashItem
{
    public string Name { get; set; }
    public int StashedCount { get; set; }
    public int RemainsAvailable { get; set; }
}

