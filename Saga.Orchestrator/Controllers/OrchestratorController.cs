using Microsoft.AspNetCore.Mvc;
using Saga.Events;
using Saga.Events.Common;
using Saga.Orchestrator.Services.Orchestrator;

namespace Saga.Orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class OrchestratorController : ControllerBase
{
    private OrderOrchestrator _orchestrator;
    private ILogger<OrchestratorController> _logger;

    public OrchestratorController(ILogger<OrchestratorController> logger, OrderOrchestrator orchestrator)
    {
        _logger = logger;
        _orchestrator = orchestrator;
    }

    [HttpPost]
    [Route("cardconfirmed")]
    public async Task<IActionResult> CardConfirmedEventHandler(DaprData<CardConfirmedEvent> daprEvt)
    {
        var evt = daprEvt.Data;
        _logger.LogInformation($"event: {evt.ToString()}");
        _orchestrator.CardConfirmedEventHandler(evt);

        return Accepted();
    }
}

