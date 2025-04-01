using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities.InterestEntity;
using DatingAppProject.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DatingAppProject.Repositories.InterestRepository;

public class InterestRepository(DataContext dataContext, IMapper mapper) : IInterestRepository {
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