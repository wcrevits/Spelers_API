using Spelers_API.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelers_API.Services.Interfaces
{
    public interface ISpelerService
    {
        Task<IEnumerable<Speler>> GetAll();
        Task<Speler?> GetById(int id);
        Task Add(Speler speler);
        Task Update(Speler speler);
        Task Delete(int id);
    }
}
