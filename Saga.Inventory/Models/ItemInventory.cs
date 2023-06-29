namespace Saga.Inventory.Models;

public class ItemInventory
{
    public string? Name { get; set; }
    public int Count { get; set; }
}

public static class ItemInventoryGenerator
{
    public static List<ItemInventory> GenerateInventory()
    {
        return new List<ItemInventory>
        {
          new ItemInventory{Name = "Jacket", Count = 15},
          new ItemInventory{Name = "Board game" , Count = 3},
          new ItemInventory{Name = "Computer", Count = 0}
        };
    }
}
