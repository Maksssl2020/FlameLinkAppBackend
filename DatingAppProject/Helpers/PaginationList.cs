using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Helpers;

public class PaginationList<T> : List<T> {
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalItemsCount { get; set; }

    public PaginationList(IEnumerable<T> items, int count, int pageNumber, int pageSize){
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalItemsCount = count;
        AddRange(items);
    }

    public static async Task<PaginationList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize){
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new PaginationList<T>(items, count, pageNumber, pageSize);
    }
}