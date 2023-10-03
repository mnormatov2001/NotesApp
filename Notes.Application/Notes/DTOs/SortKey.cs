using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
