﻿using AutoMapper;
using MessagingAppApi.Dtos.Messages;
using MessagingAppApi.Dtos.Users;
using MessagingAppApi.Filters;
using MessagingAppApi.Model;
using MessagingAppApi.Model.Entities;
using MessagingAppApi.Shared.Controllers;
using MessagingAppApi.Shared.Exceptions;
using MessagingAppApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MessagingAppApi.Shared.Security;

namespace MessagingAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizationAction]
    public class MessagesController : BaseController
    {
        private readonly IHubContext<MessageHub> MessageHubContext;
        public MessagesController(MessagingAppDbContext dbContext, IMapper mapper, IHubContext<MessageHub> messageHubContext) : base(dbContext, mapper)
        {
            MessageHubContext = messageHubContext;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<GetMessageDto>> Get([FromQuery] MessagesFilter filters) 
        {
            await CheckRoomMembership(filters.RoomId);
            var list = await DbContext.Messages
                .Where(a => filters.FromCreatedDate == null || a.CreatedAt >= filters.FromCreatedDate)
                .Where(a => filters.ToCreatedDate == null || a.CreatedAt <= filters.ToCreatedDate)
                .Where(a => a.Deleted == false)

                .Where(a => a.RoomId == filters.RoomId)
                .Where(a => a.Id != UserId)
                .OrderByDescending(a => a.CreatedAt)
                .Skip(filters?.Skip ?? 0)
                .Take(filters?.Take ?? 10)
                .OrderBy(a => a.CreatedAt)
                .AsNoTracking()
                .ToListAsync();

            var listDto = Mapper.Map<List<GetMessageDto>>(list);
            return listDto;
        }

        [HttpPost]
        [Route("Create")]
        public async Task Create([FromBody] CreateMessageDto dto)
        {
            await CheckRoomMembership(dto.RoomId);
            var entity = Mapper.Map<Message>(dto);
            entity.Id = Guid.NewGuid();
            entity.SetCreateInfo(UserId);
            await DbContext.Messages.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            await MessageHubContext.Clients.Group(entity.RoomId.ToString()).SendAsync("ReceiveMessage", new GetMessageDto()
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                Text = entity.Text,
                RoomId = entity.RoomId,
            });
        }
        private async Task CheckRoomMembership(Guid roomId)
        {
            var entity = await DbContext.RoomMemberships
                                        .Where(a=>a.RoomId == roomId && a.UserId == UserId && a.Deleted == false && a.Accepted)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();
            if (entity == null)
                throw new BaseException("YouAreNotMemberInThisRoom");
        }

    }
}
