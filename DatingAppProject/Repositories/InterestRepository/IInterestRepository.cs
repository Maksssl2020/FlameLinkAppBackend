using DatingAppProject.DTO;

namespace DatingAppProject.Repositories.InterestRepository;

public interface IInterestRepository {
    
    Task SaveInterest(InterestRequestDto interestRequest);
    Task<IEnumerable<InterestDto>> FindAllInterests();
    Task<bool> IsInterestAvailable(string interestName);
    Task<bool> SaveChanges();
}