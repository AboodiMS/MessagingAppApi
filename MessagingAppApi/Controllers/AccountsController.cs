using AutoMapper;
using MessagingAppApi.Dtos.Accounts;
using MessagingAppApi.Model;
using MessagingAppApi.Model.Entities;
using MessagingAppApi.Shared.Controllers;
using MessagingAppApi.Shared.Exceptions;
using MessagingAppApi.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MessagingAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        public AccountsController(MessagingAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        [Route("Get")]
        [AuthorizationAction]
        public async Task<GetAccountDto> Get()
        {
            var entity = await DbContext.Users.Where(a=>a.Id==UserId && a.Deleted == false)
                  .AsNoTracking().FirstOrDefaultAsync();
            var dto = Mapper.Map<GetAccountDto>(entity);
            return dto;
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<string> Login([FromBody] LoginAccountDto dto)
        {
            var entity = await DbContext.Users.Where(a => a.Username == dto.Username && a.Deleted == false)
                .AsNoTracking().FirstOrDefaultAsync();
            if (entity == null || entity.Password != dto.Password.HashPassword())
                throw new BaseException("ErrorUsernameOrPassword");
            return JwtExtension.CreateToken(entity.Id.ToString(), entity.Username);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<string> Register([FromBody] RegisterAccountDto dto)
        {
            var entity = Mapper.Map<User>(dto);
            await CheckUserName(entity.Username,Guid.Empty);
            entity.Id = Guid.NewGuid();
            entity.Password = dto.Password.HashPassword();
            entity.SetCreateInfo(entity.Id);
            DbContext.Users.Add(entity);
            await DbContext.SaveChangesAsync();
            return JwtExtension.CreateToken(entity.Id.ToString(), entity.Username);
        }

        private async Task CheckUserName(string username,Guid id) 
        {
            bool UsernameNotAvailable = await DbContext.Users.AnyAsync(a => a.Username == username && a.Deleted == false && a.Id != id );
            if (UsernameNotAvailable)
                throw new BaseException("UsernameNotAvailable");
        }
    }
}
