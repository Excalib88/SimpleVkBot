using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace SimpleVkBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVkApi _vkApi;
        
        public CallbackController(IVkApi vkApi, IConfiguration configuration)
        {
            _vkApi = vkApi;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Callback([FromBody] Updates updates)
        {
            switch (updates.Type)
            {
                case "confirmation":
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":{

                    var msg = Message.FromJson(new VkResponse(updates.Object));

                    _vkApi.Messages.Send(new MessagesSendParams{ 
                        RandomId = new DateTime().Millisecond,
                        PeerId = msg.PeerId.Value,
                        Message = msg.Text
                    });
                    break;
                }
            }

            return Ok("ok");
        }
    }
}