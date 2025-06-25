using hms_backend.Models;
using hms_backend.Repositories.Interfaces;
using HmsBackend;
using Microsoft.EntityFrameworkCore;
/*
namespace hms_backend.Repositories
{
    public class JobRepository:IJobRepository
    {
        private readonly AppDbContext _context;


        public JobRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Job job)
        {
            await _context.Job.AddAsync(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Job job)
        {
            var job2 = await _context.Job.FindAsync(id);
            if (job2 != null)
            {
                _context.Job.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Job>> GetAllAync()
        {
            return await _context.Job.ToListAsync();
        }

        public async Task<Job> GetByIdAsync(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Job.Update(job);
            await _context.SaveChangesAsync();
        }
    }
}
*/