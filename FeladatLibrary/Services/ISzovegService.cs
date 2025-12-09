using FeladatLibrary.Models;

namespace FeladatLibrary.Services
{
    public interface ISzovegService
    {
        Task<List<DataSzoveg>> GetSzovegek();
        Task<MainResponse> Add(DataSzoveg szoveg);
        Task<MainResponse> Remove(DataSzoveg szoveg);
        Task<MainResponse> Update(DataSzoveg szoveg);
    }
}
