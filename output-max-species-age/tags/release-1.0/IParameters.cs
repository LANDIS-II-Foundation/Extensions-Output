using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.MaxSpeciesAge
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
		/// Template for the filenames for output maps.
		/// </summary>
		string MapNames
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Collection of species for which output maps are generated.
		/// </summary>
		IEnumerable<ISpecies> SelectedSpecies
		{
			get;
		}
	}
}
