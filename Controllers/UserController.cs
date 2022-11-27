using metronic_extensions_api.Services;
using metronic_extensions_api.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace metronic_extensions_api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        private readonly ManagementAPIService _mgmtSvc;

        public UserController(ILogger<UserController> logger , ManagementAPIService svc)
        {
            _logger = logger;
            _mgmtSvc = svc;
        }

        [HttpGet("users",Name = "GetUsers")]
        public UserResponse Get(int page= 1,int items_per_page= 10, string? sort= null, string? order=null)
        {
            return _mgmtSvc.GetUsers(page, items_per_page, sort, order);
                   
        }
    }
}