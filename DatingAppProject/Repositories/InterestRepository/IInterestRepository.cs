using DatingAppProject.DTO;
using DatingAppProject.Entities;

namespace DatingAppProject.Repositories.InterestRepository;

public interface IInterestRepository {
    
    Task<Interest?> FindByName(string name);
    Task<List<Interest>> GetAllByNames(List<string> names);
    Task SaveInterest(InterestRequestDto interestRequest);
    Task<IEnumerable<InterestDto>> FindAllInterests();
    Task<bool> IsInterestAvailable(string interestName);
    Task<bool> SaveChanges();
}