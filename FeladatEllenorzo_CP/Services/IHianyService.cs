using FeladatEllenorzo_CP.Models;

namespace FeladatEllenorzo_CP.Services
{
    public interface IHianyService
    {
        Task<List<Tanulo>> GetMaiHiany();
        Task<List<Tanulo>> GetHianyok(string datum);
        Task<MainResponse> Add(FeladatHiany hiany);
        Task<MainResponse> Remove(FeladatHiany hiany);
        Task<MainResponse> RemoveAll();
        Task<MainResponse> Update(FeladatHiany hiany);

    }
}
