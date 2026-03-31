using Spelers_API.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelersAPI.Repositories.Interfaces
{
    public interface ISpelerDAO
    {
        Task<IEnumerable<Speler>> GetAllAsync();
        Task<Speler?> GetByIdAsync(int id);
        Task AddAsync(Speler employee);
        Task UpdateAsync(Speler employee);
        Task DeleteAsync(int id);
    }
}
