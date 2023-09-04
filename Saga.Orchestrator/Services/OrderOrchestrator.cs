using Dapr.Client;
using Saga.Events;

namespace Saga.Orchestrator.Services.Orchestrator;

public class OrderOrchestrator
{
    private readonly ILogger<OrderOrchestrator> _logger;
    private DaprClient _client;

    public OrderOrchestrator(ILogger<OrderOrchestrator> logger, DaprClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task CardConfirmedEventHandler(CardConfirmedEvent evt)
    {
        _logger.LogInformation("CardConfirmedEvent received");
        var orderRegistered = new OrderRegisteredEvent
        {
            Id = Guid.NewGuid(),
            Items = evt.Items,
        };
        await _client.PublishEventAsync("pubsub", "inventorytopic", orderRegistered);
    }
}
