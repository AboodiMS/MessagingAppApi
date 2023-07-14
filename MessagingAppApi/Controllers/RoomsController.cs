using AutoMapper;
using MessagingAppApi.Dtos.Messages;
using MessagingAppApi.Dtos.Rooms;
using MessagingAppApi.Dtos.Users;
using MessagingAppApi.Filters;
using MessagingAppApi.Model;
using MessagingAppApi.Model.Entities;
using MessagingAppApi.Shared.Controllers;
using MessagingAppApi.Shared.Exceptions;
using MessagingAppApi.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizationAction]
    public class RoomsController : BaseController
    {
        public RoomsController(MessagingAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<GetRoomDto>> Get([FromQuery] RoomsFilter filters)
        {
            var list = await DbContext.Rooms
                .Where(a => filters.FromCreatedDate == null || a.CreatedAt >= filters.FromCreatedDate)
                .Where(a => filters.ToCreatedDate == null || a.CreatedAt <= filters.ToCreatedDate)
                .Where(a => a.Deleted == false)

                .Where(a => string.IsNullOrEmpty(filters.Name) || a.Name.Contains(filters.Name))

                .Include(a => a.RoomMemberships)
                .Where(a=>a.RoomMemberships.Any(a=>a.UserId==UserId && a.Deleted==false && a.Accepted))

                .Skip(filters?.Skip ?? 0)
                .Take(filters?.Take ?? 10)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();

            var listDto = Mapper.Map<List<GetRoomDto>>(list);
            return listDto;
        }

        [HttpGet]
        public async Task<GetRoomDto> Get(Guid id)
        {
            var entity = await DbContext.Rooms
                .Include(a => a.RoomMemberships)
                .Where(a => a.Deleted == false && a.Id==id)

                .Where(a => a.RoomMemberships.Any(a => a.UserId == UserId && a.Deleted == false))
                .FirstOrDefaultAsync();
            if (entity == null)
                throw new BaseException("NotFoundRoom");
            var dto = Mapper.Map<GetRoomDto>(entity);
            return dto;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<Guid> Create([FromBody] CreateRoomDto dto)
        {
            var entity = Mapper.Map<Room>(dto);
            await CheckName(entity.Name, Guid.Empty);
            entity.Id = Guid.NewGuid();
            entity.SetCreateInfo(UserId);
            DbContext.Rooms.Add(entity);
            var roomMemberships = new RoomMembership()
            {
                Id = Guid.NewGuid(),
                RoomId = entity.Id,
                UserId = UserId,
                Accepted = true,
            };
            roomMemberships.SetCreateInfo(UserId);
            DbContext.RoomMemberships.Add(roomMemberships);
            await DbContext.SaveChangesAsync();
            return entity.Id;
        }

        private async Task CheckName(string name, Guid id)
        {
            bool NameNotAvailable = await DbContext.Rooms.AnyAsync(a => a.Name == name && a.Id != id);
            if (NameNotAvailable)
                throw new BaseException("NameNotAvailable");
        }

        [HttpPost]
        [Route("Update")]
        public async Task Update([FromBody] UpdateRoomDto dto)
        {
            var entity = await DbContext.Rooms.FindAsync(dto.Id);
            if (entity == null)
                throw new BaseException("NotFound");
            await CheckName(entity.Name, entity.Id);
            Mapper.Map(dto, entity);            
            entity.SetUpdateInfo(UserId);
            await DbContext.SaveChangesAsync();
        }
    }
}























