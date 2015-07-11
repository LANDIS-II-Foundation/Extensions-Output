//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

namespace Landis.Output.Reclass
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public class Parameters
		: IParameters
	{
		private int timestep;
		private double[] coefficients;
		private IMapDefinition[] mapDefns;
		private string mapFileNames;

		//---------------------------------------------------------------------

		/// <summary>
		/// Timestep (years)
		/// </summary>
		public int Timestep
		{
			get {
				return timestep;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Reclass coefficients for species
		/// </summary>
		public double[] ReclassCoefficients
		{
			get {
				return coefficients;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Reclass maps
		/// </summary>
		public IMapDefinition[] ReclassMaps
		{
			get {
				return mapDefns;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Template for the filenames for reclass maps.
		/// </summary>
		public string MapFileNames
		{
			get {
				return mapFileNames;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="timestep"></param>
		/// <param name="coefficients"></param>
		/// <param name="mapDefns"></param>
		/// <param name="mapFileNames"></param>
		public Parameters(int              timestep,
		                  double[]         coefficients,
		                  IMapDefinition[] mapDefns,
		                  string           mapFileNames)
		{
			this.timestep = timestep;
			this.coefficients = coefficients;
			this.mapDefns = mapDefns;
			this.mapFileNames = mapFileNames;
		}
	}
}
