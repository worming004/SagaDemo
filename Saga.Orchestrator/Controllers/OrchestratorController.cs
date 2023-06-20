using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Saga.Events;

namespace Saga.Orchestrator.Controllers;

[ApiController]
[Route("main")]
public class OrchestratorController : ControllerBase
{
    private DaprClient _client;
    private ILogger<OrchestratorController> _logger;

    public OrchestratorController(ILogger<OrchestratorController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Topic("main", "orchestrator")]
    public async Task<IActionResult> CardCompletedEventHandler(CardConfirmedEvent evt)
    {
        // await _client.PublishEventAsync("main", "orchestrator", Map(card));
        System.IO.File.WriteAllText("data.log", evt.ToString());
       _logger.LogInformation("CardConfirmedEvent received") ;

        return Ok();
    }
}

