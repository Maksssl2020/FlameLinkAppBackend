using AutoMapper;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories;

public class MessagesRepository(DataContext dataContext, IMapper mapper) : IMessagesRepository {

    public async Task<MessageDto> SaveMessage(Message message){
        await dataContext.Messages.AddAsync(message);
        await dataContext.SaveChangesAsync();
        
        var messageDto = mapper.Map<MessageDto>(message);
        return messageDto;
    }

    public async Task<List<MessageDto>> GetMessages(long userId, long recipientId){
        var messages = await dataContext.Messages
            .Where(m => (m.SenderId == userId && m.RecipientId == recipientId) || 
                        (m.SenderId == recipientId && m.RecipientId == userId))
            .OrderBy(m => m.SentAt)
            .Include(m => m.Sender.UserProfile.MainPhoto)
            .Include(m => m.Recipient.UserProfile.MainPhoto)
            .ToListAsync();

        return messages.Select(m => new MessageDto {
            Id = m.Id,
            SenderId = m.SenderId,
            RecipientId = m.RecipientId,
            SenderUsername = $"{m.Sender.FirstName} {m.Sender.LastName}",
            RecipientUsername = $"{m.Recipient.FirstName} {m.Recipient.LastName}",
            SenderAvatar = mapper.Map<ImageDto>(m.Sender.UserProfile.MainPhoto),
            RecipientAvatar = mapper.Map<ImageDto>(m.Recipient.UserProfile.MainPhoto),
            Content = m.Content,
            SentAt = m.SentAt
        })
            .ToList();
    }

    public async Task<List<ConversationPreviewDto>> GetConversations(long userId){
        var messages = await dataContext.Messages
            .Where(m => m.SenderId == userId || m.RecipientId == userId)
            .OrderByDescending(m => m.SentAt)
            .Include(m => m.Sender.UserProfile.MainPhoto)
            .Include(m => m.Recipient.UserProfile.MainPhoto)
            .ToListAsync();
        
        var conversations = messages
            .GroupBy(m => m.SenderId == userId ? m.RecipientId : m.SenderId)
            .Select(g => g.First())
            .Select(m => new ConversationPreviewDto {
                UserId = m.SenderId == userId ? m.RecipientId : m.SenderId,
                Username = m.SenderId == userId
                    ? $"{m.Recipient.FirstName} {m.Recipient.LastName}"
                    : $"{m.Sender.FirstName} {m.Sender.LastName}",
                Avatar = mapper.Map<ImageDto>(
                    m.SenderId == userId ? m.Recipient.UserProfile.MainPhoto : m.Sender.UserProfile.MainPhoto),
                LastMessage = m.Content,
                LastMessageSentAt = m.SentAt
            })
            .ToList();
        
        return conversations;
    }
}