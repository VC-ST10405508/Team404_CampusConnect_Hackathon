using Microsoft.EntityFrameworkCore;
using Team404_CampusConnect_Hackathon.Data;
using Team404_CampusConnect_Hackathon.Interface;

namespace Team404_CampusConnect_Hackathon.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        /*
         * usually you would have a custom interface + repository for every model. To make this project a little easier to manage and not have 1000 Files,
         * we decided it would be best to make a generic Interface + repository that recieves the model and does the crud operations for that model.
         * T - basically represents table - AppDbContext explains this further
        */
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        // Returns all entries for that table - T
        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.ToListAsync();
        //finding specific entry
        public async Task<T> GetByIdAsync(string id) =>
            await _dbSet.FindAsync(id);
        //adding to the database
        public async Task AddAsync(T entity) =>
            await _dbSet.AddAsync(entity);
        //updating existing entity
        public async Task UpdateAsync(T entity) =>
            _dbSet.Update(entity);
        //deleting entry 
        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }
        // save Asnyc
        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();

    }
}
