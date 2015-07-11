//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.Reclass
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public class InputParameters
		: IInputParameters
	{
		private int timestep;
		private double[] coefficients;
		private List<IMapDefinition> mapDefns;
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
            set {
                if (value < 0)
                    throw new InputValueException(value.ToString(), "Value must be = or > 0.");
                timestep = value;
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
		public List<IMapDefinition> ReclassMaps
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
            set {
                Reclass.MapFileNames.CheckTemplateVars(value);
                mapFileNames = value;
            }
		}

		//---------------------------------------------------------------------


        public InputParameters(int speciesCount)
        {
            coefficients = new double[speciesCount];
            mapDefns = new List<IMapDefinition>();
        }
        /// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="timestep"></param>
		/// <param name="coefficients"></param>
		/// <param name="mapDefns"></param>
		/// <param name="mapFileNames"></param>
/*		public Parameters(int              timestep,
		                  double[]         coefficients,
		                  List<IMapDefinition> mapDefns,
		                  string           mapFileNames)
		{
			this.timestep = timestep;
			this.coefficients = coefficients;
			this.mapDefns = mapDefns;
			this.mapFileNames = mapFileNames;
		}*/
	}
}
