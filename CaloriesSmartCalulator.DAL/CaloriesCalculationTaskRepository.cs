using CaloriesSmartCalulator.Data;
using CaloriesSmartCalulator.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.DAL
{
    public class CaloriesCalculationTaskRepository : IRepository<CaloriesCalculationTask>
    {
        private readonly CaloriesCalulatorDBContext _context;

        public CaloriesCalculationTaskRepository(CaloriesCalulatorDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CaloriesCalculationTask>> FilterAsync(Func<CaloriesCalculationTask, bool> predicate, int count)
        {
            return await Task.FromResult(_context.CaloriesCalculationTasks.Where(predicate).Take(count).ToList());
        }

        public async Task<CaloriesCalculationTask> GetAsync(Guid Id)
        {
            return await _context.CaloriesCalculationTasks.Include(x => x.CaloriesCalculationTaskItems).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<CaloriesCalculationTask> InsertAsync(CaloriesCalculationTask entity)
        {
            await _context.CaloriesCalculationTasks.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CaloriesCalculationTask> UpdateAsync(CaloriesCalculationTask entity)
        {
            _context.Entry(await _context.CaloriesCalculationTasks.FindAsync(entity.Id)).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
