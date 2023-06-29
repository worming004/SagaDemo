using Dapr.Client;
using Saga.Events;
using Saga.Events.Common;
using Saga.Inventory.Commands;
using Saga.Inventory.Models;

namespace Saga.Inventory.Services;

public class InventoryService
{
    private ILogger<InventoryService> _logger;
    private DaprClient _client;


    public InventoryService(ILogger<InventoryService> logger, DaprClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task ResetInventory(ResetInventory reset)
    {
        await _client.DeleteBulkStateAsync(DefaultValues.Dapr.DefaultStateStore, null);
        var defaultInventory = ItemInventoryGenerator.GenerateInventory();
        var toAwait = defaultInventory.Select(async i =>
        {
            await _client.SaveStateAsync(DefaultValues.Dapr.DefaultStateStore, i.Name, i);
        }).ToList();

        await Task.WhenAll(toAwait);
    }




    public void HandlerOrderRegistered(OrderRegisteredEvent evt)
    {

    }
}
