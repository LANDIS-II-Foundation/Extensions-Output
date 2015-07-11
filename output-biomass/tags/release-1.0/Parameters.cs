using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.Biomass
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public class Parameters
		: IParameters
	{
		private int timestep;
		private IEnumerable<ISpecies> selectedSpecies;
		private string speciesMapNames;
		private SelectedDeadPools selectedPools;
		private string poolMapNames;

		//---------------------------------------------------------------------

		public int Timestep
		{
			get {
				return timestep;
			}
		}

		//---------------------------------------------------------------------

		public IEnumerable<ISpecies> SelectedSpecies
		{
			get {
				return selectedSpecies;
			}
		}

		//---------------------------------------------------------------------

		public string SpeciesMapNames
		{
			get {
				return speciesMapNames;
			}
		}

		//---------------------------------------------------------------------

		public SelectedDeadPools SelectedPools
		{
			get {
				return selectedPools;
			}
		}

		//---------------------------------------------------------------------

		public string PoolMapNames
		{
			get {
				return poolMapNames;
			}
		}

		//---------------------------------------------------------------------

		public Parameters(int                   timestep,
		                  IEnumerable<ISpecies> selectedSpecies,
		                  string                speciesMapNames,
		                  SelectedDeadPools     selectedPools,
		                  string                poolMapNames)
		{
			this.timestep = timestep;
			this.selectedSpecies = selectedSpecies;
			this.speciesMapNames = speciesMapNames;
			this.selectedPools = selectedPools;
			this.poolMapNames = poolMapNames;
		}
	}
}
