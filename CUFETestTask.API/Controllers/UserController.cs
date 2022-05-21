using CUFETestTask.API.Data.Models;
using CUFETestTask.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CUFETestTask.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;
        public UserController(IUserService userService)
        {
            _UserService = userService;
        }
        // POST: api/users
        [HttpPost]
        public IActionResult Post([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest("User is null.");
            }
            if (user.Password.Length < 8 || !user.Password.Any(c => char.IsUpper(c)) || !user.Password.Any(c => char.IsLower(c)) || !user.Password.Any(char.IsDigit))
            {
                return BadRequest("password is invalid!, it should contain capital letter, small letter and numbers!, min length is 8");
            }

            try
            {
                _UserService.AddNewUser(user);
                if (user.UserId == 0)
                {
                    return Problem("There is a problem in creating user");
                }
                else
                {
                    return Ok("user created");

                }

            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }
        // POST: api/users/getuser
        [HttpPost]
        [Route("getuser")]
        public IActionResult GetUsers([FromBody] UserModel user)
        {
            UserModel resUser = _UserService.GetUserData(user).Result;
            if (resUser == null)
            {
                return NotFound("cannot find user with this data");
            }
            else
            {
                return Ok(resUser);
            }
        }
        // POST: api/users/resetpassword
        [HttpPost]
        [Route("resetpassword")]
        public IActionResult ResetPassword([FromBody] UserModel user, string newPassword)
        {
            if (user == null)
            {
                return BadRequest("User is null.");
            }
            if (newPassword.Length < 8 || !newPassword.Any(c => char.IsUpper(c)) || !newPassword.Any(c => char.IsLower(c)) || !newPassword.Any(char.IsDigit)

                || user.Password.Length < 8 || !user.Password.Any(c => char.IsUpper(c)) || !user.Password.Any(c => char.IsLower(c)) || !user.Password.Any(char.IsDigit)
                )
            {
                return BadRequest("password is invalid!, it should contain capital letter, small letter and numbers!, min length is 8");
            }
            else
            {
                bool resUser = _UserService.ResetPassword(user, newPassword).Result;
                if (resUser == false)
                {
                    return NotFound("cannot find user with this data");
                }
                else
                {
                    return Ok("password updated");
                }
            }
        }
    }
}
