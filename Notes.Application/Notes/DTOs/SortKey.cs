using System.Text.Json.Serialization;

namespace Notes.Application.Notes.DTOs
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortKey
    {
        CreationDate,
        EditDate,
        Title
    }
}
