using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class SignalingHub : Hub
{
    // Khi người dùng tham gia phòng
    public async Task JoinRoom(string roomId)
    {
        // thêm vào group
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        // gửi thông báo đến tất cả người trong phòng
        await Clients.GroupExcept(roomId, Context.ConnectionId)
                     .SendAsync("UserJoined", Context.ConnectionId);
    }

    public async Task CreateRoom(string roomId)
    {
        // thêm vào group, tạo group nếu không tồn tại
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    // Khi người dùng rời khỏi phòng (bạn có thể gọi hàm này từ client)
    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        // Gửi thông báo đến các người khác
        await Clients.Group(roomId).SendAsync("UserLeft", Context.ConnectionId);
    }

    public override async Task OnConnectedAsync()
    {
        // gửi thông báo đến người dùng
        await Clients.Caller.SendAsync("ReceiveConnectionId", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    // Gửi offer tới tất cả người trong phòng
    public async Task SendOfferToRoom(string roomId, string offer)
    {
        await Clients.Group(roomId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
    }

    // Gửi ice candidate tới tất cả người trong phòng
    public async Task SendIceCandidateToRoom(string roomId, string candidate)
    {
        await Clients.Group(roomId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
    }

    // Gửi answer tới người gửi offer
    public async Task SendAnswer(string targetConnectionId, string answer)
    {
        await Clients.Client(targetConnectionId)
                     .SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
    }
    // Khi kết nối bị ngắt thì xóa khỏi tất cả group
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    // Gửi offer tới một user cụ thể trong phòng
    public async Task SendOffer(string targetConnectionId, string offer)
    {
        await Clients.Client(targetConnectionId)
                     .SendAsync("ReceiveOffer", Context.ConnectionId, offer);
    }


    // Gửi ICE candidate
    public async Task SendIceCandidate(string targetConnectionId, string candidate)
    {
        await Clients.Client(targetConnectionId)
                     .SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
    }
}
