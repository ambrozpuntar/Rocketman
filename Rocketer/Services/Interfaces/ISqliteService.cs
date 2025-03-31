using Rocketer.Data.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocketer.Services.Interfaces;

public interface ISqliteService
{
    Task<List<Launch>> GetLaunchesAsync();
    Task<Launch> GetLaunchByIdAsync(string id);

    Task AddLaunchAsync(Launch launch);
    Task<bool> UpdateLaunchAsync(Launch updatedLaunch);
    Task<List<Launch>> GetUnnotifiedLaunchesAsync();
}
