using FeladatLibrary.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatLibrary.Services
{
    public interface ISzorgalmiService
    {
        Task<List<Tanulo>> GetSzorgalmiByNev(string nev);
        Task<List<Tanulo>> GetSzorgalmiByOsztaly(string osztaly);
        Task<MainResponse> Add(Szorgalmi szorgalmi);
        Task<MainResponse> Remove(Szorgalmi szorgalmi);
        //Task<MainResponse> RemoveAll();
        Task<MainResponse> Update(Szorgalmi szorgalmi);
    }
}
