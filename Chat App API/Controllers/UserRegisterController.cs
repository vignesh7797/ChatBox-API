using Chat_App_API.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Chat_App_API.Controllers
{
    [ApiController]
    [Route("users/[controller]")]
    public class UserRegisterController : Controller
    {
        
        private readonly ILogger<UserRegisterController> _logger;
        private readonly IConfiguration _config;

        public UserRegisterController(ILogger<UserRegisterController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }


        [Authorize]
        [HttpGet]
        [Route("List")]
        public JsonResult getUsersList()
        {
            MongoClient dbClient = new MongoClient(_config.GetConnectionString("ChatCon"));
            var usersList = dbClient.GetDatabase("ChatDB").GetCollection<userRegister>("usersList").AsQueryable();

            return new JsonResult(usersList);
        }

        [HttpPost]
        [Route("Adduser")]
        public JsonResult AddUser(userRegister user)
        {
            MongoClient dbClient = new MongoClient(_config.GetConnectionString("ChatCon"));

            dbClient.GetDatabase("ChatDB").GetCollection<userRegister>("usersList").InsertOne(user);

            var result = new { message = "Updated Successfully", token = GenerateToken(user) };

            return new JsonResult(result);

        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUser")]
        public JsonResult UpdateUser(userRegister user)
        {
            MongoClient dbClient = new MongoClient(_config.GetConnectionString("ChatCon"));
            var filter = Builders<userRegister>.Filter.Eq("Id", user.Id);
            var update = Builders<userRegister>.Update.Set("FirstName", user.FirstName)
                                                        .Set("LastName", user.LastName)
                                                        .Set("Email", user.Email)
                                                        .Set("Story", user.Story)
                                                        .Set("Img", user.Img)
                                                        .Set("Friends", user.Friends);

            dbClient.GetDatabase("ChatDB").GetCollection<userRegister>("usersList").UpdateOne(filter, update);

            

            return new JsonResult("Updated Successfully");
        }
        [Authorize]
        [HttpPut]
        [Route("status")]
        public JsonResult UpdateUserStatus(userRegister user)
        {
            MongoClient dbClient = new MongoClient(_config.GetConnectionString("ChatCon"));
            var filter = Builders<userRegister>.Filter.Eq("Id", user.Id);
            var update = Builders<userRegister>.Update.Set("isOnline", user.IsOnline);

            dbClient.GetDatabase("ChatDB").GetCollection<userRegister>("usersList").UpdateOne(filter, update);


            return new JsonResult("Updated Successfully");
        }

        [Authorize]
        [HttpPut]
        [Route("ChangePassword")]
        public JsonResult UpdatePassWord(userRegister user)
        {
            MongoClient dbClient = new MongoClient(_config.GetConnectionString("ChatCon"));
            var filter = Builders<userRegister>.Filter.Eq("Id", user.Id);
            var update = Builders<userRegister>.Update.Set("Password", user.Password);

            dbClient.GetDatabase("ChatDB").GetCollection<userRegister>("usersList").UpdateOne(filter, update);


            return new JsonResult("Updated Successfully");
        }


        private string GenerateToken(userRegister user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var crediantials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["JWT:Issuer"], _config["JWT:Audience"], null, expires:DateTime.Now.AddMonths(3), signingCredentials:crediantials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
