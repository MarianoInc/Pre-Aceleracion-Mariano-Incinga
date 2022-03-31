using AlkemyChallenge.API.Models;
using AlkemyChallenge.API.Services;
using AlkemyChallenge.API.ViewModels.Auth.Login;
using AlkemyChallenge.API.ViewModels.Auth.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlkemyChallenge.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //Administador de usuarios
        private readonly UserManager<User> _userManager;

        //Administrador de ingreso a la app   
        private readonly SignInManager<User> _signInManager;

        private readonly IMailService _mailService;

        public AuthenticationController(UserManager<User> userManager,
            SignInManager<User> signInManager, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        //Registro
        [HttpPost]
        [Route(template: "register")]
        public async Task<IActionResult> Register(RegisterRequestViewModel model)
        {
            //Revisar existencia del usuario
            var userExists = await _userManager.FindByNameAsync(model.UserName);

            //Si existe devolver un error
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            //Si no existe, registrar el usuario
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                IsActive = true
            };
            //Esto devuelve el resultado de la creación
            var result = await _userManager.CreateAsync(user, model.Password);

            //Validación del resultado
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "Error",
                        Message = $"User creation Failed! Errors {string.Join(", ", result.Errors.Select(x => x.Description))}"
                    });
            }

            //Envío del Mail
            await _mailService.SendEmailAsync(model.Email, "User created successfully!", $"<h1>Hey {model.UserName}! Welcome to our DisneyWorld!</h1>");

            return Ok(new
            {
                Status = "Succes",
                Message = "User created Successfully!"
            });
        }

        //Login
        [HttpPost]
        [Route(template: "login")]
        public async Task<IActionResult> Login(LoginRequestViewModel model)
        {
            //Chequeamos que el user existe y que la password entregada es correcta
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            //Verificamos el estado de la cuenta
            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(model.UserName);
                if (currentUser.IsActive)
                {
                    //Generamos y devolvemos el Token
                    return Ok(await GetToken(currentUser));
                }
            }

            //Generamos un error genérico por seguridad 
            return StatusCode(StatusCodes.Status401Unauthorized,
                    new
                    {
                        Status = "Error",
                        Message = $"User login Failed! {model.UserName} is not authorized!"
                    });
        }

        private async Task<LoginResponseViewModel> GetToken(User currenUser)
        {
            //Necesitamos los roles asignado al usuario
            var userRoles = await _userManager.GetRolesAsync(currenUser);

            //Creamos la lista de claims o privilegios
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, currenUser.UserName),
                //Declaramos protocolo estándar
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Agregamos roles a los claims
            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            //Levanto clave de firma
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyDeAutorizacion"));

            //Genero el Token
            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: authClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new LoginResponseViewModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };

        }
    }
}
