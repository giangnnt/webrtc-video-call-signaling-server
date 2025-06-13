using System.Collections.Concurrent;

public sealed class RoomManager
{
    private static readonly Lazy<RoomManager> _instance = new Lazy<RoomManager>(() => new RoomManager());
    public static RoomManager Instance => _instance.Value;

    private readonly ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();

    public Room CreateRoom(string name)
    {
        var room = new Room
        {
            Name = name,
        };
        _rooms.TryAdd(room.Id, room);
        return room;
    }

    public void AddConnectionToRoom(string roomId, string connectionId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.ConnectionIds.Add(connectionId);
        }
    }

    public void RemoveConnectionFromRoom(string roomId, string connectionId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.ConnectionIds.Remove(connectionId);
        }
    }

    public Room? GetRoom(string roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            return room;
        }
        return null;
    }
}