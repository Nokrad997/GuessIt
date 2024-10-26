namespace Backend.Services.Interfaces;

public interface IMonthlyUsageService
{
    Task<string> UpdateMonthlyUsage(int userUsage);
    Task<int> GetMonthlyUsage();
}