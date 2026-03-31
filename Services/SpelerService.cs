using Spelers_API.Domain.EntitiesDB;
using Spelers_API.Services.Interfaces;
using SpelersAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelers_API.Services
{
    public class SpelerService : ISpelerService
    {
        private readonly ISpelerDAO _spelerDAO;

        public SpelerService(ISpelerDAO spelerDAO)
        {
            _spelerDAO = spelerDAO;
        }

        public async Task Add(Speler speler)
        {
            await _spelerDAO.AddAsync(speler);
        }

        public async Task Delete(int id)
        {
            await _spelerDAO.DeleteAsync(id);
        }

        public async Task<IEnumerable<Speler>> GetAll()
        {
            return await _spelerDAO.GetAllAsync();
        }

        public async Task<Speler?> GetById(int id)
        {
            return await _spelerDAO.GetByIdAsync(id);
        }

        public async Task Update(Speler speler)
        {
            await _spelerDAO.UpdateAsync(speler);
        }
    }
}
