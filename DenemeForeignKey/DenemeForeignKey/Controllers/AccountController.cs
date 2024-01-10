using DenemeForeignKey.DTOs;
using DenemeForeignKey.Entities;
using DenemeForeignKey.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Windows.System;
using User = DenemeForeignKey.Entities.User;

namespace DenemeForeignKey.Controllers
{
 
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<User> userManager,TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {  throw new ArgumentNullException(nameof(user), "User cannot be null");

            return Unauthorized(); }
            else
            {
                return new UserDto
                {
                    Email = user.Email,
                    Token = await _tokenService.GenerateToken(user)
                };
            }
        }
        [HttpPost("register")]
        public async Task <ActionResult> Register(RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Username, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);

                }
                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(201);
       }

        [Authorize]
        [HttpGet("currentUser")]

        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user)
            };
        
        }
    }
}
