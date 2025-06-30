using HmsBackend.Repositories.Interfaces;
using HmsBackend.Services.Interfaces;

namespace HmsBackend.Services
{
    public class UserCountService : IUserCountService
    {
        private readonly IUserCountRepository _repository;

        public UserCountService(IUserCountRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetUserCountAsync()
        {
            return await _repository.GetTotalUserCountAsync();
        }
    }
}
