using System.Text;

namespace Saga.Events;

public class CardConfirmedEvent
{
    public Guid Id { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"CardConfirmedEvent: {{Id: {Id}}}, ");
        sb.Append("Items: [");
        sb.AppendJoin(", ", Items);
        sb.Append("]");
        return sb.ToString();
    }
}

public class Item
{
    public string Name { get; set; } = string.Empty;
    public Decimal Price { get; set; } = default;

    public override string ToString()
    {
        return $"{{Price: {{Name: {Name}}}, Price: {{Price: {Price}}}}}";
    }
}

