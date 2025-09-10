namespace Team404_CampusConnect_Hackathon.Interface
{
    public interface IRepository <T> where T : class
    {
        /*
         * usually you would have a custom interface + repository for every model. To make this project a little easier to manage,
         * we decided it would be best to make a generic Interface + repository that recieves the model and does the crud operations for that model. 
         * Custom functions and extra actions will be performed in controllers were required
        */
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task SaveAsync();
    }
}
