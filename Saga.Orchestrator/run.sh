dapr run --app-id orchestrator --resources-path ../components - --app-port 5001 --log-level debug -- dotnet run -lp httpdapr run --app-id card --app-port 5000 -- dotnet run -lp http
