using AuctionManagementSystem.Models;
using AuctionManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SampleDBContext _dbContext;
        public UserController(SampleDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/User/SignUp
        [HttpPost("SignUp")]
        public ActionResult<User> SignUp(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            //Check if the email address is available
            var registered_user = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (registered_user != null)
            {
                return BadRequest("Email Address is already registered");
            }

            // Hash the password
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // POST: api/User/SignIn
        [HttpPost("SignIn")]
        public ActionResult<User> SignIn(User SignInDetails)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == SignInDetails.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Email Address");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, SignInDetails.Password);

            if (verificationResult == PasswordVerificationResult.Success)
            {
                //Create session
                HttpContext.Session.SetInt32("UserId",user.UserId);

                return Ok(user);
            }
            else
            {
                return Unauthorized("Invalid Password");
            }
        }

        // GET: api/User/LogOut
        [HttpGet("LogOut")]
        public ActionResult LogOut()
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
        public ActionResult<User> GetUser()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if(UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            var user = _dbContext.Users.Find(UserId);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // PUT: api/User/UpdatePersonalDetails
        [HttpPut("UpdatePersonalDetails")]
        public ActionResult<User> UpdateUserPersonalDetails(UserPersonalDetailsUpdateModel userPersonalDetailsUpdateModel)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            if(userPersonalDetailsUpdateModel.FirstName != null)
            {
                user.FirstName = userPersonalDetailsUpdateModel.FirstName;
            }

            if (userPersonalDetailsUpdateModel.LastName != null)
            {
                user.LastName = userPersonalDetailsUpdateModel.LastName;
            }

            if (userPersonalDetailsUpdateModel.Address != null)
            {
                user.Address = userPersonalDetailsUpdateModel.Address;
            }

            if (userPersonalDetailsUpdateModel.PhoneNumber != null)
            {
                user.PhoneNumber = userPersonalDetailsUpdateModel.PhoneNumber;
            }

            _dbContext.SaveChanges();

            return Ok(user);
        }

        // PUT: api/User/UpdateEmail
        [HttpPut("UpdateEmail")]
        public ActionResult<User> UpdateUserEmail(User userDetails)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, userDetails.Password);

            if(verificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Incorrect Password");
            }

            user.Email = userDetails.Email;

            _dbContext.SaveChanges();

            return Ok(user);

        }

        // PUT: api/User/UpdatePassword
        [HttpPut("UpdatePassword")]
        public ActionResult<User> UpdateUserPassword(UserPasswordUpdateModel userPasswordUpdateModel)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, userPasswordUpdateModel.OldPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Incorrect Current Password");
            }

            user.Password = passwordHasher.HashPassword(user, userPasswordUpdateModel.NewPassword);

            _dbContext.SaveChanges();

            return Ok(user);
        }

        // Delete: api/User
        [HttpDelete]
        public ActionResult DeleteUser()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return BadRequest("User is not signed in");
            }

            var user = _dbContext.Users
                .Include(u => u.Seller)
                .FirstOrDefault(u => u.UserId == UserId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            if(user.Seller != null)
            {
                var auctions = _dbContext.Auctions
                    .Where(a => a.SellerId == user.Seller.SellerId)
                    .ToList();

                if (auctions.Count > 0)
                {
                    return BadRequest("User cannot be deleted. Because there are auctions associated with this seller account");
                }
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

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
