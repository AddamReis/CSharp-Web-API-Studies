using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Catalogo.DTOs;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace WebAPI_Catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em : " + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UsuarioDTO model) //recebe no corpo as informações do usuário
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password); //criando instancia do usuário

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, false);
            return Ok(GeraToken(model));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            //verifica se as credenciais do usuário retorna um valor
            var result = await _signInManager.PasswordSignInAsync
                (userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false); //lockoutOnFailure: se tentar mais de 3 vezes bloqueia

            if (result.Succeeded)
                return Ok(GeraToken(userInfo));
            else
            {
                ModelState.AddModelError(string.Empty, "Login inválido...");
                return BadRequest(ModelState);
            }
        }

        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            var claims = new[]
            {
                //define declarações do usuário
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("cat", "ted"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //gera uma chave com base em um algoritmo simétrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"])); //faz leitura da chave no appsettings

            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //tempo de expiracao do token
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            //classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credenciais);

            //retorna os dados com o token e informações
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT Ok"
            };
        }
    }
}
