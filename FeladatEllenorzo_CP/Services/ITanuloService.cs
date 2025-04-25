using FeladatEllenorzo_CP.Data;
using FeladatEllenorzo_CP.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Services
{
	public interface ITanuloService
	{
		Task<List<Tanulo>> GetTanulok();
		Task<Tanulo> GetTanulo(Guid id);
		Task<List<TanuloSync>> GetTanulokToIds();
		Task<List<TanuloSync>> GetTanulokToIds(string osztaly);
		Task<MainResponse> CreateTanulo(Tanulo tanulo);
		Task<MainResponse> UpdateTanulo(Guid id, Tanulo tanulo);
		Task<MainResponse> DeleteTanulo(Guid id);
	}
}
