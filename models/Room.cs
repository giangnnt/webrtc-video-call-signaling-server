public class Room
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Name { get; set; }
    public List<string> ConnectionIds { get; set; } = new List<string>();
}

