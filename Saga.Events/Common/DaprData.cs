using System.Text.Json.Serialization;
namespace Saga.Events.Common;

public record DaprData<T> ([property: JsonPropertyName("data")] T Data); 
