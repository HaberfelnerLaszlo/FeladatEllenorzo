using FeladatEllenorzo_CP.Models;

namespace FeladatEllenorzo_CP.Services
{
    public interface IHibaService
    {
        Task<List<HibasFeladat>> GetMaiHibak();
        Task<List<HibasFeladat>> GetHibak(string datum);
        Task<MainResponse> Add(HibasFeladat hiba);
        Task<MainResponse> Remove(HibasFeladat hiba);
        Task<MainResponse> RemoveAll();
        Task<MainResponse> Update(HibasFeladat hiba);

    }
}
