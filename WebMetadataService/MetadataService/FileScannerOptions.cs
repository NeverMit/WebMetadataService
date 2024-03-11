using Newtonsoft.Json;

namespace WebMetadataService.MetadataService;

public sealed class FileScannerOptions
{
    //путь к директории
    [JsonProperty("Path")]
    public string? Path { get; set; }
    //маска файла
    [JsonProperty("SearchPattern")]
    public string? SearchPattern { get; set; }
    //интервал времени
    [JsonProperty("Interval")]
    public int Interval { get; set; }
}