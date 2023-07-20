using Microsoft.AspNetCore.Mvc;

namespace Saga.Inventory.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase
{
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(ILogger<InventoryController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        
      return Ok();
    }
}

