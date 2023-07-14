using AutoMapper;
using MessagingAppApi.Dtos.Messages;
using MessagingAppApi.Dtos.RoomMemberships;
using MessagingAppApi.Dtos.Users;
using MessagingAppApi.Filters;
using MessagingAppApi.Model;
using MessagingAppApi.Model.Entities;
using MessagingAppApi.Shared.Controllers;
using MessagingAppApi.Shared.Exceptions;
using MessagingAppApi.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MessagingAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizationAction]
    public class RoomMembershipsController : BaseController
    {
        public RoomMembershipsController(MessagingAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<GetRoomMembershipDto>> Get([FromQuery] RoomMembershipsFilter filters)
        {
            await CheckUserMembership(filters.RoomId);
            var list = await DbContext.RoomMemberships
                            .Where(a => filters.FromCreatedDate == null || a.CreatedAt >= filters.FromCreatedDate)
                            .Where(a => filters.ToCreatedDate == null || a.CreatedAt <= filters.ToCreatedDate)
                            .Where(a => a.Deleted == false)

                            .Where(a => a.RoomId == filters.RoomId)
                            .Where(a=> filters.Accepted == null || a.Accepted == filters.Accepted)

                            .OrderBy(a => a.CreatedAt)
                            .Skip(filters?.Skip ?? 0)
                            .Take(filters?.Take ?? 10)                           
                            .Include(a=>a.User)
                            .ToListAsync();
            var listDto = Mapper.Map<List<GetRoomMembershipDto>>(list);
            return listDto;
        }

        private async Task CheckUserMembership(Guid roomId)
        {
            var entity = await DbContext.RoomMemberships
                .Where(a=>a.RoomId == roomId && a.UserId==UserId && a.Deleted==false).FirstOrDefaultAsync();
            if (entity == null)
                throw new BaseException("YouNotMemberInRoom");
        }

        [HttpPost]
        [Route("Invitation")]
        public async Task Invitation([FromBody] InvitationRoomMembershipDto dto)
        {
            await CheckUserInRoomMembership(dto);
            var entity = Mapper.Map<RoomMembership>(dto);
            entity.Accepted = false;
            entity.SetCreateInfo(UserId);
            DbContext.RoomMemberships.Add(entity);
            await DbContext.SaveChangesAsync();
        }
        private async Task CheckUserInRoomMembership(InvitationRoomMembershipDto dto)
        {
            var entity = await DbContext.RoomMemberships.Where(a => a.UserId == dto.UserId 
                                                                 && a.RoomId == dto.RoomId 
                                                                 && a.Deleted == false).FirstOrDefaultAsync();
            if (entity != null && entity.Accepted)
                throw new BaseException("thisUserAlreadyInRoom");
            if (entity != null && entity.Accepted == false)
                throw new BaseException("InvitationHasAlreadySentToThisUser");

        }

        [HttpPost]
        [Route("Accepte")]
        public async Task Accepte(Guid id)
        {
            var entity = await DbContext.RoomMemberships.Where(a => a.Id == id && a.Deleted == false && a.UserId == UserId).FirstOrDefaultAsync();
            if (entity == null)
                throw new BaseException("NotFound");
            if (entity.Accepted)
                throw new BaseException("AlreadyAccepted");
            entity.Accepted = true;
            entity.SetUpdateInfo(UserId);
            await DbContext.SaveChangesAsync();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(Guid id)
        {
            var entity = await DbContext.RoomMemberships.Where(a => a.Id == id 
                                                                 && a.Deleted == false 
                                                                 && a.Accepted == false
                                                                 && a.UserId == UserId)                                           
                                                                 .FirstOrDefaultAsync();
            if (entity == null)
                throw new BaseException("NotFound");
            entity.Accepted = false;
            entity.SetDeleteInfo(UserId);
            await DbContext.SaveChangesAsync();
        }

        [HttpGet]
        [Route("GetUsersInvitations")]
        public async Task<List<GetUserInvitationsDto>> GetUsersInvitations()
        {
            var list = await DbContext.RoomMemberships
                                .Where(a => a.Deleted == false)
                                .Where(a => a.Accepted == false)
                                .Where(a => a.UserId == UserId)
                                .Include(a=>a.Room)
                                .ToListAsync();
            var listDto = Mapper.Map<List<GetUserInvitationsDto>>(list);
            return listDto;
        }
    }
}
