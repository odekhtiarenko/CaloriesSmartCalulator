using CaloriesSmartCalulator.Data;
using CaloriesSmartCalulator.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.DAL
{
    public class CaloriesCalculationTaskItemRepository : IRepository<CaloriesCalculationTaskItem>
    {
        private readonly CaloriesCalulatorDBContext _context;

        public CaloriesCalculationTaskItemRepository(CaloriesCalulatorDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CaloriesCalculationTaskItem>> FilterAsync(Func<CaloriesCalculationTaskItem, bool> predicate, int count)
        {
            return await Task.FromResult(_context.CaloriesCalculationTaskItems.Where(predicate).Take(count).ToList());
        }

        public async Task<CaloriesCalculationTaskItem> GetAsync(Guid Id)
        {
            return await _context.CaloriesCalculationTaskItems.FindAsync(Id);
        }

        public async Task<CaloriesCalculationTaskItem> InsertAsync(CaloriesCalculationTaskItem entity)
        {
            await _context.CaloriesCalculationTaskItems.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CaloriesCalculationTaskItem> UpdateAsync(CaloriesCalculationTaskItem entity)
        {
            _context.Entry(await _context.CaloriesCalculationTaskItems
                                        .Include(x => x.CaloriesCalculationTask)
                                        .FirstOrDefaultAsync(x => x.Id == entity.Id))
                   .CurrentValues
                   .SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
