using Chat_App_API.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat_App_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatBoxController : Controller
    {
        private static chatBox[] chats = 
        {
            new chatBox {Date = new DateTime(), Name = "Vicky", Text = "Hello" },
            new chatBox {Date = new DateTime(), Name = "Vicky", Text = "How are you" }
        };

        private List<chatBox> chatsList = chats.ToList();

       

        private readonly ILogger<ChatBoxController>_logger;
        private readonly IConfiguration _config;

        public ChatBoxController(ILogger<ChatBoxController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }





        [Authorize]
        [HttpGet(Name = "GetChatBox")]
        public JsonResult GetChats()
        {
            MongoClient dbClient = new MongoClient(_config.GetConnectionString("CharCon"));
            var chatList = dbClient.GetDatabase("ChatDB").GetCollection<chatBox>("chats").AsQueryable();
            return new JsonResult(chatList);
        }


        [Authorize]
        [HttpPost]
        [Route("setChatBox")]
        public JsonResult sendChat(chatBox chat)
        {
            MongoClient dbClient = new MongoClient(_config.GetConnectionString("CharCon"));

            dbClient.GetDatabase("ChatDB").GetCollection<chatBox>("chats").InsertOne(chat);

            return new JsonResult("Sent Successfully");
        }

    }
}