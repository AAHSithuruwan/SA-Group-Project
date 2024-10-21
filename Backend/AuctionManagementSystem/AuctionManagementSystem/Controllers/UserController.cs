using AuctionManagementSystem.Models;
using AuctionManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using AuctionManagementSystem.Services;
using AuctionManagementSystem.JwtAuthentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AuctionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtAuthenticationService _jwtAuthenticationService;

        public UserController(UserService userService, JwtAuthenticationService jwtAuthenticationService)
        {
            _userService = userService;
            _jwtAuthenticationService = jwtAuthenticationService;
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

            var jwtToken = _jwtAuthenticationService.GenerateToken(user.UserId);

            return Ok(new { Token = jwtToken });
        }

        // GET: api/User/LogOut
        [Authorize]
        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {
            return Ok("Sign Out Successfull");
        }

        // GET: api/User
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var user = await  _userService.GetUser(int.Parse(userId));

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(user);
        }

        // PUT: api/User/UpdatePersonalDetails
        [Authorize]
        [HttpPut("UpdatePersonalDetails")]
        public async Task<IActionResult> UpdateUserPersonalDetails([FromForm] UserPersonalDetailsUpdateModel userPersonalDetailsUpdateModel)
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var user = await _userService.UpdateUserPersonalDetails(userPersonalDetailsUpdateModel, int.Parse(userId));

            if (user == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(user);
        }

        // PUT: api/User/UpdateEmail
        [Authorize]
        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateUserEmail([FromBody] User userDetails)
        {
            if (userDetails == null)
            {
                return BadRequest();
            }

            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var (user, isSuccess, errorMessage) = await _userService.UpdateUserEmail(userDetails, int.Parse(userId));

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
        [Authorize]
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UserPasswordUpdateModel userPasswordUpdateModel)
        {
            if (userPasswordUpdateModel == null)
            {
                return BadRequest();
            }

            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var (user, isSuccess) = await _userService.UpdateUserPassword(userPasswordUpdateModel, int.Parse(userId));

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
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            String? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not signed in");
            }

            var (isUserFound, isUserDeleted) = await _userService.DeleteUser(int.Parse(userId));

            if(isUserFound == false)
            {
                return NotFound("User Not Found");
            }

            if (isUserDeleted == false)
            {
                return BadRequest("User cannot be deleted. Because there are auctions associated with this seller account");
            }

            return Ok("User successfully deleted");
        }
    }
}
