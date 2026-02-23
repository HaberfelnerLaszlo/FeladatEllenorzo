using FeladatLibrary.Data;
using FeladatLibrary.Models;

namespace FeladatLibrary.Services
{
    public interface IPontService
    {
        public string ErrorMessage { get; set; }
        Task<List<Pont>> GetPontById(int id);
        Task<List<Pont>> GetPontByTanulo(Guid tId);
        Task<MainResponse> Add(Pont pont);
        Task<List<Tanulo>> GetPontByOsztaly(string osztaly);
        Task<TanuloData> GetTanuloData(string id);
        Task<bool> Remove(int id);
        Task<Pont?> Update(Pont pont);
    }
}
