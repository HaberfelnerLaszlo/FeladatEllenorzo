using FeladatLibrary.Models;

namespace FeladatLibrary.Services
{
    public interface IHibaService
    {
        Task<List<Tanulo>> GetMaiHibak();
        Task<List<Tanulo>> GetHibak(string datum);
        Task<MainResponse> Add(HibasFeladat hiba);
        Task<MainResponse> Remove(HibasFeladat hiba);
        Task<MainResponse> RemoveAll();
        Task<MainResponse> RemoveClass(string osztaly, string date);
        Task<MainResponse> Update(HibasFeladat hiba);

    }
}
