using MessagingAppApi.Dtos.Messages;
using Microsoft.AspNetCore.SignalR;

namespace MessagingAppApi.Hubs
{
    public class MessageHub:Hub
    {
        public async Task ReceiveMessage(GetMessageDto dto)
        {
            await Clients.All.SendAsync("ReceiveMessage", dto);
        }
    }
}
