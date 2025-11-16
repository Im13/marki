using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Constants;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [Route("admin/api/[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null) 
            {
                user = await _userManager.FindByNameAsync(loginDTO.Email);
            }

            if (user == null) return Unauthorized(new ApiResponse(401, "Invalid email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid email or password"));

            // Check if user has admin/employee role
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(RoleConstants.SUPER_ADMIN) && 
                !roles.Contains(RoleConstants.ADMIN) && 
                !roles.Contains(RoleConstants.EMPLOYEE))
            {
                return Unauthorized(new ApiResponse(401, "You don't have permission to access admin panel"));
            }

            return new UserDTO
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [Authorize(Roles = RoleConstants.SUPER_ADMIN)]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> RegisterAdmin(RegisterDTO registerDTO)
        {
            if (CheckEmailExistsAsync(registerDTO.Email).Result)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse 
                { 
                    Error = new[] { "Email address is in use" } 
                });
            }

            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var roleResult = await _userManager.AddToRoleAsync(user, RoleConstants.ADMIN);

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
