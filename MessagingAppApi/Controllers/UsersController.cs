using AutoMapper;
using MessagingAppApi.Dtos.Messages;
using MessagingAppApi.Dtos.Users;
using MessagingAppApi.Filters;
using MessagingAppApi.Model;
using MessagingAppApi.Shared.Controllers;
using MessagingAppApi.Shared.Dtos;
using MessagingAppApi.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MessagingAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizationAction]
    public class UsersController : BaseController
    {
        public UsersController(MessagingAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<GetUserDto>> Get([FromQuery] UsersFilter filters)
        {
            var list = await DbContext.Users
                            .Where(a => filters.FromCreatedDate == null || a.CreatedAt >= filters.FromCreatedDate)
                            .Where(a => filters.ToCreatedDate == null || a.CreatedAt <= filters.ToCreatedDate)
                            .Where(a => a.Deleted == false)
                           
                            .Where(a => string.IsNullOrEmpty(filters.Username) || a.Username.Contains(filters.Username))
                            .Where(a=>a.Id != UserId)
                            .OrderBy(a => a.CreatedAt)
                            .Skip(filters?.Skip ?? 0)
                            .Take(filters?.Take ?? 10)  
                                                       
                            .ToListAsync();

            var listDto = Mapper.Map<List<GetUserDto>>(list);
            return listDto;
        }

        [HttpGet]
        [Route("SearchUsersForInvetation")]
        public async Task<List<GetUserDto>> SearchUsersForInvetation([FromQuery] [Required] Guid roomId)
        {
            var list = await DbContext.Users
                            .Where(a => a.Deleted == false)
                            .Include(a=>a.RoomMemberships)
                            .Where(a=>a.RoomMemberships.Count(a=>a.RoomId == roomId && a.Deleted ==false)==0)
                            .OrderBy(a => a.CreatedAt)
                            .ToListAsync();
            var listDto = Mapper.Map<List<GetUserDto>>(list);
            return listDto;
        }

    }
}
