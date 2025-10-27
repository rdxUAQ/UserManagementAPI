using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagementAPI.Api.Interfaces;
using UserManagementAPI.Api.Models;
using UserManagementAPI.Api.Models.Dtos;
using UserManagementAPI.Base;
using UserManagementAPI.Const;

namespace UserManagementAPI.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _logger = logger;
            _userService = userService;
        }

        // GET api/v1/users
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<BaseResponse<List<User>>>> GetAll()
        {
            
            var users = await _userService.GetAllUsersAsync();
            return Ok(new BaseResponse<List<User>> { Data = users });
        }

        // GET api/v1/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<User>>> GetById(string id)
        {
            var user = await _userService.GetUsersByIdAsync(id);
            if (user is null)
                return NotFound(new BaseResponse<User> { Data = null, Error = new Base.BaseError { Code = "USR404", Description = "User not found" } });

            return Ok(new BaseResponse<User> { Data = user });
        }

        // POST api/v1/users
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BaseResponse<bool>>> Create([FromBody] UserDto dto)
        {
            // basic model validation
            if (!ModelState.IsValid){
                    return BadRequest(new BaseResponse<bool> { Data = false, Error = ResponseErrors.UserInvalidAttribs });
                }

            

            var ok = await _userService.CreateNewUser(dto);
            if (!ok) return StatusCode(500, new BaseResponse<bool> { Data = false, Error = ResponseErrors.ServerDataSaveError });

            // success
            return Ok(new BaseResponse<bool> { Data = true });
        }

        // PUT api/v1/users/{id}
        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> Update(string id, [FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

           
            var ok = await _userService.UpdateUserById(id, dto);
            if (!ok) return NotFound(new BaseResponse<bool> { Data = false, Error = new Base.BaseError { Code = "USR404", Description = "User not found" } });
            return Ok(new BaseResponse<bool> { Data = true });
        }

        // DELETE api/v1/users/{id}
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> Delete(string id)
        {
            var ok = await _userService.DeleteUserById(id);
            if (!ok) return NotFound(new BaseResponse<bool> { Data = false, Error = new Base.BaseError { Code = "USR404", Description = "User not found" } });
            return Ok(new BaseResponse<bool> { Data = true });
        }
    }
}