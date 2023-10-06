using System.Text.Json.Serialization;

namespace BackgroundService.Models;

public class TempData
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    [JsonPropertyName("hum")]
    public double Hum { get; set; }
}
