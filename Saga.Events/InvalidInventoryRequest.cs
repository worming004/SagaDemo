namespace Saga.Events;

public class InvalidInventoryRequest
{
    public InvalidInventoryRequestType InvalidType { get; set; }
    public Guid OriginEventId { get; set; }
    public string ItemName { get; set; }
    public string Message { get; set; }
}

public enum InvalidInventoryRequestType
{
    InsufficientInventory,
    InventoryNotFound,
}
