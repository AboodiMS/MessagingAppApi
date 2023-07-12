using AutoMapper;
using MessagingAppApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MessagingAppApi.Shared.Controllers
{
    public class BaseController: ControllerBase
    {
        protected readonly MessagingAppDbContext DbContext;
        protected readonly IMapper Mapper;
        public Guid UserId
        {
            get { return GetUserId(); }
        }
        public string Username
        {
            get { return GetUsername(); }
        }
        public BaseController(MessagingAppDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        protected Guid GetUserId()
        {
            try
            {
                string id = HttpContext.User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value;
                return Guid.Parse(id);
            }
            catch
            {
                return Guid.Empty;
            }

        }
        protected string GetUsername()
        {
            try
            {
                string Username = HttpContext.User.Claims.Where(x => x.Type == "Username").FirstOrDefault().Value;
                return Username;
            }
            catch
            {
                return string.Empty;
            }

        }
    }
}
