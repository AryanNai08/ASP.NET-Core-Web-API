using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CollegeApi.Controllers
{
    [Route("api/[controller]")] //Defines the base URL route
    [ApiController]//Marks the class as a Web API controller
                   //[Authorize(Roles = "Superadmin,Admin")]
    [AllowAnonymous]
    public class LoginController:ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public ActionResult Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide username & password");
            }

            LoginResponseDTO response = new() { Username = model.Username };


            byte[] key = null;
            if (model.Policy == "Local")
            {
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForLocal")); 
            }
            else if (model.Policy == "Microsoft")
            {
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForGoogle"));
            }
            else if (model.Policy == "Google")
            {
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForMicrosoft"));
            }



            

 

            if (model.Username == "Aryan" && model.Password == "Aryan9724")
            {
             
               
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        //username
                        new Claim(ClaimTypes.Name, model.Username),
                        //role
                        new Claim(ClaimTypes.Role, "Admin")
                                        
                    }),
                    Expires = DateTime.Now.AddHours(4),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.token = tokenHandler.WriteToken(token);
            }
            else
            {
                return Ok("Invalid username & password");
            }

            return Ok(response);
        }
    }
}
