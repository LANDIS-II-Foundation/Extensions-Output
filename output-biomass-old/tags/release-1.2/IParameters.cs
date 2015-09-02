using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.Biomass
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public interface IParameters
	{
		/// <summary>
		/// Timestep (years)
		/// </summary>
		int Timestep
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Collection of species for which biomass maps are generated.
		/// </summary>
		/// <remarks>
		/// null if no species are selected.
		/// </remarks>
		IEnumerable<ISpecies> SelectedSpecies
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Template for the filenames for species biomass maps.
		/// </summary>
		/// <remarks>
		/// null if no species are selected.
		/// </remarks>
		string SpeciesMapNames
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Dead pools for which biomass maps are generated.
		/// </summary>
		SelectedDeadPools SelectedPools
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Template for the filenames for dead-pool biomass maps.
		/// </summary>
		/// <remarks>
		/// null if no pools are selected.
		/// </remarks>
		string PoolMapNames
		{
			get;
		}
	}
}
