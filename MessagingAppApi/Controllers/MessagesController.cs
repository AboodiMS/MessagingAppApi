using AutoMapper;
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

namespace MessagingAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            //await MessageHubContext.Clients.Group(dto.RoomId.ToString()).SendAsync("ReceiveMessage", dto);
            await MessageHubContext.Clients.All.SendAsync("ReceiveMessage", new GetMessageDto()
            {
                Id=entity.Id,
                CreatedAt=entity.CreatedAt,
                CreatedBy=entity.CreatedBy,
                Text=entity.Text,   
                RoomId= entity.RoomId,
            });
        }
        private async Task CheckRoomMembership(Guid roomId)
        {
            var entity = await DbContext.RoomMemberships
                                        .Where(a=>a.RoomId == roomId && a.UserId == UserId && a.Deleted == false)
                                        .FirstOrDefaultAsync();
            if (entity == null)
                throw new BaseException("YouAreNotMemberInThisRoom");
        }

        //[HttpPost]
        //[Route("Update")]
        //public async Task Update([FromBody] UpdateMessageDto dto)
        //{
        //    var entity = await Get(dto.Id);
        //    await CheckRoomMembership(entity.RoomId);
        //    Mapper.Map(dto,entity);
        //    entity.SetUpdateInfo(UserId);
        //    await DbContext.Messages.AddAsync(entity);
        //    await DbContext.SaveChangesAsync();
        //}

        //private async Task<Message> Get(Guid id)
        //{
        //    var entity = await DbContext.Messages.Where(a => a.Id == id && a.Deleted == false).FirstOrDefaultAsync();
        //    if (entity == null)
        //        throw new BaseException("NotFound");
        //    if(entity.CreatedBy != UserId)
        //        throw new UnauthorizedAccessException("CanNotAccessThisMessage");
        //    return entity;
        //}
    }
}
