using AutoMapper;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories;

public class ConversationRepository(DataContext dataContext, IMapper mapper) : IConversationRepository {
    public async Task<ConversationPreviewDto> CreateConversation(Conversation conversation) {
        dataContext.Conversations.Add(conversation);
        await dataContext.SaveChangesAsync();

        return new ConversationPreviewDto {
            UserId = conversation.SenderId,
            Username = $"{conversation.Sender.FirstName} {conversation.Sender.LastName}",
            Avatar = mapper.Map<ImageDto>(conversation.Sender.UserProfile.MainPhoto),
            LastMessage = conversation.LastMessage,
            LastMessageSentAt =  DateTime.Now
        };
    }

    public async Task<ConversationPreviewDto?> GetConversation(long conversationId) {
        var conversation = await dataContext.Conversations
            .Include(c => c.Sender.UserProfile.MainPhoto)
            .Include(c => c.Recipient.UserProfile.MainPhoto)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null) {
            return null;
        }
        
        var otherUser = conversation.Recipient;

        return new ConversationPreviewDto {
            UserId = otherUser.Id,
            Username = $"{otherUser.FirstName} {otherUser.LastName}",
            Avatar = mapper.Map<ImageDto>(otherUser.UserProfile.MainPhoto),
            LastMessage = conversation.LastMessage,
            LastMessageSentAt =  DateTime.Now        
        };
    }

    public async Task<List<ConversationPreviewDto>> GetConversations(long userId) {
        var conversations = await dataContext.Conversations
            .Where(c => c.SenderId == userId || c.RecipientId == userId)
            .Include(m => m.Sender.UserProfile.MainPhoto)
            .Include(m => m.Recipient.UserProfile.MainPhoto)
            .ToListAsync();
        
        var conversationsDto = conversations
            .GroupBy(m => m.SenderId == userId ? m.RecipientId : m.SenderId)
            .Select(g => g.First())
            .Select(m => new ConversationPreviewDto {
                UserId = m.SenderId == userId ? m.RecipientId : m.SenderId,
                Username = m.SenderId == userId
                    ? $"{m.Recipient.FirstName} {m.Recipient.LastName}"
                    : $"{m.Sender.FirstName} {m.Sender.LastName}",
                Avatar = mapper.Map<ImageDto>(
                    m.SenderId == userId ? m.Recipient.UserProfile.MainPhoto : m.Sender.UserProfile.MainPhoto),
                LastMessage = m.LastMessage,
                LastMessageSentAt = DateTime.Now
            })
            .ToList();
        
        return conversationsDto;
    }
}