using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Saga.Events;
using Saga.Events.Common;

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
    public async Task<IActionResult> Post(Card card, CancellationToken token)
    {
        _logger.LogInformation("sending card");
        var cardConfirmedEvent = NewCardConfirmedEvent(card);
        if (cardConfirmedEvent.Items.Count == 0) {
          return BadRequest("Request should have items");
        }

        await _client.SaveStateAsync(DefaultValues.Dapr.DefaultStateStore,  cardConfirmedEvent.Id.ToString(), cardConfirmedEvent, cancellationToken: token);
        await _client.PublishEventAsync("pubsub", "orchestratortopic", cardConfirmedEvent);

        return Ok();
    }

    private CardConfirmedEvent NewCardConfirmedEvent(Card card)
    {
        return new CardConfirmedEvent
        {
            Items = card.Items.Select(i => new Saga.Events.Item
            {
                Name = i.Name,
                Price = i.Price
            }).ToList(),
            Id = Guid.NewGuid()
        };
    }
}

public class Card
{
    public List<Item> Items { get; set; }
}

public class Item
{
    public string Name { get; set; } = string.Empty;
    public Decimal Price { get; set; } = default;
}
