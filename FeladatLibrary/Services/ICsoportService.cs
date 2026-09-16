using FeladatLibrary.Models;

namespace FeladatLibrary.Services
{
    public interface ICsoportService
    {
        public Task<MainResponse> CreateCsoport(Csoport csoport);
        public Task<List<Csoport>?> GetCsoportok();
        public Task<Csoport?> GetCsoport(Guid id);
        public Task<MainResponse> UpdateCsoport(Guid id, Csoport csoport);
        public Task<MainResponse> DeleteCsoport(Guid id);
    }
}
