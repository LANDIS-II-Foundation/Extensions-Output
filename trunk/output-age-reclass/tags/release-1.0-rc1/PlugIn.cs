using Edu.Wisc.Forest.Flel.Util;
using Landis.Landscape;
using Landis.RasterIO;
using Landis.Species;

using System.Collections.Generic;
using System.IO;

namespace Landis.Output.Reclass
{
	public class PlugIn
		: Landis.PlugIns.IOutput
	{
		private int timestep;
		private int nextTimeToRun;
		private string mapNameTemplate;
		private double[] reclassCoefs;
		private IEnumerable<IMapDefinition> mapDefs;
		private ILandscapeCohorts<AgeCohort.ICohort> cohorts;

		//---------------------------------------------------------------------

		/// <summary>
		/// The name that users refer to the plug-in by.
		/// </summary>
		public string Name
		{
			get {
				return "Reclass Output";
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
		/// <param name="startTime">
		/// Initial timestep (year): the timestep that will be passed to the
		/// first call to the component's Run method.
		/// </param>
		public void Initialize(string dataFile,
		                       int    startTime)
		{
			ParametersParser.SpeciesDataset = Model.Species;
			ParametersParser parser = new ParametersParser();
			IParameters parameters = Data.Load<IParameters>(dataFile,
			                                                parser);
			this.timestep = parameters.Timestep;
			this.nextTimeToRun = startTime - 1;

			this.mapNameTemplate = parameters.MapFileNames;
			this.reclassCoefs = parameters.ReclassCoefficients;
			this.mapDefs = parameters.ReclassMaps;

			cohorts = Model.GetSuccession<AgeCohort.ICohort>().Cohorts;
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
			foreach (IMapDefinition map in mapDefs)
			{
				IForestType[] forestTypes = map.ForestTypes;
				IOutputRaster<ClassPixel> newmap = CreateMap(map.Name, currentTime);
				string path = newmap.Path;
				using (newmap) {
					ClassPixel pixel = new ClassPixel();
					foreach (Site site in Model.Landscape.AllSites) {
						if (site.IsActive)
							pixel.Band0 = calcForestType(cohorts[site],
								forestTypes);
						else
							pixel.Band0 = 0;
						newmap.WritePixel(pixel);
					}
				}

				//Erdas74TrailerFile.Write(path, map.ForestTypes);
			}

			nextTimeToRun += timestep;
		}

		//---------------------------------------------------------------------

		private IOutputRaster<ClassPixel> CreateMap(string mapname,
		                                          int    currentTime)
		{
			string path = MapFileNames.ReplaceTemplateVars(mapNameTemplate, mapname, currentTime);
			UI.WriteLine("Writing reclass map to {0} ...", path);
			return Util.Raster.Create<ClassPixel>(path,
			                                    Model.LandscapeMapDims,
			                                    Model.LandscapeMapMetadata);
		}

		//---------------------------------------------------------------------

		private byte calcForestType(ISiteCohorts<AgeCohort.ICohort> cohorts,
			IForestType[] forestTypes)
		{
			IDataset SpeciesDataset = Model.Species;
			double[] forTypValue = new double[Model.Species.Count];
			int forTypeCnt = 0;
			foreach(IForestType ftype in forestTypes)
			{
				double sppValue = 0.0;
				ushort maxSpeciesAge = 0;
				foreach(ISpecies species in SpeciesDataset)
				{
					ISpeciesCohorts<AgeCohort.ICohort> speciesCohorts =
						cohorts[species];
					maxSpeciesAge = MaxAge(speciesCohorts);

					if(maxSpeciesAge > 0)
						sppValue = (double) maxSpeciesAge /
						(double) species.Longevity * 
						(double) reclassCoefs[species.Index];

					if(ftype[species.Index] != 0)
					{
						if(ftype[species.Index] == -1)
							forTypValue[forTypeCnt] -= sppValue;
						if(ftype[species.Index] == 1)
							forTypValue[forTypeCnt] += sppValue;
					}
				}
				forTypeCnt++;
			}
			
			int finalForestType = 0;
			double maxValue = 0.0;
			forTypeCnt = 0;
			foreach(IForestType ftype in forestTypes)
			{
				//System.Console.WriteLine("ForestTypeNum={0}, Value={1}.",forTypeCnt,forTypValue[forTypeCnt]); 
				if(forTypValue[forTypeCnt]>maxValue)
				{
					maxValue = forTypValue[forTypeCnt];
					finalForestType = forTypeCnt+1;
				}
				forTypeCnt++;
			}
			return (byte) finalForestType;
		}

		//---------------------------------------------------------------------

		private ushort MaxAge(ISpeciesCohorts<AgeCohort.ICohort> cohorts)
		{
			if (cohorts == null)
				return 0;
			ushort max = 0;
			foreach (ushort age in cohorts.Ages)
				if (age > max)
					max = age;
			return max;
		}
		//---------------------------------------------------------------------

	}
}
