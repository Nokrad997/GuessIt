using System.Text.Json;
using Backend.Services.Interfaces;
using Backend.Utility;

namespace Backend.Services;

public class MonthlyUsageService : IMonthlyUsageService
{
    private readonly string _filePath = "Utility/MonthlyUsage.json";

    public async Task<string> UpdateMonthlyUsage(int userUsage)
    {
        var monthlyUsageData = await EnsureMonthlyUsageData();
        
        if (monthlyUsageData.LastUpdated.Month != DateTime.UtcNow.Month)
        {
            monthlyUsageData.MonthlyUsage = 0;
        }
        
        monthlyUsageData.MonthlyUsage += userUsage;
        monthlyUsageData.LastUpdated = DateTime.UtcNow;
        
        await SaveMonthlyUsageData(monthlyUsageData);

        return $"Monthly usage updated to: {monthlyUsageData.MonthlyUsage}";
    }
    
    public async Task<int> GetMonthlyUsage()
    {
        var monthlyUsageData = await EnsureMonthlyUsageData();

        return monthlyUsageData.MonthlyUsage;
    }
    
    private async Task<MonthlyUsageData> EnsureMonthlyUsageData()
    {
        MonthlyUsageData monthlyUsageData;

        if (!File.Exists(_filePath))
        {
            monthlyUsageData = new MonthlyUsageData
            {
                MonthlyUsage = 0,
                LastUpdated = DateTime.UtcNow
            };
            
            await SaveMonthlyUsageData(monthlyUsageData);
        }
        else
        {
            try
            {
                var jsonData = await File.ReadAllTextAsync(_filePath);
                
                monthlyUsageData = JsonSerializer.Deserialize<MonthlyUsageData>(jsonData);
                
                if (monthlyUsageData == null)
                {
                    throw new Exception("Deserialized MonthlyUsageData is null.");
                }
                
                if (monthlyUsageData.LastUpdated == default(DateTime))
                {
                    monthlyUsageData.LastUpdated = DateTime.UtcNow;
                }
                
                if (monthlyUsageData.MonthlyUsage < 0)
                {
                    monthlyUsageData.MonthlyUsage = 0;
                }
            }
            catch (Exception ex)
            {
                monthlyUsageData = new MonthlyUsageData
                {
                    MonthlyUsage = 0,
                    LastUpdated = DateTime.UtcNow
                };

                await SaveMonthlyUsageData(monthlyUsageData);
            }
        }

        return monthlyUsageData;
    }
    
    private async Task SaveMonthlyUsageData(MonthlyUsageData data)
    {
        try
        {
            var updatedJson = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(_filePath, updatedJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving the file: {ex.Message}");
        }
    }
}