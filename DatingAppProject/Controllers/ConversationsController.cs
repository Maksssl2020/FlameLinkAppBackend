using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/conversations")]
public class ConversationsController(IConversationRepository conversationRepository, DataContext dataContext) : ControllerBase {

    [HttpGet("{conversationId:long}")]
    public async Task<ActionResult<ConversationPreviewDto>> GetConversation(long conversationId) {
        var conversation = await conversationRepository.GetConversation(conversationId);

        if (conversation is null) {
            return NotFound("Conversation not found");
        }
        
        return Ok(conversation);
    }
    
    [HttpGet("all/by-user/{userId:long}")]
    public async Task<ActionResult<List<ConversationPreviewDto>>> GetConversations(long userId){
        var foundUser = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (foundUser == null) {
            return NotFound("User not found.");
        }
        
        var conversations = await conversationRepository.GetConversations(userId);
        return Ok(conversations);
    }

    [HttpPost]
    public async Task<ActionResult<ConversationPreviewDto>> CreateConversation([FromBody] ConversationRequestDto conversationRequest) {
        var existing = await dataContext.Conversations.FirstOrDefaultAsync(c =>
            (c.SenderId == conversationRequest.SenderId && c.RecipientId == conversationRequest.RecipientId) ||
            (c.SenderId == conversationRequest.RecipientId && c.RecipientId == conversationRequest.SenderId));

        if (existing != null) {
            return Ok(existing);
        }
        
        var sender = await dataContext.Users
            .Include(u => u.UserProfile.MainPhoto)
            .FirstOrDefaultAsync(u => u.Id == conversationRequest.SenderId);

        var recipient = await dataContext.Users
            .Include(u => u.UserProfile.MainPhoto)
            .FirstOrDefaultAsync(u => u.Id == conversationRequest.RecipientId);
        
        if (sender == null || recipient == null) {
            return BadRequest("Sender or recipient not found.");
        }

        var conversation = new Conversation {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Sender = sender,
            Recipient = recipient,
            LastMessage = ""
        };

        var conversationPreview = await conversationRepository.CreateConversation(conversation);
        return Ok(conversationPreview);
    }
}