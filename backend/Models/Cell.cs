
using System.Text.Json.Serialization;

namespace backend.Models;

public class Cell
{
    public int Id { get; set; }
    public int CellNumber {get; set;}
    public int SessionId { get; set; }

    [JsonIgnore]
    public Session? Session { get; set; }
    public char Status { get; set; } = '-';
}