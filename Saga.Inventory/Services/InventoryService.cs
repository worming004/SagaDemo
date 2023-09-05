using Dapr.Client;
using Saga.Events;
using Saga.Events.Common;
using Saga.Inventory.Commands;
using Saga.Inventory.Exceptions;
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
        var groupedItems = evt.Items.GroupBy(t => t.Name);

        var toAwait = groupedItems.Select<IGrouping<string, Item>, Task<(ItemInventory, IEnumerable<Item>)>>(async it =>
        {
            // TODO use GetBulkStateAsync
            var items = await _client.GetStateAsync<ItemInventory>(DefaultValues.Dapr.DefaultStateStore, it.Key, ConsistencyMode.Strong);
            await ValidateAndSendError(evt, it, items);

            return (items, it);
        });

        var inventoryItemPair = await Task.WhenAll(toAwait);

        // If no error happened, we can apply the event handler
        foreach (var (iventory, itemRequest) in inventoryItemPair)
        {
            iventory.AvailableCount -= itemRequest.Count();
        }

        var toStore = inventoryItemPair.Select(iv => iv.Item1).Select(iv => new SaveStateItem<ItemInventory>(iv.Name, iv, null)).ToList();

        await _client.SaveBulkStateAsync(DefaultValues.Dapr.DefaultStateStore, toStore);
    }


    // TODO compensate available items;

    private async Task ValidateAndSendError(OrderRegisteredEvent evt, IGrouping<string, Item> it, ItemInventory? items)
    {
        if (items is null)
        {
            await _client.PublishEventAsync("pubsub", "invalidinventoryrequest", new Events.InvalidInventoryRequest
            {
                OriginEventId = evt.Id,
                ItemName = it.Key,
                Message = $"Item with name {it.Key} is not found in inventory",
                InvalidType = InvalidInventoryRequestType.InventoryNotFound
            });

            throw new InvalidRequestException();
        }

        if (it.Count() > items.AvailableCount)
        {
            await _client.PublishEventAsync("pubsub", "invalidinventoryrequest", new Events.InvalidInventoryRequest
            {
                OriginEventId = evt.Id,
                ItemName = it.Key,
                Message = $"Item with name {it.Key} do not have sufficient available storage",
                InvalidType = InvalidInventoryRequestType.InsufficientInventory
            });

            throw new InvalidRequestException();
        }
    }
}
