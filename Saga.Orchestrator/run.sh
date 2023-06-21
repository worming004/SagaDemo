dapr run --app-id orchestrator --resources-path ../components --resources-path ~/.dapr/components --app-port 5001 --log-level debug -- dotnet run -lp http
