namespace Saga.Inventory.Models;

public class ItemInventory
{
    public string? Name { get; set; }
    public int InventoryCount { get; set; }
    public int AvailableCount { get; set; }
}

public static class ItemInventoryGenerator
{
    public static List<ItemInventory> GenerateInventory()
    {
        return new List<ItemInventory>
        {
          new ItemInventory{Name = "Jacket", InventoryCount = 15, AvailableCount = 15},
          new ItemInventory{Name = "Board game" , InventoryCount = 3, AvailableCount = 3},
          new ItemInventory{Name = "Computer", InventoryCount = 0, AvailableCount = 0}
        };
    }
}
