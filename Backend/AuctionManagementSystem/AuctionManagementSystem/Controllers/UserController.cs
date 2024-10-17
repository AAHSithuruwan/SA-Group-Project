using AuctionManagementSystem.Models;
using AuctionManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using AuctionManagementSystem.Services;

namespace AuctionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // POST: api/User/SignUp
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var createdUser = await _userService.SignUp(user);

            if (createdUser == null)
            {
                return BadRequest("Email Address is already registered with another account");
            }
            return Ok(createdUser);
        }

        // POST: api/User/SignIn
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] User signInDetails)
        {
            if (signInDetails == null)
            {
                return BadRequest();
            }

            var user = await _userService.SignIn(signInDetails);

            if (user == null)
            {
                return Unauthorized("Invalid Email or Passowrd");
            }

            //Create session
            HttpContext.Session.SetInt32("UserId", user.UserId);

            return Ok(user);    
        }

        // GET: api/User/LogOut
        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {
            //Clear the session
            HttpContext.Session.Clear();

            //Clear the session cookie
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1), // Expire the cookie immediately
                HttpOnly = true, // Prevent JavaScript access to the cookie
                Secure = true // Set to true if using HTTPS
            };

            Response.Cookies.Append(".AspNetCore.Session", "", cookieOptions);

            return Ok("Sign Out Successfull");
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if(userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var user = await  _userService.GetUser((int)userId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(user);
        }

        // PUT: api/User/UpdatePersonalDetails
        [HttpPut("UpdatePersonalDetails")]
        public async Task<IActionResult> UpdateUserPersonalDetails([FromForm] UserPersonalDetailsUpdateModel userPersonalDetailsUpdateModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var user = await _userService.UpdateUserPersonalDetails(userPersonalDetailsUpdateModel, (int)userId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(user);
        }

        // PUT: api/User/UpdateEmail
        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateUserEmail([FromBody] User userDetails)
        {
            if (userDetails == null)
            {
                return BadRequest();
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var (user, isSuccess, errorMessage) = await _userService.UpdateUserEmail(userDetails, (int)userId);

            if(user == null)
            {
                return NotFound("User Not Found");
            }

            if(isSuccess == false &&  errorMessage != null)
            {
                return BadRequest(errorMessage);
            }

            if(isSuccess == false && errorMessage == null)
            {
                return Unauthorized("Incorrect Password");
            }

            return Ok(user);
        }

        // PUT: api/User/UpdatePassword
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UserPasswordUpdateModel userPasswordUpdateModel)
        {
            if (userPasswordUpdateModel == null)
            {
                return BadRequest();
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var (user, isSuccess) = await _userService.UpdateUserPassword(userPasswordUpdateModel, (int)userId);

            if(user == null)
            {
                return NotFound("User Not Found");
            }

            if(isSuccess == false)
            {
                return Unauthorized("Incorrect Current Password");
            }

            return Ok(user);
        }

        // Delete: api/User
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            var (isUserFound, isUserDeleted) = await _userService.DeleteUser((int)userId);

            if(isUserFound == false)
            {
                return NotFound("User Not Found");
            }

            if (isUserDeleted == false)
            {
                return BadRequest("User cannot be deleted. Because there are auctions associated with this seller account");
            }

            //Clear the session
            HttpContext.Session.Clear();

            //Clear the session cookie
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1), // Expire the cookie immediately
                HttpOnly = true, // Prevent JavaScript access to the cookie
                Secure = true // Set to true if using HTTPS
            };

            Response.Cookies.Append(".AspNetCore.Session", "", cookieOptions);

            return Ok("User successfully deleted");
        }
    }
}
