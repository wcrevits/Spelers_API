using Microsoft.EntityFrameworkCore;
using Spelers_API.Domain.DataDB;
using Spelers_API.Domain.EntitiesDB;
using SpelersAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelers_API.Repositories
{
    public class SpelerDAO : ISpelerDAO
    {
        private readonly ApplicationDbContext _context;

        public SpelerDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Speler speler)
        {
            // Debug point: Check if 'speler.Naam' is null here!
            if (string.IsNullOrEmpty(speler.Naam))
            {
                throw new Exception("Mapping failed: Speler.Naam is null before saving!");
            }

            await _context.Spelers.AddAsync(speler);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var speler = await _context.Spelers
                .FirstOrDefaultAsync(e => e.Id == id);

            if (speler != null)
            {
                _context.Spelers.Remove(speler);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Speler>> GetAllAsync()
        {
            return await _context.Spelers
                .Include(s => s.Team)
                .Include(s => s.Positie)
                .ToListAsync();
        }

        public async Task<Speler?> GetByIdAsync(int id)
        {
            return await _context.Spelers
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(Speler speler)
        {
            _context.Spelers.Update(speler);
            await _context.SaveChangesAsync();
        }
    }
}
