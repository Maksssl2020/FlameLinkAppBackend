using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DatingAppProject.Repositories.InterestRepository;

public class InterestRepository(DataContext dataContext, IMapper mapper) : IInterestRepository {
    public async Task<Interest?> FindByName(string name) {
        return await dataContext.Interests
            .Where(i => i.InterestName.ToUpper() == name.ToUpper())
            .FirstOrDefaultAsync();
    }

    public async Task<List<Interest>> GetAllByNames(List<string> names) {
        List<Interest> interests = [];
        
        foreach (var name in names) {
            var foundInterest = await FindByName(name);
            if (foundInterest != null) {
                interests.Add(foundInterest);
            }
        }
        
        return interests;
    }

    public async Task SaveInterest(InterestRequestDto interestRequest){

        if (interestRequest.InterestName.IsNullOrEmpty()) {
            throw new Exception("Invalid interest name.");
        }
        
        if (!await IsInterestAvailable(interestRequest.InterestName)) {
            throw new Exception("Interest not available.");
        }
        
        var interest = new Interest {
            InterestName = interestRequest.InterestName,
        };
        
        await dataContext.Interests.AddAsync(interest);
    }

    public async Task<IEnumerable<InterestDto>> FindAllInterests(){
        return await dataContext.Interests
            .ProjectTo<InterestDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> IsInterestAvailable(string interestName){
        return !await dataContext.Interests.AnyAsync(interest => interest.InterestName == interestName);
    }

    public async Task<bool> SaveChanges(){
        return await dataContext.SaveChangesAsync() > 0;
    }
}