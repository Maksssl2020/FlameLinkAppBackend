using DatingAppProject.DTO;
using DatingAppProject.Entities;

namespace DatingAppProject.Repositories;

public interface IConversationRepository {
    Task<ConversationPreviewDto> CreateConversation(Conversation conversation);
    Task<ConversationPreviewDto?> GetConversation(long conversationId);
    Task<List<ConversationPreviewDto>> GetConversations(long userId);
}