using System.Text.Json;
using DatingAppProject.Helpers;

namespace DatingAppProject.extensions;

public static class HttpExtensions {
    public static void AddPaginationHeader<T>(this HttpResponse response, PaginationList<T> paginationList){
        var paginationHeader = new PaginationHeader(
            paginationList.CurrentPage,
            paginationList.PageSize, 
            paginationList.TotalItemsCount,
            paginationList.TotalPages
            );

        var jsonSerializerOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, jsonSerializerOptions));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}