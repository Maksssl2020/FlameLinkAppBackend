using DatingAppProject.DTO;
using DatingAppProject.Entities;

namespace DatingAppProject.Repositories;

public interface IMessagesRepository {
    
    Task<MessageDto> SaveMessage(Message message);
    Task<List<MessageDto>> GetMessages(long userId, long recipientId);
}