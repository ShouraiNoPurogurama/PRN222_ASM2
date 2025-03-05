namespace SalesManagement.Repository.Dtos;

public record ValidationResponse(
    bool IsValid,
    Dictionary<string, List<string>> Errors);