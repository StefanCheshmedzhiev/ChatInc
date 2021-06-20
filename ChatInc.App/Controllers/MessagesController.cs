using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatInc.App.Models;
using ChatInc.Data;
using ChatInc.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatInc.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ChatIncDbContext context;

        public MessagesController(ChatIncDbContext context)
        {
            this.context = context;
        }

        [HttpGet(Name = "All")]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<Message>>> AllOrderByCreatedOnAscending()
        {
            return this.context.Messages
                .OrderBy(message => message.CreatedOn)
                .ToList();
        }

        [HttpPost(Name = "Create")]
        [Route("create")]
        public async Task<ActionResult> Create(MessageCreateBindingModel model)
        {
            var userFromDb = await this.context.Users
                .SingleOrDefaultAsync(user => user.Username == model.User);

            Message message = new Message
            {
                Content = model.Content,
                User = userFromDb,
                CreatedOn = DateTime.UtcNow
            };

            await this.context.Messages.AddAsync(message);

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}