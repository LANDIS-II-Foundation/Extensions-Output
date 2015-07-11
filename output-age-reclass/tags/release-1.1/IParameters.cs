//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

namespace Landis.Output.Reclass
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
		/// Reclass coefficients for species
		/// </summary>
		double[] ReclassCoefficients
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Reclass maps
		/// </summary>
		IMapDefinition[] ReclassMaps
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Template for the filenames for reclass maps.
		/// </summary>
		string MapFileNames
		{
			get;
		}
	}
}
