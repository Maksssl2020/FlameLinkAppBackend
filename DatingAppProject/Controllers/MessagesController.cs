using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MessagesController(IMessagesRepository messagesRepository, DataContext dataContext) : ControllerBase {
    
    [HttpGet("{userId:long}/{recipientId:long}")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages([FromRoute] long userId, [FromRoute] long recipientId) {
        var foundUser = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var foundRecipient = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == recipientId);

        if (foundRecipient == null || foundUser == null) {
            return NotFound("User not found.");
        }
        
        var messages = await messagesRepository.GetMessages(userId, recipientId);
        return Ok(messages);
    }
    
    [HttpPost]
    public async Task<ActionResult<MessageDto>> SendMessage([FromBody] MessageRequestDto messageRequest) {
        if (messageRequest.RecipientId == messageRequest.SenderId) {
            return BadRequest("You cannot message yourself!");
        }

        var foundSender = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == messageRequest.SenderId);
        var foundRecipient = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == messageRequest.RecipientId);

        if (foundRecipient == null || foundSender == null) {
            return NotFound("User not found.");
        }

        var message = new Message {
            RecipientId = foundRecipient.Id,
            SenderId = foundSender.Id,
            Sender = foundSender,
            Recipient = foundRecipient,
            Content = messageRequest.Content,
            SentAt = DateTime.Now
        };

        var savedMessage = await messagesRepository.SaveMessage(message);

        return Ok(savedMessage);
    }
}