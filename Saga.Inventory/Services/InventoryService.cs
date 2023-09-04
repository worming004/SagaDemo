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

    public async Task HandlerOrderRegistered(OrderRegisteredEvent evt)
    {
        if (!await ValidateAndPublishError(evt))
        {
            return;
        }

        var toAwait = evt.Items.Select(async it =>
        {
            var items = await _client.GetStateAsync<ItemInventory>(DefaultValues.Dapr.DefaultStateStore, it.Name);
            if (items is null)
            {
                await _client.PublishEventAsync("pubsub", "invalidinventoryrequest", new Events.InvalidInventoryRequest
                {
                    OriginEventId = evt.Id,
                    ItemName = it.Name,
                    Message = $"Item with name {duplicates.First().Key} is duplicated in the request",
                    InvalidType = InvalidInventoryRequestType.InventoryNotFound
                });
            }

            if (items.Count < it.)
        });

        var inventory = await Task.WaitAll(toAwait);
    }


    private async Task<bool> Validate(OrderRegisteredEvent evt)
    {
        var duplicates = evt.Items.GroupBy(x => x.Name).Where(g => g.Count() > 1);
        if (duplicates.Any())
        {
            await _client.PublishEventAsync("pubsub", "invalidinventoryrequest", new Events.InvalidInventoryRequest
            {
                OriginEventId = evt.Id,
                ItemName = duplicates.First().Key,
                Message = $"Item with name {duplicates.First().Key} is duplicated in the request",
                InvalidType = InvalidInventoryRequestType.Duplicates
            });

            return false;
        }

        return true;
    }
}
