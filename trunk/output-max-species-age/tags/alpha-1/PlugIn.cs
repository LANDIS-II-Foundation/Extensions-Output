using Landis;
using Landis.Landscape;
using Landis.Raster;
using Landis.Species;

using System.Collections.Generic;
using System.IO;

namespace Landis.Output.MaxSpeciesAge
{
	public class PlugIn
		: Landis.PlugIns.IOutput
	{
		private int timestep;
		private int nextTimeToRun;
		private string mapNameTemplate;
		private IEnumerable<ISpecies> selectedSpecies;
		private ILandscapeCohorts<AgeOnly.ICohort> cohorts;

		//---------------------------------------------------------------------

		/// <summary>
		/// The name that users refer to the plug-in by.
		/// </summary>
		public string Name
		{
			get {
				return "Max Species Age";
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// The next timestep where the component should run.
		/// </summary>
		public int NextTimeToRun
		{
			get {
				return nextTimeToRun;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Initializes the component with a data file.
		/// </summary>
		/// <param name="dataFile">
		/// Path to the file with initialization data.
		/// </param>
		public void Initialize(string dataFile)
		{
			ParametersParser.SpeciesDataset = Model.Species;
			ParametersParser parser = new ParametersParser();
			IParameters parameters = Data.Load<IParameters>(dataFile,
			                                                parser);
			this.timestep = parameters.Timestep;
			this.nextTimeToRun = 0;

			this.mapNameTemplate = parameters.MapNames;
			this.selectedSpecies = parameters.SelectedSpecies;

			cohorts = Model.GetSuccession<AgeOnly.ICohort>().Cohorts;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Runs the component for a particular timestep.
		/// </summary>
		/// <param name="currentTime">
		/// The current model timestep.
		/// </param>
		public void Run(int currentTime)
		{
			foreach (ISpecies species in selectedSpecies) {
				string path = MapNames.ReplaceTemplateVars(mapNameTemplate, species.Name, currentTime);
				Log.Info("Writing map to {0} ...", path);
				IOutputRaster<AgePixel> map = Util.Raster.Create<AgePixel>(path,
				                                                           Model.LandscapeMapDims,
				                                                           null);
				using (map) {
					AgePixel pixel = new AgePixel();
					foreach (Site site in Model.Landscape.AllSites) {
						if (site.IsActive)
							pixel.Band0 = MaxAge(cohorts[site][species]);
						else
							pixel.Band0 = 0;
						map.WritePixel(pixel);
					}
				}
			}

			nextTimeToRun += timestep;
		}

		//---------------------------------------------------------------------

		private ushort MaxAge(ISpeciesCohorts<AgeOnly.ICohort> cohorts)
		{
			if (cohorts == null)
				return 0;
			ushort max = 0;
			foreach (ushort age in cohorts.Ages)
				if (age > max)
					max = age;
			return max;
		}
	}
}
