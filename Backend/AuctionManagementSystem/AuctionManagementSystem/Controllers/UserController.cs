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

            try
            {
                User createdUser = await _userService.SignUp(user);
                return Ok(createdUser);
            }
            catch (InvalidOperationException ex1)
            {
                return BadRequest(ex1.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/User/SignIn
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] User signInDetails)
        {
            if (signInDetails == null)
            {
                return BadRequest();
            }

            try
            {
                User user = await _userService.SignIn(signInDetails);

                //Create session
                HttpContext.Session.SetInt32("UserId", user.UserId);

                return Ok(user);
            }
            catch(UnauthorizedAccessException ex1)
            {
                return Unauthorized(ex1.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            try
            {
                var user = await  _userService.GetUser((int)userId);

                return Ok(user);
            }
            catch(KeyNotFoundException ex1)
            {
                return NotFound(ex1.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/User/UpdatePersonalDetails
        [HttpPut("UpdatePersonalDetails")]
        public async Task<IActionResult> UpdateUserPersonalDetails([FromBody] UserPersonalDetailsUpdateModel userPersonalDetailsUpdateModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return BadRequest("User is not signed in");
            }

            try
            {
                var user = await _userService.UpdateUserPersonalDetails(userPersonalDetailsUpdateModel, (int)userId);
                return Ok(user);
            }
            catch (KeyNotFoundException ex1)
            {
                return NotFound(ex1.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            try
            {
                var user = await _userService.UpdateUserEmail(userDetails, (int)userId);
                return Ok(user);
            }
            catch (KeyNotFoundException ex1)
            {
                return NotFound(ex1.Message);
            }
            catch(InvalidOperationException ex2)
            {
                return BadRequest(ex2.Message);
            }
            catch(UnauthorizedAccessException ex3)
            {
                return Unauthorized(ex3.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            try
            {
                var user = await _userService.UpdateUserPassword(userPasswordUpdateModel, (int)userId);
                return Ok(user);
            }
            catch (KeyNotFoundException ex1)
            {
                return NotFound(ex1.Message);
            }
            catch(UnauthorizedAccessException ex2)
            {
                return Unauthorized(ex2.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            try
            {
                await _userService.DeleteUser((int)userId);

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
            catch (KeyNotFoundException ex1)
            {
                return NotFound(ex1.Message);
            }
            catch(InvalidOperationException ex2)
            {
                return BadRequest(ex2.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
