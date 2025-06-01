using FeladatEllenorzo_CP.Models;

namespace FeladatEllenorzo_CP.Services
{
    internal interface IPontService
    {
        Task<List<Pont>> GetPontById(int id);
        Task<List<Pont>> GetPontByTanulo(Guid tId);
        Task<MainResponse> Add(Pont pont);
        Task<List<Tanulo>> GetPontByOsztaly(string osztaly);
        //Task<MainResponse> Remove(Pont pont);
        //Task<MainResponse> Update(Pont pont);
    }
}
