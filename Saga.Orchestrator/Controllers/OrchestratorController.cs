using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Saga.Events;
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
    public async Task<IActionResult> CardConfirmedEventHandler(CardConfirmedEvent evt)
    {
        _orchestrator.CardConfirmedEventHandler(evt);

        return Accepted();
    }
}

