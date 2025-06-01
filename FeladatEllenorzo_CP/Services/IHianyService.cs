using FeladatEllenorzo_CP.Models;

namespace FeladatEllenorzo_CP.Services
{
    public interface IHianyService
    {
        Task<List<Tanulo>> GetMaiHiany();
        Task<List<Tanulo>> GetHianyok(string datum);
        Task<List<FeladatHiany>> GetHianyokByFeladat(string fId);//feladatId
        Task<MainResponse> Add(FeladatHiany hiany);
        Task<MainResponse> Remove(int hianyId);
        Task<MainResponse> RemoveAll();
        Task<MainResponse> Update(FeladatHiany hiany);

    }
}
