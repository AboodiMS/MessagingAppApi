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
        [Route("SearchUsersForInvetation")]
        public async Task<List<GetUserDto>> SearchUsersForInvetation([FromQuery] [Required] Guid roomId)
        {
            var list = await DbContext.Users
                            .Where(a => a.Deleted == false)
                            .Include(a=>a.RoomMemberships)
                            .Where(a=>a.RoomMemberships.Count(a=>a.RoomId == roomId && a.Deleted ==false)==0)
                            .OrderBy(a => a.CreatedAt)
                            .AsNoTracking()
                            .ToListAsync();
            var listDto = Mapper.Map<List<GetUserDto>>(list);
            return listDto;
        }

    }
}
