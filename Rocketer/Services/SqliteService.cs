using Microsoft.EntityFrameworkCore;
using Rocketer.Data;
using Rocketer.Data.Models.Database;
using Rocketer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocketer.Services;

public class SqliteService : ISqliteService
{
    private readonly ApplicationDbContext _dbContext;

    public SqliteService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddLaunchAsync(Launch launch)
    {
        _dbContext.Launches.Add(launch);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Launch> GetLaunchByIdAsync(string id)
    {
        var result = await _dbContext.Launches
            .FirstOrDefaultAsync(l => l.Id == id);
        return result;
    }

    public Task<List<Launch>> GetLaunchesAsync()
    {
        return _dbContext.Launches.ToListAsync();
    }

    public Task<List<Launch>> GetUnnotifiedLaunchesAsync()
    {
        return _dbContext.Launches
            .Where(x => x.NotifiedStatus == Enums.NotifiedStatus.NotNotified)
            .ToListAsync();
    }

    public async Task<bool> UpdateLaunchAsync(Launch updatedLaunch)
    {
        var existing = await _dbContext.Launches
            .FirstOrDefaultAsync(l => l.Id == updatedLaunch.Id);

        if (existing == null)
            return false;

        existing.Status = updatedLaunch.Status;
        existing.LaunchDate = updatedLaunch.LaunchDate;
        existing.NotifiedStatus = updatedLaunch.NotifiedStatus;

        await _dbContext.SaveChangesAsync();
        return true;
    }
}
