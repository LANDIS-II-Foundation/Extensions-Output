//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.AgeReclass
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
                MapFiles.CheckTemplateVars(value);
                mapFileNames = value;
            }
		}

		//---------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InputParameters(int speciesCount)
        {
            coefficients = new double[speciesCount];
            mapDefns = new List<IMapDefinition>();
        }
	}
}
