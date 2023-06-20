using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Saga.Events;

namespace Saga.Card.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    private DaprClient _client;
    private ILogger<CardController> _logger;

    public CardController(DaprClient client, ILogger<CardController> logger)
    {
        _client = client;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Card card)
    {
      _logger.LogInformation("sending card");
        await _client.PublishEventAsync("main", "orchestrator", Map(card));

        return Ok();
    }

    private CardConfirmedEvent Map(Card card)
    {
        return new CardConfirmedEvent
        {
            Items = card.Items.Select(i => new Saga.Events.Item
            {
                Name = i.Name,
                Price = i.Price
            }).ToList()
        };
    }
}

public class Card
{
    public List<Item> Items { get; set; } = new List<Item>();
}

public class Item
{
    public string Name { get; set; } = string.Empty;
    public Decimal Price { get; set; } = default;
}
