using MessagingAppApi.Dtos.Messages;
using MessagingAppApi.Model;
using MessagingAppApi.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;

namespace MessagingAppApi.Hubs
{
    [AuthorizationAction]
    public class MessageHub:Hub
    {
        private readonly MessagingAppDbContext DbContext;
        public MessageHub(MessagingAppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public override async Task OnConnectedAsync()

        {
            var userId = GetUserId();
            var rooms = await DbContext.RoomMemberships.Where(a => a.UserId == userId && a.Accepted && a.Deleted == false).ToListAsync();
            foreach (var room in rooms)
                await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId.ToString());
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = GetUserId();
            var rooms = await DbContext.RoomMemberships.Where(a => a.UserId == userId && a.Accepted && a.Deleted == false).ToListAsync();
            foreach (var room in rooms)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.RoomId.ToString());
            await base.OnDisconnectedAsync(exception);
        }
        protected Guid GetUserId()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                if (httpContext != null)
                {
                    var data = httpContext.User.Claims.ToList();
                    string id = httpContext.User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value;
                    return Guid.Parse(id);
                }
                return Guid.Empty;
            }
            catch
            {
                return Guid.Empty;
            }

        }
    }
}
