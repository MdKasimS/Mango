using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.Security.Claims;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokGenerator,
                UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// AspNetUserClaims — empty is normal
        /*        AspNetUserClaims

            Id
            UserId
            ClaimType
            ClaimValue

            This table is populated only when you explicitly add a claim to a user.

            For example:

            await _userManager.AddClaimAsync(
                user,
                new Claim("Department", "Sales")
            );

            Then you will see something like:

            Id UserId  ClaimType ClaimValue
            1   user-guid Department  Sales
            Important for your Mango Auth API

            If your JWT contains claims such as:

            sub = user-id
            email = kasim@example.com
            role = Admin

            that does not necessarily mean those claims came from AspNetUserClaims.

            Your Auth API can construct JWT claims dynamically during login:

            new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            new Claim(JwtRegisteredClaimNames.Email, user.Email)

        Those claims exist inside the JWT, but aren't necessarily stored in the database.*/
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if not exists
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            //If user found, generate JWT token
            //Check None mode - shows whole encrypted values conatining header, token value, etc
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            //AuthAPI contains ApplicationUser (IdentityUser - a default User entity proivided by ASP.NET Identity
            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;

        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            //TODO: User creation timestamp missing.
            //Like Name, add timestamp too.
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);
                    UserDto userDto = new UserDto()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";

        }
    }
}

/*  AspNetUserTokens
    
    UserId
    LoginProvider
    Name
    Value

    ASP.NET Core Identity does not automatically store your JWT access token here just because your Auth API generated a JWT.

    This table is generally used by Identity APIs/providers to persist things such as:

    External authentication provider tokens
    Authenticator-related tokens
    Other provider-specific persistent tokens

    For example, an application might store data associated with:

    Google
    Microsoft
    Facebook

    authentication flows.
*/