using AutoMapper;
using ShopApp.Business.Abstract;
using ShopApp.Core.Utilities.IoC;
using ShopApp.Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.WebUI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Core.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShopApp.Core.Entities.Concrete;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace ShopApp.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IMapper _mapper;
        public AuthController(IAuthService authService,IUserService userService, IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet("auth/login")]
        public IActionResult Login(string returnUrl=null)
        {
            return View(new UserForLoginModel()
            {
                ReturnUrl = returnUrl
            });
        }



        [HttpPost("auth/login")]
        public IActionResult Login(UserForLoginModel userForLoginModel)
        {

            UserForLoginDto userForLoginDto = _mapper.Map<UserForLoginDto>(userForLoginModel);

            var userToLogin = _authService.Login(userForLoginDto);

            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }
            
            var result = _authService.CreateAccessToken(userToLogin.Data);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(result.Data.Token.ToString());
            var tokenS = jsonToken as JwtSecurityToken;
            if (result.Success)
            {
                var claimRoles = _userService.GetClaims(userToLogin.Data);
                //var claims = new List<Claim> { };

                //claims.Add(new Claim(ClaimTypes.Name, userToLogin.Data.FirstName+" "+ userToLogin.Data.LastName));
                //claims.Add(new Claim(ClaimTypes.Email, userToLogin.Data.Email));
                //claims.Add(new Claim(ClaimTypes.NameIdentifier, userToLogin.Data.Id.ToString()));

                //foreach (var item in claimRoles)
                //{
                //    claims.Add(new Claim(ClaimTypes.Role, item.Name));
                //};
                var claims = tokenS.Claims;


                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                principal.GetClaimsUserId();
                var props = new AuthenticationProperties();
                HttpContext.Session.SetString("JWToken", result.Data.Token);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
                

                return Redirect(userForLoginModel.ReturnUrl ?? "~/");
                //using (var httpclient = new HttpClient())
                //{
                //    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userForLoginModel), Encoding.UTF8, "application/json");
                //    HttpContext.Session.SetString("JWToken", result.Data.Token);
                //    var accessToken = HttpContext.Session.GetString("JWToken");

                //    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //    HttpContext.Session.SetString("JWToken", result.Data.Token);


                //    var grandmaClaims = new List<Claim>()
                //         {
                //         new Claim(ClaimTypes.Name,"Bob"),
                //         new Claim(ClaimTypes.Email,userForLoginDto.Email),
                //         new Claim("Grandma.Says","very nice boi.")
                //         };
                //    var licenceClaims = new List<Claim>()
                //{
                //         new Claim(ClaimTypes.Name,"bob k boob"),
                //         new Claim("DrivingLicance","A+")
                //};
                //    var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
                //    var licenceIdentity = new ClaimsIdentity(licenceClaims, "Goverment");
                //    var userPrencipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenceIdentity });

                //    //await HttpContext.SignInAsync(userPrencipal);

                //}



                // return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("auth/register")]
        public IActionResult Register() {
            return View(new UserForRegisterDto());
        }


        [HttpPost("auth/register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {

            var userExists = _authService.UserExists(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {

                return Login(new UserForLoginModel()
                {
                    Email = userForRegisterDto.Email,
                    Password = userForRegisterDto.Password
                });
            }

            return BadRequest(result.Message);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }


    }
}
